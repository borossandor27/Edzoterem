using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface ITagokService
{
    Task<List<TagResponse>> GetAllAsync();
    Task<TagResponse> GetByIdAsync(int id);
    Task<TagResponse> GetSajatAdataimAsync(int tagId);
    Task<TagRegisztracioValaszResponse> RegisztracioAsync(TagRegisztracioRequest request);
    Task<TagResponse> UpdateAsync(int id, TagUpdateRequest request);
    Task InaktivalasAsync(int id);
}
