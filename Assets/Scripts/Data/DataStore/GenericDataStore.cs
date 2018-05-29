using System.Collections.Generic;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Utility;
using CAFU.Generics.Data.Entity;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace CAFU.Generics.Data.DataStore
{
    [PublicAPI]
    public interface IGenericDataStore : IDataStore
    {
    }

    [PublicAPI]
    public class GenericDataStore : ObservableLifecycleMonoBehaviour, IGenericDataStore
    {
        [SerializeField] private List<GenericEntity> genericEntityList;

        private IEnumerable<IGenericEntity> GenericEntityList => genericEntityList;

        [SerializeField] private List<ScriptableObjectGenericEntity> scriptableObjectGenericEntityList;

        private IEnumerable<IGenericEntity> ScriptableObjectGenericEntityList => scriptableObjectGenericEntityList;

        private IDataStoreResolver<IGenericEntityContainerDataStore> DataStoreResolver { get; } = new DefaultFactory<GenericEntityContainerDataStore.DefaultResolver>().Create();

        protected override void OnAwake()
        {
            base.OnAwake();
            foreach (var genericEntity in GenericEntityList)
            {
                DataStoreResolver.Resolve().Retain(genericEntity);
            }
            foreach (var scriptableObjectGenericEntity in ScriptableObjectGenericEntityList)
            {
                DataStoreResolver.Resolve().Retain(scriptableObjectGenericEntity);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var genericEntity in GenericEntityList)
            {
                DataStoreResolver.Resolve().Release(genericEntity);
            }
            foreach (var scriptableObjectGenericEntity in ScriptableObjectGenericEntityList)
            {
                DataStoreResolver.Resolve().Release(scriptableObjectGenericEntity);
            }
            genericEntityList.Clear();
            scriptableObjectGenericEntityList.Clear();
        }
    }
}