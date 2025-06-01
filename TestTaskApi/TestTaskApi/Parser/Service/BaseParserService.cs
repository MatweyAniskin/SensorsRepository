using MongoDB.Driver.Linq;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using TestTaskApi.Parser.Models;
using TestTaskApi.Parser.Reflection;

namespace TestTaskApi.Parser.Service
{ 
    //Основа парсера
    public abstract class BaseParserService : IParserService
    {
        protected abstract char RowSeparator { get; }
        protected abstract char ColSeparator { get; }

        public abstract T Parse<T>(string input);

        public Task<T> ParseAsync<T>(string input) =>
            Task.Run(() => Parse<T>(input)); //Так же делаю возможность парсить в другом потоке

        //получение таблицы из файла
        protected ParseTable GetTableFromInput(string input)
        {        
            var rows = input.Split(RowSeparator).Where(row => row.Length > 1).ToArray();
            var header = rows.First()
                .Split(ColSeparator)
                .Select(i => RemoveSpecialCharacters(i))
                .ToArray(); //получение заголовков и очищаю от спец символов
            var body = rows.Skip(1)
                .Select(i => i.Split(ColSeparator)
                    .Select(j => RemoveSpecialCharacters(j))
                    .ToArray())
                .ToArray(); //получаю тело таблицы
            var reshapedBody = Enumerable.Range(0, body[0].Length)
                                      .Select(j => body.Select(row => row[j]).ToArray())
                                      .ToArray(); //переварачиваю тело таблицы
            return new ParseTable
            {
                Headers = header,
                Body = reshapedBody,
            };
        }
        //получение полей по которым буду парсить таблицу из модели
        protected IEnumerable<ParseField> GetParseFields<T>()
        {
            var type = typeof(T);
            var parseAttributeType = typeof(ParsePropertyAttribute);
            var fields = type.GetProperties().Where(prop => Attribute.IsDefined(prop, parseAttributeType)); //получил все поля у которых есть атрибут - указатель для парсинга
            foreach (var field in fields) 
            {
                var parseAttribute = (ParsePropertyAttribute)Attribute.GetCustomAttribute(field, parseAttributeType);
                yield return new ParseField //возвращаю данные о полях
                {
                    Property = field, 
                    ParseHeaderName = string.IsNullOrEmpty(parseAttribute.FieldName) ? field.Name : parseAttribute.FieldName,                 
                };
            }
        }
        protected T SetFieldInInstance<T>(T instance, ParseField parseField, IEnumerable<string> fieldValues)
        {
            object convertedValue = ConvertFieldType(fieldValues, parseField.Property); //конвертирую тип данных для инстанса

            parseField.Property.SetValue(instance, convertedValue); //устанавливаю данные в свойство инстанса
            
            return instance;
        }
        //конвертация типа
        private object ConvertFieldType(IEnumerable<string> fieldValues, PropertyInfo property)
        {
            if (!property.PropertyType.IsArray) //если конвертируемый тип не является массивом
            {
                var fieldValue = fieldValues.FirstOrDefault(); // то получаем первое же поле из значений с файла
                if (fieldValue is null)
                    throw new OverflowException($"Data in body not found for {property.Name}");                
                return Convert.ChangeType(fieldValue, property.PropertyType, CultureInfo.InvariantCulture);
            }
            
            var elementType = property.PropertyType.GetElementType(); //если является массивом
            var array = Array.CreateInstance(elementType,fieldValues.Count()); //то создаю массив
            var index = 0;

            foreach ( var value in fieldValues) //и заполняю его данными
            {
                array.SetValue(Convert.ChangeType(value,elementType, CultureInfo.InvariantCulture), index++);
            }
            return  array; //уже не стал заморачиваться по поводу всех возможных коллекций
        }
        private string RemoveSpecialCharacters(string input) //очистка от невидимых символов, иногда встречаются в файлах
        => Regex.Replace(input, @"[\n\r\t]", string.Empty);
    }
}
