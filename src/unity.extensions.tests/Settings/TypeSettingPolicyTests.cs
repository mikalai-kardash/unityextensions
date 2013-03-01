using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Settings;
using NUnit.Framework;

namespace unity.extensions.tests.Settings
{
    [TestFixture]
    public class TypeSettingPolicyTests
    {
        [Test]
        public void Removes_property_setter_if_it_throws()
        {
            var tester = new ThrowableTester();
            var type = typeof(ThrowableTester);
            var policy = new TypeSettingPolicy();
            policy.AddSetting(type.GetProperty("Key"), new ThrowableAttribute());
            policy.Apply(new BuilderContextStub(), tester);
            Assert.That(policy.SettingCount, Is.EqualTo(0));
        }
    }

    public class BuilderContextStub : IBuilderContext
    {
        public void AddResolverOverrides(IEnumerable<ResolverOverride> newOverrides)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolverPolicy GetOverriddenResolver(Type dependencyType)
        {
            throw new NotImplementedException();
        }

        public object NewBuildUp(NamedTypeBuildKey newBuildKey)
        {
            return new ThrowableRepository();
        }

        public object NewBuildUp(NamedTypeBuildKey newBuildKey, Action<IBuilderContext> childCustomizationBlock)
        {
            throw new NotImplementedException();
        }

        public IStrategyChain Strategies { get; private set; }
        public ILifetimeContainer Lifetime { get; private set; }
        public NamedTypeBuildKey OriginalBuildKey { get; private set; }
        public NamedTypeBuildKey BuildKey { get; set; }
        public IPolicyList PersistentPolicies { get; private set; }
        public IPolicyList Policies { get; private set; }
        public IRecoveryStack RecoveryStack { get; private set; }
        public object Existing { get; set; }
        public bool BuildComplete { get; set; }
        public object CurrentOperation { get; set; }
        public IBuilderContext ChildContext { get; private set; }
    }
}
