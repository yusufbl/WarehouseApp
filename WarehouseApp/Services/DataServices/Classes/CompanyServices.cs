using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class CompanyServices : ICompany
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateCompanyAsync(CompanyInfo company)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(company, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                
                conditions.Add(new DbParameters
                {
                    ParameterName = "COMPANY_ID",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = company.CompanyId
                });

                string tableName = "COMPANY_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteCompanyAsync(string companyId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM COMPANY_TAB WHERE COMPANY_ID = :CompanyId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":CompanyId", OracleDbType.Varchar2).Value = companyId;

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<List<CompanyInfo>> GetCompanyAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<CompanyInfo> companies = new List<CompanyInfo>();
            string query = "SELECT COMPANY_ID as CompanyId, COMPANY_NAME as CompanyName, TAX_OFFICE_ID as TaxOfficeId, COUNTRY_CODE as CountryCode FROM COMPANY_TAB";
            string where = "";
            string orderBy = "";

            companies = Execute.Select<CompanyInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return companies;
        }

        public async Task AddCompanyAsync(CompanyInfo company)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO COMPANY_TAB (COMPANY_ID, COMPANY_NAME, TAX_OFFICE_ID, COUNTRY_CODE) VALUES (:CompanyId, :CompanyName, :TaxOfficeId, :CountryCode)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = company.CompanyId;
                        cmd.Parameters.Add(":WarehouseName", OracleDbType.Varchar2).Value = company.CompanyName;
                        cmd.Parameters.Add(":TaxOfficeId", OracleDbType.Decimal).Value = company.TaxOfficeId;
                        cmd.Parameters.Add(":CountryCode", OracleDbType.Varchar2).Value = company.CountryCode;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }

