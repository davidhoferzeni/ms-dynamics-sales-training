using Microsoft.Xrm.Sdk.Query;

[System.AttributeUsage(System.AttributeTargets.Property)]  
public class DynamicsOrderAttribute : System.Attribute  
{  
    public OrderType orderType;  
  
    public DynamicsOrderAttribute(OrderType orderType = OrderType.Ascending)  
    {  
        this.orderType = orderType;
    }  
} 