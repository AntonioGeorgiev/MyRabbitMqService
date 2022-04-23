using System;
using Confluent.Kafka;
using System.Threading;
using MyRabbitMqService.Models;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class KafkaConsumer:IHostedService
    {
        IConsumer<int, Person> consumer;
        public KafkaConsumer()
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 5000,
                FetchWaitMaxMs = 50,
            };

           consumer = new ConsumerBuilder<int, Person>(config)
                .SetValueDeserializer(new MsgPackDeserializer<Person>())
                .Build();
            consumer.Subscribe("test2");
           
            

            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = consumer.Consume(cancellationToken);
                        Console.WriteLine($"Person with ID:{ cr.Message.Value.Id}and Name:{cr.Message.Value.Name}");

                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                    }

                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Close();
            return Task.CompletedTask;

        }
    }
}
