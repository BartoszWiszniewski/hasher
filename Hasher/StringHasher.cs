namespace Hasher
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class StringHasher
    {
        private const int HashIterations = 64;
        private const int SaltLength = 64;
        private const string HashingAlgorithm = "SHA-512";

        public void Hash(string strToHash, out string hashedStr, out string salt)
        {
            salt = this.CreateSalt();
            hashedStr = this.Hash(strToHash, salt);
        }

        public bool HashesMatch(string hashedStr, string clearStr, string salt)
        {
            var hashedStrBytes = Convert.FromBase64String(hashedStr);
            var clearStrHashedBytes = Convert.FromBase64String(this.Hash(clearStr, salt));
            return hashedStrBytes.SequenceEqual(clearStrHashedBytes);
        }

        private string Hash(string str, string salt)
        {
            var strBytes = this.GetSaltedPassword(str, salt);

            using (var hashAlgorithm = this.GetHashAlgorithm())
            {
                for (var i = 0; i < HashIterations; i++)
                {
                    strBytes = hashAlgorithm.ComputeHash(strBytes);
                }
            }

            return Convert.ToBase64String(strBytes);
        }

        private byte[] GetSaltedPassword(string str, string salt)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            var saltBytes = Convert.FromBase64String(salt);
            var saltedStrBytes = new byte[strBytes.Length + saltBytes.Length];

            Array.Copy(strBytes, 0, saltedStrBytes, 0, strBytes.Length);
            Array.Copy(saltBytes, 0, saltedStrBytes, strBytes.Length, saltBytes.Length);

            return saltedStrBytes;
        }

        private string CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[SaltLength];
            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }

        private HashAlgorithm GetHashAlgorithm()
        {
            // HashAlgorithm.Create fails on .Net core <= 2.0.0
#if NETCOREAPP2_1
            return HashAlgorithm.Create(HashingAlgorithm);
#endif

            return (HashAlgorithm)CryptoConfig.CreateFromName(HashingAlgorithm);
        }
    }
}