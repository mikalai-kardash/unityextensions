using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Settings
{
    public class TypeRegistrationSettingStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            var type = context.Existing.GetType();
            var policy = context.Policies.Get<TypeSettingPolicy>(type);
            if (policy == null)
            {
                return;
            }
            if (policy.SettingCount == 0)
            {
                context.Policies.Set<TypeSettingPolicy>(null, type);
                return;
            }
            policy.Apply(context, context.Existing);
        }
    }
}
