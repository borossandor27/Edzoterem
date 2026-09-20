using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class TagokListViewModel : ObservableObject
{
    private readonly TagokService _tagokService;

    public ObservableCollection<TagResponse> Tagok { get; } = new();

    [ObservableProperty]
    private string hibaUzenet = string.Empty;

    [ObservableProperty]
    private string sikerUzenet = string.Empty;

    [ObservableProperty]
    private string ujVezeteknev = string.Empty;

    [ObservableProperty]
    private string ujKeresztnev = string.Empty;

    [ObservableProperty]
    private string ujEmail = string.Empty;

    [ObservableProperty]
    private string ujTelefon = string.Empty;

    [ObservableProperty]
    private string ujNem = "Ferfi";

    [ObservableProperty]
    private DateTime ujSzuletesiDatum = DateTime.Today.AddYears(-25);

    [ObservableProperty]
    private string ujVhKapcsolattartoNev = string.Empty;

    [ObservableProperty]
    private string ujVhKapcsolattartoTelefon = string.Empty;

    public string[] NemOpciok { get; } = { "Ferfi", "No", "Egyeb" };

    public TagokListViewModel(TagokService tagokService)
    {
        _tagokService = tagokService;
    }

    [RelayCommand]
    public async Task BetoltesAsync()
    {
        HibaUzenet = string.Empty;
        try
        {
            var lista = await _tagokService.GetAllAsync() ?? new List<TagResponse>();
            Tagok.Clear();
            foreach (var t in lista)
            {
                Tagok.Add(t);
            }
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task RegisztracioAsync()
    {
        HibaUzenet = string.Empty;
        SikerUzenet = string.Empty;

        if (string.IsNullOrWhiteSpace(UjVezeteknev) || string.IsNullOrWhiteSpace(UjKeresztnev))
        {
            HibaUzenet = "A vezetéknév és a keresztnév megadása kötelező.";
            return;
        }

        try
        {
            var request = new TagRegisztracioRequest(
                UjVezeteknev, UjKeresztnev, DateOnly.FromDateTime(UjSzuletesiDatum),
                null, UjTelefon, UjEmail, UjVhKapcsolattartoNev, UjVhKapcsolattartoTelefon, UjNem, null);

            var valasz = await _tagokService.RegisztracioAsync(request);
            if (valasz is not null)
            {
                SikerUzenet = $"Tag regisztrálva. Felhasználónév: {valasz.Felhasznalonev}, ideiglenes jelszó: {valasz.IdeiglenesJelszo} (add át a tagnak).";
            }

            UjVezeteknev = string.Empty;
            UjKeresztnev = string.Empty;
            UjEmail = string.Empty;
            UjTelefon = string.Empty;
            UjVhKapcsolattartoNev = string.Empty;
            UjVhKapcsolattartoTelefon = string.Empty;

            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task InaktivalasAsync(int tagId)
    {
        try
        {
            await _tagokService.InaktivalasAsync(tagId);
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }
}
