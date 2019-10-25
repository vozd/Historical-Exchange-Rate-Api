using System;
using Xunit;
using Moq;
using HER.Repository;
using HER.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using HER.Controllers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HER.Test
{
    public class ExchangerateControllerShould
    {
        [Fact]
        public void ReturnExchangeRate()
        {
            string fordates = "2018-02-01, 2018-02-15, 2018-03-01";
            string rateCurrency = "SEK->NOK";

            Mock<IExchangerateRepository> mokRepo = new Mock<IExchangerateRepository>();
            Mock<ILogger<HerController>> mokLogger = new Mock<ILogger<HerController>>();

            var sut = new HerController(mokRepo.Object, mokLogger.Object);


            
            var res1 = new Rates
            {
                Cyrrency = "NOK",
                Date = Convert.ToDateTime("1.2.2018. 00:00:00"),
                Rate = 0.9762827706
            };

            var res2 = new Rates
            {
                Cyrrency = "NOK",
                Date = Convert.ToDateTime("15.2.2018. 00:00:00"),
                Rate = 0.9815486993
            };

            var res3 = new Rates
            {
                Cyrrency = "NOK",
                Date = Convert.ToDateTime("1.3.2018. 00:00:00"),
                Rate = 0.9546869595
            };

            var rates = new List<Rates> { res1,res2,res3};



            mokRepo.Setup(x => x.GetRratesFromApiAsync(fordates, rateCurrency)).Returns(() => Task<IEnumerable<Rates>>.Factory.StartNew(() => rates));


            var  result = sut.Get(fordates, rateCurrency) as OkObjectResult;


            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsType<ExchangeRateResult>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

    }
}
