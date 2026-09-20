using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Contracts.Responses;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class CsoportosFoglalkozasokViewModel : ObservableObject
{
    private readonly CsoportosFoglalkozasokService _csoportosService;
    private readonly TagokService _tagokService;

    public ObservableCollection<CsoportosFoglalkozasResponse> Foglalkozasok { get; } = new();
    public ObservableCollection<TagResponse> Tagok { get; } = new();

    [ObservableProperty]
    private string hibaUzenet = string.Empty;

    [ObservableProperty]
    private CsoportosFoglalkozasResponse? kivalasztottFoglalkozas;

    [ObservableProperty]
    private TagResponse? kivalasztottTag;

    [ObservableProperty]
    private string elutasitasIndoka = string.Empty;

    public CsoportosFoglalkozasokViewModel(CsoportosFoglalkozasokService csoportosService, TagokService tagokService)
    {
        _csoportosService = csoportosService;
        _tagokService = tagokService;
    }

    [RelayCommand]
    public async Task BetoltesAsync()
    {
        HibaUzenet = string.Empty;
        try
        {
            var lista = await _csoportosService.GetAllAsync() ?? new List<CsoportosFoglalkozasResponse>();
            Foglalkozasok.Clear();
            foreach (var f in lista) Foglalkozasok.Add(f);

            var tagok = await _tagokService.GetAllAsync() ?? new List<TagResponse>();
            Tagok.Clear();
            foreach (var t in tagok.Where(t => t.Aktiv)) Tagok.Add(t);
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task JovahagyasAsync(int foglalkozasId)
    {
        try
        {
            await _csoportosService.JovahagyasAsync(foglalkozasId);
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task ElutasitasAsync(int foglalkozasId)
    {
        try
        {
            await _csoportosService.ElutasitasAsync(foglalkozasId, ElutasitasIndoka);
            ElutasitasIndoka = string.Empty;
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }

    [RelayCommand]
    private async Task JelentkezesAsync()
    {
        if (KivalasztottFoglalkozas is null || KivalasztottTag is null)
        {
            HibaUzenet = "Válassz foglalkozást és tagot.";
            return;
        }

        try
        {
            await _csoportosService.JelentkezesAsync(KivalasztottFoglalkozas.Id, KivalasztottTag.Id);
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }
}
