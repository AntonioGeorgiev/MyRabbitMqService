using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Interfaces
{
    public interface IKafkaAdminService
    {
        Task<bool> CreateTopicAsync(string topicName);

        Task<bool> DeleteTopicAsync(string topicName);
    }
}
