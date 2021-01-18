using Restpirator.Messaging;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Services
{
    public interface IEmergencyService
    {
        Task AddEmergency(EmergencyReport emergencyReport);
        IAsyncEnumerable<Emergency> GetEmergencies();
        Task<Emergency> GetEmergency(int id);
        Task UpdateEmergency(Emergency emergency);
    }
    public class EmergencyService : IEmergencyService
    {
        private readonly IRepository<Emergency> _emergencyRepository;

        public EmergencyService(IRepository<Emergency> emergencyRepository)
        {
            _emergencyRepository = emergencyRepository;
        }

        public async Task AddEmergency(EmergencyReport emergencyReport)
        {
            var entity = new Emergency
            {
                EmergencyTypeId = emergencyReport.EmergencyTypeId,
                Location = emergencyReport.Location,
                Description = emergencyReport.Description,
                ReportTime = emergencyReport.ReportTime,
                Status = emergencyReport.Status,
            };

            await _emergencyRepository.Insert(entity);
            await _emergencyRepository.SaveChanges();
        }

        public IAsyncEnumerable<Emergency> GetEmergencies()
        {
            return _emergencyRepository.GetAll();
        }

        public async Task<Emergency> GetEmergency(int id)
        {
            return await _emergencyRepository.Get(id);
        }

        public async Task UpdateEmergency(Emergency emergency)
        {
            await _emergencyRepository.Update(emergency);
            await _emergencyRepository.SaveChanges();
        }
    }
}
