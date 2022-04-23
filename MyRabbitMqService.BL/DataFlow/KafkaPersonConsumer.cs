using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using MyRabbitMqService.BL.DataFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    public class KafkaPersonConsumer : IHostedService
    {
        private readonly IConsumer<int, byte[]> consumer;
        private readonly IPersonDataFlow _personDataFlow;

        public KafkaPersonConsumer(IPersonDataFlow personDataFlow)
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

            consumer = new ConsumerBuilder<int, byte[]>(config)
               .Build();

            consumer.Subscribe("test2");
            _personDataFlow = personDataFlow;
        }

        CancellationTokenSource cts = new CancellationTokenSource();

        public Task StartAsync(CancellationToken cancellationToken)
        {

            try
            {
                Task.Factory.StartNew(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);

                            _personDataFlow.SendPerson(cr.Message.Value);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error {e.Error.Reason}");
                        }
                    }
                }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
