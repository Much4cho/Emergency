using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restpirators.DataAccess.Entities;
using Restpirators.Dispatcher.Services;
using System.Collections.Generic;

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
        public IAsyncEnumerable<Emergency> Get()
        {
            return _emergencyService.GetEmergencies();
        }
    }
}
