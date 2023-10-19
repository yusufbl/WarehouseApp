using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataServices.Data
{
    public interface IHallwayRow
    {
        Task<List<HallwayRowInfo>> GetHallwayRowAsync();
        Task<bool> DeleteHallwayRowAsync(string RowNo);
        Task AddHallwayRowAsync(HallwayRowInfo hallwayRow);
        Task<bool> UpdateHallwayRowAsync(HallwayRowInfo hallwayRow);
    }
}