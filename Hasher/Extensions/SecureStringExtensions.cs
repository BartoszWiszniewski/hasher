namespace Hasher.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    public static class SecureStringExtensions
    {
        public static byte[] AsByteArray(this SecureString secureString)
        {
            return Convert.FromBase64String(secureString.AsString());
        }

        public static byte[] AsByteArrayUTF8(this SecureString secureString)
        {
            return Encoding.UTF8.GetBytes(secureString.AsString());
        }

        public static char[] AsCharArray(this SecureString secureString)
        {
            return secureString.AsString().ToCharArray();
        }

        public static string AsString(this SecureString secureString)
        {
            var secureStringPtr = Marshal.SecureStringToBSTR(secureString);
            var str = Marshal.PtrToStringBSTR(secureStringPtr);
            Marshal.ZeroFreeBSTR(secureStringPtr);
            if (!secureString.IsReadOnly())
            {
                secureString.Clear();
            }

            return str;
        }
    }
}