namespace TDDStore.Core.Abstractions
{
    public interface IShoppingCardRepository
    {
        void SaveShoppingCart(ShoppingCart shoppingCart);
        ShoppingCart GetShoppingCartForUser(string userId);
    }
}