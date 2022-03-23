using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyRabbitMqService.Models;

namespace MyRabbitMqService.DL 
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMongoCollection<Person> _collection;
        public PersonRepository(IOptions<MongoDbConfiguration> mOptions)
        {
            var client = new MongoClient(mOptions.Value.ConnectionString);
            var database = client.GetDatabase(mOptions.Value.DatabaseName);
            _collection = database.GetCollection<Person>("Person");
        }
        public async Task Add(Person p)
        {
            await _collection.InsertOneAsync(p);
        }
        public async Task<IEnumerable<Person>> GetAllByDate(DateTime lastUpdated)
        {
            var result = await _collection.FindAsync(p => p.LastUpdated >= lastUpdated);
            return result.ToList();
        }
    }
}
