using MessagePack;

namespace MyRabbitMqService.Models
{
    [MessagePackObject]
    public class Person
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public string Name { get; set; }
    }
}
