using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class CountryServices : ICountry
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateCountryAsync(CountryInfo country)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(country, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                conditions.Add(new DbParameters
                {
                    ParameterName = "COUNTRY_CODE",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = country.CountryCode
                });

                string tableName = "COUNTRY_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteCountryAsync(string countryCode)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM COUNTRY_TAB WHERE COUNTRY_CODE = :CountryCode";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":CountryCode", OracleDbType.Varchar2).Value = countryCode;

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

        public async Task<List<CountryInfo>> GetCountryAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<CountryInfo> countries = new List<CountryInfo>();
            string query = "SELECT COUNTRY_CODE as CountryCode, COUNTRY_NAME as CountryName, IS_ACTIVE as IsActive FROM COUNTRY_TAB";
            string where = "";
            string orderBy = "";

            countries = Execute.Select<CountryInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return countries;
        }


        public async Task AddCountryAsync(CountryInfo country)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO COUNTRY_TAB (COUNTRY_CODE, COUNTRY_NAME, IS_ACTIVE) VALUES (:CountryCode, :CountryName, :IsActive)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":CountryCode", OracleDbType.Varchar2).Value = country.CountryCode;
                        cmd.Parameters.Add(":CountryName", OracleDbType.Varchar2).Value = country.CountryName;
                        cmd.Parameters.Add(":IsActive", OracleDbType.Varchar2).Value = country.IsActive;

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

