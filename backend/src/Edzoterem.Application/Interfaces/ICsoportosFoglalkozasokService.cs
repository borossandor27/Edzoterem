using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface ICsoportosFoglalkozasokService
{
    Task<List<CsoportosFoglalkozasResponse>> GetAllAsync();
    Task<CsoportosFoglalkozasResponse> GetByIdAsync(int id);
    Task<List<CsoportosFoglalkozasResponse>> GetEdzoSajatjaiAsync(int edzoId);
    Task<List<CsoportosFoglalkozasResponse>> GetTagSajatjaiAsync(int tagId);

    Task<CsoportosFoglalkozasResponse> MeghirdetesAsync(int edzoId, int meghirdetteFelhasznaloId, CsoportosFoglalkozasMeghirdetesRequest request);
    Task<CsoportosFoglalkozasResponse> JovahagyasAsync(int id, int tulajdonosFelhasznaloId);
    Task<CsoportosFoglalkozasResponse> ElutasitasAsync(int id, int tulajdonosFelhasznaloId, CsoportosFoglalkozasElutasitasRequest request);
    Task<CsoportosFoglalkozasResponse> JelentkezesAsync(int id, CsoportosJelentkezesRequest request, int rogzitveAltalId);
    Task JelentkezesTorleseAsync(int id, int tagId);
    Task<CsoportosFoglalkozasResponse> JelenletRogzitesAsync(int id, int edzoId, JelenletRogzitesRequest request);
    Task<CsoportosFoglalkozasResponse> LezarasAsync(int id, int edzoId);
}
