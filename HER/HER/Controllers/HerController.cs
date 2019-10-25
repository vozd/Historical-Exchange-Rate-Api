using HER.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using HER.Repository;
using Microsoft.Extensions.Logging;

namespace HER.Controllers
{
    [Route("/api/exchange-rate/{fordates}/{rateCurrency}")]
    public class HerController : ControllerBase
    {
        private readonly IExchangerateRepository _exchangeRateRepository;
        private readonly ILogger<HerController> _logger;

        public HerController(IExchangerateRepository exchangeRateRepository, ILogger<HerController> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Get(string fordates, string rateCurrency)
        {
            try
            {

                var rates = _exchangeRateRepository.GetRratesFromApiAsync(fordates, rateCurrency).Result;

                var max = rates.Select(x => x.Rate).Max();
                var min = rates.Select(x => x.Rate).Min();

                return Ok(new ExchangeRateResult
                {
                    MaximumExchangeRate = "A max rate of " + max + " on " + rates.First(x => x.Rate == max).Date.ToString("yyyy-MM-dd"),
                    MinimumExchangeRate = "A min rate of " + min + " on " + rates.First(x => x.Rate == min).Date.ToString("yyyy-MM-dd"),
                    AverageRate = "An average rate of " + rates.Select(x => x.Rate).Average().ToString()
                });
            }
            catch (Exception httpRequestException)
            {
                _logger.LogError($"Failed to get Exchange rate: {httpRequestException} ");
                return BadRequest($"Error getting exchangeRates from exchangerates api: {httpRequestException.Message}");
            }
        }
        
    }
}
