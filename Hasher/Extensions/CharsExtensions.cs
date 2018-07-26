namespace Hasher.Extensions
{
    using System;
    using System.Security;

    public static class CharsExtensions
    {
        public static SecureString AsSecureString(this char[] chars)
        {
            var secureString = new SecureString();

            foreach (var c in chars)
            {
                secureString.AppendChar(c);
            }

            Array.Clear(chars, 0, chars.Length);

            secureString.MakeReadOnly();
            return secureString;
        }
    }
}