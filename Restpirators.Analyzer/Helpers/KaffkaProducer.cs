using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Helpers
{
    public class KaffkaProducer : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "kafka:29092",

            };
            var _producer = new ProducerBuilder<Null, string>(config).Build();
            for (var i = 0; i < 10000; ++i)
            {
                var value = (new Random().NextDouble() * (35 + 30)) - 30;
                await _producer.ProduceAsync("xxxxxx", new Message<Null, string>()
                {
                    Value = value.ToString()
                }, stoppingToken);
            }
            _producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
