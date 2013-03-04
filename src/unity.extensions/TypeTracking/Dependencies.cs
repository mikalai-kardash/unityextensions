using System.Collections.Generic;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal class Dependencies : List<Dependency>
    {
// ReSharper disable InconsistentNaming
        private static readonly Dependencies _none = new Dependencies();
// ReSharper restore InconsistentNaming

        public static Dependencies None
        {
            get
            {
                return _none;
            }
        }

        public bool NoDependencies
        {
            get
            {
                return Count == 0;
            }
        }
    }
}