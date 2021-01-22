using Restpirators.Dispatcher.Models.ConfigurationModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Restpirator.Messaging;
using Restpirators.Dispatcher.Services;
using System;
using System.IO;

namespace Restpirators.Dispatcher.Handlers
{
    public class EmergencyReportHandler : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IEmergencyService _emergencyService;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;
        public EmergencyReportHandler(IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IEmergencyService emergencyService)
        {
            _hostname = rabbitMqOptions.Value.HostName;
            _queueName = "emergency";
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _port = rabbitMqOptions.Value.Port;
            _emergencyService = emergencyService;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest" 
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            //_channel.BasicQos(1, 0, false);
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var emergencyReport = JsonSerializer.Deserialize<EmergencyReport>(content);

                HandleMessage(emergencyReport);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(EmergencyReport emergencyReport)
        {
            var res = _emergencyService.AddEmergency(emergencyReport);
            emergencyReport.Id = res;
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //channel.BasicQos(1, 0, false);
            channel.QueueDeclare(queue: "statistics",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                EmergencyId = emergencyReport.Id,
                EmergencyTypeId = emergencyReport.EmergencyTypeId,
                Status = emergencyReport.Status,
                Description = emergencyReport.Description,
                Location = emergencyReport.Location,
                ModDate = DateTime.Now
            }));
            channel.BasicPublish(exchange: "",
                                routingKey: "statistics",
                                basicProperties: null,
                                body: body);
            channel.Close();
            connection.Close();
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
