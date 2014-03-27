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
            var key = typeof(TModel).FullName + "Security";
            var getSecurityTypeDelegate = (Func<Type>)(() =>
            {
                return GetSecurityType<TModel>();
            });

            var securityType = (Type)Joe.Caching.Cache.Instance.GetOrAdd(key, new TimeSpan(999, 0, 0), getSecurityTypeDelegate);
            //Check to see if we already found the model type

            if (securityType != null)
            {
                var compiledLambda = Expression.Lambda(Expression.Block(Expression.New(securityType))).Compile();
                return (ISecurity<TModel>)compiledLambda.DynamicInvoke();
            }

            return null;
        }

        private static Type GetSecurityType<TModel>()
        {
            Type securityType = null;
            Types = Types ?? AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type =>
                type.IsClass
                && !type.IsAbstract
                && type.GetInterfaces().Where(iface => iface.IsGenericType
                && typeof(ISecurity<>).IsAssignableFrom(iface.GetGenericTypeDefinition())).Count() > 0);
            try
            {
                securityType = securityType ?? Types.SingleOrDefault(type => typeof(ISecurity<TModel>).IsAssignableFrom(type));
            }
            catch (Exception ex)
            {
                throw new Exception(@"Error Creating Security Object. There may be multiple security objects for the model.", ex);
            }


            return securityType;
        }
    }
}
