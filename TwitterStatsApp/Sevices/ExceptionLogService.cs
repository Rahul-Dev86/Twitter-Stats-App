using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Sevices
{
    public class ExceptionLogService : IExceptionLogService
    {
        public void LogException(Exception ex)
        {
            String ErrorlineNo, Errormsg, Extype, ErrorLocation;
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            Extype = ex.GetType().ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                string filepath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Log\\";

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + Extype + line + "Error Location :" + " " + ErrorLocation;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");                  
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");                    
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
