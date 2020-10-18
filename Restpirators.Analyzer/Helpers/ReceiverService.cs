using Restpirators.Analyzer.DataAccess;
using Restpirators.Analyzer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Helpers
{
    public class ReceiverService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ReceiverService(IServiceProvider services)
        {
            _services = services;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "queueName", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: "queueName", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            EmergencyHistory emergencyHistory = JsonConvert.DeserializeObject<EmergencyHistory>(message);
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IScopedProcessingService>();

                scopedProcessingService.DoWork(emergencyHistory);
            }
        }
        internal interface IScopedProcessingService
        {
            void DoWork(EmergencyHistory emergencyHistory);
        }
        internal class ScopedProcessingService : IScopedProcessingService
        {
            private readonly IEmergenciesRepository emergenciesRepository;

            public ScopedProcessingService(IEmergenciesRepository emergenciesRepository)
            {
                this.emergenciesRepository = emergenciesRepository;
            }

            public void DoWork(EmergencyHistory emergencyHistory)
            {
                emergenciesRepository.InsertEmergencyHistory(emergencyHistory);
            }
        }
    }
}
