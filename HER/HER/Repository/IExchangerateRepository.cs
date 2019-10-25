using HER.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HER.Repository
{
    public interface IExchangerateRepository
    {
        Task<IEnumerable<Rates>> GetRratesFromApiAsync(string fordates, string rateCurrency);
    }
}