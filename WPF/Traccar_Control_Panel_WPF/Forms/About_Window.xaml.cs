using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Traccar_Control_Panel_WPF
{
    /// <summary>
    /// Interaction logic for About_Window.xaml
    /// </summary>
    public partial class About_Window : Window
    {
        public About_Window()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://tutorials.techrad.co.za");
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
