using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Logging
{
    public class CommonLoggingExtensionStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            var type = context.Existing.GetType();
            var policy = context.Policies.Get<CommonLoggingPolicy>(type);
            if (policy == null)
            {
                return;
            }
            if (policy.LoggersCount == 0)
            {
                context.Policies.Set<CommonLoggingPolicy>(null, type);
                return;
            }
            policy.SetLogger(context.Existing);
        }
    }
}
