using System.Collections.Generic;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal class RegisteredTypesList : List<RegisteredType>
    {
// ReSharper disable InconsistentNaming
        private static readonly RegisteredTypesList _none = new RegisteredTypesList();
// ReSharper restore InconsistentNaming

        public static RegisteredTypesList None
        {
            get { return _none; }
        }

        public bool NoDependencies
        {
            get { return Count == 0; }
        }
    }
}