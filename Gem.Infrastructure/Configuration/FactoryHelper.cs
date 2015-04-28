    using System;
    using System.Reflection;
namespace Gem.Infrastructure.Configuration
{
    /// <summary>
    /// Object construction helper.
    /// </summary>
    internal class FactoryHelper
    {
        private static Type[] emptyTypes = new Type[0];
        private static object[] emptyParams = new object[0];

        private FactoryHelper()
        {
        }

        internal static object CreateInstance(Type t)
        {
            ConstructorInfo constructor = t.GetConstructor(emptyTypes);
            if (constructor != null)
            {
                return constructor.Invoke(emptyParams);
            }
            else
            {
                //TODO: create an application specific exception
                throw new Exception("Cannot access the constructor of type: " + t.FullName);
            }
        }
    }
}
