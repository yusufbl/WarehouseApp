using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class WarehouseServices : IWarehouse
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateWarehouseAsync(WarehouseInfo warehouse)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(warehouse, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                conditions.Add(new DbParameters
                {
                    ParameterName = "WAREHOUSE_ID",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = warehouse.WarehouseId
                });

                string tableName = "WAREHOUSE_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteWarehouseAsync(string warehouseId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM WAREHOUSE_TAB WHERE WAREHOUSE_ID = :WarehouseId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = warehouseId;

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

        public async Task<List<WarehouseInfo>> GetWarehouseAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<WarehouseInfo> warehouses = new List<WarehouseInfo>();
            string query = "SELECT WAREHOUSE_ID as WarehouseId, WAREHOUSE_NAME as WarehouseName FROM WAREHOUSE_TAB";
            string where = "";
            string orderBy = "";

            warehouses = Execute.Select<WarehouseInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return warehouses;
        }

        public async Task AddWarehouseAsync(WarehouseInfo warehouse)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO WAREHOUSE_TAB (WAREHOUSE_ID, WAREHOUSE_NAME) VALUES (:WarehouseId, :WarehouseName)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = warehouse.WarehouseId;
                        cmd.Parameters.Add(":WarehouseName", OracleDbType.Varchar2).Value = warehouse.WarehouseName;

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

