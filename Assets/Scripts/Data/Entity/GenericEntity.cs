using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.Entity;
using UnityEngine;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable UnusedTypeParameter

#pragma warning disable 649

namespace CAFU.Generics.Data.Entity {

    public interface IGenericEntity : IEntity {

    }

    public interface IGenericEntity<TValue> : IGenericEntity {

        TValue Value { get; set; }

    }

    public interface IGenericPairEntity : IGenericEntity {

    }

    public interface IGenericPairEntity<TKey> : IGenericPairEntity {

        TKey Key { get; set; }

    }

    public interface IGenericPairEntity<TKey, TValue> : IGenericPairEntity<TKey> {

        TValue Value { get; set; }

    }

    public interface IGenericListEntity : IGenericEntity {

    }

    public interface IGenericListEntity<TValue> : IGenericListEntity {

        IList<TValue> List { get; set; }

    }

    public interface IGenericEntityList<TGenericEntity> : IGenericListEntity<TGenericEntity> where TGenericEntity : IGenericEntity {

    }

    public interface IGenericPairEntityList<TGenericPairEntity> : IGenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

    // 本当は abstract にしたいが、Serialize 出来なくなるので通常 class にする
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
            set {
                this.value = value;
            }
        }

    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    public class GenericPairEntity<TKey, TValue> : GenericEntity, IGenericPairEntity<TKey, TValue> {

        [SerializeField]
        private TKey key;

        public TKey Key {
            get {
                return this.key;
            }
            set {
                this.key = value;
            }
        }

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
            }
        }

        // ScriptableObjectGenericPairEntity<TKey, TValue>.DummyForAOT() と併せるために書いたが、こっちは要らないはず
        private void DummyForAOT() {
            new GenericPairEntityList<ScriptableObjectGenericPairEntity<TKey, TValue>>().GetChildEntity((genericEntity) => true);
        }

    }

    public class GenericListEntity<TValue> : GenericEntity, IGenericListEntity {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List {
            get {
                return this.list;
            }
            set {
                this.list = (List<TValue>)value;
            }
        }

    }

    public class GenericEntityList<TGenericEntity> : GenericListEntity<TGenericEntity>, IGenericEntityList<TGenericEntity> where TGenericEntity : IGenericEntity {

    }

    public class GenericPairEntityList<TGenericPairEntity> : GenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

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
            set {
                this.value = value;
            }
        }

    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    public class ScriptableObjectGenericPairEntity<TKey, TValue> : ScriptableObjectGenericEntity, IGenericPairEntity<TKey, TValue> {

        [SerializeField]
        private TKey key;

        public TKey Key {
            get {
                return this.key;
            }
            set {
                this.key = value;
            }
        }

        [SerializeField]
        private TValue value;

        public TValue Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
            }
        }

        // enum を受け取るメソッドは呼び出しコードが存在しないと AOT コンパイラがコードを生成してくれない
        // See also: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        private void DummyForAOT() {
            CreateInstance<ScriptableObjectGenericPairEntityList<ScriptableObjectGenericPairEntity<TKey, TValue>>>().GetChildEntity(this.Key);
        }

    }

    public class ScriptableObjectGenericListEntity<TValue> : ScriptableObjectGenericEntity, IGenericListEntity {

        [SerializeField]
        private List<TValue> list;

        public IList<TValue> List {
            get {
                return this.list;
            }
            set {
                this.list = (List<TValue>)value;
            }
        }

    }

    public class ScriptableObjectGenericEntityList<TGenericEntity> : ScriptableObjectGenericListEntity<TGenericEntity>, IGenericEntityList<TGenericEntity> where TGenericEntity : IGenericEntity {

    }

    public class ScriptableObjectGenericPairEntityList<TGenericPairEntity> : ScriptableObjectGenericEntityList<TGenericPairEntity>, IGenericPairEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity {

    }

    // XXX: 拡張メソッドではなく、各クラスに生やす手もある。コードが分散するが AOT 問題が起きる場合は有効かも？有効じゃないかも？
    public static class GenericEntityExtension {

        public static TGenericEntity GetChildEntity<TGenericEntity>(this IGenericEntityList<TGenericEntity> self, Func<TGenericEntity, bool> predicate)
            where TGenericEntity : IGenericEntity {
            return self.List.First(predicate);
        }

        public static TGenericPairEntity GetChildEntity<TGenericPairEntity, TKey>(this IGenericPairEntityList<TGenericPairEntity> self, TKey key)
            where TGenericPairEntity : IGenericPairEntity<TKey> {
            return self.List.First(x => x.Key.Equals(key));
        }

    }

}