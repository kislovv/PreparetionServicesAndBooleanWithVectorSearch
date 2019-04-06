using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PreparatorSearchData.Services.Model;

namespace PreparatorSearchData.Services
{
    public static class VectorSearch
    {
        public static void Start()
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Словари загружаются...");
            var tfIdfDocs = CommonService.TfIdfDocs;
            var idfWords = CommonService.IdfWords;
            if (tfIdfDocs == null)
            {
                Console.WriteLine("Что то пошло не так, обратитесь к администратору поисковика");
                return;
            }
            Console.WriteLine("Все отлично! Можете работать с поисковиком:");
            while (true)
            {
                Console.WriteLine("Введите запрос:");

                var inputDoc = Console.ReadLine().Split();

                inputDoc = CommonService.GetStremmingWordsForQuery(inputDoc);

                var tfInputDoc = TfIdf.GetWordsLengthInFile(inputDoc);

                var tfIdfInputQuery = TfIdf.GetThIdfWords(tfInputDoc, idfWords);

                var queryVectorLength = Math.Sqrt(tfIdfInputQuery.Select(x => x.TfIdf).Sum(y => Math.Pow(y, 2)));

                var resultUrls = GetResultInQuery(tfIdfDocs, tfIdfInputQuery, queryVectorLength);

                var resultIsNone = true;
                foreach (var url in resultUrls)
                {
                    if (url.Value == 0.0)
                    {
                        break;
                    }
                    resultIsNone = false;
                    Console.WriteLine($"See {url.Key} , it's weigth is {url.Value}");
                }

                if (resultIsNone)
                {
                    Console.WriteLine("Запрос не дал результатов");
                }
            }
        }

        private static Dictionary<string, double> GetResultInQuery(List<Docs> tfIdfDocs, List<TfIdfWord> tfIdfInputQuery, double queryVectorLength)
        {
            var result = new Dictionary<string, double>();

            foreach (var doc in tfIdfDocs)
            {
                var resultCos = 0.0;
                for (var i = 0; i < tfIdfInputQuery.Count; i++)
                {
                    resultCos += tfIdfInputQuery[i].TfIdf * doc.ThIdfWords[i].TfIdf;
                }
                resultCos = queryVectorLength == 0
                    ? 0.0
                    : resultCos / (queryVectorLength * doc.VectorLength);   
                
                var urlNum = int.Parse(doc.DocumentName);
                result.Add(CommonService.Urls[urlNum-1], resultCos);
            }
            var sortesResult = result.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
           
            return sortesResult;
        }
    }
}
