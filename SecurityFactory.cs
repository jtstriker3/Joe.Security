using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Joe.Security
{
    public class SecurityFactory : ISecurityFactory
    {
        protected static IEnumerable<Type> Types { get; set; }

        private static Dictionary<String, Delegate> _createSecurityTypes;
        protected static Dictionary<String, Delegate> CreateSecurityTypes
        {
            get
            {
                _createSecurityTypes = _createSecurityTypes ?? new Dictionary<String, Delegate>();
                return _createSecurityTypes;
            }
        }
        private static ISecurityFactory _instance;
        public static ISecurityFactory Instance
        {
            get
            {
                _instance = _instance ?? new SecurityFactory();
                return _instance;
            }
        }

        //Make Private Constructor this way you must use as a singleton
        protected SecurityFactory()
        {

        }

        public virtual ISecurity<TModel> Create<TModel>()
        {
            //Check to see if we already found the model type
            if (CreateSecurityTypes.ContainsKey(typeof(TModel).FullName))
            {
                var compiledLambda = CreateSecurityTypes[typeof(TModel).FullName];
                if (compiledLambda != null)
                    return (ISecurity<TModel>)compiledLambda.DynamicInvoke();
                return null;
            }

            Type SecurityType = null;
            Types = Types ?? AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type =>
                type.IsClass
                && !type.IsAbstract
                && type.GetInterfaces().Where(iface => iface.IsGenericType
                && typeof(ISecurity<>).IsAssignableFrom(iface.GetGenericTypeDefinition())).Count() > 0);
            try
            {
                SecurityType = SecurityType ?? Types.SingleOrDefault(type => typeof(ISecurity<TModel>).IsAssignableFrom(type));
            }
            catch (Exception ex)
            {
                throw new Exception(@"Error Creating Security Object. There may be multiple security objects for the model.", ex);
            }

            if (SecurityType != null)
            {
                var compiledLambda = Expression.Lambda(Expression.Block(Expression.New(SecurityType))).Compile();
                CreateSecurityTypes.Add(typeof(TModel).FullName, compiledLambda);
                return (ISecurity<TModel>)compiledLambda.DynamicInvoke();
            }
            else
                CreateSecurityTypes.Add(typeof(TModel).FullName, null);
            return null;
        }
    }
}
