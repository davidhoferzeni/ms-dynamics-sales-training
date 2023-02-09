
public class AccountEntity : IDynamicsEntity
{
    [DynamicsGuid]
    public Guid AccountId { get; set; }
    [DynamicsColumn("name")]
    public string? AccountName { get; set; }
    [DynamicsColumn("new_accountindex")]
    [DynamicsDataflow(DataflowType.Read)]
    public uint? CurrentAccountIndex { get; set; }
    [DynamicsColumn("new_accountindex")]
    [DynamicsDataflow(DataflowType.Write)]
    public uint? NewAccountIndex { get; set; }
}

