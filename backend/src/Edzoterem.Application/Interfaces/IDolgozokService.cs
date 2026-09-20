using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface IDolgozokService
{
    Task<List<DolgozoResponse>> GetAllAsync();
    Task<DolgozoResponse> GetByIdAsync(int id);
    Task<DolgozoResponse> CreateAsync(DolgozoUpsertRequest request);
    Task<DolgozoResponse> UpdateAsync(int id, DolgozoUpsertRequest request);
    Task KileptetesAsync(int id);
    Task<List<MunkarendResponse>> GetMunkarendAsync(int dolgozoId);
    Task<MunkarendResponse> AddMunkarendAsync(int dolgozoId, MunkarendRequest request);

    Task<List<EdzoResponse>> GetEdzokAsync();
    Task<int> GetEdzoIdByDolgozoIdAsync(int dolgozoId);
    Task<EdzoResponse> CreateEdzoAsync(int dolgozoId, EdzoUpsertRequest request);
    Task<EdzoResponse> UpdateEdzoAsync(int edzoId, EdzoUpsertRequest request);
    Task<List<EdzoElerhetosegResponse>> GetElerhetosegAsync(int edzoId);
    Task<EdzoElerhetosegResponse> AddElerhetosegAsync(int edzoId, EdzoElerhetosegRequest request);
}
