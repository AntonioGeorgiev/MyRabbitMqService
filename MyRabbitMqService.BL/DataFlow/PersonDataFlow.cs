using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MessagePack;
using MyRabbitMqService.Models;


namespace MyRabbitMqService.BL.DataFlow
{
    
    public class PersonDataFlow:IPersonDataFlow
    {
        private readonly TransformBlock<byte[], Person> entryBlock;
        public PersonDataFlow()
        {
            entryBlock = new TransformBlock<byte[], Person>(data => MessagePackSerializer.Deserialize<Person>(data));
            var enrichBlock = new TransformBlock<Person, Person> (p =>
             {
                 p.LastUpdated = DateTime.Now;
                 return p;

             });
            var publishBlock = new ActionBlock<Person>(p =>
            {
                Console.WriteLine($"Updated value:{p.LastUpdated}");
              
            });
            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };
            entryBlock.LinkTo(enrichBlock, linkOptions);
            entryBlock.LinkTo(publishBlock, linkOptions);

       
                
        }

        public async Task SendPerson(byte[] data)
        {
           await entryBlock.SendAsync(data);
           
        }
    }
}
