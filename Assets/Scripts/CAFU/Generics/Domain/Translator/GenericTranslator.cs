using CAFU.Core.Domain.Translator;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Model;

namespace CAFU.Generics.Domain.Translator {

    public interface IGenericModelTranslator<in TGenericEntity, out TGenericModel> : IModelTranslator<TGenericEntity, TGenericModel>
        where TGenericEntity : IGenericEntity
        where TGenericModel : IGenericModel {

    }

    public class GenericModelTranslator<TKey, TValue, TGenericEntity, TGenericModel> : IGenericModelTranslator<TGenericEntity, TGenericModel>
        where TGenericEntity : IGenericEntity<TKey, TValue>
        where TGenericModel : IGenericModel<TKey, TValue>, new() {

        public class Factory : DefaultTranslatorFactory<GenericModelTranslator<TKey, TValue, TGenericEntity, TGenericModel>> {

        }

        public TGenericModel Translate(TGenericEntity entity) {
            // ココも Zenject 対応したい
            return new TGenericModel() {
                Key = entity.Key,
                Value = entity.Value,
            };
        }

    }

}