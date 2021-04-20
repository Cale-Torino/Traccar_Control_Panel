using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traccar_Control_Panel_WPF
{
    class EmailVarClass
    {
        public static class EmailVars
        {
            //Email variables/user details
            private static string smtpServert = "smtp.address.co.za";
            private static string mailFrom = "name@address.co.za";
            private static string mailTo = "name@gmail.com";
            private static string mailSubject = "Test Mail";
            private static string mailBody = "This is for testing SMTP mail";
            private static string userName = "name2@address.co.za";
            private static string password = "password";

            public static string _SmtpServert   // property
            {
                get { return smtpServert; }   // get method
                set { smtpServert = value; }  // set method
            }
            public static string _mailFrom   // property
            {
                get { return mailFrom; }   // get method
                set { mailFrom = value; }  // set method
            }

            public static string _mailTo   // property
            {
                get { return mailTo; }   // get method
                set { mailTo = value; }  // set method
            }

            public static string _mailSubject   // property
            {
                get { return mailSubject; }   // get method
                set { mailSubject = value; }  // set method
            }

            public static string _mailBody   // property
            {
                get { return mailBody; }   // get method
                set { mailBody = value; }  // set method
            }

            public static string _userName   // property
            {
                get { return userName; }   // get method
                set { userName = value; }  // set method
            }

            public static string _password   // property
            {
                get { return password; }   // get method
                set { password = value; }  // set method
            }

            public static string a   // property
            {
                get { return mailTo; }   // get method
                set { mailTo = value; }  // set method
            }
        }
    }
}
