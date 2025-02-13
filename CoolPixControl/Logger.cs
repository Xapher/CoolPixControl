using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolPixControl
{
    public static class Logger
    {
        static List<string> logs = new List<string>();

        public static void log(string message)
        {
            if (Program.loggingEnabled())
            {
                logs.Add(message);
            }
        }

        public static void appendLogToLine(string m)
        {
            if (Program.loggingEnabled())
            {
                logs[logs.Count - 1] += m;
            }
        }

        public static void saveLog()
        {
            File.WriteAllLines(Program.getLogPath(), logs.ToArray());
        }

    }
}
