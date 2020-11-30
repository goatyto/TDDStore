namespace TDDStore.Core.Abstractions
{
    public interface IUserCreditService
    {
        decimal GetUserCurrentBalance(string userId);
    }
}