using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Serilog;

namespace ClassLibrary.Common
{
    public static class LogManager
    {
        private static ILogger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.ColoredConsole()
            .WriteTo.RollingFile(@"Log\Log-{Date}.txt")
            .CreateLogger();

        public static ILogger Logger { get { return logger; } }

        public static void WriteNotFound(string name, string nameHeader, string numberSF)
        {
            Logger.Warning("SFNumber: {numberSF}, {nameHeader} not found value: {name}", numberSF, nameHeader, name);
        }
    }
}
