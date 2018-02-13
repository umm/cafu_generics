using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.DataStore;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;
using UniRx;
using UnityEngine;

// ReSharper disable UseStringInterpolation

namespace CAFU.Generics.Data.DataStore {

    public interface IGenericDataStore : IDataStore {

        TGenericEntity GetEntity<TGenericEntity>(bool checkStrict);

    }

    public class GenericDataStore : ObservableLifecycleMonoBehaviour, IGenericDataStore {

        [SerializeField]
        private List<GenericEntity> genericEntityList;

        private IEnumerable<IGenericEntity> GenericEntityList => this.genericEntityList;

        [SerializeField]
        private List<ScriptableObjectGenericEntity> scriptableObjectGenericEntityList;

        private IEnumerable<IGenericEntity> ScriptableObjectGenericEntityList => this.scriptableObjectGenericEntityList;

        protected override void OnAwake() {
            base.OnAwake();
            GenericRepository.GenericDataStore = this;
        }

        public TGenericEntity GetEntity<TGenericEntity>(bool checkStrict) {
            if (this.GenericEntityList.Any(x => x is TGenericEntity)) {
                return this.GenericEntityList.OfType<TGenericEntity>().First();
            }
            if (this.ScriptableObjectGenericEntityList.Any(x => x is TGenericEntity)) {
                return this.ScriptableObjectGenericEntityList.OfType<TGenericEntity>().First();
            }
            if (checkStrict) {
                throw new InvalidOperationException(string.Format("Type of '{0}' does not found in GenericDataStore", typeof(TGenericEntity).FullName));
            }
            return default(TGenericEntity);
        }

    }

}