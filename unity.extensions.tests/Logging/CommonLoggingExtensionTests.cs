using Common.Logging;
using Common.Logging.Simple;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Logging;
using NUnit.Framework;

namespace unity.extensions.tests.Logging
{
    [TestFixture]
    public class CommonLoggingExtensionTests
    {
        [Test]
        public void Assigns_logger()
        {
            var container = new UnityContainer();
            container.AddNewExtension<CommonLoggingExtension>();
            container.RegisterType<LoggerTester>();
            var l = container.Resolve<LoggerTester>();
            Assert.That(l.Log, Is.Not.Null);
            Assert.That(l.Log, Is.TypeOf<ConsoleOutLogger>());
        }
    }

    public class LoggerTester
    {
        [ConsoleOutLogger]
        public ILog Log { get; set; }
    }
}
