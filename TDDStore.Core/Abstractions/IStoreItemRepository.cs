using System;
using System.Collections;
using System.Collections.Generic;

namespace TDDStore.Core.Abstractions
{
    public interface IStoreItemRepository
    {
        IEnumerable<StoreItem> GetAll();
        StoreItem GetOne(Guid id);
    }
}