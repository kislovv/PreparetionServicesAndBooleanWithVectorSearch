﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace PreparatorSearchData.Services
{
    public class Lemmiter
    {
        public void LemmitedWords()
        {
           
        }

        public void LemmitedSite()
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\Kirill\AppData\Local\Programs\Python\Python37\python.exe",              
                Arguments = @"C:\Users\Kirill\source\repos\PreparatorSearchData\PreparatorSearchData\Lemmiter.py"
            };
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    Console.WriteLine(result);
                }
            }

            //ScriptEngine engine = Python.CreateEngine();
            //engine.ExecuteFile("Lemmiter.py");
            //Console.Read();
        }
    }
}