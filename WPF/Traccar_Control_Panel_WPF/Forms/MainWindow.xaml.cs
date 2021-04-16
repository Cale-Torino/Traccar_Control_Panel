using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Traccar_Control_Panel_WPF.EmailVarClass;
using static Traccar_Control_Panel_WPF.LoggerClass;
//https://stackoverflow.com/questions/5483565/how-to-use-wpf-background-worker
namespace Traccar_Control_Panel_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker1 = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        private readonly BackgroundWorker worker3 = new BackgroundWorker();
        private readonly BackgroundWorker worker4 = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            worker1.DoWork += worker1_DoWork;
            worker2.DoWork += worker2_DoWork;
            worker3.DoWork += worker3_DoWork;          
            worker4.DoWork += (obj, e) => worker4_DoWork(_Subject, _body);
        }
        private string _Subject = "";
        private string _body = "";
        private void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // work1
            try
            {
                //start
                Dispatcher.BeginInvoke(new Action(delegate {
                    string _StartService = ServiceClass.StartService(textbox.Text, 9000);
                    progressbar.Value = 100;
                    richtextbox_traccar_service.AppendText("[" + DateTime.Now.ToString() + "] : " + _StartService + Environment.NewLine);
                    start_button.IsEnabled = true;
                }));
        }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    start_button.IsEnabled = true;
                MessageBox.Show(ex.ToString(), "Start Service Error", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
                }));
            }
        }

        private void worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            // work2
            try
            {
                //stop
                Dispatcher.BeginInvoke(new Action(delegate {
                    string _StopService = ServiceClass.StopService(textbox.Text, 9000);
                    progressbar.Value = 100;
                    richtextbox_traccar_service.AppendText("[" + DateTime.Now.ToString() + "] : " + _StopService + Environment.NewLine);
                    stop_button.IsEnabled = true;
                }));
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    stop_button.IsEnabled = true;
                MessageBox.Show(ex.ToString(), "Stop Service Error", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
                }));
            }
        }

        private void worker3_DoWork(object sender, DoWorkEventArgs e)
        {
            // restart
            try
            {
                //restart Dispatcher.BeginInvoke(new Action(delegate{ progressbar.Value = 100;}));
                Dispatcher.BeginInvoke(new Action(delegate {
                    string _RestartService = ServiceClass.RestartService(textbox.Text, 30000);
                    progressbar.Value = 100;
                    richtextbox_traccar_service.AppendText("[" + DateTime.Now.ToString() + "] : " + _RestartService + Environment.NewLine);
                    restart_button.IsEnabled = true;
                }));
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    restart_button.IsEnabled = true;
                MessageBox.Show(ex.ToString(), "Restart Service Error", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
                }));
            }
        }

        private void worker4_DoWork(string _Subject, string _body)
        {
            // work4
            try
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    MailMessage mail = new MailMessage();
                     SmtpClient SmtpServer = new SmtpClient(EmailVars._SmtpServert);//smtp.techrad.co.za

                    mail.From = new MailAddress(EmailVars._mailFrom);
                    mail.To.Add(EmailVars._mailTo);
                    mail.Subject = _Subject;
                    mail.Body = _body;

                    SmtpServer.Port = 587;//port
                    SmtpServer.Credentials = new NetworkCredential(EmailVars._userName, EmailVars._password);//details
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Logger.WriteLine(" *** Mail Sent! : send success!");
                    //MessageBox.Show("Normal mail Sent", "Mail Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            }
            catch (Exception ex)
            {
                Logger.WriteLine(" *** Mail Error! : " + ex.Message);
                //MessageBox.Show(ex.ToString(), "Mail Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshServices()
        {
            try
            {
                richtextbox_installed_services.Document.Blocks.Clear();
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
                        richtextbox_installed_services.AppendText("Service : " + scTemp.ServiceName + Environment.NewLine + "Display name: " + scTemp.DisplayName + Environment.NewLine + "-----------------" + Environment.NewLine);
                    }
                }
                progressbar.Value = 100;
                refresh_button.IsEnabled = true;
                MessageBox.Show("Services Refreshed", " Please Note :) ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                refresh_button.IsEnabled = true;
                MessageBox.Show(ex.ToString(), "Refresh Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void CheckServices()
        {
            try
            {
                string _CheckServiceStatus = ServiceClass.CheckServiceStatus("traccar");
                if (_CheckServiceStatus == "traccar is Stopped")
                {
                    status_T.Text = "[" + DateTime.Now.ToString() + "] : " + _CheckServiceStatus;
                    status_T.Background = Brushes.Red;
                    richtextbox_traccar_service.AppendText("[" + DateTime.Now.ToString() + "] : " + "traccar is Stopped" + Environment.NewLine);
                    worker4_DoWork("[" + DateTime.Now.ToString() + "] : TRACCAR STOPPED!", "ALERT TRACCAR STOPPED!!!");
                    Logger.WriteLine(" ***- ALERT TRACCAR STOPPED!!! -*** ");
                }
                else if (_CheckServiceStatus == "traccar is Running")
                {
                    //...
                    status_T.Text = "[" + DateTime.Now.ToString() + "] : " + _CheckServiceStatus;
                    status_T.Background = Brushes.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Check Services Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            //start
            start_button.IsEnabled = false;
            progressbar.Value = 50;
            Logger.WriteLine(" *** Start Service Clicked *** ");
            worker1.RunWorkerAsync();
        }
        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            //stop
            stop_button.IsEnabled = false;
            progressbar.Value = 50;
            Logger.WriteLine(" *** Stop Service Clicked *** ");
            worker2.RunWorkerAsync();
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            //restart
            restart_button.IsEnabled = false;
            progressbar.Value = 50;
            Logger.WriteLine(" *** Restart Service Clicked *** ");
            worker3.RunWorkerAsync();
        }
        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            //refresh all
            refresh_button.IsEnabled = false;
            progressbar.Value = 50;
            RefreshServices();
            richtextbox_installed_services.AppendText("[" + DateTime.Now.ToString() + "] : " + "Services Refreshed" + Environment.NewLine);
            Logger.WriteLine(" *** Refresh Services Clicked *** ");
        }
        private void save_menuitem_Click(object sender, RoutedEventArgs e)
        {
            //save
            Logger.WriteLine(" ***Save As Clicked*** ");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string richtextbox_installed = new TextRange(richtextbox_installed_services.Document.ContentStart, richtextbox_installed_services.Document.ContentEnd).Text;
                File.WriteAllText(saveFileDialog.FileName + "_richtextbox_installed_services", richtextbox_installed);

                string richtextbox_traccar = new TextRange(richtextbox_traccar_service.Document.ContentStart, richtextbox_traccar_service.Document.ContentEnd).Text;
                File.WriteAllText(saveFileDialog.FileName + "_richtextbox_traccar_service", richtextbox_traccar);
            }
        }

        private void save_as_menuitem_Click(object sender, RoutedEventArgs e)
        {
            //save as
            Logger.WriteLine(" ***Save Clicked*** ");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true) 
            {
                string richtextbox_installed = new TextRange(richtextbox_installed_services.Document.ContentStart, richtextbox_installed_services.Document.ContentEnd).Text;
                File.WriteAllText(saveFileDialog.FileName+ "_richtextbox_installed_services", richtextbox_installed);

                string richtextbox_traccar = new TextRange(richtextbox_traccar_service.Document.ContentStart, richtextbox_traccar_service.Document.ContentEnd).Text;
                File.WriteAllText(saveFileDialog.FileName+ "_richtextbox_traccar_service", richtextbox_traccar);
            }
        }

        private void about_menuitem_Click(object sender, RoutedEventArgs e)
        {
            //about
            Logger.WriteLine(" *** About Box Clicked *** ");
            About_Window w1 = new About_Window();
            w1.ShowDialog();
        }
        private void Reallyexit()
        {
            //exit YES/NO
            string _text = "Do you really want to exit the application?";
            string _caption = "Exit Application?";
            MessageBoxResult selectedOption = MessageBox.Show(_text, _caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (selectedOption == MessageBoxResult.Yes)
            {
                try
                {
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Exit Error", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }

            }
            else if (selectedOption == MessageBoxResult.No)
            {
                //Do nothing
            }
        }
        private void exit_menuitem_Click(object sender, RoutedEventArgs e)
        {
            //exit
            Logger.WriteLine(" ***Exit Clicked*** ");
            Reallyexit();
        }
        private void CreateFolder()
        {
            try
            {
                //CreatFolder
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Directory.CreateDirectory(path + "\\Logs");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Create Folder Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void Ini()
        {
            try
            {
                CreateFolder();
                bool _Service = ServiceClass.IsServiceInstalled("traccar");
                if (_Service == false)
                {
                    menu_strip.IsEnabled = true;
                    controls_groupbox.IsEnabled = true;
                    traccar_service_groupbox.IsEnabled = true;
                    installed_services_groupbox.IsEnabled = true;
                    start_button.IsEnabled = true;
                    stop_button.IsEnabled = true;
                    refresh_button.IsEnabled = true;
                    restart_button.IsEnabled = true;
                    textbox.IsEnabled = true;
                    save_menuitem.IsEnabled = true;
                    save_as_menuitem.IsEnabled = true;
                    menu_drop.IsEnabled = true;
                    status_yes_no.Text = "YES";
                    status_yes_no.FontStyle = FontStyles.Italic;
                    status_yes_no.Foreground = Brushes.Green;
                    textbox.Text = "traccar";
                    TimerStart();
                    CheckServices();
                    RefreshServices();
                }
                else if (_Service == true)
                {
                    menu_strip.IsEnabled = false;
                    controls_groupbox.IsEnabled = false;
                    traccar_service_groupbox.IsEnabled = false;
                    installed_services_groupbox.IsEnabled = false;
                    start_button.IsEnabled = false;
                    stop_button.IsEnabled = false;
                    refresh_button.IsEnabled = false;
                    restart_button.IsEnabled = false;
                    textbox.IsEnabled = false;
                    save_menuitem.IsEnabled = false;
                    save_as_menuitem.IsEnabled = false;
                    menu_drop.IsEnabled = false;
                    status_yes_no.Text = "NO, Please Install the Traccar Service";
                    status_yes_no.FontStyle = FontStyles.Italic;
                    status_yes_no.Foreground = Brushes.Red;
                    textbox.Text = "traccar";
                }
                Logger.WriteLine(" *** Service Installed Status :" + status_yes_no.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ini Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Ini();
            status_ready.Text = "Ready";
            Logger.WriteLine(" ***- APPLICATION STARTED -*** ");
        }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private void TimerStart()
        {
            try
            {
                if (dispatcherTimer.IsEnabled)
                {
                    Logger.WriteLine(" ***- [*Already Running*] No Need To Start [dispatcherTimer] -*** ");
                }
                else
                {
                    Logger.WriteLine(" ***- Started [dispatcherTimer] -*** ");
                    DispatcherTimer dispatcherTimer = new DispatcherTimer();
                    dispatcherTimer.Tick += dispatcherTimer_Tick;
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 15);//15 seconds
                    dispatcherTimer.Start();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "TimerStart Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void TimerStop()
        {
            try
            {
                if (dispatcherTimer.IsEnabled)
                {
                    Logger.WriteLine(" ***- Stopped [dispatcherTimer] -*** ");
                    dispatcherTimer.Stop();
                }
                else
                {
                    Logger.WriteLine(" ***- [*Already Stopped*] No Need To Stop [dispatcherTimer] -*** ");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "TimerStop Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckServices();
        }

        private void email_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Logger.WriteLine(" *** SENT MAIL => email_menuitem_Click clicked");
            string _date = DateTime.Now.ToString();
            worker4_DoWork("[" + _date + "] : testing subject @", "[" + _date + "] : testing body @");
        }

        private void email_settings_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Logger.WriteLine(" *** MAIL SETTINGS => email_settings_menuitem_Click clicked");
            //about
            Logger.WriteLine(" *** Email Settings Window Clicked *** ");
            Email_Settings_Window w1 = new Email_Settings_Window();
            w1.ShowDialog();
        }
    }
}
