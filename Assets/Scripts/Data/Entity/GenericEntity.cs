using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Data.Entity;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable Unity.RedundantSerializeFieldAttribute

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
    public class GenericEntity<TValue> : GenericEntity<TValue, TValue>
    {
    }

    [PublicAPI]
    public class GenericEntity<TValueInterface, TValueConcrete> : GenericEntity, IGenericEntity<TValueInterface>
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private TValueConcrete value;

        public TValueInterface Value
        {
            get { return value; }
            set { this.value = (TValueConcrete) value; }
        }
    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    [PublicAPI]
    public class GenericPairEntity<TKey, TValue> : GenericPairEntity<TKey, TValue, TKey, TValue>
    {
    }

    [PublicAPI]
    public class GenericPairEntity<TKeyInterface, TValueInterface, TKeyConcrete, TValueConcrete> : GenericEntity, IGenericPairEntity<TKeyInterface, TValueInterface>
        where TKeyConcrete : TKeyInterface
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private TKeyConcrete key;

        public TKeyInterface Key
        {
            get { return key; }
            set { key = (TKeyConcrete) value; }
        }

        [SerializeField] private TValueConcrete value;

        public TValueInterface Value
        {
            get { return value; }
            set { this.value = (TValueConcrete) value; }
        }

        // ScriptableObjectGenericPairEntity<TKey, TValue>.DummyForAOT() と併せるために書いたが、こっちは要らないはず
        private void DummyForAOT()
        {
            new GenericPairEntityList<ScriptableObjectGenericPairEntity<TKeyInterface, TValueInterface, TKeyConcrete, TValueConcrete>>().GetChildEntity((genericEntity) => true);
        }
    }

    [PublicAPI]
    public class GenericListEntity<TValue> : GenericListEntity<TValue, TValue>
    {
    }

    [PublicAPI]
    public class GenericListEntity<TValueInterface, TValueConcrete> : GenericEntity, IGenericListEntity
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private List<TValueConcrete> list;

        public IList<TValueInterface> List
        {
            get { return list.Cast<TValueInterface>().ToList(); }
            set { list = value.Cast<TValueConcrete>().ToList(); }
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
    public class ScriptableObjectGenericEntity<TValue> : ScriptableObjectGenericEntity<TValue, TValue>
    {
    }

    [PublicAPI]
    public class ScriptableObjectGenericEntity<TValueInterface, TValueConcrete> : ScriptableObjectGenericEntity, IGenericEntity<TValueInterface>
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private TValueConcrete value;

        public TValueInterface Value
        {
            get { return value; }
            set { this.value = (TValueConcrete) value; }
        }
    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    [PublicAPI]
    public class ScriptableObjectGenericPairEntity<TKey, TValue> : ScriptableObjectGenericPairEntity<TKey, TValue, TKey, TValue>
    {
    }

    // GenericEntity<TValue> を継承する手もあるが、Inspector 上で Key/Value の並びが逆になってしまうので断念
    [PublicAPI]
    public class ScriptableObjectGenericPairEntity<TKeyInterface, TValueInterface, TKeyConcrete, TValueConcrete> : ScriptableObjectGenericEntity, IGenericPairEntity<TKeyInterface, TValueInterface>
        where TKeyConcrete : TKeyInterface
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private TKeyConcrete key;

        public TKeyInterface Key
        {
            get { return key; }
            set { key = (TKeyConcrete) value; }
        }

        [SerializeField] private TValueConcrete value;

        public TValueInterface Value
        {
            get { return value; }
            set { this.value = (TValueConcrete) value; }
        }

        // enum を受け取るメソッドは呼び出しコードが存在しないと AOT コンパイラがコードを生成してくれない
        // See also: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        private void DummyForAOT()
        {
            CreateInstance<ScriptableObjectGenericPairEntityList<ScriptableObjectGenericPairEntity<TKeyInterface, TValueInterface, TKeyConcrete, TValueConcrete>>>().GetChildEntity(Key);
        }
    }

    [PublicAPI]
    public class ScriptableObjectGenericListEntity<TValue> : ScriptableObjectGenericListEntity<TValue, TValue>
    {
    }

    [PublicAPI]
    public class ScriptableObjectGenericListEntity<TValueInterface, TValueConcrete> : ScriptableObjectGenericEntity, IGenericListEntity<TValueInterface>
        where TValueConcrete : TValueInterface
    {
        [SerializeField] private List<TValueConcrete> list;

        public IList<TValueInterface> List
        {
            get { return list.Cast<TValueInterface>().ToList(); }
            set { list = value.Cast<TValueConcrete>().ToList(); }
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