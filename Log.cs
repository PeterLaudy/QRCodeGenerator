// <copyright file="Log.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

namespace QRCodeGenerator
{
    public  enum LogLevel
    {
        DEBUG,
        DETAILED,
        INFO,
        WARNING,
        ERROR,
        CRITICAL,
        OFF
    }

    public static class Log
    {
        private static Dictionary<LogLevel, string> LogLevel2String = new() {
            { LogLevel.DEBUG, "DBG" },
            { LogLevel.DETAILED, "DET" },
            { LogLevel.INFO, "INF" },
            { LogLevel.WARNING, "WRN" },
            { LogLevel.ERROR, "ERR" },
            { LogLevel.CRITICAL, "CRT" }
        };

        public static LogLevel Level { get; set; } = LogLevel.ERROR;

        public static void LogMessage([CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.DEBUG, "-", file!, line!.Value, member!);
        }

        public static void LogDebug(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.DEBUG, msg, file!, line!.Value, member!);
        }

        public static void LogDetailed(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.DETAILED, msg, file!, line!.Value, member!);
        }

        public static void LogInfo(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.INFO, msg, file!, line!.Value, member!);
        }

        public static void LogWarning(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.WARNING, msg, file!, line!.Value, member!);
        }

        public static void LogError(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.ERROR, msg, file!, line!.Value, member!);
        }

        public static void LogCritical(string msg, [CallerFilePath] string? file = null, [CallerLineNumber] int? line = null, [CallerMemberName] string? member = null)
        {
            LogLine(LogLevel.CRITICAL, msg, file!, line!.Value, member!);
        }

        private static void LogLine(LogLevel level, string msg, string file, int line, string member)
        {
            if ((Level != LogLevel.OFF) && (level >= Level))
            {
                Console.Out.WriteLine($"{LogLevel2String[level]} {Path.GetFileName(file)} Line {line} {member}: {msg}");
            }
        }
    }
}