
using System;
using System.IO;
using System.Text;
namespace EShoppingAPI.Helpers
{
    public sealed class Log  : ILog
    {
        //Private Constructor to Restrict Class Instantiation from outside the Log class
        private Log()
        {

        }
        //Creating Log Instance using Eager Loading
        private static  Log LogInstance = null;
        //Returning the Singleton LogInstance
        //This Method is Thread Safe as it uses Eager Loading
        public static Log GetInstance()
        {
            if (LogInstance == null)
            {
                LogInstance = new Log();
            }
            return LogInstance;
        }
        //This Method Log the Exception Details in a Log File
        public void LogException(string message)
        {
            //Create the Dynamic File Name
            string fileName = $"Exception_{DateTime.Now.ToString("mm-dd-yyyy")}.log";
            //Create the Path where you want to Create the Log file
            string logFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}";
            //Build the String Object using StringBuilder for a Better Performance
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            //Write the StringBuilder Message into the Log File Path using StreamWriter Object
            if(!File.Exists(logFilePath))
            {
                File.Create(fileName);
            }
                File.WriteAllText(logFilePath,sb.ToString());
        }
    }
}
