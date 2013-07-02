using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Joe.Security
{
    public class SecurityProviderFactory
    {
        protected SecurityProviderFactory()
        {

        }

        private static SecurityProviderFactory _instance;
        public static SecurityProviderFactory Instance
        {
            get
            {
                _instance = _instance ?? new SecurityProviderFactory();
                return _instance;
            }
        }

        public ISecurityProvider CreateSecurityProvider(Type iSecurityType)
        {
            if (typeof(ISecurityProvider).IsAssignableFrom(iSecurityType) && iSecurityType.IsClass && !iSecurityType.IsAbstract)
                return (ISecurityProvider)Expression.Lambda(Expression.Block(Expression.New(iSecurityType))).Compile().DynamicInvoke();
            else
                throw new InvalidCastException("You passed in a type that does not implement ISecurityProvider or is not an instantiable Class, in the future you should do this");

        }

        public ISecurityProvider CreateSecurityProviderByLookup()
        {
            try
            {
                //This only runs once so no reason to cache assemblies
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type =>
                    type.IsClass
                    && !type.IsAbstract
                    && type.GetInterfaces().Where(iface => typeof(ISecurityProvider).IsAssignableFrom(iface)).Count() > 0);

                if (types.Count() == 1 && Security._provider == null)
                    return SecurityProviderFactory.Instance.CreateSecurityProvider(types.Single());
            }
            catch (Exception ex)
            {
                try
                {
                    System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                    appLog.Source = "Joe.Security";
                    appLog.WriteEntry("Could not set Security Object: " + ex.Message);
                }
                catch
                {
                    //Do Nothing
                }
            }
            return null;
        }

    }
}
