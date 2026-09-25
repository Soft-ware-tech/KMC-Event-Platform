namespace KMC_API.Helpers
{
    using System.Security.Cryptography;
    using System.Text;

    // Very small helper used to keep organizer passwords out of the database in
    // plain text. This is a one-way SHA256 hash - simple and good enough for a
    // campus / assignment project, not intended as production-grade security.
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }
}
