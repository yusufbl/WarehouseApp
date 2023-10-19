using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface IHallway
    {
        Task<List<HallwayInfo>> GetHallwayAsync();
        Task<bool> DeleteHallwayAsync(string hallwayId);
        Task AddHallwayAsync(HallwayInfo hallway);
        Task<bool> UpdateHallwayAsync(HallwayInfo hallway);
    }
}