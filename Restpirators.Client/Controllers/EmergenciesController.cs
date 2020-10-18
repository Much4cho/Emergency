using Microsoft.AspNetCore.Mvc;
using Restpirators.Client.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Restpirators.Client.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class EmergenciesController : ControllerBase
    {
        [HttpPost]  
        public ActionResult<bool> CreateEmergency(EmergencyReport emergency)  
        {
            if(emergency != null)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                    channel.QueueDeclare(queue: "emergency",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(emergency));
                    channel.BasicPublish(exchange: "",
                                        routingKey: "emergency",
                                        basicProperties: null,
                                        body: body);
                return true;
            }
            return false;
        } 
    }
}