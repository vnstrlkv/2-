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
        const int port = 8888;
        const string address = "127.0.0.1";
        NetworkStream stream;
        public MainWindow()
        {

            InitializeComponent();
            TcpClient client = null;
            client = new TcpClient(address, port);
            stream = client.GetStream();
            ConnectBtn.Click += Connect_Click;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
           string userName = UserNameTxt.Text;
          bool flag= Clnt.Authorizat(userName, stream);
            if (flag==false)
            {
                AutorizatTextBlock.Text="NOT CONNECT";
            }
            else AutorizatTextBlock.Text = "CONNECTED";
        }

    }


}
