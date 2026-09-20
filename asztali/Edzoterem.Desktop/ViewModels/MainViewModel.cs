using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DolgozokListViewModel _dolgozokListViewModel;
    private readonly TagokListViewModel _tagokListViewModel;
    private readonly BerletekViewModel _berletekViewModel;
    private readonly CsoportosFoglalkozasokViewModel _csoportosFoglalkozasokViewModel;

    public SessionState Session { get; }

    [ObservableProperty]
    private object? currentViewModel;

    public MainViewModel(
        SessionState session,
        DolgozokListViewModel dolgozokListViewModel,
        TagokListViewModel tagokListViewModel,
        BerletekViewModel berletekViewModel,
        CsoportosFoglalkozasokViewModel csoportosFoglalkozasokViewModel)
    {
        Session = session;
        _dolgozokListViewModel = dolgozokListViewModel;
        _tagokListViewModel = tagokListViewModel;
        _berletekViewModel = berletekViewModel;
        _csoportosFoglalkozasokViewModel = csoportosFoglalkozasokViewModel;

        CurrentViewModel = Session.Tulajdonos ? _dolgozokListViewModel : _tagokListViewModel;
    }

    [RelayCommand]
    private async Task MutasdDolgozokatAsync()
    {
        CurrentViewModel = _dolgozokListViewModel;
        await _dolgozokListViewModel.BetoltesCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task MutasdTagokatAsync()
    {
        CurrentViewModel = _tagokListViewModel;
        await _tagokListViewModel.BetoltesCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task MutasdBerleteketAsync()
    {
        CurrentViewModel = _berletekViewModel;
        await _berletekViewModel.BetoltesCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task MutasdCsoportosFoglalkozasokatAsync()
    {
        CurrentViewModel = _csoportosFoglalkozasokViewModel;
        await _csoportosFoglalkozasokViewModel.BetoltesCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private void Kijelentkezes()
    {
        Session.Kijelentkezes();
        Application_Shutdown();
    }

    private static void Application_Shutdown() => System.Windows.Application.Current.Shutdown();
}
