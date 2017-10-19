using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SocketClient;
namespace ClntWpfForm
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Clnt client = new Clnt();
      

        public MainWindow()
        {
            
            InitializeComponent();


            client.ConnectionTrue += ChangeConnectionButtonTrue;
            client.ConnectionFalse += ChangeConnectionButtonFalse;

        }
        
/*
        private void ConnectionButton_Loaded(object sender, RoutedEventArgs e)
        {
            client.ConnectionTrue += ChangeConnectionButtonTrue;
            client.ConnectionFalse += ChangeConnectionButtonFalse;
        }
   */
        private void ChangeConnectionButtonTrue()
        {
            
            ConnectionButton.Content = "Disconnect";
            ConnectionButton.Click -= ConnectionButton_Click;
            ConnectionButton.Click += ConnectionButton_Click_Disconnect;
        }
        private void ChangeConnectionButtonFalse()
        {
            ConnectionButton.Content = "Connect";
            ConnectionButton.Click -= ConnectionButton_Click_Disconnect;
            ConnectionButton.Click += ConnectionButton_Click;
        }

        private void ConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            client.Connect();
            if (client.Connected)
            {
                AutorizatTextBlock.Text = "Успешное подключение";
            }
            else AutorizatTextBlock.Text = "Сервер не доступен";
        }

        private void ConnectionButton_Click_Disconnect(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
            if (!client.Connected)
            {
                AutorizatTextBlock.Text = "Отключен";
            }
            else AutorizatTextBlock.Text = "Сервер не доступен";
        }


    }


}
