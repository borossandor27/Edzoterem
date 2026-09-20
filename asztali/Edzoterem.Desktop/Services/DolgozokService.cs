using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Desktop.Services;

public class DolgozokService
{
    private readonly ApiClient _api;

    public DolgozokService(ApiClient api)
    {
        _api = api;
    }

    public Task<List<DolgozoResponse>?> GetAllAsync() => _api.GetAsync<List<DolgozoResponse>>("api/dolgozok");

    public Task<DolgozoResponse?> CreateAsync(DolgozoUpsertRequest request) =>
        _api.PostAsync<DolgozoUpsertRequest, DolgozoResponse>("api/dolgozok", request);

    public Task<DolgozoResponse?> UpdateAsync(int id, DolgozoUpsertRequest request) =>
        _api.PutAsync<DolgozoUpsertRequest, DolgozoResponse>($"api/dolgozok/{id}", request);

    public Task KileptetesAsync(int id) => _api.DeleteAsync($"api/dolgozok/{id}");

    public Task<List<EdzoResponse>?> GetEdzokAsync() => _api.GetAsync<List<EdzoResponse>>("api/edzok");
}
