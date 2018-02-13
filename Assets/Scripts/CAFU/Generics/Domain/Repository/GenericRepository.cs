using System;
using System.Collections.Generic;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Domain.Repository;
using CAFU.Generics.Data.DataStore;
using CAFU.Generics.Data.Entity;

namespace CAFU.Generics.Domain.Repository {

    public interface IGenericRepository<in TKey, TValue, TGenericEntity> : IRepository
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        TGenericEntity GetEntity(TKey key);

        IList<TGenericEntity> GetEntityList();

        IList<TGenericEntity> GetEntityList(Predicate<TGenericEntity> predicate);

    }

    public class GenericRepository<TKey, TValue, TGenericEntity> : IGenericRepository<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        public static IDataStoreFactory<IGenericDataStore<TKey, TValue, TGenericEntity>> DataStoreFactory { private get; set; }

        public class Factory : DefaultRepositoryFactory<GenericRepository<TKey, TValue, TGenericEntity>> {

            protected override void Initialize(GenericRepository<TKey, TValue, TGenericEntity> instance) {
                base.Initialize(instance);
                instance.GenericDataStore = DataStoreFactory.Create();
            }

        }

        private IGenericDataStore<TKey, TValue, TGenericEntity> GenericDataStore { get; set; }

        public TGenericEntity GetEntity(TKey key) {
            return this.GenericDataStore.GetEntity(key);
        }

        public IList<TGenericEntity> GetEntityList() {
            return this.GenericDataStore.GetEntityList();
        }

        public IList<TGenericEntity> GetEntityList(Predicate<TGenericEntity> predicate) {
            return this.GenericDataStore.GetEntityList(predicate);
        }

    }

}