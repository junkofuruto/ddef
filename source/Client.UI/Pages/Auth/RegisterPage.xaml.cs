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
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void RegisterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                User.Current = await User.RegisterAsync(FirstNameTextBox.Text, LastNameTextBox.Text, UsernameTextBox.Text, PasswordTextBox.Password); 
                new MainWindow().Show();
                Window.GetWindow(this).Close();
            }
            catch (AuthenticationException ex)
            {
                LoginMessageTextBlock.Text = ex.Message.ToUpper();
            }
        }

        private void LocateLoginPageClick(object sender, RoutedEventArgs e)
        {
            App.AuthentificationFrame.Navigate(new LoginPage());
        }
    }
}
