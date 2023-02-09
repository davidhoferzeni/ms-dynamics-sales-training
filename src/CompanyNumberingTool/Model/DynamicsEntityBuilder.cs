using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Xrm.Sdk;

public static class DynamicsEntityBuilder<T> where T : class, IDynamicsEntity, new()
{
    public static T? Build(Entity dynamicsEntity) {
        Type entityType = typeof(T);
        var newEntity = new T();
        var dynamicsGuidColumn = entityType.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(DynamicsGuidAttribute),true));
        if (dynamicsGuidColumn == null) {
            return null;
        }
        dynamicsGuidColumn.SetValue(newEntity, dynamicsEntity.Id);
        var dynamicsColumns = entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute),false));
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dataflowTypeAttribute = dynamicsColumn.GetCustomAttribute<DynamicsDataflowAttribute>();
            if (dataflowTypeAttribute != null && dataflowTypeAttribute.dataflowType == DataflowType.Write) {
                continue;
            }
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
            if (dynamicsEntity.Contains(dynamicsCoumnName)) {
                dynamicsColumn.SetValue(newEntity, dynamicsEntity[dynamicsCoumnName]);
            }
        }
        return newEntity;
    }

    public static Entity Build(T entity) {
        Type entityType = typeof(T);
        var dynamicsColumns = entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute),false));
        var newEntity = new Entity();
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dataflowTypeAttribute = dynamicsColumn.GetCustomAttribute<DynamicsDataflowAttribute>();
            if (dataflowTypeAttribute != null && dataflowTypeAttribute.dataflowType == DataflowType.Write) {
                continue;
            }
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
        }
        return newEntity;
    }
}

