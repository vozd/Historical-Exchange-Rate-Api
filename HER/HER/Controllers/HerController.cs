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

namespace HER.Controllers
{
    [Route("/api/exchange-rate/{fordates}/{rateCurrency}")]
    public class HerController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Get(string fordates, string rateCurrency)
        {
            // geting parameters from route

            var dates = fordates.Split(',');
            var currency = rateCurrency.Split("->");

            
            List<DateTime> Listdates = dates.Select(date => DateTime.Parse(date)).ToList();

            using (var client = new HttpClient())
            {
                try
                {

                    var rCurrency = currency[0];
                    var bCurrency = currency[1];

                    //getting data from api
                    var apiUri = "/history?start_at=" + Listdates.Min().ToString("yyyy-MM-dd") + "&end_at=" + Listdates.Max().ToString("yyyy-MM-dd") + "&symbols=" + rCurrency + "&base=" + bCurrency;

                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync(apiUri);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    var exchangeRates = JsonConvert.DeserializeObject<ExchangeRate>(stringResult);

                    var rates = exchangeRates.Rates.Where(x => Listdates.Contains(x.Key.Date));

                    var onlyRates = rates.Select(x => new Rates
                    {
                        Cyrrency = x.Value.Keys.First().ToString(),
                        Rate = x.Value.Values.First(),
                        Date = x.Key
                    });

                    var max = onlyRates.Select(x => x.Rate).Max();
                    var min = onlyRates.Select(x => x.Rate).Min();

                    return Ok(new ExchangeRateResult
                    {
                        MaximumExchangeRate = "A max rate of " + max + " on " + onlyRates.First(x => x.Rate == max).Date.ToString("yyyy-MM-dd"),
                        MinimumExchangeRate = "A min rate of " + min + " on " + onlyRates.First(x => x.Rate == min).Date.ToString("yyyy-MM-dd"),
                        AverageRate = "An average rate of " + onlyRates.Select(x => x.Rate).Average().ToString()
                    });


                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting exchangeRates from exchangerates api: {httpRequestException.Message}");
                }
            }

        }

    }
}
