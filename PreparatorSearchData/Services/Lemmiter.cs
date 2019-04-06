using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace PreparatorSearchData.Services
{
    public static class Lemmiter
    {
        
        public static void LemmitedSite()
        { 
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\Kirill\AppData\Local\Programs\Python\Python37\python.exe",              
                Arguments = @"C:\Users\Kirill\source\repos\PreparatorSearchData\PreparatorSearchData\Lemmiter.py"
            };
            using (Process process = Process.Start(start))
            {
                if (process.HasExited)
                {
                    CallBackProcess();
                }
            }    
        }

        private static void CallBackProcess()
        {
            Console.WriteLine("Леммитизация прошла успешно! Словари готовы");
        }
    }
}
