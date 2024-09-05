using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynFolders_C_
{

    //This class is responsible to send a log message to command line console and to append the same message to our log file \t
    //Only when a file as been Deleted,Created in SourceFolder and Updated
    class WriteinFile
    {
        public static void Log(string logFilePath, string message)
        {
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                string logMessage = $"{DateTime.Now}: {message}";
                sw.WriteLine(logMessage);
                Console.WriteLine(logMessage);
            }
        }
    }
}
