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
    /// Interaction logic for RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        private Client client = ClientView.client;
        public RequestPage()
        {
            InitializeComponent();
        }
        private void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            string request = RequestTextBox.Text;
            client.AddRequest(request);
        }
    }
}
