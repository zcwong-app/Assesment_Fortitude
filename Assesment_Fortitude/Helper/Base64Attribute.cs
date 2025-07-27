namespace Assesment_Fortitude.Helper
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Base64Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true; 

            var strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                return true;  

            Span<byte> buffer = new Span<byte>(new byte[strValue.Length]);
            return Convert.TryFromBase64String(strValue, buffer, out _);
        }
    }
}
