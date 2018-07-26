namespace Hasher.Extensions
{
    using System;
    using System.Security;

    public static class BytesExtensions
    {
        public static SecureString AsSecureString(this byte[] bytes)
        {
            var chars = new char[(int)(Math.Ceiling((double)bytes.Length / 3) * 4)];
            Convert.ToBase64CharArray(bytes, 0, bytes.Length, chars, 0);
            var secureString = new SecureString();

            foreach (var c in chars)
            {
                secureString.AppendChar(c);
            }

            Array.Clear(chars, 0, chars.Length);
            Array.Clear(bytes, 0, bytes.Length);

            secureString.MakeReadOnly();
            return secureString;
        }
    }
}