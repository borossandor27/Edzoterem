using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Desktop.Services;

namespace Edzoterem.Desktop.ViewModels;

public partial class BerletekViewModel : ObservableObject
{
    private readonly BerletekService _berletekService;
    private readonly TagokService _tagokService;

    public ObservableCollection<BerletTipusResponse> Tipusok { get; } = new();
    public ObservableCollection<BerletResponse> LejaroBerletek { get; } = new();
    public ObservableCollection<TagResponse> Tagok { get; } = new();

    [ObservableProperty]
    private string hibaUzenet = string.Empty;

    [ObservableProperty]
    private string sikerUzenet = string.Empty;

    [ObservableProperty]
    private TagResponse? kivalasztottTag;

    [ObservableProperty]
    private BerletTipusResponse? kivalasztottTipus;

    [ObservableProperty]
    private DateTime kezdoDatum = DateTime.Today;

    [ObservableProperty]
    private DateTime lejaratiDatum = DateTime.Today.AddMonths(1);

    public BerletekViewModel(BerletekService berletekService, TagokService tagokService)
    {
        _berletekService = berletekService;
        _tagokService = tagokService;
    }

    [RelayCommand]
    public async Task BetoltesAsync()
    {
        HibaUzenet = string.Empty;
        try
        {
            var tipusok = await _berletekService.GetTipusokAsync() ?? new List<BerletTipusResponse>();
            Tipusok.Clear();
            foreach (var t in tipusok) Tipusok.Add(t);

            var lejarok = await _berletekService.GetLejarokAsync() ?? new List<BerletResponse>();
            LejaroBerletek.Clear();
            foreach (var b in lejarok) LejaroBerletek.Add(b);

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
    private async Task HozzarendelesAsync()
    {
        HibaUzenet = string.Empty;
        SikerUzenet = string.Empty;

        if (KivalasztottTag is null || KivalasztottTipus is null)
        {
            HibaUzenet = "Válassz tagot és bérlettípust.";
            return;
        }

        try
        {
            var request = new BerletHozzarendelesRequest(
                KivalasztottTag.Id, KivalasztottTipus.Id,
                DateOnly.FromDateTime(KezdoDatum), DateOnly.FromDateTime(LejaratiDatum),
                ArFelulir: null, AlkalmakFelulir: null);

            await _berletekService.HozzarendelesAsync(request);
            SikerUzenet = "Bérlet sikeresen hozzárendelve.";
            await BetoltesAsync();
        }
        catch (ApiException ex)
        {
            HibaUzenet = ex.Message;
        }
    }
}
