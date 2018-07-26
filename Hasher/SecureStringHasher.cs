namespace Hasher
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Security.Cryptography;

    using Hasher.Extensions;

    public class SecureStringHasher
    {
        private const int HashIterations = 64;
        private const int SaltLength = 64;
        private const string HashingAlgorithm = "SHA-512";

        public void Hash(SecureString secureString, out SecureString hashedSecureString, out SecureString salt)
        {
            salt = this.CreateSalt();
            hashedSecureString = this.Hash(secureString, salt);
        }

        public bool HashesMatch(SecureString hashedSecureString, SecureString clearSecureString, SecureString salt)
        {
            var hashedStrBytes = hashedSecureString.AsByteArray();
            var clearStrHashedBytes = this.Hash(clearSecureString, salt).AsByteArray();
            return hashedStrBytes.SequenceEqual(clearStrHashedBytes);
        }

        private SecureString Hash(SecureString secureString, SecureString salt)
        {
            var pwdBytes = this.GetSaltedBytes(secureString, salt);
            using (var hashAlgorithm = this.GetHashAlgorithm())
            {
                for (var i = 0; i < HashIterations; i++)
                {
                    pwdBytes = hashAlgorithm.ComputeHash(pwdBytes);
                }
            }

            return pwdBytes.AsSecureString();
        }

        private byte[] GetSaltedBytes(SecureString secureString, SecureString salt)
        {
            var secureStringBytes = secureString.AsByteArrayUTF8();
            var saltBytes = salt.AsByteArray();
            var saltedPwdBytes = new byte[secureStringBytes.Length + saltBytes.Length];

            Array.Copy(secureStringBytes, 0, saltedPwdBytes, 0, secureStringBytes.Length);
            Array.Copy(saltBytes, 0, saltedPwdBytes, secureStringBytes.Length, saltBytes.Length);

            return saltedPwdBytes;
        }

        private SecureString CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[SaltLength];
            rng.GetBytes(salt);

            return salt.AsSecureString();
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