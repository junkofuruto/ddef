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

namespace Client.UI
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            App.AuthentificationFrame = AuthFrame;
            App.AuthentificationFrame.Navigate(new Pages.Auth.LoginPage());
        }

        private async void PlayStartupAnimation(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            double logoGradientStop = 1;
            double logoHeigth = 320;
            double opacity = -1;
            double fontSize = 150;
            while (true)
            {
                if (fontSize > 60)
                {
                    LogoTextBlock.FontSize = fontSize;
                    fontSize--;
                }
                if (logoHeigth < 430)
                {
                    LogoTextBlock.Height = logoHeigth;
                    logoHeigth++;
                }
                if (logoGradientStop > 0)
                {
                    LogoGradientStop.Offset = logoGradientStop;
                    logoGradientStop -= 0.01;
                }
                if (opacity < 1)
                {
                    AuthFrame.Opacity = opacity;
                    opacity += 0.01;
                }
                if (fontSize < 60 && logoHeigth > 450)
                {
                    break;
                }

                await Task.Delay(15);
            }
        }
        private void MainWindowsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void MainWindowsMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(this);
                Left = currentPosition.X + Left - Mouse.GetPosition(this).X;
                Top = currentPosition.Y + Top - Mouse.GetPosition(this).Y;
            }
        }
    }
}
