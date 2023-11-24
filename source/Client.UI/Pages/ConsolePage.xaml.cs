using Client.Core.Monitoring;
using Client.UI.Elements;
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
        }

        private async void AddPacket(Core.Monitoring.Utilities.AnalyzerEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (PacketsStackPanel.Children.Count > 9) PacketsStackPanel.Children.RemoveAt(0);
                PacketsStackPanel.Children.Add(new PacketRepresenter(e.Packet));
            });
        }

        public static ConsolePage Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConsolePage();
                return instance;
            }
        }
    }
}
