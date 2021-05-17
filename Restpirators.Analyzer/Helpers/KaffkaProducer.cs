using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var pconfig = new ProducerConfig()
            {
                BootstrapServers = "kafka:29092",
            };
            var _producer = new ProducerBuilder<int, string>(pconfig).Build();
            var id = 1;
            while(true)
            {
                for (var i = 1; i <= 1000; ++i)
                {
                    var value = new Random().Next(0, 100);
                    var json = JsonConvert.SerializeObject(new { Id = id, Temperature = value });
                    Console.WriteLine("Produced value: " + value);
                    await _producer.ProduceAsync("test", new Message<int, string>()
                    {
                        Key = id,
                        Value = json
                    }, stoppingToken);
                }
                id++;
            }    
        }
    }
}
