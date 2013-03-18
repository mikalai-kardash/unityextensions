using System;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal class RegisteredType
    {
        private readonly string _name;
        private readonly Type _type;

        public RegisteredType(Type type, string name)
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

        protected bool Equals(RegisteredType other)
        {
            return string.Equals(_name, other._name) && _type == other._type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_name != null ? _name.GetHashCode() : 0)*397) ^ (_type != null ? _type.GetHashCode() : 0);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((RegisteredType)obj);
        }
    }
}