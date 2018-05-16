using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.Entity;
using JetBrains.Annotations;
using UnityEngine;

#pragma warning disable 649

namespace CAFU.Generics.Data.Entity
{
    [PublicAPI]
    public interface IGenericEntity : IEntity
    {
    }

    [PublicAPI]
    public interface IGenericEntity<TValue> : IGenericEntity
    {
        TValue Value { get; set; }
    }

    [PublicAPI]
    public interface IGenericPairEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public interface IGenericPairEntity<TKey> : IGenericPairEntity
    {
        TKey Key { get; set; }
    }

    [PublicAPI]
    public interface IGenericPairEntity<TKey, TValue> : IGenericPairEntity<TKey>
    {
        TValue Value { get; set; }
    }

    [PublicAPI]
    public interface IGenericListEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public interface IGenericListEntity<TValue> : IGenericListEntity
    {
        IList<TValue> List { get; set; }
    }

    [PublicAPI]
    public interface IGenericEntityList<TGenericEntity> : IGenericListEntity<TGenericEntity> where TGenericEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public interface IGenericPairEntityList<TGenericPairEntity> : IGenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity
    {
    }

    // 本当は abstract にしたいが、Serialize 出来なくなるので通常 class にする
    [Serializable]
    [PublicAPI]
    public class GenericEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public class GenericEntity<TValue> : GenericEntity, IGenericEntity<TValue>
    {
        [SerializeField] private TValue value;

        public TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    [PublicAPI]
    public class GenericPairEntity<TKey, TValue> : GenericEntity, IGenericPairEntity<TKey, TValue>
    {
        [SerializeField] private TKey key;

        public TKey Key
        {
            get { return key; }
            set { key = value; }
        }

        [SerializeField] private TValue value;

        public TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }

        // ScriptableObjectGenericPairEntity<TKey, TValue>.DummyForAOT() と併せるために書いたが、こっちは要らないはず
        private void DummyForAOT()
        {
            new GenericPairEntityList<ScriptableObjectGenericPairEntity<TKey, TValue>>().GetChildEntity((genericEntity) => true);
        }
    }

    [PublicAPI]
    public class GenericListEntity<TValue> : GenericEntity, IGenericListEntity
    {
        [SerializeField] private List<TValue> list;

        public IList<TValue> List
        {
            get { return list; }
            set { list = (List<TValue>) value; }
        }
    }

    [PublicAPI]
    public class GenericEntityList<TGenericEntity> : GenericListEntity<TGenericEntity>, IGenericEntityList<TGenericEntity> where TGenericEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public class GenericPairEntityList<TGenericPairEntity> : GenericEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity
    {
    }

    [PublicAPI]
    public class ScriptableObjectGenericEntity : ScriptableObject, IGenericEntity
    {
    }

    [PublicAPI]
    public class ScriptableObjectGenericEntity<TValue> : ScriptableObjectGenericEntity
    {
        [SerializeField] private TValue value;

        public TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    [PublicAPI]
    public class ScriptableObjectGenericPairEntity<TKey, TValue> : ScriptableObjectGenericEntity, IGenericPairEntity<TKey, TValue>
    {
        [SerializeField] private TKey key;

        public TKey Key
        {
            get { return key; }
            set { key = value; }
        }

        [SerializeField] private TValue value;

        public TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }

        // enum を受け取るメソッドは呼び出しコードが存在しないと AOT コンパイラがコードを生成してくれない
        // See also: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        private void DummyForAOT()
        {
            CreateInstance<ScriptableObjectGenericPairEntityList<ScriptableObjectGenericPairEntity<TKey, TValue>>>().GetChildEntity(Key);
        }
    }

    [PublicAPI]
    public class ScriptableObjectGenericListEntity<TValue> : ScriptableObjectGenericEntity, IGenericListEntity
    {
        [SerializeField] private List<TValue> list;

        public IList<TValue> List
        {
            get { return list; }
            set { list = (List<TValue>) value; }
        }
    }

    [PublicAPI]
    public class ScriptableObjectGenericEntityList<TGenericEntity> : ScriptableObjectGenericListEntity<TGenericEntity>, IGenericEntityList<TGenericEntity> where TGenericEntity : IGenericEntity
    {
    }

    [PublicAPI]
    public class ScriptableObjectGenericPairEntityList<TGenericPairEntity> : ScriptableObjectGenericEntityList<TGenericPairEntity>, IGenericPairEntityList<TGenericPairEntity> where TGenericPairEntity : IGenericPairEntity
    {
    }

    // XXX: 拡張メソッドではなく、各クラスに生やす手もある。コードが分散するが AOT 問題が起きる場合は有効かも？有効じゃないかも？
    [PublicAPI]
    public static class GenericEntityExtension
    {
        public static TGenericEntity GetChildEntity<TGenericEntity>(this IGenericEntityList<TGenericEntity> self, Func<TGenericEntity, bool> predicate)
            where TGenericEntity : IGenericEntity
        {
            return self.List.First(predicate);
        }

        public static TGenericPairEntity GetChildEntity<TGenericPairEntity, TKey>(this IGenericPairEntityList<TGenericPairEntity> self, TKey key)
            where TGenericPairEntity : IGenericPairEntity<TKey>
        {
            return self.List.First(x => x.Key.Equals(key));
        }
    }
}