

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Repository.Reflection;


namespace TestTaskApi.Models.Repository.Base
{
    //Обобщенный репозиторий для монго
    public abstract class BaseRepository<T, Tid> : IRepository<T, Tid> where T : class
    {

        protected readonly string _collectionName;
        protected readonly string _idFieldName;      
        protected readonly IMongoDatabase _dataBase;
        protected readonly IMongoCollection<T> _collection;
        public BaseRepository(IMongoDb mongoDb)
        {
            _idFieldName = GetIdFieldName();
            _collectionName = GetCollectionName();
            _dataBase = mongoDb.Database;
            _collection = _dataBase.GetCollection<T>(_collectionName);            
        }
        //Для объявления названия коллекции использую собственные атрибуты
        protected virtual string GetCollectionName()
        {
            var type = typeof(T);
            var attributeName = (CollectionNameAttribute)Attribute.GetCustomAttribute(type, typeof(CollectionNameAttribute));
            return attributeName?.Name ?? $"{typeof(T).Name}Collection"; //если такого нету, то название типа модели
        }
        //Отлавливаю название свойства которое является идентификатором, чтобы получать данные по id
        protected virtual string GetIdFieldName()
        {
            var type = typeof(T);
            var idProperty = type.GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute)));
            if (idProperty != null)
            {
                var bsonElementAttr = (BsonElementAttribute)Attribute.GetCustomAttribute(idProperty, typeof(BsonElementAttribute)); //Название беру из другого атрибута, отвечающего за название поля
                return bsonElementAttr?.ElementName ?? idProperty.Name; //если такого нету - беру имя свойства
            }
            throw new InvalidOperationException($"No BsonId property found in type {type.Name}");
        }
            
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Tid id)
        {
            var filter = Builders<T>.Filter.Eq(_idFieldName, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public void Update(T entity)
        {
            var type = typeof(T);
            var idProperty = type.GetProperties()
               .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute)));
            if (idProperty == null)
            {
                throw new InvalidOperationException($"Entity must have an {_idFieldName} property.");
            }

            var id = (Tid)idProperty.GetValue(entity);
            var filter = Builders<T>.Filter.Eq(_idFieldName, id);
            _collection.ReplaceOne(filter, entity);
        }

        public async Task DeleteByIdAsync(Tid id)
        {
            var filter = Builders<T>.Filter.Eq(_idFieldName, id);
            await _collection.DeleteOneAsync(filter);
        }   
        public async Task ClearAsync()
        {
            await _collection.DeleteManyAsync(Builders<T>.Filter.Empty);
        }
    }
}
