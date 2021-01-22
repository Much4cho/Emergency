using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restpirators.DataAccess.Entities;
using Restpirators.Dispatcher.Services;
using Restpirators.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ILogger<TeamsController> _logger;
        private readonly ITeamService _teamService;


        public TeamsController(ILogger<TeamsController> logger,
            ITeamService teamService)
        {
            _logger = logger;
            _teamService = teamService;
        }

        [HttpGet]
        public IEnumerable<Team> Get()
        {
            return _teamService.GetTeams();
        }

        [HttpGet]
        [Route("{id}")]
        public Team Get(int id)
        {
            return _teamService.GetTeam(id);
        }

        [HttpPut]
        public void Update([FromBody] Team team)
        { 
            _teamService.UpdateTeam(team);
        }
        [HttpGet]
        [Route("teamEmergency/{id}")]
        public Emergency GetEmergencyByTeam(int id)
        {
            return _teamService.GetEmergencyByTeam(id);
        }
        [HttpGet]
        [Route("emergency/{identifier}")]
        public EmergencyDto GetEmergencyByIdentifier(string identifier)
        {
            return _teamService.GetEmergencyByIdentifier(identifier);
        }
    }
}
