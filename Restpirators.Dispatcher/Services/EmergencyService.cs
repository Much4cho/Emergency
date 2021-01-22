using Restpirator.Messaging;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Services
{
    public interface IEmergencyService
    {
        int AddEmergency(EmergencyReport emergencyReport);
        IEnumerable<Emergency> GetEmergencies();
        Emergency GetEmergency(int id);
        void UpdateEmergency(Emergency emergency);
    }
    public class EmergencyService : IEmergencyService
    {
        private readonly IRepository<Emergency> _emergencyRepository;

        public EmergencyService(IRepository<Emergency> emergencyRepository)
        {
            _emergencyRepository = emergencyRepository;
        }

        public int AddEmergency(EmergencyReport emergencyReport)
        {
            var entity = new Emergency
            {
                EmergencyTypeId = emergencyReport.EmergencyTypeId,
                Location = emergencyReport.Location,
                Description = emergencyReport.Description,
                ReportTime = emergencyReport.ReportTime,
                Status = Common.Enums.EmergencyStatus.New,
                Identifier = emergencyReport.Identifier
            };

            return _emergencyRepository.Insert(entity);
            //await _emergencyRepository.SaveChanges();
        }

        public IEnumerable<Emergency> GetEmergencies()
        {
            return _emergencyRepository.GetAll();
        }

        public Emergency GetEmergency(int id)
        {
            return _emergencyRepository.Get(id);
        }

        public void UpdateEmergency(Emergency emergency)
        {
             _emergencyRepository.Update(emergency);
            //await _emergencyRepository.SaveChanges();
        }
    }
}
