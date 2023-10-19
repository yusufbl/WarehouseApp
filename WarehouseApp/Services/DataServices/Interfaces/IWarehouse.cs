using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServices.Data
{
    public interface IWarehouse
    {
        Task<List<WarehouseInfo>> GetWarehouseAsync();
        Task<bool> DeleteWarehouseAsync(string warehouseId);
        Task AddWarehouseAsync(WarehouseInfo warehouse);
        Task<bool> UpdateWarehouseAsync(WarehouseInfo warehouse);
    }
}
