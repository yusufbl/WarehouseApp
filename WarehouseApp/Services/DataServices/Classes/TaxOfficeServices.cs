using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using DataServices.Data;
using Mer.Data.Core.Models;
using Mer.Data.Core.Db;

public class TaxOfficeServices : ITaxOffice
{
    private readonly string _connectionString = "...";

    public async Task<bool> UpdateTaxOfficeAsync(TaxOfficeInfo taxOffice)
    {
        try
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string updateQuery = "UPDATE TAX_OFFICE_TAB SET TAX_OFFICE_NAME = :TaxOfficeName, COUNTRY_CODE = :CountryCode WHERE TAX_OFFICE_ID = :TaxOfficeId";

                using (OracleCommand cmd = new OracleCommand(updateQuery, connection))
                {
                    cmd.Parameters.Add(":TaxOfficeName", OracleDbType.Varchar2).Value = taxOffice.TaxOfficeName;
                    cmd.Parameters.Add(":CountryCode", OracleDbType.Varchar2).Value = taxOffice.CountryCode;
                    cmd.Parameters.Add(":TaxOfficeId", OracleDbType.Varchar2).Value = taxOffice.TaxOfficeId;

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }
        catch (OracleException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteTaxOfficeAsync(string TaxOfficeId)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            try
            {
                await connection.OpenAsync();

                string deleteQuery = "DELETE FROM TAX_OFFICE_TAB WHERE TAX_OFFICE_ID = :TaxOfficeId";

                using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                {
                    cmd.Parameters.Add(":TaxOfficeId", OracleDbType.Varchar2).Value = TaxOfficeId;

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

    public async Task<List<TaxOfficeInfo>> GetTaxOfficeAsync()
    {
        OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
        List<TaxOfficeInfo> taxOffices = new List<TaxOfficeInfo>();
        string query = "SELECT TAX_OFFICE_ID, TAX_OFFICE_NAME, COUNTRY_CODE FROM TAX_OFFICE_TAB";
        string where = "";
        string orderBy = "";

        taxOffices = Execute.Select<TaxOfficeInfo>(query, where, orderBy, connectionStringBuilder).ToList();
        return taxOffices;
    }

    public async Task AddTaxOfficeAsync(TaxOfficeInfo taxOffice)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            try
            {
                await connection.OpenAsync();

                string insertQuery = "INSERT INTO TAX_OFFICE_TAB (TAX_OFFICE_ID, TAX_OFFICE_NAME, COUNTRY_CODE) VALUES (:TaxOfficeId, :TaxOfficeName, :CountryCode)";

                using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                {
                    cmd.Parameters.Add(":TaxOfficeId", OracleDbType.Decimal).Value = taxOffice.TaxOfficeId;
                    cmd.Parameters.Add(":TaxOfficeName", OracleDbType.Varchar2).Value = taxOffice.TaxOfficeName;
                    cmd.Parameters.Add(":CountryCode", OracleDbType.Varchar2).Value = taxOffice.CountryCode;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
