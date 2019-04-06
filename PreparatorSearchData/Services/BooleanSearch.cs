using Newtonsoft.Json;
using PreparatorSearchData.Services.Model;
using System;
using System.Collections.Generic;
using System.IO;
using LingvoNET;
using System.Linq;
using System.Text;

namespace PreparatorSearchData.Services
{
    class BooleanSearch
    {

        public void StartSearch()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var dictionary = CommonService.IndexWords;

            while (true)
            {
                Console.WriteLine("Input your request:");
                var request = Console.ReadLine().Split();
                var lemmingRequest = CommonService.GetStremmingWords(request);

                var searchQuery = new List<Word>();
                foreach (var word in lemmingRequest)
                {
                    var findWord = dictionary.FirstOrDefault(x => x.Name == word);
                    if (findWord != null)
                    {
                        searchQuery.Add(findWord);
                    }
                }
                if (searchQuery.Count == 0)
                {
                    Console.WriteLine("404 Not Found (");
                    continue;
                }
                var resultIsNone = true;
                for (int i = 0; i < 100; i++)
                {
                    var flag = new StringBuilder();
                    foreach (var analyseWord in searchQuery)
                    {
                        flag.Append(analyseWord.Index[i]);
                    }
                    if (!flag.ToString().Contains('0'))
                    {
                        resultIsNone = false;
                        Console.WriteLine($"See {CommonService.Urls[i]}");
                    }
                }
                if (resultIsNone)
                {
                    Console.WriteLine("Sorry, no matches :( Try again");
                }
            }
        }
    }
}
