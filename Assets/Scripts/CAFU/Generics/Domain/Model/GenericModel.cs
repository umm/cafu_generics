using CAFU.Core.Domain.Model;

namespace CAFU.Generics.Domain.Model {

    public interface IGenericModel : IModel {



    }

    public interface IGenericModel<TKey, TValue> : IGenericModel {

        TKey Key { get; set; }

        TValue Value { get; set; }

    }

    public class GenericModel<TKey, TValue> : IGenericModel<TKey, TValue> {

        public TKey Key { get; set; }

        public TValue Value { get; set; }

    }

}