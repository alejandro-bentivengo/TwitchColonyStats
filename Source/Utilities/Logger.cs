using System;
using Verse;

namespace Colonystats.Utilities
{
    static class Logger
    {
        public static readonly string UNKNOWN = "Well, this is awkward. An unknown error just occured... Report it to me if you would be so kind!";

        public static void Log(string message, LogType logType)
        {
            Verse.Log.Message("<color=" + logType.getLogColor() + ">[TwitchColonyStats]</color> " + DateTime.Now.ToString() + " - " + logType.getPretext() + " - " + message, true);
        }
    }

    public sealed class LogType
    {
        public static readonly LogType INFO = new LogType("#53b1e0", "INFO");
        public static readonly LogType WARN = new LogType("#f0d74d", "WARN");
        public static readonly LogType ERROR = new LogType("#ed4d2d", "ERROR");

        private readonly string LOG_COLOR;
        private readonly string PRETEXT;

        LogType(string logColor, string pretext)
        {
            this.LOG_COLOR = logColor;
            this.PRETEXT = pretext;
        }

        public string getLogColor()
        {
            return LOG_COLOR;
        }

        public string getPretext()
        {
            return PRETEXT;
        }
    }
}
