using CAFU.Core.Domain.Repository;
using CAFU.Generics.Data.DataStore;
using CAFU.Generics.Data.Entity;

namespace CAFU.Generics.Domain.Repository {

    public interface IGenericRepository : IRepository {

    }

    public interface IGenericRepository<out TGenericEntity> : IGenericRepository
        where TGenericEntity : IGenericEntity {

        TGenericEntity GetEntity(bool checkStrict = true);

    }

    public class GenericRepository : IGenericRepository {

        public static IGenericDataStore GenericDataStore { protected get; set; }

    }

    public class GenericRepository<TGenericEntity> : GenericRepository, IGenericRepository<TGenericEntity>
        where TGenericEntity : IGenericEntity {

        public class Factory : DefaultRepositoryFactory<GenericRepository<TGenericEntity>> {

        }

        public TGenericEntity GetEntity(bool checkStrict = true) {
            return GenericDataStore.GetEntity<TGenericEntity>(checkStrict);
        }

    }

}