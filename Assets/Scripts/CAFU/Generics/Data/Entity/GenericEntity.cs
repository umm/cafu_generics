using System;
using System.Collections.Generic;
using CAFU.Core.Data.Entity;
using UnityEngine;

// ReSharper disable ArrangeAccessorOwnerBody

#pragma warning disable 649

namespace CAFU.Generics.Data.Entity {

    public interface IGenericEntity : IEntity {

    }

    public interface IGenericPairEntity : IGenericEntity {

    }

    public interface IGenericListEntity : IGenericEntity {

    }

    // 本当は abstract にしたいが、Serialize 出来なくなるので通常 class にする
    [Serializable]
    public class GenericEntity : IGenericEntity {

    }

    public class GenericEntity<TValue> : GenericEntity {

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    public class GenericPairEntity<TKey, TValue> : GenericEntity, IGenericPairEntity {

        [SerializeField]
        private TKey key;

        public TKey Key {
            get {
                return this.key;
            }
        }

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    public class GenericListEntity<TValue> : GenericEntity, IGenericListEntity {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List => this.list;

    }

    public class GenericEntityList<TEntity> : GenericListEntity<TEntity> where TEntity : IGenericEntity {

    }

    public class GenericPairEntityList<TGenericPairEntity> : GenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

    [Obsolete("Please use 'GenericPairEntityList<TGenericPairEntity>' instead of thi class.")]
    public class GenericPairEntityList<TKey, TValue, TGenericPairEntity> : GenericPairEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

    public class ScriptableObjectGenericEntity : ScriptableObject, IGenericEntity {
    }

    public class ScriptableObjectGenericEntity<TValue> : ScriptableObjectGenericEntity {

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    public class ScriptableObjectGenericPairEntity<TKey, TValue> : ScriptableObjectGenericEntity, IGenericPairEntity {

        [SerializeField]
        private TKey key;

        public TKey Key {
            get {
                return this.key;
            }
        }

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    public class ScriptableObjectGenericListEntity<TValue> : ScriptableObjectGenericEntity, IGenericListEntity {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List => this.list;

    }

    public class ScriptableObjectGenericEntityList<TEntity> : ScriptableObjectGenericListEntity<TEntity> where TEntity : IGenericEntity {

    }

    public class ScriptableObjectGenericPairEntityList<TGenericPairEntity> : ScriptableObjectGenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

    [Obsolete("Please use 'GenericPairEntityList<TGenericPairEntity>' instead of thi class.")]
    public class ScriptableObjectGenericPairEntityList<TKey, TValue, TGenericPairEntity> : ScriptableObjectGenericPairEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

}