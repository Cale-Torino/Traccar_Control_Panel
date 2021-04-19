using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using static Traccar_Control_Panel.LoggerClass;

namespace Traccar_Control_Panel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void RefreshServices()
        {
            try
            {
                richTextBox2.Clear();
                ServiceController[] scServices;
                scServices = ServiceController.GetServices();

                // Display the list of services currently running on this computer.
                Console.WriteLine("Services running on the local computer:");
                foreach (ServiceController scTemp in scServices)
                {
                    if (scTemp.Status == ServiceControllerStatus.Running)
                    {
                        // Write the service name and the display name
                        // for each running service.
                        richTextBox2.AppendText("Service : " + scTemp.ServiceName + Environment.NewLine + "Display name: " + scTemp.DisplayName + Environment.NewLine + "-----------------" + Environment.NewLine);
                    }
                }
                toolStripProgressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Refresh Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CheckServices()
        {
            try
            {
                //Check that the service is still running
                string _CheckServiceStatus = ServiceClass.CheckServiceStatus("traccar");
                if (_CheckServiceStatus =="traccar is Stopped")
                {
                    textBox2.Text = "[" + DateTime.Now.ToString() + "] : " + _CheckServiceStatus;
                    textBox2.BackColor = Color.Red;
                    richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] : " + "traccar is Stopped");
                    Logger.WriteLine(" ***- ALERT TRACCAR STOPPED!!! -*** ");
                }
                else if (_CheckServiceStatus == "traccar is Running")
                {
                    //...
                    textBox2.Text = "[" + DateTime.Now.ToString() + "] : " + _CheckServiceStatus;
                    textBox2.BackColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Check Services Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void CreateFolder()
        {
            try
            {
                //Create the logs folder
                string path = Application.StartupPath;
                Directory.CreateDirectory(path + "\\Logs");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Create Folder Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void Ini()
        {
            try
            {
                //Initiate the application
                CreateFolder();
                bool _Service = ServiceClass.IsServiceInstalled("traccar");
                if (_Service == true)
                {
                    menuStrip1.Enabled = true;
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    toolStripStatusLabel3.Text = "YES";
                    toolStripStatusLabel3.Font = new Font(toolStripStatusLabel3.Font, FontStyle.Bold);
                    toolStripStatusLabel3.ForeColor = Color.Green;
                    //int _procID = Process.GetProcessesByName(textBox1.Text)[0].Id;
                    //richTextBox1.AppendText("PID: " + _procID + Environment.NewLine);
                    textBox1.Text = "traccar";
                    TimerStart();
                    CheckServices();
                    RefreshServices();                   
                }
                else if (_Service == false)
                {
                    menuStrip1.Enabled = false;
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;           
                    toolStripStatusLabel3.Text = "NO, Please Install the Traccar Service";
                    toolStripStatusLabel3.Font = new Font(toolStripStatusLabel3.Font, FontStyle.Bold);
                    toolStripStatusLabel3.ForeColor = Color.Red;
                }
                Logger.WriteLine(" *** Service Installed Status :" + toolStripStatusLabel3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ini Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Form load
            Ini();
            statusStrip1.Text = "Ready";
            Logger.WriteLine(" ***- APPLICATION STARTED -*** ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Start the service
            toolStripProgressBar1.Value = 50;
            Logger.WriteLine(" *** Start Service Clicked *** ");
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Stop the service
            toolStripProgressBar1.Value = 50;
            Logger.WriteLine(" *** Stop Service Clicked *** ");
            backgroundWorker2.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Restart the service
            toolStripProgressBar1.Value = 50;
            Logger.WriteLine(" *** Restart Service Clicked *** ");
            backgroundWorker3.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Refresh all available services
            toolStripProgressBar1.Value = 50;
            RefreshServices();
            richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] : " + "Services Refreshed" + Environment.NewLine);
            Logger.WriteLine(" *** Refresh Services Clicked *** ");
        }
        private void Reallyexit()
        {
            //ask if you really want to exit YES/NO
            string _text = "Do you really want to exit the application?";
            string _caption = "Exit Application?";
            var selectedOption = MessageBox.Show(_text, _caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                try
                {
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Exit Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    return;
                }

            }
            else if (selectedOption == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Tool strip menu exit clicked
            Logger.WriteLine(" ***Exit Clicked*** ");
            Reallyexit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //About button clicked
            Logger.WriteLine(" ***About Box Clicked*** ");
            Form f2 = new Form2();
            f2.ShowDialog();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Font button clicked. choose font
            Logger.WriteLine(" ***Font Box Clicked*** ");
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = richTextBox1.Font = new Font(fontDialog1.Font, fontDialog1.Font.Style);
                richTextBox1.ForeColor = fontDialog1.Color;
                richTextBox2.Font = richTextBox2.Font = new Font(fontDialog1.Font, fontDialog1.Font.Style);
                richTextBox2.ForeColor = fontDialog1.Color;
            }
        }
        string path = "";
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Save to a file button clicked
            Logger.WriteLine(" ***Save Clicked*** ");
            if (path != "")
            {
                File.WriteAllText(path + "_Traccar_Service", richTextBox1.Text);
                File.WriteAllText(path + "_Installed_Services", richTextBox2.Text);
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Save as file button clicked
            Logger.WriteLine(" ***Save As Clicked*** ");
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(path = saveFileDialog1.FileName + "_Traccar_Service", richTextBox1.Text);
                File.WriteAllText(path = saveFileDialog1.FileName + "_Installed_Services", richTextBox2.Text);
            }
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Right click copy all button clicked
            Logger.WriteLine(" ***Copy All Clicked richTextBox1*** ");
            Clipboard.Clear();
            richTextBox1.SelectAll();
            richTextBox1.Copy();
        }

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Right click copy selected button clicked
            Logger.WriteLine(" ***Copy Selected Clicked richTextBox1*** ");
            Clipboard.Clear();
            try
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(" ***Copy Selected Error*** " + ex);
                return;
            }
        }

        private void cutAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Right click cut all button clicked
            Logger.WriteLine(" ***Cut All Clicked richTextBox1*** ");
            Clipboard.Clear();
            richTextBox1.SelectAll();
            richTextBox1.Copy();
            richTextBox1.Clear();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Right click clear all button clicked
            Logger.WriteLine(" ***Clear All Clicked richTextBox1*** ");
            Clipboard.Clear();
            richTextBox1.SelectAll();
            richTextBox1.Clear();
        }

        private void copyAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Right click copy all button clicked
            Logger.WriteLine(" ***Copy All Clicked richTextBox2*** ");
            Clipboard.Clear();
            richTextBox2.SelectAll();
            richTextBox2.Copy();
        }

        private void copySelectedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Right click copy selected button clicked
            Logger.WriteLine(" ***Cut All Clicked richTextBox2*** ");
            Clipboard.Clear();
            richTextBox2.SelectAll();
            richTextBox2.Copy();
            richTextBox2.Clear();
        }

        private void cutAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Right click cut all button clicked
            Logger.WriteLine(" ***Clear All Clicked richTextBox2*** ");
            Clipboard.Clear();
            richTextBox2.SelectAll();
            richTextBox2.Clear();
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Right click clear all button clicked
            Logger.WriteLine(" ***Clear All Clicked richTextBox2*** ");
            Clipboard.Clear();
            richTextBox2.SelectAll();
            richTextBox2.Clear();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Start service background worker
                string _StartService = ServiceClass.StartService(textBox1.Text, 9000);
                Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = 100));
                Invoke((MethodInvoker)(() => richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] : " + _StartService + Environment.NewLine)));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Start Service Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Stop service background worker
                string _StopService = ServiceClass.StopService(textBox1.Text, 9000);
                Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = 100));
                Invoke((MethodInvoker)(() => richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] : " + _StopService + Environment.NewLine)));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Stop Service Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Restart service background worker
                string _RestartService = ServiceClass.RestartService(textBox1.Text, 9000);
                Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = 100));
                Invoke((MethodInvoker)(() => richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] : " + _RestartService + Environment.NewLine)));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Restart Service Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
        }
        private Timer t;
        private void TimerStart()
        {
            //Timer to check service is running every 15 seconds
            Timer t = new Timer();
            t.Interval = 15000; // 15seconds
            t.Tick += new EventHandler(timer1_Tick);
            t.Start();
        }
        private void TimerStop()
        {
            //Stop timer
            t.Stop();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Call this method every time the timer elapses
            CheckServices();
        }
    }
}
