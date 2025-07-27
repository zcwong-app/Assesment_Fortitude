using Assesment_Fortitude.Helper;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Assesment_Fortitude.Model
{
    public class RequestModel : IValidatableObject
    {
        [Required(ErrorMessage = "PartnerKey is required.")]
        [MaxLength(50, ErrorMessage = "PartnerKey cannot be longer than 50 characters.")]
        public required string PartnerKey { get; set; }

        [MaxLength(50, ErrorMessage = "PartnerKey cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "PartnerRefNo is required.")]
        public required string PartnerRefNo { get; set; }

        [MaxLength(50, ErrorMessage = "PartnerKey cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "PartnerPassword is required.")]
        [Base64(ErrorMessage = "PartnerPassword must be a valid Base64 string.")]
        public required string PartnerPassword { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "TotalAmount must be positive.")]
        public long TotalAmount { get; set; }

        [Required(ErrorMessage = "Items cannot be null.")] 
        public required List<ItemDetail> Items { get; set; }

        [Required(ErrorMessage = "TimeStamp is required.")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}Z$", ErrorMessage = "TimeStamp must be in ISO 8601 format (e.g. 2024-08-15T02:11:22.0000000Z).")]
        public required string TimeStamp { get; set; }

        [Required(ErrorMessage = "Sig is required.")]
        //[StringLength(64, MinimumLength = 64, ErrorMessage = "Sig must be exactly 64 characters.")]
        public required string Sig { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsValidSignature())
            {
                yield return new ValidationResult("Unauthorized partner or Signature Mismatch.");
            }

            if (!DateTime.TryParseExact(
                TimeStamp,
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out var parsedTime))
            {
                yield return new ValidationResult("TimeStamp must be a valid ISO 8601 UTC datetime.");
            }
            else
            {
                var now = DateTime.Now;
                if (parsedTime < now.AddMinutes(-5) || parsedTime > now.AddMinutes(5))
                {
                    yield return new ValidationResult("Provided TimeStamp exceed server time +-5min.");
                }
            }

            if (Items != null && Items.Count > 0)
            {
                long sum = Items.Sum(i => (long)(i.UnitPrice * i.Qty));

                if (sum != TotalAmount)
                {
                    yield return new ValidationResult("The total value stated in itemDetails array not equal to value in TotalAmount.");
                }
            }
        }

        private bool IsValidSignature()
        {
            if (ConvertSig.Sha256HexThenBase64(this.TimeStamp + this.PartnerKey + this.PartnerRefNo + this.TotalAmount.ToString() + this.PartnerPassword) == this.Sig)
                return true;
            else return false; 
        }
    }

    public class ItemDetail
    {
        [Required(ErrorMessage = "PartnerItemRef is required.")]
        public required string PartnerItemRef { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Range(1, 5, ErrorMessage = "Qty must be between 1 to 5.")]
        [Required(ErrorMessage = "Qty is required.")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "UnitPrice is required.")]
        public long UnitPrice { get; set; }
    }

    public class RequestSigModel  
    { 
        public required string PartnerKey { get; set; } 
        public required string PartnerRefNo { get; set; } 
        public required string PartnerPassword { get; set; } 
        public long TotalAmount { get; set; } 
    }
}
