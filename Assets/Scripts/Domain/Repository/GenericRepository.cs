using CAFU.Core.Domain.Repository;
using CAFU.Generics.Data.DataStore;
using CAFU.Generics.Data.Entity;
using JetBrains.Annotations;

namespace CAFU.Generics.Domain.Repository
{
    [PublicAPI]
    public interface IGenericRepository : IRepository
    {
    }

    [PublicAPI]
    public interface IGenericRepository<out TGenericEntity> : IGenericRepository
        where TGenericEntity : IGenericEntity
    {
        TGenericEntity GetEntity();
    }

    [PublicAPI]
    public class GenericRepository : IGenericRepository
    {
        public static IGenericDataStore GenericDataStore { protected get; set; }
    }

    [PublicAPI]
    public class GenericRepository<TGenericEntity> : GenericRepository, IGenericRepository<TGenericEntity>
        where TGenericEntity : IGenericEntity
    {
        public class Factory : DefaultRepositoryFactory<GenericRepository<TGenericEntity>>
        {
        }

        public TGenericEntity GetEntity()
        {
            return GenericDataStore.GetEntity<TGenericEntity>();
        }
    }
}