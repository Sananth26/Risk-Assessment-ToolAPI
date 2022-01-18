using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class ErrorLogDataAccess
    {
        public static void AddErrorLogs(string errorDescription, string moduleName, string MethodName, string userName)
        {
            using (dbURSContext db = new dbURSContext())
            {
                var errorLog = new ErrorLogsWeb();
                errorLog.ErrorDescription = errorDescription;
                errorLog.MethodName = MethodName;
                 errorLog.CreatedBy = userName;
                errorLog.ModuleName = moduleName;
                errorLog.CreatedDate = DateTime.Now;
               // db.ErrorLogsWeb.Add(errorLog);
               // db.SaveChanges(); 
                 
            }

        }

        internal static void AddErrorLogs(string errorMessage, object currentApplicationId, object currentUserName)
        {
            throw new NotImplementedException();
        }

        public static void WriteToErrorLog(string errorMessage, int applicationID, string moduleName)

        {
            if (!(System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + "\\Errors\\")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\Errors\\");
            }
            FileStream fs = new FileStream(System.IO.Directory.GetCurrentDirectory() + "\\Errors\\Errorlogs.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter s = new StreamWriter(fs);
            s.Close();
            fs.Close();
            FileStream fs1 = new FileStream(System.IO.Directory.GetCurrentDirectory() + "\\Errors\\Errorlogs.txt", FileMode.Append, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(fs1);
            s1.Write(Environment.NewLine + "Module Name: " + moduleName + " " + "Error Message: " + errorMessage + " " + "Application ID:" + " " + applicationID + " " + "Date/Time: " + DateTime.Now.ToString());
            s1.Close();
            fs1.Close();
        }

    }
}
