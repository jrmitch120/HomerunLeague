using System;
using System.Security.Cryptography;
using ServiceStack;

namespace HomerunLeague.ServiceInterface.Extensions
{
    public static class SecurityExtensions
    {
        public static string ToSha256String(this string password)
        {
            var sha256 = new SHA256Managed();
            var hashedPassword = string.Empty;

            byte[] bytes = password.ToUtf8Bytes();
            byte[] hashBytes = sha256.ComputeHash(bytes);

            foreach (var x in hashBytes)
                hashedPassword += $"{x:X2}";

            return hashedPassword.ToUpper();
        }
    }
}