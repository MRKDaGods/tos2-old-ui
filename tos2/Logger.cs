using System;
using System.IO;
using UnityEngine;

namespace MRK
{
    public class Logger
    {
        public static void Initialize()
        {
            // Try and alloc console
            Natives.AllocConsole();

            // Redirect to new console handle
            var writer = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(writer);

            Log("Initialized logger");
        }

        public static void Log(string fmt, params object[] args)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var message = $"[MRK] [{timestamp}] {string.Format(fmt, args)}";
            Console.WriteLine(message);
            Debug.Log(message);
        }
    }
}
