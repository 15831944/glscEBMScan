using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlscEBMProject
{
    class LogHelper {

        public enum LogType {
            Info,
            Error,
            Vacuum,
            Temperature
        }
        public enum LogLevel {
            Info,
            Error
        }

        public static string LogPath {
            get {
                return AppDomain.CurrentDomain.BaseDirectory + @"\log";
            }
        }
        public static void Info(string message, string caption = null, LogType logType = LogType.Info) {
            if (string.IsNullOrEmpty(message))
                return;
            var path = string.Format(@"\{0}\", logType.ToString());
            WriteLog(path, "Info ", message, "");
        }

        public static void Error(string message, string caption = null, LogType logType = LogType.Error) {
            if (string.IsNullOrEmpty(message))
                return;
            var path = string.Format(@"\{0}\", logType.ToString());
            WriteLog(path, "Error ", message, "");
        }
        public static void VacuumLog(string message, string caption = null, LogType logType = LogType.Vacuum) {
            if (string.IsNullOrEmpty(message))
                return;
            var path = string.Format(@"\{0}\", logType.ToString());
            WriteLog(path, "Vacuum ", message, "");
        }
        public static void TemperLog(string message, string caption = null, LogType logType = LogType.Temperature) {
            if (string.IsNullOrEmpty(message))
                return;
            var path = string.Format(@"\{0}\", logType.ToString());
            WriteLog(path, "Temperature ", message, "");
        }
        private static void WriteLog(string path, string prefix, string message, string str) {
            path = LogPath + path;
            var fileName = string.Format("{0}{1}.log", prefix, DateTime.Now.ToString("yyyyMMdd"));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (FileStream fs = new FileStream(path + fileName, FileMode.Append, FileAccess.Write,
                                                  FileShare.Write, 1024, FileOptions.Asynchronous)) {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("HH:mm:ss") + " " + message + " " + str + "\r\n");
                IAsyncResult writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                    (asyncResult) => {
                        var fStream = (FileStream)asyncResult.AsyncState;
                        fStream.EndWrite(asyncResult);
                    },

                    fs);
                fs.Close();
            }
        }
    }
}
