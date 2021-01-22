using Restpirators.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Repository.Repositories.Abstract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> GetAll();
        public TEntity Get(int id);
        public int Insert(TEntity entity);
        public void Update(TEntity entity);
        public void SaveChanges();
        Emergency GetEmergencyByTeam(int id);
        EmergencyDto GetEmergencyByIdentifier(string identifier);
    }
}
