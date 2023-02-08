[System.AttributeUsage(System.AttributeTargets.Property)]  
public class DynamicsColumnAttribute : System.Attribute  
{  
    public string name;  
    public double version;  
  
    public DynamicsColumnAttribute(string name)  
    {  
        this.name = name;  
        version = 1.0;  
    }  
} 