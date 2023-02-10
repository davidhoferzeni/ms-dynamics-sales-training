using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Xrm.Sdk;

public static class DynamicsEntityBuilder<T> where T : class, IDynamicsEntity, new()
{
    public static T Build(Entity dynamicsEntity) {
        Type entityType = typeof(T);
        var newEntity = new T();
        var dynamicsGuidColumn = entityType.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(DynamicsGuidAttribute),true));
        if (dynamicsGuidColumn == null) {
            return newEntity;
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

    public static Entity? BuildDynamics(T entity) {
        Type entityType = typeof(T);
        var entityTableName = entityType.GetCustomAttribute<DynamicsTableAttribute>()?.name;
        if (entityTableName == null) {
            return null;
        }
        var dynamicsGuidColumn = entityType.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(DynamicsGuidAttribute),true));
        if (dynamicsGuidColumn == null) {
            return null;
        }
        var entityGuid = dynamicsGuidColumn.GetValue(entity);
        if (entityGuid == null) {
            return null;
        }
        var newDynamicsEntity = new Entity(entityTableName, (Guid)entityGuid);
        var dynamicsColumns = entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute),false));
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dataflowTypeAttribute = dynamicsColumn.GetCustomAttribute<DynamicsDataflowAttribute>();
            if (dataflowTypeAttribute != null && dataflowTypeAttribute.dataflowType == DataflowType.Read) {
                continue;
            }
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
            var dynamicsColumnValue = dynamicsColumn.GetValue(entity);
            if (dynamicsCoumnName == null || dynamicsColumnValue == null) {
                continue;
            }
            newDynamicsEntity.Attributes.Add(new KeyValuePair<string, object>(dynamicsCoumnName, dynamicsColumnValue));
        }
        return newDynamicsEntity;
    }
}

