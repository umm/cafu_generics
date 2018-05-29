using System.Collections.Generic;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Domain.Repository;
using CAFU.Core.Utility;
using CAFU.Generics.Data.DataStore;
using CAFU.Generics.Data.Entity;
using JetBrains.Annotations;

namespace CAFU.Generics.Domain.Repository
{
    [PublicAPI]
    public interface IGenericRepository : IRepository
    {
    }

    [PublicAPI]
    public interface IGenericRepository<out TGenericEntity> : IGenericRepository
        where TGenericEntity : IGenericEntity
    {
        TGenericEntity GetEntity();

        IEnumerable<TGenericEntity> GetEntities();
    }

    [PublicAPI]
    public class GenericRepository<TGenericEntity> : GenericRepository<TGenericEntity, GenericEntityContainerDataStore.DefaultResolver>
        where TGenericEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public class GenericRepository<TGenericEntity, TGenericEntityContainerDataStoreResolver> : IGenericRepository<TGenericEntity>
        where TGenericEntity : IGenericEntity
        where TGenericEntityContainerDataStoreResolver : IDataStoreResolver<IGenericEntityContainerDataStore>, new()
    {
        public class Factory : DefaultRepositoryFactory<GenericRepository<TGenericEntity, TGenericEntityContainerDataStoreResolver>>
        {
            protected override void Initialize(GenericRepository<TGenericEntity, TGenericEntityContainerDataStoreResolver> instance)
            {
                base.Initialize(instance);
                instance.DataStoreResolver = new DefaultFactory<TGenericEntityContainerDataStoreResolver>().Create();
            }
        }

        private IDataStoreResolver<IGenericEntityContainerDataStore> DataStoreResolver { get; set; }

        public TGenericEntity GetEntity()
        {
            return DataStoreResolver.Resolve().GetEntity<TGenericEntity>();
        }

        public IEnumerable<TGenericEntity> GetEntities()
        {
            return DataStoreResolver.Resolve().GetEntities<TGenericEntity>();
        }
    }
}