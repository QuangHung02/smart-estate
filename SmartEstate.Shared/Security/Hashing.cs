using System.Security.Cryptography;
using System.Text;

namespace SmartEstate.Shared.Security;

public static class Hashing
{
    public static string Sha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
