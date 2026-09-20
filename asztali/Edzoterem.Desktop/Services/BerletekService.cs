using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Desktop.Services;

public class BerletekService
{
    private readonly ApiClient _api;

    public BerletekService(ApiClient api)
    {
        _api = api;
    }

    public Task<List<BerletTipusResponse>?> GetTipusokAsync() => _api.GetAsync<List<BerletTipusResponse>>("api/berlettipusok");

    public Task<BerletTipusResponse?> CreateTipusAsync(BerletTipusUpsertRequest request) =>
        _api.PostAsync<BerletTipusUpsertRequest, BerletTipusResponse>("api/berlettipusok", request);

    public Task<BerletResponse?> HozzarendelesAsync(BerletHozzarendelesRequest request) =>
        _api.PostAsync<BerletHozzarendelesRequest, BerletResponse>("api/berletek", request);

    public Task<List<BerletResponse>?> GetLejarokAsync() => _api.GetAsync<List<BerletResponse>>("api/berletek/lejaro");
}
