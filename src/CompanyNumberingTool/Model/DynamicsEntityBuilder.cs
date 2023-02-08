using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Xrm.Sdk;

public static class DynamicsEntityBuilder<T> where T : class, IDynamicsEntity, new()
{
    public static T? Build(Entity dynamicsEntity) {
        Type entityType = typeof(T);
        var dynamicsColumns = entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute),false));
        var newEntity = new T();
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
            if (dynamicsEntity.Contains(dynamicsCoumnName)) {
                dynamicsColumn.SetValue(newEntity, dynamicsEntity[dynamicsCoumnName]);
            }
        }
        return newEntity;
    }
}

