using Microsoft.Xrm.Sdk.Query;

public class AccountLogic : DynamicsCrudLogic<AccountEntity>
{
    public AccountLogic(DynamicsSession session) : base(session) { }

    public static List<AccountEntity> Reindex(List<AccountEntity> accountEntities, uint startIndex = 1)
    {
        foreach (var (account, index) in accountEntities.Select((a, i) => (a, i)))
        {
            if (account == null)
            {
                continue;
            }
            account.NewAccountIndex = (int?)((int?)index + startIndex);
        }
        return accountEntities;
    }
}

