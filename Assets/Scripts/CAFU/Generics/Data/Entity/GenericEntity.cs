using System;
using System.Collections.Generic;
using CAFU.Core.Data.Entity;
using UnityEngine;

// ReSharper disable ArrangeAccessorOwnerBody

#pragma warning disable 649

namespace CAFU.Generics.Data.Entity {

    public interface IGenericEntity : IEntity {

    }

    public interface IGenericEntity<out TValue> : IGenericEntity {

        TValue Value { get; }

    }

    public interface IGenericEntity<out TKey, out TValue> : IGenericEntity<TValue> {

        TKey Key { get; }

    }

    public interface IGenericPairEntity<out TKey, out TValue> : IGenericEntity {

        TKey Key { get; }

        TValue Value { get; }

    }

    public interface IGenericListEntity<TValue> : IGenericEntity {

        IList<TValue> List { get; }

    }

    [Serializable]
    public class GenericEntity : IGenericEntity {

    }

    public class GenericEntity<TValue> : GenericEntity, IGenericEntity<TValue> {

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    public class GenericPairEntity<TKey, TValue> : GenericEntity, IGenericPairEntity<TKey, TValue> {

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

    public class GenericListEntity<TValue> : GenericEntity, IGenericListEntity<TValue> {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List => this.list;

    }

    public class GenericEntityList<TEntity> : GenericListEntity<TEntity> where TEntity : IGenericEntity {

    }

    public class GenericPairEntityList<TKey, TValue, TGenericPairEntity> : GenericListEntity<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity<TKey, TValue> {

    }

    public abstract class ScriptableObjectGenericEntity : ScriptableObject, IGenericEntity {
    }

    public class ScriptableObjectGenericEntity<TValue> : ScriptableObjectGenericEntity, IGenericEntity<TValue> {

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
        }

    }

    public class ScriptableObjectGenericPairEntity<TKey, TValue> : ScriptableObjectGenericEntity, IGenericPairEntity<TKey, TValue> {

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

    public class ScriptableObjectGenericListEntity<TValue> : ScriptableObjectGenericEntity, IGenericListEntity<TValue> {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List => this.list;

    }

    public class ScriptableObjectGenericEntityList<TEntity> : ScriptableObjectGenericListEntity<TEntity> where TEntity : IGenericEntity {

    }

    public class ScriptableObjectGenericPairEntityList<TKey, TValue, TGenericPairEntity> : ScriptableObjectGenericListEntity<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity<TKey, TValue> {

    }

}