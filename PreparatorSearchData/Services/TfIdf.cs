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
            string json = JsonConvert.SerializeObject(docs);
            File.WriteAllText($@"{CommonService.ProjectDir}\Resources\TfIdf\TfIdf.json", json);
        }

        public class Docs
        {
            public string DocumentName { get; set; }
            public double VectorLength { get; set; }
            public List<ThIdfWord> ThIdfWords { get; set; }
        }

        public class ThIdfWord
        {
            public string Value { get; set; }
            public double TfIdf { get; set; }
        }

        private static Dictionary<string, double> GetWordsLengthInFile(string[] words)
        {
            Dictionary<string, double> wordsLengthInFile = new Dictionary<string, double>();

            foreach (var unicalWord in CommonService.UnicWord)
            {
                double wordLength = words.Count(x => x == unicalWord);
                wordsLengthInFile.Add(unicalWord, wordLength / words.Length);
            }
            return wordsLengthInFile;
        }

        public static List<ThIdfWord> GetThIdfWords(Dictionary<string, double> wordsLengthInFile , Dictionary<string, double> idfDictionary)
        {
            List<ThIdfWord> tfIdfWords = new List<ThIdfWord>();

            foreach (var wordKey in wordsLengthInFile.Keys)
            {
                ThIdfWord thIdfWord = new ThIdfWord
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
