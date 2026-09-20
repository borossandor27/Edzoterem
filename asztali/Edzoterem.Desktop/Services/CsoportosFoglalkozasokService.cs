using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Desktop.Services;

public class CsoportosFoglalkozasokService
{
    private readonly ApiClient _api;

    public CsoportosFoglalkozasokService(ApiClient api)
    {
        _api = api;
    }

    public Task<List<CsoportosFoglalkozasResponse>?> GetAllAsync() =>
        _api.GetAsync<List<CsoportosFoglalkozasResponse>>("api/csoportos-foglalkozasok");

    public Task<CsoportosFoglalkozasResponse?> JovahagyasAsync(int id) =>
        _api.PutAsync<object, CsoportosFoglalkozasResponse>($"api/csoportos-foglalkozasok/{id}/jovahagyas", new { });

    public Task<CsoportosFoglalkozasResponse?> ElutasitasAsync(int id, string indok) =>
        _api.PutAsync<CsoportosFoglalkozasElutasitasRequest, CsoportosFoglalkozasResponse>(
            $"api/csoportos-foglalkozasok/{id}/elutasitas", new CsoportosFoglalkozasElutasitasRequest(indok));

    public Task<CsoportosFoglalkozasResponse?> JelentkezesAsync(int id, int tagId) =>
        _api.PostAsync<CsoportosJelentkezesRequest, CsoportosFoglalkozasResponse>(
            $"api/csoportos-foglalkozasok/{id}/jelentkezes", new CsoportosJelentkezesRequest(tagId));

    public Task JelentkezesTorleseAsync(int id, int tagId) =>
        _api.DeleteAsync($"api/csoportos-foglalkozasok/{id}/jelentkezes/{tagId}");
}
