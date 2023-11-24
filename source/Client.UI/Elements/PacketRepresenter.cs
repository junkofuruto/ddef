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
            packet.TotalLength);
    }

    private Grid CreateBorderComponentsGrid(EndPoint source, EndPoint destination, ProtocolType protocolType, ushort length)
    {
        var grid = new Grid();

        grid.Children.Add(CreateAddressesComponentBorder(source, destination));
        grid.Children.Add(CreateInformationComponentBorder(protocolType, length));

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
            FontFamily = new FontFamily("Bahnschrift Light"),
            Margin = new Thickness(20, 0, 0, 0),
            FontSize = 12
        };

        var destinationAddressTextBlock = new TextBlock()
        {
            Text = source.ToString(),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = Brushes.White,
            FontFamily = new FontFamily("Bahnschrift Light"),
            Margin = new Thickness(20, 0, 0, 0),
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
            Text = protocolType.ToString(),
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
    private Border CreateCompositionComponentBorder(ushort totalLength, byte HeaderLength)
    {
        var textBlock = new TextBlock()
        {
            
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
            },
        };

        var border = new Border()
        {
            Margin = new Thickness(280, 8, 10, 8),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Child = grid
        };

        return border;
    }
}