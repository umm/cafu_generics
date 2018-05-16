using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.DataStore;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace CAFU.Generics.Data.DataStore
{
    [PublicAPI]
    public interface IGenericDataStore : IDataStore
    {
        TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity;
    }

    [PublicAPI]
    public class GenericDataStore : ObservableLifecycleMonoBehaviour, IGenericDataStore
    {
        [SerializeField] private List<GenericEntity> genericEntityList;

        private IEnumerable<IGenericEntity> GenericEntityList => genericEntityList;

        [SerializeField] private List<ScriptableObjectGenericEntity> scriptableObjectGenericEntityList;

        private IEnumerable<IGenericEntity> ScriptableObjectGenericEntityList => scriptableObjectGenericEntityList;

        protected override void OnAwake()
        {
            base.OnAwake();
            GenericRepository.GenericDataStore = this;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            genericEntityList.Clear();
            scriptableObjectGenericEntityList.Clear();
        }

        public TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity
        {
            if (GenericEntityList.Any(x => x is TGenericEntity))
            {
                return GenericEntityList.OfType<TGenericEntity>().First();
            }

            if (ScriptableObjectGenericEntityList.Any(x => x is TGenericEntity))
            {
                return ScriptableObjectGenericEntityList.OfType<TGenericEntity>().First();
            }

            throw new InvalidOperationException($"Type of '{typeof(TGenericEntity).FullName}' does not found in GenericDataStore");
        }
    }
}