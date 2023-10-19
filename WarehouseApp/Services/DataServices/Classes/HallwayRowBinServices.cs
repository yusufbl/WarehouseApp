using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Mer.Data.Core.Db;
using DataServices.Data;
using Mer.Data.Core.Models;


public class HallwayRowBinServices : IHallwayRowBin
    {
        private readonly string _connectionString = "...";

        public async Task<bool> UpdateHallwayRowBinAsync(HallwayRowBinInfo hallwayRowBin)
        {
            try
            {
                OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
                List<DbParameters> updateParameters = Helper.CreateParameterFromClass(hallwayRowBin, ParameterDirections.In);
                List<DbParameters> conditions = new List<DbParameters>();

                conditions.Add(new DbParameters
                {
                    ParameterName = "BIN_ID",
                    ParameterDirection = ParameterDirections.In,
                    ParameterDataType = ParameterDataTypes.Varchar2,
                    ParameterValue = hallwayRowBin.HallwayId
                });

                string tableName = "HALLWAY_ROW_BIN_TAB";

                ProcessResult result = Execute.Update(tableName, updateParameters, conditions, connectionStringBuilder);

                return result.Success;
            }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


        public async Task<bool> DeleteHallwayRowBinAsync(string binId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM HALLWAY_ROW_BIN_TAB WHERE BIN_ID = :BinId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add(":BinId", OracleDbType.Varchar2).Value = binId;

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

        public async Task<List<HallwayRowBinInfo>> GetHallwayRowBinAsync()
        {
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder(_connectionString);
            List<HallwayRowBinInfo> hallwayRowBins = new List<HallwayRowBinInfo>();
            string query = "SELECT WAREHOUSE_ID as WarehouseId, HALLWAY_ID as HallwayId, ROW_NO as RowNo, BIN_ID as BinId FROM HALLWAY_ROW_BIN_TAB";
            string where = "";
            string orderBy = "";

            hallwayRowBins = Execute.Select<HallwayRowBinInfo>(query, where, orderBy, connectionStringBuilder).ToList();
            return hallwayRowBins;
        }

        public async Task AddHallwayRowBinAsync(HallwayRowBinInfo hallwayRowBin)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO HALLWAY_ROW_BIN_TAB (WAREHOUSE_ID, HALLWAY_ID, ROW_NO, BIN_ID) VALUES (:WarehouseId, :HallwayId, :RowNo, :BinId)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(":WarehouseId", OracleDbType.Varchar2).Value = hallwayRowBin.WarehouseId;
                        cmd.Parameters.Add(":HallwayId", OracleDbType.Varchar2).Value = hallwayRowBin.HallwayId;
                        cmd.Parameters.Add(":RowNo", OracleDbType.Varchar2).Value = hallwayRowBin.RowNo;
                        cmd.Parameters.Add(":BinId", OracleDbType.Varchar2).Value = hallwayRowBin.BinId;
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

