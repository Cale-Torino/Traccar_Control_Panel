using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Traccar_Control_Panel_WPF
{
    class LoggerClass
    {
        public static class Logger
        {
            //Create the log file
            private static string LogFile = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Traccar_Control_Panel_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log";

            public static void WriteLine(string txt)
            {
                try
                {
                    //Write to the log file
                    File.AppendAllText(LogFile, "[" + DateTime.Now.ToString() + "] : " + txt + "\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Could Not Append Text To Log File", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            public static void DeleteLog()
            {
                try
                {
                    //delete the log file
                    File.Delete(LogFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Could Not Delete Log File", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
