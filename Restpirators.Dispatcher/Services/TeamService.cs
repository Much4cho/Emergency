using Restpirators.DataAccess.Entities;
using Restpirators.Repository;
using Restpirators.Repository.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Services
{
    public interface ITeamService
    {
        void AddTeam(Team team);
        IEnumerable<Team> GetTeams();
        Team GetTeam(int id);
        void UpdateTeam(Team team);
        Emergency GetEmergencyByTeam(int id);
        EmergencyDto GetEmergencyByIdentifier(string identifier);
    }
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> _teamRepository;

        public TeamService(IRepository<Team> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public void AddTeam(Team team)
        {
            _teamRepository.Insert(team);
            //_teamRepository.SaveChanges();
        }

        public IEnumerable<Team> GetTeams()
        {
            return _teamRepository.GetAll();
        }

        public Team GetTeam(int id)
        {
            return _teamRepository.Get(id);
        }

        public void UpdateTeam(Team emergency)
        {
            _teamRepository.Update(emergency);
            //_teamRepository.SaveChanges();
        }

        public Emergency GetEmergencyByTeam(int id)
        {
            return _teamRepository.GetEmergencyByTeam(id);
        }

        public EmergencyDto GetEmergencyByIdentifier(string identifier)
        {
            return _teamRepository.GetEmergencyByIdentifier(identifier);
        }
    }
}
