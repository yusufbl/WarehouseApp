namespace DataServices.Data
{
    public class CompanyInfo
    {
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int TaxOfficeId { get; set; }
        public string? CountryCode { get; set; }
    }
}
