using Client.Core.Analyzing.DataAccess.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.UI.Pages
{
    public partial class AccountPage : Page
    {
        private static AccountPage? instance;

        private AccountPage()
        {
            InitializeComponent();

            GreetingsMessageTextBlock.Text = $"{GetGreetingsMessage()}, {User.Current.FirstName.ToUpper()}";
            switch (User.Current.Plan.Id)
            {
                case 0: StarterPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 1: ConsolePlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 2: ProfessionalPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
            }
        }

        private string GetGreetingsMessage()
        {
            var hour = DateTime.Now.Hour;
            if (hour > 18) return "GOOD EVENING";
            else if (hour > 12) return "GOOD DAY";
            else if (hour > 6) return "GOOD MORNING";
            else return "GOOD NIGHT";
        }

        public static AccountPage Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountPage();
                return instance;
            }
        }

        private async void UpdateUserData(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                await User.Current.ModifyAsync(PasswordTextBox.Password, FirstNameTextBox.Text, LastNameTextBox.Text);
                MainWindow.UpdateData((MainWindow)Window.GetWindow(this));
            }
            catch { }
        }

        private async void StarterPlanSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Are you sure (vstavte suda funcional dlya oplaty)?", "Plan change", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No || result == MessageBoxResult.None) return;
            if (await UserPlan.Starter.UpdateUserPlanAsync(User.Current)) MessageBox.Show("Plan changed");
            switch (UserPlan.Starter.Id)
            {
                case 0: StarterPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 1: ConsolePlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 2: ProfessionalPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
            }
        }

        private async void ConsolePlanSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Are you sure (vstavte suda funcional dlya oplaty)?", "Plan change", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No || result == MessageBoxResult.None) return;
            if (await UserPlan.Console.UpdateUserPlanAsync(User.Current)) MessageBox.Show("Plan changed");
            switch (UserPlan.Console.Id)
            {
                case 0: StarterPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 1: ConsolePlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 2: ProfessionalPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
            }
        }

        private async void ProfessionalPlanSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Are you sure (vstavte suda funcional dlya oplaty)?", "Plan change", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No || result == MessageBoxResult.None) return;
            if (await UserPlan.Professional.UpdateUserPlanAsync(User.Current)) MessageBox.Show("Plan changed");
            switch (UserPlan.Professional.Id)
            {
                case 0: StarterPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 1: ConsolePlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
                case 2: ProfessionalPlanBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 124, 241)); break;
            }
        }
    }
}
