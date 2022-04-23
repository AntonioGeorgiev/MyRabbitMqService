using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MyRabbitMqService.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    public class KafkaAdminService : IKafkaAdminService, IDisposable
    {
        private readonly IAdminClient _adminClient;

        public KafkaAdminService()
        {
            var config = new AdminClientConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            var adminClientBuilder = new AdminClientBuilder(config);

            _adminClient = adminClientBuilder
            .SetErrorHandler((client, error) =>
            {
                Console.WriteLine($"Client: {client.Name} Error: {error.Reason}");
            })
            .Build();


        }

        public async Task<bool> CreateTopicAsync(string topicName)
        {
            try
            {
                await _adminClient.CreateTopicsAsync(new List<TopicSpecification>()
                {
                    new TopicSpecification() {
                        Name = topicName,
                        NumPartitions = 1,
                        ReplicationFactor = 1
                    }
                }, new CreateTopicsOptions()
                {
                    OperationTimeout = TimeSpan.FromSeconds(5),
                    RequestTimeout = TimeSpan.FromSeconds(5)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _adminClient.Dispose();
        }

        public async Task<bool> DeleteTopicAsync(string topicName)
        {
            try
            {
                await _adminClient.DeleteTopicsAsync(new List<string>()
                {
                   topicName
                }, new DeleteTopicsOptions()
                {
                    OperationTimeout = TimeSpan.FromSeconds(5),
                    RequestTimeout = TimeSpan.FromSeconds(5)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}
