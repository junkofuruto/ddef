using Client.Core.Analyzing.Address;
using Client.Core.Analyzing.Application;
using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Monitoring;
using Client.UI.Elements;
using FontAwesome.Sharp;
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
    public partial class ConsolePage : Page
    {
        private static ConsolePage? instance;

        private ConsolePage()
        {
            InitializeComponent();

            MonitoringServer.PacketHandler += AddPacket;
            AddressAnalyzer.OnBadAddress += AddAddressIssue;
            ApplicationAnalyzer.OnBadApplication += AddApplicationIssue;
        }

        private async void AddApplicationIssue(BadApplicationEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                IssuesStackPanel.Children.Add(new IssueRepresenter(IconChar.MobileAlt, e.Application, e.Reason, e.Message));
                if (IssuesStackPanel.Children.Count > 8) IssuesStackPanel.Children.RemoveAt(0);
            });
        }

        private async void AddAddressIssue(BadAddressEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                IssuesStackPanel.Children.Add(new IssueRepresenter(IconChar.Globe, e.Address!.ToString(), e.Reason, e.Message));
                if (IssuesStackPanel.Children.Count > 8) IssuesStackPanel.Children.RemoveAt(0);
            });
        }

        private async void AddPacket(Core.Monitoring.Utilities.AnalyzerEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                PacketsStackPanel.Children.Add(new PacketRepresenter(e.Packet));
                if (PacketsStackPanel.Children.Count > 9) PacketsStackPanel.Children.RemoveAt(0);
            });
        }

        public static ConsolePage Instance
        {
            get
            {
                if (User.Current.Plan.ConsoleAccess == false) throw new InvalidOperationException("You dont have access to this feature");
                if (User.Current.Plan.ConsoleAccess && instance == null) instance = new ConsolePage();
                return instance;
            }
        }
    }
}
