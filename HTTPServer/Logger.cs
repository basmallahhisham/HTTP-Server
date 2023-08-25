using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static string  filepath = "log.txt";
        static StreamWriter sr = new StreamWriter(filepath);
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
          //  

                sr.WriteLine("Date :" + DateTime.Now.ToString() + "<br/>" + "Message :" + ex.Message);
            
        }
    }
}
