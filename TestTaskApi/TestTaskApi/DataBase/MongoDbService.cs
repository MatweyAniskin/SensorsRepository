using MongoDB.Driver;

namespace TestTaskApi.DataBase
{
    //Service for using mongo db
    public class MongoDbService : IMongoDb
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _database;

        public IMongoDatabase Database => _database;

        public MongoDbService(IConfiguration configuration) : this(configuration.GetConnectionString("MongoConnection"))
        {
            
        }
        public MongoDbService(string connectionString)
        {            
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}
