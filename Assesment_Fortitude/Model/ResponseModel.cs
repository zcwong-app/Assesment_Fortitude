namespace Assesment_Fortitude.Model
{
    public class ResponseModel
    { 
        public int Result { get; set; }
        public long TotalAmount { get; set; }
        public long TotalDiscount { get; set; }
        public long FinalAmount { get; set; }
        public string ResultMesssage { get; set; }
    }
}
