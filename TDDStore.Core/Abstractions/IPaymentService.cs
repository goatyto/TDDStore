using System;

namespace TDDStore.Core.Abstractions
{
    public interface IPaymentService
    {
        Guid Pay(string userId, Guid shoppingCartId);
    }
}