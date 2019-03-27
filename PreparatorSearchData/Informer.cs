using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PreparatorSearchData
{
    public static class Informer
    {
        public static DirectoryInfo CurrentProjDirInfo { get; } = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;

        private static readonly List<string> services = new List<string> { "Crauler", "Lemmiter", "InvertionIndexer", "TFIdf" };
        private static List<DirectoryInfo> SubDirectory { get; } = CurrentProjDirInfo.EnumerateDirectories().ToList();

        /// <summary>
        /// get command stack or system info
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string GetInfoFromOption(string option)
        {
            if (option != "info" || option != "command")
            {
                return "This option is not supported!";
            }
            StringBuilder state = new StringBuilder();
            var namesSubdir = SubDirectory.Select(x => x.Name);
            foreach (var service in services)
            {
                if (!namesSubdir.Contains(service))
                {
                    if (option == "info")
                    {
                        state.Append($"{service} is dont working. Please call {service} for next step!");                       
                    }
                    else
                    {
                        state.Append($"call {service}");
                    }
                    state.AppendLine();
                }
            }
            
            foreach (var subDir in SubDirectory.Where(x => services.Contains(x.Name)))
            {
                var files = subDir.GetFiles();

                switch (subDir.Name)
                {
                    case "Crauler":
                    case "Lemmiter":
                    case "TFIdf":
                        if(option == "command" && files.Length != 101)
                        {
                            state.Append($"call {subDir.Name}");
                            break;
                        }
                        state.Append(GetCurrentServiseState(subDir.Name, files.Length == 101));
                        break;
                    case "InvertionIndexer":
                        if (option == "command" && files.Length != 1)
                        {
                            state.Append($"call {subDir.Name}");
                            break;
                        }
                        state.Append(GetCurrentServiseState(subDir.Name, files.Length == 1));
                        break;
                }

                state.AppendLine();
            }
            return state.ToString();
        }

        private static string GetCurrentServiseState(string serviceName, bool isStatusOk)
        {
            return isStatusOk
                ? $"{serviceName} is called! See the next!"
                : $"{serviceName} is not called! Do this!";
        }
    }
}
