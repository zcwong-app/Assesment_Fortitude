using Assesment_Fortitude.Model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Assesment_Fortitude.Helper
{
    public class Authentication
    {
        private static List<User> users = new List<User>() {
            new User() {  PartnerKey = "FAKEGOOGLE", PartnerRefNo = "FG-00001", PartnerPassword = "FAKEPASSWORD1234" },
            new User() {  PartnerKey = "FAKEPEOPLE", PartnerRefNo = "FG-00002", PartnerPassword = "FAKEPASSWORD4578" }
        };

        public static bool IsValid(string partnerKey, string partnerRefNo, string encodedPassword)
        {
            var user = users.Where(a => a.PartnerKey == partnerKey && a.PartnerRefNo == partnerRefNo
            && a.PartnerPassword == DecodeBase64(encodedPassword)).FirstOrDefault();

            if (user != null)
                return true;

            return false;
        }

        private static string DecodeBase64(string encodedPassword)
        {
            byte[] decodedBytes = Convert.FromBase64String(encodedPassword);
            return System.Text.Encoding.UTF8.GetString(decodedBytes);
        }
    }
}
