using Assesment_Fortitude.Calculation;
using Assesment_Fortitude.Helper;
using Assesment_Fortitude.Model;
using Microsoft.AspNetCore.Mvc;

namespace Assesment_Fortitude.Controllers
{
    [Route("api")]
    [ApiController]
    public class AssesmentController : ControllerBase
    {
        [HttpPost("SubmitTrxMessage")]
        public IActionResult SubmitTrxMessage(RequestModel req)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseModel res = new();
            res.TotalDiscount = TotalAmountCalculation.CalculateDiscount(req.TotalAmount);
            res.TotalAmount = req.TotalAmount;
            res.Result = 1;
            res.FinalAmount = req.TotalAmount - (req.TotalAmount * res.TotalDiscount / 100); 
            return Ok(res);
        }

        [HttpPost("SampleSig")]
        public string SampleSig(RequestSigModel req)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            return 
                ConvertSig.Sha256HexThenBase64(timeStamp + req.PartnerKey + req.PartnerRefNo + req.TotalAmount.ToString() + req.PartnerPassword) 
                + ";" + timeStamp;
        }
    }
}
