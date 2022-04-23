using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.DL;

namespace MyRabbitMqService.BL.Services
{
    public class CachePublisher : IHostedService, IDisposable 
       
    {
        private Timer _timer;
        private DateTime _lastUpdated = DateTime.Now;
        private readonly IPersonRepository _personRepository;
        private readonly IRabbitMqService _rabbitMqService; 

        public CachePublisher(IPersonRepository personRepository, IRabbitMqService rabbitMqService)
        {
            _personRepository = personRepository;
            _rabbitMqService = rabbitMqService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }
        private async void DoWork(object? state)
        {
            var persons = await _personRepository.GetAllByDate(_lastUpdated.ToUniversalTime());
            if (!persons.Any())
            {
                return;
            }
            foreach (var person in persons)
            {
                _rabbitMqService.SendPersonAsync(person);
            }
            _lastUpdated = DateTime.Now;
            
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    
}
