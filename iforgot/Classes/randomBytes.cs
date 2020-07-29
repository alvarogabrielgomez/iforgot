using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace iforgot.Classes
{
    public class randomBytes
    {
        public static string GenerateRandomBytes(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var hexString = BitConverter.ToString(randomBytes);
            
            hexString = hexString.Replace("-", "");
            return hexString;
        }
    }
}