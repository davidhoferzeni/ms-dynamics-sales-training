using System.Reflection;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

public class DynamicsCrudLogic<TEntity> where TEntity : class, new()
{
    public DynamicsCrudLogic(DynamicsSession session)
    {
        _session = session;
    }

    protected DynamicsSession _session;
    private Type _entityType = typeof(TEntity);
    private string? TableName
    {
        get
        {
            return _entityType.GetCustomAttribute<DynamicsTableAttribute>()?.name;
        }
    }

    private ColumnSet ColumnQuerySet
    {
        get
        {
            var columnNames = _entityType.GetProperties()
                .Select(prop => prop.GetCustomAttribute<DynamicsColumnAttribute>())
                .Select(colAtt => colAtt?.name ?? string.Empty)
                .Where(colName => colName != String.Empty).ToArray();
            return new ColumnSet(columnNames);
        }
    }

    public List<TEntity>? GetList()
    {
        var query = BuildListQuery();
        var entityCollection = _session?.ServiceClient?.RetrieveMultiple(query);
        var entityList = entityCollection?.Entities.Select(Build);
        return entityList?.ToList();
    }

    public TEntity? GetById(Guid id)
    {
        var query = BuildListQuery();
        var dynamicsEntity = _session?.ServiceClient?.Retrieve(TableName, id, ColumnQuerySet);
        if(dynamicsEntity == null) {
            return null;
        }
        var entity = Build(dynamicsEntity);
        return entity;
    }

    public void Update(IEnumerable<TEntity> entitiesToSave)
    {
        var dynamicsEntitiesToSave = entitiesToSave.Select(BuildDynamics);
        foreach (var dynamicsEntityToSave in dynamicsEntitiesToSave)
        {
            // is there honestly no bulk update?!
            _session.ServiceClient?.Update(dynamicsEntityToSave);
        }
    }

    public void Delete(IEnumerable<TEntity> entitiesToDelete)
    {
        var dynamicsEntitiesToDelete = entitiesToDelete.Select(BuildDynamics);
        foreach (var dynamicsEntityToDelete in dynamicsEntitiesToDelete)
        {
            if (dynamicsEntityToDelete == null)
            {
                continue;
            }
            // is there honestly no bulk delete?!
            _session.ServiceClient?.Delete(dynamicsEntityToDelete.LogicalName, dynamicsEntityToDelete.Id);
        }
    }

    private TEntity Build(Entity dynamicsEntity)
    {
        var newEntity = new TEntity();
        var dynamicsGuidColumn = _entityType.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(DynamicsGuidAttribute), true));
        if (dynamicsGuidColumn == null)
        {
            return newEntity;
        }
        dynamicsGuidColumn.SetValue(newEntity, dynamicsEntity.Id);
        var dynamicsColumns = _entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute), false));
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dataflowTypeAttribute = dynamicsColumn.GetCustomAttribute<DynamicsDataflowAttribute>();
            if (dataflowTypeAttribute != null && dataflowTypeAttribute.dataflowType == DataflowType.Write)
            {
                continue;
            }
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
            if (dynamicsEntity.Contains(dynamicsCoumnName))
            {
                dynamicsColumn.SetValue(newEntity, dynamicsEntity[dynamicsCoumnName]);
            }
        }
        return newEntity;
    }

    private Entity? BuildDynamics(TEntity entity)
    {
        Type entityType = typeof(TEntity);
        var entityTableName = entityType.GetCustomAttribute<DynamicsTableAttribute>()?.name;
        if (entityTableName == null)
        {
            return null;
        }
        var dynamicsGuidColumn = entityType.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(DynamicsGuidAttribute), true));
        if (dynamicsGuidColumn == null)
        {
            return null;
        }
        var entityGuid = dynamicsGuidColumn.GetValue(entity);
        if (entityGuid == null)
        {
            return null;
        }
        var newDynamicsEntity = new Entity(entityTableName, (Guid)entityGuid);
        var dynamicsColumns = entityType.GetProperties().Where(prop => prop.IsDefined(typeof(DynamicsColumnAttribute), false));
        foreach (var dynamicsColumn in dynamicsColumns)
        {
            var dataflowTypeAttribute = dynamicsColumn.GetCustomAttribute<DynamicsDataflowAttribute>();
            if (dataflowTypeAttribute != null && dataflowTypeAttribute.dataflowType == DataflowType.Read)
            {
                continue;
            }
            var dynamicsColumnAttribute = dynamicsColumn.GetCustomAttribute<DynamicsColumnAttribute>();
            var dynamicsCoumnName = dynamicsColumnAttribute?.name;
            var dynamicsColumnValue = dynamicsColumn.GetValue(entity);
            if (dynamicsCoumnName == null || dynamicsColumnValue == null)
            {
                continue;
            }
            newDynamicsEntity.Attributes.Add(new KeyValuePair<string, object>(dynamicsCoumnName, dynamicsColumnValue));
        }
        return newDynamicsEntity;
    }

    private QueryExpression BuildListQuery()
    {
        var query = new QueryExpression() {
            EntityName = TableName,
            ColumnSet = ColumnQuerySet
        };
        return query;
    }

    private void SetTableQuery(QueryExpression query)
    {
        var entityTableName = _entityType.GetCustomAttribute<DynamicsTableAttribute>()?.name;
        if (entityTableName == null)
        {
            return;
        }
        query.EntityName = entityTableName;
    }
}
