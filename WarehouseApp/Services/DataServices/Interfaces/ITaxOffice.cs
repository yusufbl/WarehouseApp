using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface ITaxOffice
    {
        Task<List<TaxOfficeInfo>> GetTaxOfficeAsync();
        Task<bool> DeleteTaxOfficeAsync(string taxOfficeId);
        Task AddTaxOfficeAsync(TaxOfficeInfo taxOffice);
        Task<bool> UpdateTaxOfficeAsync(TaxOfficeInfo taxOffice);
    }
}