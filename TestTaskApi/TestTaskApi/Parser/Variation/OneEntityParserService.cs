using TestTaskApi.Parser.Service;

namespace TestTaskApi.Parser.Variation
{
    public class OneEntityParserService : BaseParserService //парсер для кейса когда в одном файле инфа за один датчик
    {
        protected override char RowSeparator => '\n'; //указываю сепараторы, т.к. они могут отличаться в документах csv

        protected override char ColSeparator => ';';

        public override T Parse<T>(string input)
        {
            var fields = GetParseFields<T>(); //получаю заголовки по которым буду парсить
            if (fields is null || fields.Count() == 0)
                throw new InvalidOperationException($"Not found any parse attribute in {typeof(T).Name}");

            var table = GetTableFromInput(input); //получаю таблицу из файла
            
            T instance = Activator.CreateInstance<T>(); //создаю экземпляр модели
           
            var headers = table.Headers.ToList();

            foreach (var field in fields) { //паршу по каждому из заголовков в таблице
                var colIndex = headers.IndexOf(field.ParseHeaderName);
                var value = table.Body.ElementAtOrDefault(colIndex);

                if (value is null)
                    throw new OverflowException($"Value on index {colIndex} is null by header {field.ParseHeaderName}");

                SetFieldInInstance<T>(instance, field, value);//устанавливаю значение в модель
            }

            return instance;
        }
    }
}
