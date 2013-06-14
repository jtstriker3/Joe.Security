using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Joe.Initialize;

namespace Joe.Security
{
    [Init(Function = "TrySetSecurityFactory")]
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

        public static void TrySetSecurityFactory()
        {
            try
            {
                //This only runs once so no reason to cache assemblies
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type =>
                    type.IsClass
                    && !type.IsAbstract
                    && type.GetInterfaces().Where(iface => iface.IsGenericType
                    && typeof(ISecurityProvider).IsAssignableFrom(iface.GetGenericTypeDefinition())).Count() > 0);

                if (types.Count() == 1 && Security.Provider == null)
                    Security.Provider = SecurityProviderFactory.Instance.CreateSecurityProvider(types.Single());
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
        }

    }
}
