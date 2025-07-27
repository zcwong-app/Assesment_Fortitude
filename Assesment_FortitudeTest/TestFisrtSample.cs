namespace Assesment_FortitudeTest
{
    using Assesment_Fortitude.Calculation;
    using Assesment_Fortitude.Controllers;
    using Assesment_Fortitude.Model;
    using Microsoft.AspNetCore.Mvc;
    using NUnit.Framework;

    [TestFixture]
    public class AsssesmentControllerTests
    {  

        [Test]
        public void SubmitTrxMessage_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RequestModel
            {
                PartnerKey = "FAKEGOOGLE",
                PartnerRefNo = "FG-00001",
                PartnerPassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                TotalAmount = 1000,
                Items = new List<ItemDetail>
                {
                    new ItemDetail { PartnerItemRef = "i-00001", Name = "Pen", Qty = 4, UnitPrice = 200 },
                    new ItemDetail { PartnerItemRef = "i-00002", Name = "Ruler", Qty = 2, UnitPrice = 100 }
                },
                TimeStamp = "2024-08-15T02:11:22.0000000Z",
                Sig = "MDE3ZTBkODg4ZDNhYzU0ZDBlZWRmNmU2NmUyOWRhZWU4Y2M1NzQ1OTIzZGRjYTc1ZGNjOTkwYzg2MWJlMDExMw=="
            };

            var controller = new AssesmentController();

            var result = controller.SubmitTrxMessage(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var response = result.Value as ResponseModel;
            Assert.IsNotNull(response);
            Assert.AreEqual(1000, response.TotalAmount);
            Assert.AreEqual(1, response.Result);
        }
    }
}