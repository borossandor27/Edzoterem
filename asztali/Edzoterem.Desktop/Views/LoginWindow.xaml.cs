using System.Windows;
using Edzoterem.Desktop.ViewModels;

namespace Edzoterem.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.SikeresBejelentkezes += (_, _) => DialogResult = true;
    }
}
