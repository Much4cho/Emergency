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
        public IEnumerable<TEntity> GetAll()
        {
            return entities.ToList();
        }

        public TEntity Get(int id)
        {
            return entities.FirstOrDefault(s => s.Id == id);
        }
        public int Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
            return entity.Id;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Update(entity);
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public Emergency GetEmergencyByTeam(int id)
        {
            return context.Emergencies.Where(x => x.Status == Common.Enums.EmergencyStatus.TeamSent && x.AssignedToTeamId == id).FirstOrDefault();
        }

        public EmergencyDto GetEmergencyByIdentifier(string identifier)
        {
            return (from e in context.Emergencies
                          join t in context.Teams on e.AssignedToTeamId equals t.Id into td
                          from t in td.DefaultIfEmpty()
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
                          }).FirstOrDefault();
        }
    }
}