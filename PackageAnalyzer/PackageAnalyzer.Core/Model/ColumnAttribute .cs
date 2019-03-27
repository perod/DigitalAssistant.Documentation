using System;

namespace PackageAnalyzer.Core.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(int order, string title)
        {
            Order = order;
            Title = title;
        }

        public ColumnAttribute(int order) : this(order, string.Empty)
        {
        }

        public int Order { get; set; }
        public string Title { get; set; }
    }
    
}
