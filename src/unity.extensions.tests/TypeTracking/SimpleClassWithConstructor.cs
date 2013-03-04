namespace unity.extensions.tests.TypeTracking
{
    public class SimpleClassWithConstructor
    {
// ReSharper disable NotAccessedField.Local
        private readonly SimpleDependency _dp;
// ReSharper restore NotAccessedField.Local

        public SimpleClassWithConstructor(SimpleDependency dp)
        {
            _dp = dp;
        }
    }
}