using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Traccar_Control_Panel_WPF
{
    class ServiceClass
    {
        public static bool IsServiceInstalled(string serviceName)
        {
            try
            {
                // get list of Windows services
                ServiceController[] services = ServiceController.GetServices();

                // try to find service name
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Is Service Installed Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static string CheckServiceStatus(string serviceName)
        {

            try
            {
                //Check the status of the service
                //ServiceController[] services = ServiceController.GetServices();
                ServiceController service = new ServiceController(serviceName);
                string _SS = service.ServiceName + " is " + service.Status;
                return _SS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Check Service Status Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Check Service Status Error";
            }
        }
        public static string StartService(string serviceName, int timeoutMilliseconds)
        {

            try
            {
                //Start the service
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                MessageBox.Show("Service Successfully Started", " Please Note :) ", MessageBoxButton.OK, MessageBoxImage.Information);
                return "Service Successfully Started";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Start Service Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Start Service Error";
            }
        }
        public static string StopService(string serviceName, int timeoutMilliseconds)
        {

            try
            {
                //Stop the service
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                MessageBox.Show("Service Successfully Stopped", " Please Note :) ", MessageBoxButton.OK, MessageBoxImage.Information);
                return "Service Successfully Stopped";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stop Service Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Stop Service Error";
            }
        }
        public static string RestartService(string serviceName, int timeoutMilliseconds)
        {

            try
            {
                //Restart the service
                ServiceController service = new ServiceController(serviceName);
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                MessageBox.Show("Service Successfully Restarted", " Please Note :) ", MessageBoxButton.OK, MessageBoxImage.Information);
                return "Service Successfully Restarted";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Restart Service Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Restart Service Error";
            }
        }
    }
}
