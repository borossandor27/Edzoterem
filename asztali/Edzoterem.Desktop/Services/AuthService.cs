using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;

namespace Edzoterem.Desktop.Services;

public class AuthService
{
    private readonly ApiClient _api;
    private readonly SessionState _session;

    public AuthService(ApiClient api, SessionState session)
    {
        _api = api;
        _session = session;
    }

    public async Task<LoginResponse> BejelentkezesAsync(string felhasznalonev, string jelszo)
    {
        var response = await _api.PostAsync<LoginRequest, LoginResponse>("api/auth/login", new LoginRequest(felhasznalonev, jelszo))
            ?? throw new ApiException("A bejelentkezés sikertelen.", 500);

        _session.Token = response.Token;
        _session.Szerepkor = response.Szerepkor;
        _session.Nev = response.Nev;

        return response;
    }
}
