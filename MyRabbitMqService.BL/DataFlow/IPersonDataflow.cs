using System.Threading.Tasks;

namespace MyRabbitMqService.BL.DataFlow
{
    public interface IPersonDataFlow
    {
        Task SendPerson(byte[] data);
    }
}
