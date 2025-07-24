
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DebugLogger;

public static class Logger
{
    private static StreamWriter? _output;
    private static bool _isInit;
    public static bool IsDisable { get; set; }

    public static string DefaultFolder => @"D:\temp\kslogs";

    private static readonly object s_lock = new object();

    public static void Log(string message, [CallerFilePath] string filepath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0
    )
    {
        if (IsDisable)
        {
            return;
        }
		
		// if (!Debugger.IsAttached)
        // {
        //     return;
        // }

        if (!_isInit)
        {
            //return xunitRunner if call was from unittest
            var fullname = Assembly.GetEntryAssembly()?.FullName;
            if (fullname == null)
            {
                fullname = Path.GetRandomFileName();
            }

            int i = fullname.IndexOf(',');
            var name = fullname.Substring(0, i != -1 ? i : fullname.Length);
            // it seems better to keep all attempts
            var file = Path.Combine(DefaultFolder,
                $"{name}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss", DateTimeFormatInfo.InvariantInfo)}.md");

            _output = new StreamWriter(file, false) { AutoFlush = true };
            _output.WriteLine("Start:" + DateTime.Now);
            //Console.SetOut(_output);

            _isInit = true;
        }

    
        lock (s_lock)
        {

            var stamp = DateTime.Now.ToString("ss:FFFF", DateTimeFormatInfo.InvariantInfo);
            _output?.WriteLine(
                $"1. {stamp} {message}  --  [{Path.GetFileName(filepath)}]({filepath}) {memberName}:{sourceLineNumber}");
        }
    }

   
}
