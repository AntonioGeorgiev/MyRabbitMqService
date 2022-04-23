using System;
using System.Threading.Tasks;
using MessagePack;
using Confluent.Kafka;
using MyRabbitMqService.Models;
using MyRabbitMqService.BL.Interfaces;


namespace Producer
{
    public class KafkaProducer: IKafkaProducer
    {
        private static IProducer<int, Person> _producer;
        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<int, Person>(config)
                .SetValueSerializer(new MsgPackSerializer<Person>())
                .Build();

           
            
           
        }

        public async Task SendPersonKafka(Person p)
        {
            try
            {
                var result = await _producer.ProduceAsync("test2", new Message<int, Person>()
                {
                    Key = p.Id,
                    Value = p,
                });
                Console.WriteLine($"Delievered '{result.Value}' to {result.TopicPartitionOffset}");


            }
            catch (ProduceException<int, Person> e)
            {
                Console.WriteLine($"Delievered failed: {e.Error.Reason}");
            }
        }
    }
}
