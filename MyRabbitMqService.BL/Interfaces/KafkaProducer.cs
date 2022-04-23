using System.Threading.Tasks;
using MyRabbitMqService.Models;

namespace MyRabbitMqService.BL.Interfaces
{
    public interface IKafkaProducer
    {
        Task SendPersonKafka(Person p);

    }
}
