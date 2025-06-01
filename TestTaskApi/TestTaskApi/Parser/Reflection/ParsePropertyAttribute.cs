namespace TestTaskApi.Parser.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParsePropertyAttribute : Attribute //атрибут-указатель для полей которые надо будет парсить
    {
        public string FieldName { get; set; }
        public ParsePropertyAttribute()
        {
         
        }
        public ParsePropertyAttribute(string fieldName) 
        {
            FieldName = fieldName;
        }
    }
}
