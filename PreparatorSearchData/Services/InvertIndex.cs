using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using PreparatorSearchData.Services.Model;

namespace PreparatorSearchData.Services
{
    class InvertIndex
    {
        public void GetInvertIndex()
        {
            List<Word> words = new List<Word>();
            foreach (var word in CommonService.UnicWord)
            {
                Word currentWord = new Word()
                {
                    Name = word,
                };
                StringBuilder index = new StringBuilder();
                foreach (var filePath in CommonService.SortedLemmitedFiles)
                {
                    var wordsInFile = File.ReadAllLines(filePath);
                    if (wordsInFile.Any(x => x == word))
                    {
                        index.Append("1");
                    }
                    else
                    {
                        index.Append("0");
                    }
                }
                currentWord.Index = index.ToString();
                words.Add(currentWord);
            }
            string json = JsonConvert.SerializeObject(words);
            File.WriteAllText($@"{CommonService.ProjectDir}\Resources\InvertIndex\InvertIndex.json", json);
        }
    }

    
}
