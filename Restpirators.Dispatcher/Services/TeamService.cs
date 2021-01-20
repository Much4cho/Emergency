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
        Task AddTeam(Team team);
        IAsyncEnumerable<Team> GetTeams();
        Task<Team> GetTeam(int id);
        Task UpdateTeam(Team team);
        Task<Emergency> GetEmergencyByTeam(int id);
        Task<EmergencyDto> GetEmergencyByIdentifier(string identifier);
    }
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> _teamRepository;

        public TeamService(IRepository<Team> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task AddTeam(Team team)
        {
            await _teamRepository.Insert(team);
            await _teamRepository.SaveChanges();
        }

        public IAsyncEnumerable<Team> GetTeams()
        {
            return _teamRepository.GetAll();
        }

        public async Task<Team> GetTeam(int id)
        {
            return await _teamRepository.Get(id);
        }

        public async Task UpdateTeam(Team emergency)
        {
            await _teamRepository.Update(emergency);
            await _teamRepository.SaveChanges();
        }

        public async Task<Emergency> GetEmergencyByTeam(int id)
        {
            return await _teamRepository.GetEmergencyByTeam(id);
        }

        public async Task<EmergencyDto> GetEmergencyByIdentifier(string identifier)
        {
            return await _teamRepository.GetEmergencyByIdentifier(identifier);
        }
    }
}
