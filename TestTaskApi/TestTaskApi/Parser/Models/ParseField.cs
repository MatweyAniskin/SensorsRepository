using System.Reflection;

namespace TestTaskApi.Parser.Models
{
    public class ParseField //модель поля для парсинга файла
    {
        public string ParseHeaderName { get; set; }
        public PropertyInfo Property { get; set; }      
    }
}
