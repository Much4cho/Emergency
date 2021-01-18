using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restpirators.DataAccess.Entities;
using Restpirators.Dispatcher.Services;
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
        public IAsyncEnumerable<Team> Get()
        {
            return _teamService.GetTeams();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Team> Get(int id)
        {
            return await _teamService.GetTeam(id);
        }

        [HttpPut]
        public async Task Update([FromBody] Team team)
        { 
            await _teamService.UpdateTeam(team);
        }
    }
}
