using Restpirators.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Repository.Repositories.Abstract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        public IAsyncEnumerable<TEntity> GetAll();
        public Task<TEntity> Get(int id);
        public Task Insert(TEntity entity);
        public Task Update(TEntity entity);
        public Task SaveChanges();
    }
}
