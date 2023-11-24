using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Client.UI.Elements;

public class IssueRepresenter : Border
{
    public IssueRepresenter(IconChar icon, string issuer, string reason, string? description) 
    {
        Background = new SolidColorBrush(Color.FromRgb(61, 81, 143));
        CornerRadius = new CornerRadius(5);
        Child = CreateIssueGrid(issuer, reason, description, icon);
    }

    private Grid CreateIssueGrid(string issuer, string reason, string? description, IconChar icon)
    {
        var timeTextBlock = new TextBlock()
        {
            Text = DateTime.Now.ToString("H:mm:ss"),
            Foreground = new SolidColorBrush(Color.FromRgb(164, 175, 208)),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 3, 5, 0),
            FontFamily = new FontFamily("Cascadia Code")
        };

        var issuerTextBlock = new TextBlock()
        {
            Text = issuer,
            Foreground = new SolidColorBrush(Color.FromRgb(164, 175, 208)),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(23, 3, 0, 0),
            FontFamily = new FontFamily("Cascadia Code")
        };

        var descriptionTextBlock = new TextBlock()
        {
            Text = description is null ? reason.ToUpper() : $"{reason.ToUpper()}: {description}",
            TextWrapping = TextWrapping.Wrap,
            Foreground = Brushes.White,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5, 3, 5, 8),
            FontFamily = new FontFamily("Cascadia Code")
        };

        var iconElement = new IconImage()
        {
            Icon = icon,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment= VerticalAlignment.Center,
            Height = 12,
            Foreground = new SolidColorBrush(Color.FromRgb(164, 175, 208)),
            Margin = new Thickness(5, 3, 5, 0),
        };

        var grid = new Grid()
        {
            Margin = new Thickness(3),
            RowDefinitions =
            {
                new RowDefinition() { Height = new GridLength(2, GridUnitType.Pixel) },
                new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) },
            }
        };

        Grid.SetRow(timeTextBlock, 0);
        Grid.SetRow(issuerTextBlock, 0);
        Grid.SetRow(descriptionTextBlock, 1);
        Grid.SetRow(iconElement, 0);

        grid.Children.Add(timeTextBlock);
        grid.Children.Add(issuerTextBlock);
        grid.Children.Add(descriptionTextBlock);
        grid.Children.Add(iconElement);

        return grid;
    }
}