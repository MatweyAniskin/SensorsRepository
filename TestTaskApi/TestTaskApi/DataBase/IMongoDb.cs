using MongoDB.Driver;

namespace TestTaskApi.DataBase
{
    public interface IMongoDb
    {
        IMongoDatabase Database { get; }
    }
}
