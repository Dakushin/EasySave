using System.Windows;
using System.Windows.Input;

namespace EasySave.view.wpf.windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MakeWindowsDraggable();
    }

    private void MakeWindowsDraggable()
    {
        MouseDown += (_, args) =>
        {
            if (args.LeftButton == MouseButtonState.Pressed) DragMove();
        };
    }

    public void OnClose(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public void OnMinimize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    public void OnMaximize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
    }
}