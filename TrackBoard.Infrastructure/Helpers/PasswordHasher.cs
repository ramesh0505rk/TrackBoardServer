using System.Security.Cryptography;
using TrackBoard.Infrastructure.Interfaces;

namespace TrackBoard.Infrastructure.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int keySize = 32;
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, keySize);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var hashToVerify = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hash, hashToVerify);
        }
    }
}
