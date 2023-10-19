using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface IHallwayRowBin
    {
        Task<List<HallwayRowBinInfo>> GetHallwayRowBinAsync();
        Task<bool> DeleteHallwayRowBinAsync(string BinId);
        Task AddHallwayRowBinAsync(HallwayRowBinInfo hallwayRowBin);
        Task<bool> UpdateHallwayRowBinAsync(HallwayRowBinInfo hallwayRowBin);
    }
}