namespace TestTaskApi.Parser.Service
{
    public interface IParserService
    {
        public T Parse<T>(string input);
        public  Task<T> ParseAsync<T>(string input);
    }
}
