using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace terra
{
    public static class Logging
    {
        // Function   : LogError
        // Description: Logs an error to the database.
        // Paramaters : string: The error message to log to the database.
        // Returns    : void
        public static void LogError(string message)
        {
            try
            {
                //Help from http://www.techdreams.org/microsoft/c-how-to-get-calling-class-name-method-name-using-stackframe-class/5815-20110507
                DAL dal = new DAL();
                if (dal.Init())
                {
                    StackFrame frame = new StackFrame(1, true);
                    string method = frame.GetMethod().Name;
                    string className = frame.GetMethod().DeclaringType.Name;
                    ErrorObject errorObject = new ErrorObject(method, message,className);
                    errorObject.Init(IDatabase.CommandType.Log);
                    errorObject.SetInsertVariables();
                    dal.NonQuery(errorObject);
                }
            }
            catch (Exception e)
            {

            }

        }

        // Function   : LogActivity
        // Description: Logs an activity to the database.
        // Paramaters : The error message to log to the database.
        // Returns    : void
        public static void LogActivity(string message)
        {
            try
            {
                //Help from http://www.techdreams.org/microsoft/c-how-to-get-calling-class-name-method-name-using-stackframe-class/5815-20110507
                DAL dal = new DAL();
                if (dal.Init())
                {
                    StackFrame frame = new StackFrame(1, true);
                    string method = frame.GetMethod().Name;
                    string className = frame.GetMethod().DeclaringType.Name;
                    Activity activity = new Activity(method, message, className);
                    activity.Init(IDatabase.CommandType.Log);
                    activity.SetInsertVariables();
                    dal.NonQuery(activity);
                }
            }
            catch (Exception e)
            {

            }

        }
    }
}