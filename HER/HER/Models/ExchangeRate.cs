using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HER.Models
{
    public class ExchangeRate
    {
        public Dictionary<DateTime, Dictionary<string,double>> Rates { get; set; }
        public string Base { get; set; }
        public DateTime Start_at { get; set; }
        public DateTime End_at { get; set; }
    }


    public class Rates
    {
        public string Cyrrency { get; set; }
        public double Rate { get; set; }
        public DateTime Date { get; set; }
    }

}
