using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restpirators.DataAccess.Entities;
using Restpirators.Dispatcher.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergenciesController : ControllerBase
    {
        private readonly ILogger<EmergenciesController> _logger;
        private readonly IEmergencyService _emergencyService;


        public EmergenciesController(ILogger<EmergenciesController> logger,
            IEmergencyService emergencyService)
        {
            _logger = logger;
            _emergencyService = emergencyService;
        }

        [HttpGet]
        // [Authorize]
        public IAsyncEnumerable<Emergency> Get()
        {
            return _emergencyService.GetEmergencies();
        }

        [HttpGet]
        [Route("{id}")]
        // [Authorize]
        public async Task<Emergency> Get(int id)
        {
            return await _emergencyService.GetEmergency(id);
        }

        [HttpPut]
        // [Authorize]
        public async Task Update([FromBody] Emergency emergency)
        {
            await _emergencyService.UpdateEmergency(emergency);
        }
    }
}
