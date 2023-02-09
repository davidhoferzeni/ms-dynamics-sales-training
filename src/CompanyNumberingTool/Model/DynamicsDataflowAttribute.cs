[System.AttributeUsage(System.AttributeTargets.Property)]  
public class DynamicsDataflowAttribute : System.Attribute  
{  
    public DataflowType dataflowType;
  
    public DynamicsDataflowAttribute(DataflowType flowType)  
    {  
        this.dataflowType = flowType;
    }  
} 