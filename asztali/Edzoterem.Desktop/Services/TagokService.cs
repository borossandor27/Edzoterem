using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Desktop.Services;

public class TagokService
{
    private readonly ApiClient _api;

    public TagokService(ApiClient api)
    {
        _api = api;
    }

    public Task<List<TagResponse>?> GetAllAsync() => _api.GetAsync<List<TagResponse>>("api/tagok");

    public Task<TagRegisztracioValaszResponse?> RegisztracioAsync(TagRegisztracioRequest request) =>
        _api.PostAsync<TagRegisztracioRequest, TagRegisztracioValaszResponse>("api/tagok", request);

    public Task<TagResponse?> UpdateAsync(int id, TagUpdateRequest request) =>
        _api.PutAsync<TagUpdateRequest, TagResponse>($"api/tagok/{id}", request);

    public Task InaktivalasAsync(int id) => _api.PutAsync($"api/tagok/{id}/inaktivalas");

    public Task<List<BerletResponse>?> GetBerleteiAsync(int tagId) =>
        _api.GetAsync<List<BerletResponse>>($"api/tagok/{tagId}/berletek");
}
