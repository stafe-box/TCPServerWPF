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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static TCPServer.Server;

namespace TCPServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server _server = new Server();
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "TCP listner: " + _server.MyIP;
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "TCP listner: " + _server.MyIP;
            _server.MessageReceived += Message_Recived;
            Status.Visibility= Visibility.Visible;
            Start.IsEnabled = false;
            await Task.Run(() =>
            {
                _server.Start();
            });
        }

        private void Message_Recived(string Message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => 
            {
                Result.Text += Message+"\n";
                this.Title = "TCP listner: " + _server.MyIP;
            }));
        }
    }
}
