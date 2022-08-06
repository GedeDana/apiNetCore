using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahas.Components
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DbTableAttribute : Attribute
    {
        public string Name { get; set; }

        public DbTableAttribute()
        {

        }

        public DbTableAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Struct)]
    public class DbColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }

        public DbColumnAttribute(string name, bool create = true, bool update = true)
        {
            Name = name;
            Create = create;
            Update = update;
        }

        public DbColumnAttribute(bool create = true, bool update = true)
        {
            Create = create;
            Update = update;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Struct)]
    public class DbKeyAttribute : Attribute
    {
        public bool Key { get; set; }
        public bool AutoIncrement { get; set; }
        public DbKeyAttribute(bool autoIncrement = false)
        {
            Key = true;
            AutoIncrement = autoIncrement;
        }
    }
}
