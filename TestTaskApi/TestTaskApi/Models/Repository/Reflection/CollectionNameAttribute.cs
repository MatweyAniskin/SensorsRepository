﻿namespace TestTaskApi.Models.Repository.Reflection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute: Attribute
    {
        public string Name { get; set; }
        public CollectionNameAttribute(string name) 
        { 
            Name = name; 
        }
    }
}
