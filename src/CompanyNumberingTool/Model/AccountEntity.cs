
public class AccountEntity : IDynamicsEntity
{
    [DynamicsColumn("name")]
    public string? AccountName { get; set; }
    [DynamicsColumn("accountid")]
    public Guid AccountId { get; set; }
    [DynamicsColumn("new_accountindex")]
    public uint? CurrentAccountIndex { get; set; }
    public uint? NewAccountIndex { get; set; }
}

