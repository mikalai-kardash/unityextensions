using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Settings
{
    public class TypeRegistrationSettingStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            var policy = context.Policies.Get<TypeSettingPolicy>(context.Existing.GetType());
            if (policy == null)
            {
                return;
            }
            policy.Apply(context, context.Existing);
        }
    }
}
