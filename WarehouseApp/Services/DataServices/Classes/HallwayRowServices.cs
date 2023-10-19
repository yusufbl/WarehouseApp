using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class HallwayRowServices : IHallwayRow
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateHallwayRowAsync(HallwayRowInfo hallwayRow)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(hallwayRow, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                conditions.Add(new DbParameters
                {
                    ParameterName = "ROW_NO",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = hallwayRow.RowNo
                });

                string tableName = "HALLWAY_ROW_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteHallwayRowAsync(string rowNo)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM HALLWAY_ROW_TAB WHERE ROW_NO = :RowNo";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":RowNo", OracleDbType.Varchar2).Value = rowNo;

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

        public async Task<List<HallwayRowInfo>> GetHallwayRowAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<HallwayRowInfo> hallwayRows = new List<HallwayRowInfo>();
            string query = "SELECT WAREHOUSE_ID as WarehouseId, HALLWAY_ID as HallwayId, ROW_NO as RowNo FROM HALLWAY_ROW_TAB";
            string where = "";
            string orderBy = "";

            hallwayRows = Execute.Select<HallwayRowInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return hallwayRows;
        }

        public async Task AddHallwayRowAsync(HallwayRowInfo hallwayRow)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO HALLWAY_ROW_TAB (WAREHOUSE_ID, HALLWAY_ID, ROW_NO) VALUES (:WarehouseId, :HallwayId, :RowNo)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = hallwayRow.WarehouseId;
                        cmd.Parameters.Add(":HallwayId", OracleDbType.Varchar2).Value = hallwayRow.HallwayId;
                        cmd.Parameters.Add(":RowNo", OracleDbType.Varchar2).Value = hallwayRow.RowNo;
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

