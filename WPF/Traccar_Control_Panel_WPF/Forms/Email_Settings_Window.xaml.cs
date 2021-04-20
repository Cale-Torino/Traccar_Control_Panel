using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Traccar_Control_Panel_WPF.EmailVarClass;
using static Traccar_Control_Panel_WPF.LoggerClass;

namespace Traccar_Control_Panel_WPF
{
    /// <summary>
    /// Interaction logic for Email_Settings_Window.xaml
    /// </summary>
    public partial class Email_Settings_Window : Window
    {
        public Email_Settings_Window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logger.WriteLine(" *** SENT MAIL => normal mail Sent clicked");
            try
            {
                    //Send a test mail
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(EmailVars._SmtpServert);//smtp.techrad.co.za

                    mail.From = new MailAddress(EmailVars._mailFrom);
                    mail.To.Add(EmailVars._mailTo);
                    mail.Subject = EmailVars._mailSubject;
                    mail.Body = EmailVars._mailBody;

                    SmtpServer.Port = 587;//port
                    SmtpServer.Credentials = new NetworkCredential(EmailVars._userName, EmailVars._password);//details
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Logger.WriteLine(" *** Mail Sent! : send success!");
                    MessageBox.Show("Normal mail Sent", "Mail Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(" *** Mail Error! : " + ex.Message);
                MessageBox.Show(ex.ToString(), "Mail Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Logger.WriteLine(" *** SENT MAIL => Attachment mail sent clicked");
            try
            {
                //Send a test mail with an attachment
                var smtpClient = new SmtpClient(EmailVars._SmtpServert)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(EmailVars._userName, EmailVars._password),
                    EnableSsl = true,
                };

                //smtpClient.Send("email", "recipient", "subject", "body");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(EmailVars._mailFrom),//email
                    Subject = "C# WinForms Test Mail",
                    Body = "<h1>Hello</h1>" +
                    "<h2>C# WinForms Test Mail</h2>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(EmailVars._mailTo);//send to

                var attachment = new Attachment("profile.jpg", MediaTypeNames.Image.Jpeg);
                mailMessage.Attachments.Add(attachment);
                smtpClient.Send(mailMessage);
                MessageBox.Show("Attachment mail sent.", "Mail Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Mail Error!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get variables
            textbox_smtp.Text = EmailVars._SmtpServert;
            textbox_from.Text = EmailVars._mailFrom;
            textbox_to.Text = EmailVars._mailTo;
            textboxsubject.Text = EmailVars._mailSubject;
            textboxbody.Text = EmailVars._mailBody;
            textbox_username.Text = EmailVars._userName;
            textbox_password.Text = EmailVars._password;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Set variables
            textbox_smtp.Text = EmailVars._SmtpServert;
            textbox_from.Text = EmailVars._mailFrom;
            textbox_to.Text = EmailVars._mailTo;
            textboxsubject.Text = EmailVars._mailSubject;
            textboxbody.Text = EmailVars._mailBody;
            textbox_username.Text = EmailVars._userName;
            textbox_password.Text = EmailVars._password;
        }
    }
}
