using System;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal class Dependency
    {
        private readonly string _name;
        private readonly Type _type;

        public Dependency(Type type, string name)
        {
            _type = type;
            _name = name;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}