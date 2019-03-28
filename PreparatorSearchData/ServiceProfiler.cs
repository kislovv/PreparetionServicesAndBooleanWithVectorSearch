using PreparatorSearchData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreparatorSearchData
{
    public static class ServiceProfiler
    {
        public static void StartProfiling()
        {
            //Console.WriteLine("Input \"info\" or \"command\" for getting instruction or information");

            //Сrawler crawler = new Сrawler("https://sobakainfo.ru");
            //crawler.CrawlingSite();
            Lemmiter lemmiter = new Lemmiter();
            lemmiter.LemmitedSite();

            //InvertIndex invertIndex = new InvertIndex();
            //invertIndex.GetInvertIndex();
            //Console.WriteLine(Informer.GetInfoFromOption(Console.ReadLine()));
            //TfIdf.GetTfIdfFile();

            //BooleanSearch boolean = new BooleanSearch();
            //boolean.StartSearch();
            //TfIdf.GetTfIdfFile();

            //VectorSearch.Start();
        }
    }
}
