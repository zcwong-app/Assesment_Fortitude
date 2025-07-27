using System;
using System.Security.Cryptography;
using System.Text;

namespace Assesment_Fortitude.Helper
{
    public class ConvertSig
    { 
        public static string Sha256HexThenBase64(string input)
        { 
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                 
                var sb = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                    sb.AppendFormat("{0:x2}", b);  

                string hexString = sb.ToString();
                 
                byte[] hexBytes = Encoding.UTF8.GetBytes(hexString);
                string base64String = Convert.ToBase64String(hexBytes);

                return base64String;
            } 
        }
    }
}
