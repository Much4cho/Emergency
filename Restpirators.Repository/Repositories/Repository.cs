using Microsoft.EntityFrameworkCore;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Emergency> GetEmergencyByTeam(int id)
        {
            return await context.Emergencies.Where(x => x.Status == Common.Enums.EmergencyStatus.TeamSent && x.AssignedToTeamId == id).FirstOrDefaultAsync();
        }

        public async Task<EmergencyDto> GetEmergencyByIdentifier(string identifier)
        {
            return await (from e in context.Emergencies
                          join t in context.Teams on e.AssignedToTeamId equals t.Id
                          join tp in context.EmergencyTypes on e.EmergencyTypeId equals tp.Id
                          where e.Identifier == identifier
                          orderby e.ReportTime descending
                          select new EmergencyDto()
                          {
                              Description = e.Description,
                              TeamLocation = t.Location,
                              TeamName = t.Name,
                              ReportDate = e.ReportTime.ToString("dd.MM.yyyy HH:mm:ss"),
                              EmergencyType = tp.Name,
                              Status = EmergencyStatusToStringConverter.ConvertTo(e.Status),
                              EmergencyLocation = e.Location
                          }).FirstOrDefaultAsync();
        }
    }
}