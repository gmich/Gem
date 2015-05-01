using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Gem.Infrastructure.Attributes
{
    /// <summary>
    /// Helper methods for retrieving values stored in DefaultValueAttribute
    /// </summary>
    public static class DefaultValueHelper
    {
        /// <summary>
        /// Retrieves the default value that's assigned to a property using the <see cref="DefaultValueAttribute"/>
        /// </summary>
        /// <typeparam name="TDefaultValueType">The property's type</typeparam>
        /// <typeparam name="TSource">The type of the class that contains the property</typeparam>
        /// <param name="instance">The instance of the class that contains the property c</param>
        /// <param name="propertyRetriever">The property that's decorated with the DefaultValueAttribute</param>
        /// <returns>The DefaultValueAttribute's value</returns>
        public static TDefaultValueType GetDefaultValueFor<TDefaultValueType, TSource>(this TSource instance,
                                                                                       Expression<Func<TSource, TDefaultValueType>> propertyRetriever)

            where TSource : class
            //Not yet tested for classes
            //where TDefaultValueType : struct
        {
            var propertyInfo = GetPropertyInfo(instance, propertyRetriever);

            return (TDefaultValueType)GetDefaultValueFor(instance, propertyInfo); 
        }

        /// <summary>
        /// Helper class that uses the property info to retrieve the value stored in DefaultValueAttribute
        /// </summary>
        /// <returns>The default value</returns>
        private static object GetDefaultValueFor<TSource>(TSource instance, PropertyInfo propertyInfo)
                where TSource : class
        {
            AttributeCollection attributes =
                TypeDescriptor.GetProperties(instance)[propertyInfo.Name].Attributes;

            var myAttribute =
                attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

            return (myAttribute != null) ?
                    myAttribute.Value :
                    Activator.CreateInstance(propertyInfo.PropertyType);
        }

        /// <summary>
        /// Helper method for retrieving property info using Expressions
        /// </summary>
        /// <typeparam name="TProperty">The property's type</typeparam>
        /// <typeparam name="TSource">The source</typeparam>
        /// <returns>The property info that's returned in the expression</returns>
        private static PropertyInfo GetPropertyInfo<TProperty, TSource>(TSource source, 
                                                                        Expression<Func<TSource, TProperty>> propertyLambda)
               where TSource : class
              //where TProperty : struct
        {
            Type type = typeof(TSource);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                //Expression refers to a method, not a property
                return null;
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                // Expression refers to a field, not a property             
                return null;
            }

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
            {
                //Expresion refers to a property that is not from type 
                return null;
            }

            return propInfo;
        }

        /// <summary>
        /// Assigns all the default values to the properties that are defined with the <see cref="DefaultValueAttribute"/>
        /// </summary>
        /// <typeparam name="TClass">The object's type</typeparam>
        /// <param name="instance">The instance of the object to assign the default values</param>
        public static void AssignDefaultValues<TClass>(this TClass instance)
            where TClass : class
        {
            typeof(TClass)
            .GetProperties()
            .Where(property => property.IsDefined(typeof(DefaultValueAttribute), true))
            .ToList()
            .ForEach(property =>
                {
                    if (property != null)
                        property.SetValue(instance, GetDefaultValueFor(instance,property));
                });
        }
    }
}
