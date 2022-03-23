using MyRabbitMqService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRabbitMqService.DL
{
    public interface IPersonRepository
    {
        Task Add(Person p);
        Task<IEnumerable<Person>> GetAllByDate(DateTime lastUpdated);
    }
}
