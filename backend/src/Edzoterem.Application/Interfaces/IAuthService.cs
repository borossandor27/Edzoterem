using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<FelhasznaloResponse> GetSajatFelhasznaloAsync(int felhasznaloId);
    Task JelszoValtoztatasAsync(int felhasznaloId, JelszoValtoztatasRequest request);
}
