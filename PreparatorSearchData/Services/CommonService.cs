﻿using LingvoNET;
using Newtonsoft.Json;
using PreparatorSearchData.Services.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PreparatorSearchData.Services
{
    public static class CommonService
    {
        /// <summary>
        /// Директория проекта
        /// </summary>
        public static string ProjectDir { get; } = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        /// <summary>
        /// IDF для всех уникальных слов 
        /// </summary>
        private static Dictionary<string,double> _idfWords;
        public static Dictionary<string, double> IdfWords
            {
            get
            {
                if (_idfWords == null)
                {
                    _idfWords = GetIdfDictionary();
                }
                return _idfWords;
            }
            }

        /// <summary>
        /// Список путей к файлам которые пролеммитизированны(Если таких нет вернет null)
        /// TODO: Реализовать сейв для получения этого списка с указанием нструкции в случае отсутствия такового
        /// </summary>
        public static string[] LemmitedFiles { get; } = Directory.GetFiles(ProjectDir + @"\Resources\Lemmiter");

        /// <summary>
        /// Список всех урлов поиска
        /// </summary>
        private static string[] _urls;
        public static string[] Urls
        {
            get
            {
                if (_urls == null)
                {
                    _urls = File.ReadAllLines(ProjectDir + @"\Resources\Site\index.txt");
                }
                return _urls;
            }
        }

        /// <summary>
        /// Список файлов отсортированные по нумерации (1,2,3...100) а не по строке (1,10,100,...99)
        /// </summary>
        private static List<string> _sortedLemmitedFiles;
        public static List<string> SortedLemmitedFiles
        {
            get
            {
                if (_sortedLemmitedFiles == null)
                {
                    _sortedLemmitedFiles = LemmitedFiles.OrderBy(FormatFileNumberForSort).ToList();
                }
                return _sortedLemmitedFiles;
            }
        }

        /// <summary>
        /// Возвращает список проллемитизированных слов из запроса (если не может найти начальную форму то стреммит его т.е. обрезает окончание) 
        /// </summary>
        /// <param name="request">список слов запроса</param>
        /// <returns> обработанный список слов из запроса</returns>
        public static List<string> GetStremmingWords(string[] request)
        {
            var result = new List<string>();
            foreach (var word in request)
            {
                var infWord = Analyser.FindAllSourceForm(word).FirstOrDefault();
                result.Add(infWord.SourceForm ?? Stemmer.Stemm(word));
            }
            return result;
        }

        /// <summary>
        /// Леммитизация слов из непосредственного запроса пользователя в векторном поиске
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string[] GetStremmingWordsForQuery(string[] request)
        {
            var result = new string[request.Length];
            for (int i = 0; i < request.Length; i++)
            {
                var infWord = Analyser.FindAllSourceForm(request[i]).FirstOrDefault();
                result[i] = (infWord.SourceForm ?? Stemmer.Stemm(request[i]));
            }
      
            return result;
        }

        /// <summary>
        /// Набор слов проиндексированные взятые из json файла 
        /// </summary>
        private static List<Word> _indexWords;
        public static List<Word> IndexWords
        {
            get
            {
                if(_indexWords == null)
                {
                     _indexWords = new List<Word>();
                    using (StreamReader reader = new StreamReader(ProjectDir + @"\Resources\InvertIndex\InvertIndex.json"))
                    {
                        var json = reader.ReadToEnd();
                        _indexWords = JsonConvert.DeserializeObject<List<Word>>(json);
                    }
                }
                return _indexWords;
            }
        }


        private static List<Docs> _tfIdfDocs;

        public static List<Docs> TfIdfDocs
        {
            get
            {
                if (_tfIdfDocs == null)
                {
                    _tfIdfDocs = GetDocs();
                }
                return _tfIdfDocs;
            }
        }

        private static List<Docs> GetDocs()
        {
            List<Docs> docs = new List<Docs>();

            using (var reader = new StreamReader(ProjectDir + @"\Resources\TfIdf\TfIdf.json"))
            {
                var json = reader.ReadToEnd();
                docs = JsonConvert.DeserializeObject<List<Docs>>(json);
            }
            return docs;
        }

        /// <summary>
        /// Список всех уникальных слов поиска
        /// </summary>
        private static List<string> _unicWord;
        public static List<string> UnicWord
        {
            get
            {
                if (_unicWord == null)
                {
                    _unicWord = GetAllUnicalWordsInSite();
                }
                return _unicWord;
            }
        }

        private static List<string> GetAllUnicalWordsInSite()
        {
            var words = new List<string>();
            foreach (var filePath in LemmitedFiles)
            {
                var wordsInFile = File.ReadAllLines(filePath);
                foreach (var word in wordsInFile)
                {
                    if (words.Any(x => x == word))
                    {
                        continue;
                    }
                    words.Add(word);
                }
            }
            return words;
        }

        private static string FormatFileNumberForSort(string inVal)
        {
            if (int.TryParse(Path.GetFileNameWithoutExtension(inVal), out int outputName))
            {

                return $"{outputName:0000000000}";
            }
            else
            {
                return inVal;
            }
        }


        private static Dictionary<string, double> GetIdfDictionary()
        {
            var idfDictionary = new Dictionary<string, double>();
            foreach (var word in IndexWords)
            {
                var idf = Math.Log(100 / word.Index.Count(x => x == '1'));
                idfDictionary.Add(word.Name, idf);
            }
            return idfDictionary;
        }

    }
}
