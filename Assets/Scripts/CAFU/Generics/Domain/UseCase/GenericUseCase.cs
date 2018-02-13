using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Domain.UseCase;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Model;
using CAFU.Generics.Domain.Repository;
using CAFU.Generics.Domain.Translator;

namespace CAFU.Generics.Domain.UseCase {

    public interface IGenericUseCase : IUseCase {

    }

    public interface IGenericUseCase<TKey, TValue, out TGenericEntity> : IGenericUseCase
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        IGenericModel<TKey, TValue> GetModel(TKey key);

        IList<IGenericModel<TKey, TValue>> GetModelList();

        IList<IGenericModel<TKey, TValue>> GetModelList(Predicate<TGenericEntity> predicate);

    }

    public class GenericUseCase<TKey, TValue, TGenericEntity> : IGenericUseCase<TKey, TValue, TGenericEntity>
        where TGenericEntity : IGenericEntity<TKey, TValue> {

        public class Factory : DefaultUseCaseFactory<GenericUseCase<TKey, TValue, TGenericEntity>> {

            protected override void Initialize(GenericUseCase<TKey, TValue, TGenericEntity> instance) {
                base.Initialize(instance);
                instance.GenericRepository = new GenericRepository<TKey, TValue, TGenericEntity>.Factory().Create();
                instance.GenericModelTranslator = new GenericModelTranslator<TKey, TValue, TGenericEntity, GenericModel<TKey, TValue>>.Factory().Create();
            }

        }

        private IGenericRepository<TKey, TValue, TGenericEntity> GenericRepository { get; set; }

        private IGenericModelTranslator<TGenericEntity, IGenericModel<TKey, TValue>> GenericModelTranslator { get; set; }

        public IGenericModel<TKey, TValue> GetModel(TKey key) {
            return this.GenericModelTranslator.Translate(this.GenericRepository.GetEntity(key));
        }

        public IList<IGenericModel<TKey, TValue>> GetModelList() {
            return this.GenericRepository.GetEntityList().Select(x => this.GenericModelTranslator.Translate(x)).ToList();
        }

        public IList<IGenericModel<TKey, TValue>> GetModelList(Predicate<TGenericEntity> predicate) {
            return this.GenericRepository.GetEntityList(predicate).Select(x => this.GenericModelTranslator.Translate(x)).ToList();
        }

    }

}