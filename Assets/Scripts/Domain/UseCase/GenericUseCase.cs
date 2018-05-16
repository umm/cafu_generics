using CAFU.Core.Domain.UseCase;
using CAFU.Generics.Data.Entity;
using CAFU.Generics.Domain.Repository;
using JetBrains.Annotations;

namespace CAFU.Generics.Domain.UseCase
{
    [PublicAPI]
    public interface IGenericUseCase : IUseCase
    {
    }

    [PublicAPI]
    public interface IGenericUseCase<out TGenericEntity> : IGenericUseCase
        where TGenericEntity : IGenericEntity
    {
        TGenericEntity GetEntity();
    }

    [PublicAPI]
    public class GenericUseCase<TGenericEntity> : IGenericUseCase<TGenericEntity>
        where TGenericEntity : IGenericEntity
    {
        public class Factory : DefaultUseCaseFactory<GenericUseCase<TGenericEntity>>
        {
            protected override void Initialize(GenericUseCase<TGenericEntity> instance)
            {
                base.Initialize(instance);
                instance.GenericRepository = new GenericRepository<TGenericEntity>.Factory().Create();
            }
        }

        private IGenericRepository<TGenericEntity> GenericRepository { get; set; }

        public TGenericEntity GetEntity()
        {
            return GenericRepository.GetEntity();
        }
    }
}