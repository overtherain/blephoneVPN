using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace blephoneVPN.Util
{
    public class Log
    {
        private static string logFile;
        private static StreamWriter writer;
        private static FileStream fileStream = null;

        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void Error(Type t, string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("[" + t + "]:\n" + info, se);
            }
        }

        public static void CreateDirectory(string infoPath)
        {
            logFile = infoPath;
            Console.WriteLine("CreateDirectory log file path is : {0}", logFile);
            DirectoryInfo directoryInfo = Directory.GetParent(logFile);
            if (!directoryInfo.Exists)
            {
                Console.WriteLine("Create log directory");
                directoryInfo.Create();
            }
        }

        public static void debug(Type t, string info)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFile);
                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Create log file");
                    fileStream = fileInfo.Create();
                    writer = new StreamWriter(fileStream);
                }
                else
                {
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(fileStream);
                }
                writer.WriteLine(t.ToString() + " - " + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss") + " : " + info);
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.ToString());
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }
    }
}
