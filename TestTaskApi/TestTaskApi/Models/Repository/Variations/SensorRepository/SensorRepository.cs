using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Text;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorRepository
{
    //Репозиторий датчиков, наследуется от обобщенного репозитория. Так же изменены методы под работу с чанками
    public class SensorRepository : BaseRepository<Sensor, string>, ISensorRepository
    {
        private readonly IGridFSBucket _gridFS;
        private readonly IMongoCollection<SensorRecord> _collectionGrid;
        public SensorRepository(IMongoDb mongoDb) : base(mongoDb)
        {
            _gridFS = new GridFSBucket(mongoDb.Database);
            _collectionGrid = _dataBase.GetCollection<SensorRecord>(_collectionName);
        }
        public override async Task AddAsync(Sensor entity)
        {
            var record = new SensorRecord
            {
                SensorName = entity.SensorName,              
                CustomerName = entity.CustomerName,
                Flags = entity.Flags,
                SensorType = entity.SensorType,
                Unit = entity.Unit,
                North = entity.North,
                East = entity.East,
                Height = entity.Height,
                Km = entity.Km
            };
            if (entity.DateTime != null)
            {
                var arrBytes = ConverDateArrayToBytes(entity.DateTime);
                record.DateTimeGridFsId = await _gridFS.UploadFromBytesAsync($"{entity.SensorName}_time_stamp", arrBytes);
            }
            if (entity.Values1 != null)
            {
                var arrBytes = ConvertDoubleArrayToBytes(entity.Values1);
                record.Values1GridFsId = await _gridFS.UploadFromBytesAsync($"{entity.SensorName}_v1", arrBytes);
            }
            if (entity.Values2 != null)
            {
                var arrBytes = ConvertDoubleArrayToBytes(entity.Values2);
                record.Values2GridFsId = await _gridFS.UploadFromBytesAsync($"{entity.SensorName}_v2", arrBytes);
            }         
            await _collectionGrid.InsertOneAsync(record);
        }
        public override async Task<IEnumerable<Sensor>> GetAllAsync()
        {
            var sensors = new List<Sensor>();

           
            using (var cursor = await _collectionGrid.FindAsync(FilterDefinition<SensorRecord>.Empty))
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var record in cursor.Current)
                    {
                        var sensor = new Sensor
                        {
                            SensorType = record.SensorType,
                            Unit = record.Unit,
                            North = record.North,
                            East = record.East,
                            Height = record.Height,
                            Km = record.Km,                         
                            Flags = record.Flags,
                            CustomerName = record.CustomerName,
                            SensorName = record.SensorName
                        };
                        if (record.DateTimeGridFsId.HasValue)
                        {
                            var arrBytes = await _gridFS.DownloadAsBytesAsync(record.DateTimeGridFsId.Value);
                            sensor.DateTime = ConverBytesArrayToDate(arrBytes);
                        }
                        if (record.Values1GridFsId.HasValue)
                        {
                            var arrBytes = await _gridFS.DownloadAsBytesAsync(record.Values1GridFsId.Value);
                            sensor.Values1 = ConvertBytesToDoubleArray(arrBytes);
                        }
                        if (record.Values2GridFsId.HasValue)
                        {
                            var arrBytes = await _gridFS.DownloadAsBytesAsync(record.Values2GridFsId.Value);
                            sensor.Values2 = ConvertBytesToDoubleArray(arrBytes);
                        }
                        sensors.Add(sensor);
                    }
                }
            }

            return sensors;
        }
        public override async Task ClearAsync()
        {
            var records = await _collectionGrid.Find(FilterDefinition<SensorRecord>.Empty).ToListAsync();

            foreach (var record in records)
            {
                if (record.Values1GridFsId.HasValue)
                {
                    await _gridFS.DeleteAsync(record.Values1GridFsId.Value);
                }

                if (record.Values2GridFsId.HasValue)
                {
                    await _gridFS.DeleteAsync(record.Values2GridFsId.Value);
                }
            }

            await _collectionGrid.DeleteManyAsync(FilterDefinition<SensorRecord>.Empty);
        }
        private byte[] ConverDateArrayToBytes(string[] arr)
        {
            var concatenated = string.Join(";", arr);
            return System.Text.Encoding.UTF8.GetBytes(concatenated);
        }
        private string[] ConverBytesArrayToDate(byte[] arr)
        {
            var concatenated = System.Text.Encoding.UTF8.GetString(arr);
            return concatenated.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private byte[] ConvertDoubleArrayToBytes(double[] arr)
        {
            var byteArr = new byte[arr.Length * sizeof(double)];
            Buffer.BlockCopy(arr, 0, byteArr, 0, byteArr.Length);
            return byteArr;
        }
        private double[] ConvertBytesToDoubleArray(byte[] bytes)
        {
            var arr = new double[bytes.Length / sizeof(double)];
            Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
            return arr;
        }
    }
}
