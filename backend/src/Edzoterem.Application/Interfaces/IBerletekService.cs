using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface IBerletekService
{
    Task<List<BerletTipusResponse>> GetTipusokAsync();
    Task<BerletTipusResponse> CreateTipusAsync(BerletTipusUpsertRequest request);
    Task<BerletTipusResponse> UpdateTipusAsync(int id, BerletTipusUpsertRequest request);

    Task<BerletResponse> HozzarendelesAsync(BerletHozzarendelesRequest request, int letrehozvaAltalId);
    Task<List<BerletResponse>> GetTagBerleteiAsync(int tagId);
    Task<List<BerletResponse>> GetLejarokAsync();
}
