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
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        };
    }
}