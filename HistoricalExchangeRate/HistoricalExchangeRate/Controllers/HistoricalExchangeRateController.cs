using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoricalExchangeRate.Controllers
{
    [Route("historical-exchange-rate")]
    public class HistoricalExchangeRateController : ControllerBase
    {
        public object Get()
        {
            return new { Dragan = "Car", Dragana = "Carica" };
        }
    }
}
