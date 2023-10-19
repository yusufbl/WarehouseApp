using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface ICountry
    {
        Task<List<CountryInfo>> GetCountryAsync();
        Task<bool> DeleteCountryAsync(string countryId);
        Task AddCountryAsync(CountryInfo country);
        Task<bool> UpdateCountryAsync(CountryInfo country);
    }
}