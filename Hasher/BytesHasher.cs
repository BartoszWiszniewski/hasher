namespace Hasher
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    public class BytesHasher
    {
        private const int HashIterations = 64;
        private const int SaltLength = 64;
        private const string HashingAlgorithm = "SHA-512";

        public void Hash(byte[] bytes, out byte[] hashedBytes, out byte[] salt)
        {
            salt = this.CreateSalt();
            hashedBytes = this.Hash(bytes, salt);
        }

        public bool HashesMatch(byte[] hashedBytes, byte[] clearBytes, byte[] salt)
        {
            var hashedPwdBytes = hashedBytes;
            var clearPwdHashedBytes = this.Hash(clearBytes, salt);
            return hashedPwdBytes.SequenceEqual(clearPwdHashedBytes);
        }

        private byte[] Hash(byte[] bytes, byte[] salt)
        {
            var pwdBytes = this.GetSaltedBytes(bytes, salt);
            using (var hashAlgorithm = this.GetHashAlgorithm())
            {
                for (var i = 0; i < HashIterations; i++)
                {
                    pwdBytes = hashAlgorithm.ComputeHash(pwdBytes);
                }
            }

            return pwdBytes;
        }

        private byte[] GetSaltedBytes(byte[] bytes, byte[] salt)
        {
            var saltedPwdBytes = new byte[bytes.Length + salt.Length];

            Array.Copy(bytes, 0, saltedPwdBytes, 0, bytes.Length);
            Array.Copy(salt, 0, saltedPwdBytes, bytes.Length, salt.Length);

            return saltedPwdBytes;
        }

        private byte[] CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[SaltLength];
            rng.GetBytes(salt);

            return salt;
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