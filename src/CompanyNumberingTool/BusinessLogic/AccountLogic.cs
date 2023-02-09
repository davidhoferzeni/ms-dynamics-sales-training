

using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

public class AccountLogic
{
    public AccountLogic(DynamicsSession session)
    {
        _session = session;
    }

    private DynamicsSession _session;
    public static List<AccountEntity> Reindex(List<AccountEntity> accountEntities, uint startIndex = 1)
    {
        var orderedAccountEntities = accountEntities.OrderBy(a => a.AccountName);
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

    public List<AccountEntity> GetAccountEntities()
    {
        QueryExpression query = new QueryExpression()
        {
            EntityName = "account",
            ColumnSet = new ColumnSet(new string[] { "name", "new_accountindex" })
        };
        var accountCollection = _session?.ServiceClient?.RetrieveMultiple(query);
        var accountEntities = accountCollection?.Entities.Select(a => DynamicsEntityBuilder<AccountEntity>.Build(a))
        .ToList();

        return accountEntities;
    }

    public void UpdateAccountEntities(List<AccountEntity> entitiesToUpdate)
    {
        var dynamicsEntitiesToUpdate = entitiesToUpdate.Select(e => DynamicsEntityBuilder<AccountEntity>.BuildDynamics(e));
        foreach (var dynamicsEntityToUpdate in dynamicsEntitiesToUpdate)
        {
            // is there honestly no bulk update?!
            _session.ServiceClient.Update(dynamicsEntityToUpdate);
        }
    }
}

