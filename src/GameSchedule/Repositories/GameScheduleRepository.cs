using System.Threading.Tasks;
using GameSchedule.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace GameSchedule.Repositories
{
    public class GameScheduleRepository : IGameScheduleRepository
    {
        private readonly ILoggerFactory _logger;
        private readonly IMongoClient _client;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public GameScheduleRepository(IMongoClient client, string databaseName, ILoggerFactory logger, string collectionName)
        {
            _client = client;
            _databaseName = databaseName;
            _logger = logger;
            _collectionName = collectionName;
        }

        public void StoreGameSchedule(dynamic response)
        {
            _logger.CreateLogger<IGameScheduleRepository>().LogDebug("Storing Full Game Schedule to MongoDb database...");

            var db = _client.GetDatabase(_databaseName);
            var collection = db.GetCollection<BsonDocument>(_collectionName);

            var jsonData = JsonConvert.SerializeObject(response);
            var array = BsonSerializer.Deserialize<BsonArray>(jsonData);

            foreach (var document in array)
            {
                collection.InsertOneAsync(document, null);
            }

            _logger.CreateLogger<IGameScheduleRepository>().LogDebug("Storing Full Game Schedule to MongoDb complete!");
        }

        public async Task<dynamic> GetAllGamesAsync()
        {
            var db =  _client.GetDatabase(_databaseName);
            var collection =  db.GetCollection<Gameentry>(_collectionName);
            var result =  collection.Find(z => true).ToList();
            var jsonResult = JsonConvert.SerializeObject(result);

            await Task.Delay(10);

            return  jsonResult;
        }
    }
}
