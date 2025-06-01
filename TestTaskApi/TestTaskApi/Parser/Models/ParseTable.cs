namespace TestTaskApi.Parser.Models
{
    public class ParseTable //модель таблицы файла
    {
        public string[] Headers { get; set; }
        public string[][] Body { get; set; }
    }
}
