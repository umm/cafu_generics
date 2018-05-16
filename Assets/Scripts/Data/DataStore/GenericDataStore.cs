using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.DataStore;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;
using UniRx;
using UnityEngine;

// ReSharper disable UseStringInterpolation

namespace CAFU.Generics.Data.DataStore
{
    public interface IGenericDataStore : IDataStore
    {
        TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity;
    }

    public class GenericDataStore : ObservableLifecycleMonoBehaviour, IGenericDataStore
    {
        [SerializeField] private List<GenericEntity> genericEntityList;

        private IEnumerable<IGenericEntity> GenericEntityList
        {
            get { return this.genericEntityList; }
        }

        [SerializeField] private List<ScriptableObjectGenericEntity> scriptableObjectGenericEntityList;

        private IEnumerable<IGenericEntity> ScriptableObjectGenericEntityList
        {
            get { return this.scriptableObjectGenericEntityList; }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            GenericRepository.GenericDataStore = this;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.genericEntityList.Clear();
            this.scriptableObjectGenericEntityList.Clear();
        }

        public TGenericEntity GetEntity<TGenericEntity>() where TGenericEntity : IGenericEntity
        {
            if (this.GenericEntityList.Any(x => x is TGenericEntity))
            {
                return this.GenericEntityList.OfType<TGenericEntity>().First();
            }

            if (this.ScriptableObjectGenericEntityList.Any(x => x is TGenericEntity))
            {
                return this.ScriptableObjectGenericEntityList.OfType<TGenericEntity>().First();
            }

            throw new InvalidOperationException(string.Format("Type of '{0}' does not found in GenericDataStore", typeof(TGenericEntity).FullName));
        }
    }
}