using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class DolgozokListViewModel : ObservableObject
{
    private readonly DolgozokService _dolgozokService;

    public ObservableCollection<DolgozoResponse> Dolgozok { get; } = new();

    [ObservableProperty]
    private string hibaUzenet = string.Empty;

    [ObservableProperty]
    private string ujVezeteknev = string.Empty;

    [ObservableProperty]
    private string ujKeresztnev = string.Empty;

    [ObservableProperty]
    private string ujEmail = string.Empty;

    [ObservableProperty]
    private string ujNem = "Ferfi";

    [ObservableProperty]
    private DateTime ujSzuletesiDatum = DateTime.Today.AddYears(-25);

    public string[] NemOpciok { get; } = { "Ferfi", "No", "Egyeb" };

    public DolgozokListViewModel(DolgozokService dolgozokService)
    {
        _dolgozokService = dolgozokService;
    }

    [RelayCommand]
    public async Task BetoltesAsync()
    {
        HibaUzenet = string.Empty;
        try
        {
            var lista = await _dolgozokService.GetAllAsync() ?? new List<DolgozoResponse>();
            Dolgozok.Clear();
            foreach (var d in lista)
            {
                Dolgozok.Add(d);
            }
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task UjDolgozoFelveteleAsync()
    {
        HibaUzenet = string.Empty;
        if (string.IsNullOrWhiteSpace(UjVezeteknev) || string.IsNullOrWhiteSpace(UjKeresztnev))
        {
            HibaUzenet = "A vezetéknév és a keresztnév megadása kötelező.";
            return;
        }

        try
        {
            // Ideiglenesen 1-es munkakörre (Tulajdonos) hivatkozunk; a valós verzióban ez egy legördülő lista lenne a Munkakorok végpontból.
            var request = new DolgozoUpsertRequest(
                UjVezeteknev, UjKeresztnev, null, MunkakorId: 3,
                null, null, null, null, UjEmail,
                DateOnly.FromDateTime(UjSzuletesiDatum), UjNem, null, null, null, null);

            await _dolgozokService.CreateAsync(request);
            UjVezeteknev = string.Empty;
            UjKeresztnev = string.Empty;
            UjEmail = string.Empty;
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task KileptetesAsync(int dolgozoId)
    {
        try
        {
            await _dolgozokService.KileptetesAsync(dolgozoId);
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }
}
