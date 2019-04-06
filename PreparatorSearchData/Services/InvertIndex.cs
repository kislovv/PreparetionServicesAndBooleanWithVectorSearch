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
            var words = new List<Word>();
            foreach (var word in CommonService.UnicWord)
            {
                var currentWord = new Word()
                {
                    Name = word,
                };
                var index = new StringBuilder();
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
            var json = JsonConvert.SerializeObject(words);
            File.WriteAllText($@"{CommonService.ProjectDir}\Resources\InvertIndex\InvertIndex.json", json);
        }
    }

    
}
