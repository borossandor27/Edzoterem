using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string felhasznalonev = string.Empty;

    [ObservableProperty]
    private string hibaUzenet = string.Empty;

    [ObservableProperty]
    private bool folyamatban;

    public event EventHandler? SikeresBejelentkezes;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task BejelentkezesAsync(string jelszo)
    {
        HibaUzenet = string.Empty;
        if (string.IsNullOrWhiteSpace(Felhasznalonev) || string.IsNullOrWhiteSpace(jelszo))
        {
            HibaUzenet = "Add meg a felhasználónevet és a jelszót.";
            return;
        }

        Folyamatban = true;
        try
        {
            await _authService.BejelentkezesAsync(Felhasznalonev, jelszo);
            SikeresBejelentkezes?.Invoke(this, EventArgs.Empty);
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
        catch (Exception)
        {
            HibaUzenet = "Nem sikerült kapcsolódni a szerverhez.";
        }
        finally
        {
            Folyamatban = false;
        }
    }
}
