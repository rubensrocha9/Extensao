using System.Security.Cryptography;
using System.Text;

namespace GestorPay.Models.Helper
{
    public class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 32;
        private static readonly int Iterations = 100000;

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = GenerateHash(password, salt);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] hash = GenerateHash(password, salt);

            return hashBytes.Skip(SaltSize).SequenceEqual(hash);
        }

        private static byte[] GenerateHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + SaltSize];
                passwordBytes.CopyTo(passwordWithSaltBytes, 0);
                salt.CopyTo(passwordWithSaltBytes, passwordBytes.Length);

                byte[] hash = passwordWithSaltBytes;
                for (int i = 0; i < Iterations; i++)
                {
                    hash = sha256.ComputeHash(hash);
                }

                return hash;
            }
        }
    }
}
