using System.Collections.Generic;
using CAFU.Core.Data.Entity;
using UnityEngine;

#pragma warning disable 649

namespace CAFU.Generics.Data.Entity {

    public interface IGenericEntity : IEntity {

    }

    public interface IGenericEntity<out TKey, out TValue> : IGenericEntity {

        TKey Key { get; }

        TValue Value { get; }

    }

    // AOT 問題回避のため、 TKey, TValue を渡しています
    public interface IGenericListEntity<TKey, TValue, TGenericEntity> : IGenericEntity
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        IList<TGenericEntity> List { get; }

    }

    public class GenericEntity<TKey, TValue> : IGenericEntity<TKey, TValue> {

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

    public class ScriptableObjectGenericEntity<TKey, TValue> : ScriptableObject, IGenericEntity<TKey, TValue> {

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

    public class GenericListEntity<TKey, TValue, TGenericEntity> : IGenericListEntity<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        [SerializeField]
        private List<TGenericEntity> list;

        public IList<TGenericEntity> List {
            get {
                return this.list;
            }
        }

    }

    public class ScriptableObjectGenericListEntity<TKey, TValue, TGenericEntity> : ScriptableObject, IGenericListEntity<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        [SerializeField]
        private List<TGenericEntity> list;

        public IList<TGenericEntity> List {
            get {
                return this.list;
            }
        }

    }

}