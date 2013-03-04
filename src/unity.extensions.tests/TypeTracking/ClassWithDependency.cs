namespace unity.extensions.tests.TypeTracking
{
    public class ClassWithDependency
    {
        private readonly IRepository _repository;

        public ClassWithDependency(IRepository repository)
        {
            _repository = repository;
        }
    }
}