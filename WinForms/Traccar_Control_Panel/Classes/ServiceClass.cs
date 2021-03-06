using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Traccar_Control_Panel
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
                MessageBox.Show(ex.Message, "Is Service Installed Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static string CheckServiceStatus(string serviceName)
        {

            try
            {
                //Check the status of a service
                //ServiceController[] services = ServiceController.GetServices();
                ServiceController service = new ServiceController(serviceName);
                string _SS = service.ServiceName + " is " + service.Status;
                return _SS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Check Service Status Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Check Service Status Error";
            }
        }
        public static string StartService(string serviceName, int timeoutMilliseconds)
        {

            try
            {
                //Try to start the service
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return "Service Successfully Started";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Start Service Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Start Service Error";
            }
        }
        public static string StopService(string serviceName, int timeoutMilliseconds)
        {
            
            try
            {
                //Try to stop the service
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                return "Service Successfully Stopped";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stop Service Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Stop Service Error";
            }
        }
        public static string RestartService(string serviceName, int timeoutMilliseconds)
        {
            
            try
            {
                //Try to stop and start the service again
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
                return "Service Successfully Restarted";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Restart Service Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Restart Service Error";
            }
        }

    }
}
