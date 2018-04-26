using System;
using CAFU.Core.Domain.UseCase;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;

namespace CAFU.Generics.Domain.UseCase {

    public interface IGenericUseCase : IUseCase {

    }

    public interface IGenericUseCase<out TGenericEntity> : IGenericUseCase
        where TGenericEntity : IGenericEntity {

        TGenericEntity GetEntity();

        [Obsolete("Please use overload method has no arguments.")]
        TGenericEntity GetEntity(bool checkStrict);

    }

    public class GenericUseCase<TGenericEntity> : IGenericUseCase<TGenericEntity>
        where TGenericEntity : IGenericEntity {

        public class Factory : DefaultUseCaseFactory<GenericUseCase<TGenericEntity>> {

            protected override void Initialize(GenericUseCase<TGenericEntity> instance) {
                base.Initialize(instance);
                instance.GenericRepository = new GenericRepository<TGenericEntity>.Factory().Create();
            }

        }

        private IGenericRepository<TGenericEntity> GenericRepository { get; set; }

        public TGenericEntity GetEntity() {
            return this.GenericRepository.GetEntity();
        }

        [Obsolete("Please use overload method has no arguments.")]
        public TGenericEntity GetEntity(bool checkStrict) {
            return this.GetEntity();
        }

    }

}