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

namespace ClntWpfForm.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            Chansdasd();
            itsoke += ChangeButton;

        }

        private void Chansdasd()
        {
            TextBox[] textboxes = { UserName, Password, FirstName, SecondName, PhoneNumber };
            if (textboxes.All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                itsoke();
            }
        }
        private void ChangeButton()
        {
            button.IsEnabled = !button.IsEnabled;
        }
        public delegate void itsokehandler();
        public event itsokehandler itsoke;
    }
}
