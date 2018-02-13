using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.DataStore;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;
using UniRx;
using UnityEngine;

namespace CAFU.Generics.Data.DataStore {

    public interface IGenericDataStore : IDataStore {

    }

    public interface IGenericDataStore<in TKey, TValue, TGenericEntity> : IGenericDataStore
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        TGenericEntity GetEntity(TKey key);

        IList<TGenericEntity> GetEntityList();

        IList<TGenericEntity> GetEntityList(Predicate<TGenericEntity> predecate);

    }

    public abstract class GenericDataStore : ObservableLifecycleMonoBehaviour {

    }

    public abstract class GenericDataStoreSingle<TKey, TValue, TGenericEntity> : GenericDataStore, IGenericDataStore<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        public class Factory : SceneDataStoreFactory<GenericDataStoreSingle<TKey, TValue, TGenericEntity>> {

        }

        [SerializeField]
        private TGenericEntity genericEntity;

        private TGenericEntity GenericEntity {
            get {
                return this.genericEntity;
            }
        }

        public TGenericEntity GetEntity(TKey key) {
            return this.GenericEntity;
        }

        public IList<TGenericEntity> GetEntityList() {
            return new List<TGenericEntity>() {
                this.GenericEntity,
            };
        }

        public IList<TGenericEntity> GetEntityList(Predicate<TGenericEntity> predecate) {
            return this.GetEntityList().Where(x => predecate(x)).ToList();
        }

        protected override void OnAwake() {
            base.OnAwake();
            GenericRepository<TKey, TValue, TGenericEntity>.DataStoreFactory = new Factory();
        }

    }

    public abstract class GenericDataStoreMultiple<TKey, TValue, TGenericEntity, TGenericListEntity> : GenericDataStore, IGenericDataStore<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue>
        where TGenericListEntity : IGenericListEntity<TKey, TValue, TGenericEntity> {

        public class Factory : SceneDataStoreFactory<GenericDataStoreMultiple<TKey, TValue, TGenericEntity, TGenericListEntity>> {

        }


        [SerializeField]
        private TGenericListEntity genericListEntity;

        private TGenericListEntity GenericListEntity {
            get {
                return this.genericListEntity;
            }
        }

        public TGenericEntity GetEntity(TKey key) {
            return this.GenericListEntity.List.First(x => x.Key.Equals(key));
        }

        public IList<TGenericEntity> GetEntityList() {
            return this.GenericListEntity.List;
        }

        public IList<TGenericEntity> GetEntityList(Predicate<TGenericEntity> predecate) {
            return this.GetEntityList().Where(x => predecate(x)).ToList();
        }

        protected override void OnAwake() {
            base.OnAwake();
            GenericRepository<TKey, TValue, TGenericEntity>.DataStoreFactory = new Factory();
        }

    }

}