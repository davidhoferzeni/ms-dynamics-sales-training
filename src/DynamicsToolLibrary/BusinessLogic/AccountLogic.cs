public class AccountHelper
{
    public AccountHelper() { }

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

