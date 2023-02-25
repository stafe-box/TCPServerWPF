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
            _server.ExcRaise += Exc_Raise;
            Status.Visibility= Visibility.Visible;
            Start.IsEnabled = false;
            await Task.Run(() =>
            {
                _server.Start();
            });
        }

        private void Exc_Raise(string except)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageBoxImage mbi = MessageBoxImage.Error;
                MessageBoxButton mbb = MessageBoxButton.OK;
                MessageBox.Show(except, "Ошибка", mbb, mbi);
            }));
        }

        private void Message_Recived(string Col, string Format)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => 
            {
                Result.Text += Format;
                Color clr = (Color)ColorConverter.ConvertFromString(Col);
                Result.Foreground = new SolidColorBrush(clr);
                //Result.Background= new SolidColorBrush(clr);
                this.Title = "TCP listner: " + _server.MyIP;
            }));
        }
    }
}
