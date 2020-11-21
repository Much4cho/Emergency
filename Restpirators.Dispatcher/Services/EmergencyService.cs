using Restpirator.Messaging;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Services
{
    public interface IEmergencyService
    {
        public Task AddEmergency(EmergencyReport emergencyReport);
        public IAsyncEnumerable<Emergency> GetEmergencies();
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
        }

        public IAsyncEnumerable<Emergency> GetEmergencies()
        {
            return _emergencyRepository.GetAll();
        }
    }
}
