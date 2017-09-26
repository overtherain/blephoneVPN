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
        public static Type TAG = typeof(Log);
        public static string logname = null;
        private static string logFile = null;
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

        public static void CreateDirectory()
        {
            logFile = logname;
            debug(TAG, "CreateDirectory log file path is : " + logFile);
            if (logFile != null)
            {
                DirectoryInfo directoryInfo = Directory.GetParent(logFile);
                if (!directoryInfo.Exists)
                {
                    debug(TAG, "Create log directory");
                    directoryInfo.Create();
                }
            }
            else
            {
                debug(TAG, "logFile directory is null!");
            }
        }

        public static void debug(Type t, string info)
        {
            try
            {
#if DEBUG
                Console.WriteLine(info);
#endif
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
                Console.WriteLine("Create log file error : " + ex);
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

        public static void console(string msg)
        { 
#if DEBUG
            Console.WriteLine(msg);
#endif
        }
    }
}
