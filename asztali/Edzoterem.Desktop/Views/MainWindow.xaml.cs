using System.Windows;
using Edzoterem.Desktop.ViewModels;

namespace Edzoterem.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
