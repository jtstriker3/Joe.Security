using System;
namespace Joe.Security
{
    public interface ISecurity
    {
        ISecurityProvider ProviderInstance { get; }
    }
}
