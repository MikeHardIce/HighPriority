using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace HighPriority
{
    class Program
    {
        static void Main(string[] args)
        {

            var timer = new Timer(LoadAndSetProcessPriorities, null, 0, 30000);

            while(true)
            {
                string exit = Console.ReadLine();

                if(exit == "exit")
                {
                    break;
                }
            }
            
        }

        protected static void LoadAndSetProcessPriorities (Object o)
        {
            var processes = ReadPriorityList();

            foreach(var process in processes)
            {
                SetProcessToHighPriority(process);
            }
        }

        private static void SetProcessToHighPriority((string name, ProcessPriorityClass priority) process)
        {
            var processes = Process.GetProcessesByName(process.name);

            foreach(var currProcess in processes)
            {
                if(!currProcess.HasExited && currProcess.PriorityClass != process.priority)
                {
                    currProcess.PriorityClass = process.priority;

                    //log.Add((currProcess.ProcessName, process.priority.ToString()));

                    Console.WriteLine($"{currProcess.ProcessName} set to {currProcess.PriorityClass}");
                }
            }   
        }

        private static IList<(string name, ProcessPriorityClass priority)> ReadPriorityList ()
        {
            var priorities  = new List<(string name, ProcessPriorityClass priority)>();
            var path        = Path.Combine(Directory.GetCurrentDirectory(), "priority.ini");

            if(File.Exists(path))
            {
                var lines = File.ReadLines(path);

                foreach(var line in lines)
                {
                    if(TryParsePriority(line, out (string name, ProcessPriorityClass priority) prio))
                    {
                        priorities.Add(prio);
                    }
                }
                

            }

            return priorities;
        }

        private static bool TryParsePriority ( string line, out (string name, ProcessPriorityClass priority) priority)
        {
            var success     = false;
            priority        = ("", ProcessPriorityClass.Normal);
            var prioParts   = line.Split("=");

            if(prioParts.Length > 1 && TryParseProcessPriority(prioParts[1], out ProcessPriorityClass prio))
            {
                priority = (prioParts[0], prio);

                success = true;
            }

            return success;
        } 

        private static bool TryParseProcessPriority (string name, out ProcessPriorityClass priority)
        {
            var success = true;
            priority    = ProcessPriorityClass.Normal;

            switch(name.ToLower())
            {
                case "high":        priority = ProcessPriorityClass.High; break;
                case "abovenormal": priority = ProcessPriorityClass.AboveNormal; break;
                case "belownormal": priority = ProcessPriorityClass.BelowNormal; break;
                case "idle":        priority = ProcessPriorityClass.Idle; break;
                case "realtime":    priority = ProcessPriorityClass.RealTime; break;
                case "normal": break;
                default: success = false; break;
            }

            return success;
        }


    }
}
