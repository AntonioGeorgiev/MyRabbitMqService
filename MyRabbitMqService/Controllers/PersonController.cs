using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.DL;
using MyRabbitMqService.Models;

namespace MyRabbitMqService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonController> _logger;
        private readonly IKafkaProducer kafkaProducer;



        public PersonController(ILogger<PersonController> logger, IRabbitMqService rabbitMqService, IPersonRepository personRepository, IKafkaProducer kafkaProducer)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            _personRepository = personRepository;
            this.kafkaProducer = kafkaProducer;
        }

        [HttpPost("Rabbit")]
        public async Task<IActionResult> SendPerson([FromBody] Person p)
        {
            await _rabbitMqService.SendPersonAsync(p);

            return Ok();
        }
        [HttpPost("Kafka")]

        public async Task<IActionResult> SendKafkaPerson([FromBody] Person p)
        {
            await kafkaProducer.SendPersonKafka(p);

            return Ok();
        }
       
    }
}
