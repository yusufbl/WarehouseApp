using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class HallwayServices : IHallway
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateHallwayAsync(HallwayInfo hallway)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(hallway, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                conditions.Add(new DbParameters
                {
                    ParameterName = "HALLWAY_ID",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = hallway.HallwayId
                });

                string tableName = "HALLWAY_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteHallwayAsync(string hallwayId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM HALLWAY_TAB WHERE HALLWAY_ID = :HallwayId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":HallwayId", OracleDbType.Varchar2).Value = hallwayId;

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

        public async Task<List<HallwayInfo>> GetHallwayAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<HallwayInfo> hallways = new List<HallwayInfo>();
            string query = "SELECT WAREHOUSE_ID as WarehouseId, HALLWAY_ID as HallwayId FROM HALLWAY_TAB";
            string where = "";
            string orderBy = "";

            hallways = Execute.Select<HallwayInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return hallways;
        }

        public async Task AddHallwayAsync(HallwayInfo hallway)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO HALLWAY_TAB (WAREHOUSE_ID, HALLWAY_ID) VALUES (:WarehouseId, :HallwayId)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = hallway.WarehouseId;
                        cmd.Parameters.Add(":HallwayId", OracleDbType.Varchar2).Value = hallway.HallwayId;

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

