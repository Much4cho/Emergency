using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using Restpirators.DataAccess.Entities;
using Restpirators.Dispatcher.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restpirators.Dispatcher.Controllers
{
    [Route("[controller]")]
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
        public IEnumerable<Emergency> Get()
        {
            return _emergencyService.GetEmergencies();
        }

        [HttpGet]
        [Route("{id}")]
        public Emergency Get(int id)
        {
            return _emergencyService.GetEmergency(id);
        }

        [HttpPut]
        public void Update([FromBody] Emergency emergency)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "statistics",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                EmergencyId = emergency.Id,
                EmergencyTypeId = emergency.EmergencyTypeId,
                Status = emergency.Status,
                Description = emergency.Description,
                Location = emergency.Location,
                ModDate = DateTime.Now
            }));
            channel.BasicPublish(exchange: "",
                                routingKey: "statistics",
                                basicProperties: null,
                                body: body);
            _emergencyService.UpdateEmergency(emergency);
        }
    }
}
