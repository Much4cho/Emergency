using Microsoft.EntityFrameworkCore;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly EmergencyContext context;
        private DbSet<TEntity> entities;
        string errorMessage = string.Empty;

        public Repository(EmergencyContext context)
        {
            this.context = context;
            entities = context.Set<TEntity>();
        }
        public IAsyncEnumerable<TEntity> GetAll()
        {
            return entities.AsAsyncEnumerable();
        }

        public async Task<TEntity> Get(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}