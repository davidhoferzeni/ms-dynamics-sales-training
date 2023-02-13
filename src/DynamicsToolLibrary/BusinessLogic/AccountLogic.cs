using Microsoft.Xrm.Sdk.Query;

public class AccountLogic : DynamicsCrudLogic<AccountEntity>
{
    public AccountLogic(DynamicsSession session) : base(session) { }

    public static List<AccountEntity> Reindex(List<AccountEntity> accountEntities, uint startIndex = 1)
    {
        var orderedAccountEntities = accountEntities.OrderBy(a => a.AccountName).ToList();
        foreach (var (account, index) in orderedAccountEntities.Select((a, i) => (a, i)))
        {
            if (account == null)
            {
                continue;
            }
            account.NewAccountIndex = (int?)((int?)index + startIndex);
        }
        return orderedAccountEntities;
    }
}

