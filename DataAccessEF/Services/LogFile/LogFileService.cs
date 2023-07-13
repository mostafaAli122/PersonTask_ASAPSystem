using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DataAccessEF.Services.LogFile
{
    public class LogFileService
    {
        private static readonly object LockLogObj = new object();

        private readonly IConfiguration Configuration;

        public LogFileService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void LogException(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_log.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogValidation(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_validations.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogSms(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_Sms.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogKashierIntegration(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_KashierIntegration.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogTrace(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_Trace.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogSystemNotification(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_SystemNotifi.txt";

            WriteToTextFile(ex, fileName);
        }


        public void LogCustomerNotification(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_CustomerNotifi.txt";

            WriteToTextFile(ex, fileName);
        }


        public void LogSurveyorNotification(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_SurveyorNotifi.txt";

            WriteToTextFile(ex, fileName);
        }

        public void LogUserNotification(string ex)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = currentDate + "_UserNotifi.txt";

            WriteToTextFile(ex, fileName);
        }


        #region Private

        private void WriteToTextFile(string ex_Txt, string fileName)
        {

            if (!Directory.Exists(this.LogsPath))
                Directory.CreateDirectory(this.LogsPath);

            lock (LockLogObj)
            {
                File.AppendAllText(Path.Combine(this.LogsPath, fileName), DateTime.Now.ToString() + Environment.NewLine + ex_Txt + Environment.NewLine + Environment.NewLine);
            }
        }

        private string LogsPath
        {
            get
            {
                return this.Configuration["LogsPath"];
            }
        }

        #endregion

    }
}
