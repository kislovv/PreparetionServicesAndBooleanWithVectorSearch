using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PreparatorSearchData.Services.Model;

namespace PreparatorSearchData.Services
{
    public static class TfIdf
    {
        public static void GetTfIdfFile()
        {           
            var indexWords = CommonService.IndexWords;
            var idfDictionary = CommonService.IdfWords;
            var docs = new List<Docs>();
            foreach (var file in CommonService.SortedLemmitedFiles)
            {
                var words = File.ReadAllLines(file);

                var tfDictionary = GetWordsLengthInFile(words);
                var tfIdfValues = GetThIdfWords(tfDictionary, idfDictionary);
                Docs document = new Docs
                {
                    DocumentName = Path.GetFileNameWithoutExtension(file),
                    ThIdfWords = tfIdfValues,
                    VectorLength = Math.Sqrt(tfIdfValues.Select(x => x.TfIdf).Sum(y => Math.Pow(y, 2)))
                };

                docs.Add(document);
            }
            var json = JsonConvert.SerializeObject(docs);
            File.WriteAllText($@"{CommonService.ProjectDir}\Resources\TfIdf\TfIdf.json", json);
        }

        public static Dictionary<string, double> GetWordsLengthInFile(string[] words)
        {
            var wordsLengthInFile = new Dictionary<string, double>();

            foreach (var unicalWord in CommonService.UnicWord)
            {
                double wordCount = words.Count(x => x == unicalWord);
                wordsLengthInFile.Add(unicalWord, wordCount / words.Length);
            }
            return wordsLengthInFile;
        }

        public static List<TfIdfWord> GetThIdfWords(Dictionary<string, double> wordsLengthInFile , Dictionary<string, double> idfDictionary)
        {
            var tfIdfWords = new List<TfIdfWord>();

            foreach (var wordKey in wordsLengthInFile.Keys)
            {
                var thIdfWord = new TfIdfWord
                {
                    Value = wordKey,
                    TfIdf = wordsLengthInFile[wordKey] * idfDictionary[wordKey]
                };
                tfIdfWords.Add(thIdfWord);
            }
            return tfIdfWords;
        }

    }

    
}
