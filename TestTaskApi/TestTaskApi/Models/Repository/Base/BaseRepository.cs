
using MongoDB.Driver;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Repository.Reflection;


namespace TestTaskApi.Models.Repository.Base
{
    //Left the BaseRepository because there are common methods, DRY principle
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly string _collectionName;        
        protected readonly IMongoDatabase _dataBase;
        protected readonly IMongoCollection<T> _collection;  //using in all repositorys  

        private readonly InsertManyOptions _options;
        public BaseRepository(IMongoDb mongoDb)
        {
         
            _collectionName = GetCollectionName();
            _dataBase = mongoDb.Database;
            _collection = _dataBase.GetCollection<T>(_collectionName);
            _options = new InsertManyOptions { IsOrdered = false };
        }        
        //Name of collection get by attribute
        private string GetCollectionName()
        {
            var type = typeof(T);
            var attributeName = (CollectionNameAttribute)Attribute.GetCustomAttribute(type, typeof(CollectionNameAttribute));
            return attributeName?.Name ?? $"{typeof(T).Name}Collection"; 
        }       
        //this method use in all repository
        public async Task AddMany(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities, _options);
        }       
        //this using in UNIT tests
        public async Task ClearAsync()
        {
            await _collection.DeleteManyAsync(Builders<T>.Filter.Empty);
        }
    }
}
