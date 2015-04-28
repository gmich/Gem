using System;
using System.Reflection;

namespace Gem.Infrastructure.Configuration
{
    internal interface IFactory
    {
        void Clear();

        void ScanTypes(Type[] type, string prefix);

        void RegisterType(Type type, string itemNamePrefix);
    }
}
