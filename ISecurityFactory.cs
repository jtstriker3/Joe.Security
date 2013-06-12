using System;
namespace Joe.Security
{
    public interface ISecurityFactory
    {
        ISecurity<TModel> Create<TModel>();
    }
}
