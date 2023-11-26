using Client.Core.Analyzing.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
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

namespace Client.UI.Pages.Auth
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                User.Current = await User.LoginAsync(UsernameTextBox.Text, PasswordTextBox.Password);
                new MainWindow().Show();
                Window.GetWindow(this).Close();
            }
            catch (AuthenticationException)
            {
                LoginMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void LocateRegisterPageClick(object sender, RoutedEventArgs e)
        {
            App.AuthentificationFrame.Navigate(new RegisterPage());
        }
    }
}
