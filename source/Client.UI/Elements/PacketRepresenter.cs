using Client.Core.Monitoring.Protocol;
using FontAwesome.Sharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.UI.Elements;

public class PacketRepresenter : Border
{
    public PacketRepresenter(Packet packet) 
    {
        Height = 50;
        CornerRadius = new CornerRadius(5);
        Background = new SolidColorBrush(Color.FromRgb(61, 81, 143));
        Margin = new Thickness(0, 0, 0, 5);
        Child = CreateBorderComponentsGrid(
            packet.SourceEndPoint, 
            packet.DestinationEndPoint, 
            packet.ProtocolType,
            packet.TotalLength,
            packet.TimeToLive,
            packet.Version);
    }

    private Grid CreateBorderComponentsGrid(EndPoint source, EndPoint destination, ProtocolType protocolType, ushort length, byte ttl, ProtocolVersion protocolVersion)
    {
        var grid = new Grid();

        grid.Children.Add(CreateAddressesComponentBorder(source, destination));
        grid.Children.Add(CreateInformationComponentBorder(protocolType, length));
        grid.Children.Add(CreateCompositionComponentBorder(ttl, protocolVersion));

        return grid;
    }

    private Border CreateAddressesComponentBorder(EndPoint source, EndPoint destination)
    {
        var sourceAddressIcon = new IconImage()
        {
            Icon = IconChar.ArrowLeft,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            Height = 10,
            Margin = new Thickness(2)
        };

        var destinationAddressIcon = new IconImage()
        {
            Icon = IconChar.ArrowRight,
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            Height = 10,
            Margin = new Thickness(2)
        };

        var sourceAddressTextBlock = new TextBlock()
        {
            Text = source.ToString(),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            FontFamily = new FontFamily("Bahnschrift"),
            Margin = new Thickness(20, 0, 0, 0),
            FontSize = 12
        };

        var destinationAddressTextBlock = new TextBlock()
        {
            Text = destination.ToString(),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            FontFamily = new FontFamily("Bahnschrift"),
            Margin = new Thickness(20, 0, 0, -2),
            FontSize = 12
        };

        var borderGrid = new Grid();
        borderGrid.Children.Add(sourceAddressIcon);
        borderGrid.Children.Add(sourceAddressTextBlock);
        borderGrid.Children.Add(destinationAddressIcon);
        borderGrid.Children.Add(destinationAddressTextBlock);

        var border = new Border()
        {
            Margin = new Thickness(10, 8, 10, 8),
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 150,
            BorderBrush = new SolidColorBrush(Color.FromRgb(101, 110, 144)),
            BorderThickness = new Thickness(0, 0, 1, 0),
            Child = borderGrid
        };


        border.Child = borderGrid;
        return border;
    }
    private Border CreateInformationComponentBorder(ProtocolType protocolType, ushort length)
    {
        var protocolIcon = new IconImage()
        {
            Icon = IconChar.Globe,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            Height = 10,
            Margin = new Thickness(2)
        };

        var lengthIcon = new IconImage()
        {
            Icon = IconChar.Ruler,
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            Height = 10,
            Margin = new Thickness(2)
        };

        var protocolTextBlock = new TextBlock()
        {
            Text = protocolType.ToString(),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            FontFamily = new FontFamily("Cascadia Code"),
            Margin = new Thickness(20, 0, 0, 0),
            FontSize = 12
        };

        var lengthTextBlock = new TextBlock()
        {
            Text = length.ToString(),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            FontFamily = new FontFamily("Cascadia Code"),
            Margin = new Thickness(20, 0, 0, -1),
            FontSize = 12
        };

        var grid = new Grid();
        grid.Children.Add(protocolIcon);
        grid.Children.Add(protocolTextBlock);
        grid.Children.Add(lengthIcon);
        grid.Children.Add(lengthTextBlock);

        var border = new Border()
        {
            Margin = new Thickness(180, 8, 10, 8),
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 75,
            BorderBrush = new SolidColorBrush(Color.FromRgb(101, 110, 144)),
            BorderThickness = new Thickness(0, 0, 1, 0),
            Child = grid
        };

        return border;
    }
    private Border CreateCompositionComponentBorder(byte ttl, ProtocolVersion protocolVersion)
    {
        var ttlTitleTextBlock = new TextBlock()
        {
            Text = "TTL",
            FontFamily = new FontFamily("Cascadia Code"),
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            Width = 75,
            TextAlignment = TextAlignment.Center,
        };
        var ttlValueTextBlock = new TextBlock()
        {
            Text = ttl.ToString(),
            FontFamily = new FontFamily("Cascadia Code"),
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            Width = 75,
            TextAlignment = TextAlignment.Center,
        };

        var verTitleTextBlock = new TextBlock()
        {
            Text = "VER",
            FontFamily = new FontFamily("Cascadia Code"),
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            Width = 75,
            TextAlignment = TextAlignment.Center,
        };
        var verValueTextBlock = new TextBlock()
        {
            Text = protocolVersion.ToString(),
            FontFamily = new FontFamily("Cascadia Code"),
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            Width = 75,
            TextAlignment = TextAlignment.Center,
        };

        var gridSplitter = new GridSplitter()
        {
            BorderBrush = new SolidColorBrush(Color.FromRgb(101, 110, 144)),
            BorderThickness = new Thickness(0.5)
        };

        var grid = new Grid()
        {
            ColumnDefinitions =
            {
                new ColumnDefinition() { Width = GridLength.Auto }, 
                new ColumnDefinition() { Width = GridLength.Auto } 
            },
            RowDefinitions =
            {
                new RowDefinition() { Height = GridLength.Auto },
                new RowDefinition() { Height = new GridLength(5) },
                new RowDefinition() { Height = GridLength.Auto },
                new RowDefinition() { Height = new GridLength(2) }
            }
        };

        Grid.SetColumn(ttlTitleTextBlock, 0);
        Grid.SetColumn(ttlValueTextBlock, 0);
        Grid.SetColumn(verTitleTextBlock, 1);
        Grid.SetColumn(verValueTextBlock, 1);
        Grid.SetRow(ttlValueTextBlock, 2);
        Grid.SetRow(verValueTextBlock, 2);
        Grid.SetRowSpan(gridSplitter, 3);

        grid.Children.Add(gridSplitter);
        grid.Children.Add(ttlTitleTextBlock);
        grid.Children.Add(verTitleTextBlock);
        grid.Children.Add(ttlValueTextBlock);
        grid.Children.Add(verValueTextBlock);


        var border = new Border()
        {
            Margin = new Thickness(260, 8, 10, 8),
            Width = 150,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Child = grid
        };

        return border;
    }
}