using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface IEgyeniFoglalkozasokService
{
    Task<List<SzolgaltatasTipusResponse>> GetSzolgaltatasTipusokAsync();
    Task<SzolgaltatasTipusResponse> CreateSzolgaltatasTipusAsync(SzolgaltatasTipusUpsertRequest request);

    Task<EgyeniFoglalkozasResponse> RogzitesAsync(EgyeniFoglalkozasRequest request, int rogzitveAltalId);
    Task<EgyeniFoglalkozasResponse> StatuszFrissitesAsync(int id, EgyeniFoglalkozasStatuszRequest request);
    Task<List<EgyeniFoglalkozasResponse>> GetTagSajatjaiAsync(int tagId);
    Task<List<EgyeniFoglalkozasResponse>> GetDolgozoSajatjaiAsync(int dolgozoId);
}
