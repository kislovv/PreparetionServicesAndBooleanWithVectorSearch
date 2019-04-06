using PreparatorSearchData.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PreparatorSearchData
{
    public static class ServiceProfiler
    {
        public static void StartProfiling()
        {
            ///TODO:
            ///Добавить профиллеровку
            ///т.е. чтобы можно было отдельно прогонять подготовку сайта к поиску 
            ///и отдельно запускать на выбор булевский поиск или векторный
            //Console.WriteLine("Input \"info\" or \"command\" for getting instruction or information");

            //Сrawler crawler = new Сrawler("https://sobakainfo.ru");
            //crawler.CrawlingSite();
            //Lemmiter lemmiter = new Lemmiter();
            //lemmiter.LemmitedSite();

            //InvertIndex invertIndex = new InvertIndex();
            //invertIndex.GetInvertIndex();
            //Console.WriteLine(Informer.GetInfoFromOption(Console.ReadLine()));
            //TfIdf.GetTfIdfFile();

            //BooleanSearch boolean = new BooleanSearch();
            //boolean.StartSearch();
            //TfIdf.GetTfIdfFile();

            //VectorSearch.Start();
        }

        private static bool GetTempPuthStatus()
        {
            return true;
        }
    }
}
