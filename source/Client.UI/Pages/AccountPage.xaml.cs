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

namespace Client.UI.Pages
{
    public partial class AccountPage : Page
    {
        private static AccountPage? instance;

        private AccountPage()
        {
            InitializeComponent();
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
    }
}
