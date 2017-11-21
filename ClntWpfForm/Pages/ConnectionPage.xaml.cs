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
using SocketClient;
namespace ClntWpfForm.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ConnectionPage : Page
    {
        private Client client = ClientView.client;

        public ConnectionPage()
        {
            InitializeComponent();
            client.ConnectionTrue += ChangeConnectionButtonTrue;
            client.ConnectionFalse += ChangeConnectionButtonFalse;
            client.AutorizationChange += AutorizationButtonChange;

        }

        /*
                private void ConnectionButton_Loaded(object sender, RoutedEventArgs e)
                {
                    ClientView.client.ConnectionTrue += ChangeConnectionButtonTrue;
                    ClientView.client.ConnectionFalse += ChangeConnectionButtonFalse;
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

        private void AutorizationButtonChange()
        {
            if (client.Connected)
            {
                RegisterButton.IsEnabled = true;
                AutorizationButton.IsEnabled = true;
            }
            else
            {
                RegisterButton.IsEnabled = false;
                AutorizationButton.IsEnabled = false;
            }
        }
        private void AutorizationButton_Click(object sender, RoutedEventArgs e)
        {
            string text = UserNameText.Text;

            if (client.Authorizat(UserNameText.Text,PasswordText.Password))
            {
                AutorizationButton.IsEnabled = false;                
                NavigationService.Navigate(new RequestPage());
            }
            else
            {
                AutorizatTextBlock.Text = "Неверное имя пользователя или пароль";
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

    }


}

