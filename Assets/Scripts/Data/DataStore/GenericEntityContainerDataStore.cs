using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Utility;
using CAFU.Generics.Data.Entity;

namespace CAFU.Generics.Data.DataStore
{
    public interface IGenericEntityContainerDataStore : IDataStore
    {
        void Retain(IGenericEntity genericEntity);

        void Release(IGenericEntity genericEntity);

        TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity;

        IEnumerable<TGenericEntity> GetEntities<TGenericEntity>() where TGenericEntity : IGenericEntity;
    }

    public class GenericEntityContainerDataStore : IGenericEntityContainerDataStore, ISingleton
    {
        public class Factory<TDataStore> : DefaultDataStoreFactory<TDataStore> where TDataStore : IDataStore, IGenericEntityContainerDataStore, new()
        {
        }

        public class DefaultResolver : IDataStoreResolver<GenericEntityContainerDataStore>, ISingleton
        {
            public GenericEntityContainerDataStore Resolve()
            {
                return new Factory<GenericEntityContainerDataStore>().Create();
            }
        }

        private Dictionary<IGenericEntity, int> ReferenceCountMap { get; } = new Dictionary<IGenericEntity, int>();

        public void Retain(IGenericEntity genericEntity)
        {
            if (!ReferenceCountMap.ContainsKey(genericEntity))
            {
                ReferenceCountMap[genericEntity] = 0;
            }

            ReferenceCountMap[genericEntity]++;
        }

        public void Release(IGenericEntity genericEntity)
        {
            if (!ReferenceCountMap.ContainsKey(genericEntity))
            {
                return;
            }

            ReferenceCountMap[genericEntity]--;
            if (ReferenceCountMap[genericEntity] <= 0)
            {
                ReferenceCountMap.Remove(genericEntity);
            }
        }

        public TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity
        {
            if (!ReferenceCountMap.Keys.Any(x => x is TGenericEntity))
            {
                throw new InvalidOperationException($"Type of '{typeof(TGenericEntity).FullName}' does not found in GenericDataStore");
            }

            return (TGenericEntity) ReferenceCountMap.Keys.First(x => x is TGenericEntity);
        }

        public IEnumerable<TGenericEntity> GetEntities<TGenericEntity>() where TGenericEntity : IGenericEntity
        {
            if (!ReferenceCountMap.Keys.Any(x => x is TGenericEntity))
            {
                throw new InvalidOperationException($"Type of '{typeof(TGenericEntity).FullName}' does not found in GenericDataStore");
            }

            return ReferenceCountMap.Keys.OfType<TGenericEntity>();
        }
    }
}