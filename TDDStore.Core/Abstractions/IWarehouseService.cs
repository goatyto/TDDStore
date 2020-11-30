using System;

namespace TDDStore.Core.Abstractions
{
    public interface IWarehouseService
    {
        int GetInventoryCount(Guid storeItemId);
    }
}