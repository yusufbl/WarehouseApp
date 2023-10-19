using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface ICompany
    {
        Task<List<CompanyInfo>> GetCompanyAsync();
        Task<bool> DeleteCompanyAsync(string companyId);
        Task AddCompanyAsync(CompanyInfo company);
        Task<bool> UpdateCompanyAsync(CompanyInfo company);
    }
}