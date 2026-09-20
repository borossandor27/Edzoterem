using System.Security.Cryptography;

namespace Edzoterem.Application.Common;

public static class JelszoGeneralo
{
    private const string Karakterek = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

    public static string Generalas(int hossz = 10)
    {
        var buffer = RandomNumberGenerator.GetBytes(hossz);
        var chars = new char[hossz];
        for (var i = 0; i < hossz; i++)
        {
            chars[i] = Karakterek[buffer[i] % Karakterek.Length];
        }

        return new string(chars);
    }
}
