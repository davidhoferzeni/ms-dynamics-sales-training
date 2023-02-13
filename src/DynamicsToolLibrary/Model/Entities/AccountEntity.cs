using System.Text.Json.Serialization;

[DynamicsTable("account")]
public class AccountEntity : IDynamicsEntity
{
    [DynamicsGuid]
    public Guid AccountId { get; set; }
    
    [DynamicsColumn("name")]
    public string? AccountName { get; set; }
    
    [DynamicsColumn("new_accountindex")]
    [DynamicsDataflow(DataflowType.Read)]
    public int? CurrentAccountIndex { get; set; }

    [DynamicsColumn("new_accountindex")]
    [DynamicsDataflow(DataflowType.Write)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NewAccountIndex { get; set; }
}

