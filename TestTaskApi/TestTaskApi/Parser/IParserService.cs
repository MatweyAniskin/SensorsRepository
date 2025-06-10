using System.Collections.Generic;

namespace TestTaskApi.Parser
{
    public interface IParserService<T> where T : class
    {
        public Task<IEnumerable<T>> ParseAsync(IFormFile file);
        public IEnumerable<T> Parse(IFormFile file);
    }
}
