using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;

namespace MRK
{
    public class Logger
    {
        private static bool _hasConsole = false;

        public static void Initialize()
        {
            // Try and alloc console
            _hasConsole = Natives.AllocConsole();
            if (_hasConsole)
            {
                // Redirect to new console handle
                var writer = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                };
                Console.SetOut(writer);
            }

            Log($"Initialized logger (hasConsole={_hasConsole})");
        }

        public static void Log(string fmt, params object[] args)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var message = $"[MRK] [{timestamp}] {string.Format(fmt, args)}";
            Console.WriteLine(message);
            Debug.Log(message);
        }

        [DoesNotReturn]
        public static void Throw(string fmt, params object[] args)
        {
            var message = $"[MRK] {string.Format(fmt, args)}";
            Console.WriteLine(message);
            Debug.LogError(message);
            throw new Exception(message);
        }
    }
}
