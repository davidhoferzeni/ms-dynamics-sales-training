[System.AttributeUsage(System.AttributeTargets.Class)]  
public class DynamicsTableAttribute : System.Attribute  
{  
    public string name;  
  
    public DynamicsTableAttribute(string name)  
    {  
        this.name = name;
    }  
} 