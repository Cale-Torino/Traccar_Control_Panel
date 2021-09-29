using System;
using System.IO;
using System.Windows.Forms;

namespace Traccar_Control_Panel
{
    class LoggerClass
    {
        public static class Logger
        {
            //Create file
            private static string LogFile = Application.StartupPath + @"\Logs\Traccar_Control_Panel_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log";

            public static void WriteLine(string txt)
            {
                try
                {
                    //Append text to file and save it
                    File.AppendAllText(LogFile, "[" + DateTime.Now.ToString() + "] : " + txt + "\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Could Not Append Text To Log File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            public static void DeleteLog()
            {
                try
                {
                    //Delete log file
                    File.Delete(LogFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Could Not Delete Log File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
