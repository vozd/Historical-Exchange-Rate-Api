using HER.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HER.Repository
{
    public class ExchangerateRepository : IExchangerateRepository
    {
        private readonly ILogger<ExchangerateRepository> _logger;

        public ExchangerateRepository(ILogger<ExchangerateRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Rates>> GetRratesFromApiAsync(string fordates, string rateCurrency)
        {
            // geting parameters from route
            var dates = fordates.Split(',');
            var currency = rateCurrency.Split("->");

            List<DateTime> listDates = dates.Select(date => DateTime.Parse(date)).ToList();

            using (var client = new HttpClient())
            {

                try
                {
                    var rCurrency = currency[1];
                    var bCurrency = currency[0];

                    //getting data from api
                    var apiUri = $"/history?start_at=" + listDates.Min().ToString("yyyy-MM-dd") + "&end_at=" + listDates.Max().ToString("yyyy-MM-dd") + "&symbols=" + rCurrency + "&base=" + bCurrency;

                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync(apiUri);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    return GetRatesExchangeForDatesByStringResult(stringResult, listDates);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to get data from api: {ex} ");
                    throw;
                }
            }

        }


        private IEnumerable<Rates> GetRatesExchangeForDatesByStringResult(string stringResult, List<DateTime> listDates)
        {
            try
            {
                var exchangeRates = JsonConvert.DeserializeObject<ExchangeRate>(stringResult);

                var exchangeRatesByDateList = exchangeRates.Rates.Where(x => listDates.Contains(x.Key.Date));

                var rates = exchangeRatesByDateList.Select(x => new Rates
                {
                    Cyrrency = x.Value.Keys.First().ToString(),
                    Rate = x.Value.Values.First(),
                    Date = x.Key
                });

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get rates exchange for dates by result of api: {ex} ");
                throw;
            }
            
        }
    }
}
