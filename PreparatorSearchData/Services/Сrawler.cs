using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace PreparatorSearchData.Services
{
    public class Сrawler
    {
        public string MainUri { get; }
        public Сrawler(string uri)
        {
            MainUri = uri;
        }

        /// <summary>
        /// Сбор постраничных данных с сайта пока не соберет 100 уникальных страниц(можно расширить)
        /// </summary>
        public void CrawlingSite()
        {
            var urlsWithFiles = new Dictionary<string, string>();

            var uri = new Uri(MainUri);
            var html = string.Empty;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(uri);

            }
            var configuration = new HtmlParserOptions()
            {
                IsScripting = false
            };
            var parser = new HtmlParser(configuration);
            using (var document = parser.ParseDocument(html))
            {
                AddWebPageInDictionary(MainUri, document, urlsWithFiles, parser);
            }
            FillFiles(urlsWithFiles);
        }

        /// <summary>
        /// Рекурсивный обход по ссылкам и запись урлов и их содержимого в Словарь
        /// </summary>
        /// <param name="url">Новый url для добавления в коллекцию</param>
        /// <param name="document">Документ html страницы</param>
        /// <param name="urlsAndBodyText">Коллекция url/bodys</param>
        private void AddWebPageInDictionary(string url, IHtmlDocument document, Dictionary<string, string> urlsAndBodyText, HtmlParser parser)
        {
            if (urlsAndBodyText.Count == 100) return;

            var resultText = string.Empty;
            foreach (var el in document.Body.ChildNodes.Where(x => x.NodeName != "SCRIPT" && x.NodeName != "HEADER" && x.NodeName != "FOOTER"))
            {
                resultText += el.TextContent;
            }
            var body = System.Text.RegularExpressions.Regex.Replace(resultText, @"\s+", " ");
            urlsAndBodyText.Add(url, body);

            foreach (var link in document.QuerySelectorAll("a").ToList())
            {
                var href = link.GetAttribute("href") ?? link.GetAttribute("src");
                if (href == null)
                {
                    continue;
                }
                var forgotUrls = urlsAndBodyText.Where(x => x.Key.Contains("forgot")).Count();

                if (urlsAndBodyText.ContainsKey(href) ||
                    (href.Contains("http") && !href.StartsWith(MainUri)) ||
                    href == "/ru" || href == "/ru/" || urlsAndBodyText.ContainsKey(MainUri + href) ||
                    href.Contains("java") || href.Contains("about:///") || href.StartsWith("#") ||
                    (href.Contains("forgot") && forgotUrls > 0))
                {
                    continue;
                }
                href = !MainUri.Contains(href) 
                    ? MainUri + href 
                    : href;
                using (var client = new WebClient())
                {
                    var html = new WebClient().DownloadString(href);
                    using (document = parser.ParseDocument(html))
                    {
                        AddWebPageInDictionary(href, document, urlsAndBodyText, parser);
                    }
                }
            }
        }

        private static void FillFiles(Dictionary<string, string> urlsAndBodys)
        {
            var file = File.CreateText(@"C:\Users\Kirill\Desktop\Site\urls.txt");
            var index = 1;
            foreach (var urlBody in urlsAndBodys)
            {
                File.WriteAllText(@"C:\Users\Kirill\Desktop\Site\" + index + ".txt", urlBody.Value);
                file.WriteLine(urlBody.Key);
                index++;
            }
        }
    }
}
