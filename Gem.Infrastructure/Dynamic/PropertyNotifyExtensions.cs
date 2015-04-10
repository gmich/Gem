using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Gem.Infrastructure.Dynamic
{
    /// <summary>
    /// PropertyChangedEventHandler extensions to setup notify property changed events using lambda expressions
    /// <remarks>The class must implement INotifyPropertyChanged / INotifyPropertyChanging interface</remarks>
    /// </summary>
    public static class PropertyNotifyExtensions
    {
        /// <summary>
        /// Raises notify property changed events.
        /// </summary>
        /// <typeparam name="TProperty">The property's type</typeparam>
        /// <param name="handler">The <see cref="PropertyChangedEventHandler"/> handler</param>
        /// <param name="newValue">The new value </param>
        /// <param name="oldValueExpression">A delegate that returns the old value</param>
        /// <param name="setter">The new value setter</param>
        /// <returns>The new value</returns>
        public static TProperty SetNotifyProperty<TProperty>(this PropertyChangedEventHandler handler,
                                                             TProperty newValue,
                                                             Expression<Func<TProperty>> oldValueExpression,
                                                             Action<TProperty> setter)
        {
            return SetNotifyProperty(handler, null, newValue,
            oldValueExpression, setter);
        }

        /// <summary>
        /// Raises notify property changed / changing events
        /// </summary>
        /// <typeparam name="TProperty">The property's type</typeparam>
        /// <param name="postHandler">The <see cref="PropertyChangedEventHandler"/> handler</param>
        /// <param name="preHandler">The <see cref="PropertyChangingEventHandler"/> handler</param>
        /// <param name="newValue">The new value </param>
        /// <param name="oldValueExpression">A delegate that returns the old value</param>
        /// <param name="setter">The new value setter</param>
        /// <returns>The new value</returns>
        public static TProperty SetNotifyProperty<TProperty>(this PropertyChangedEventHandler postHandler,
                                             PropertyChangingEventHandler preHandler,
                                             TProperty newValue, 
                                             Expression<Func<TProperty>> oldValueExpression,
                                             Action<TProperty> setter)
        {
            Func<TProperty> getter = oldValueExpression.Compile();
            TProperty oldValue = getter();

            if (!oldValue.Equals(newValue))
            {
                var body = oldValueExpression.Body as MemberExpression;
                var propInfo = body.Member as PropertyInfo;
                string propName = body.Member.Name;
                var targetExpression = body.Expression as ConstantExpression;
                object target = targetExpression.Value;

                if (preHandler != null)
                {
                    preHandler(target, new
                    PropertyChangingEventArgs(propName));
                }
                setter(newValue);

                if (postHandler != null)
                {
                    postHandler(target, new PropertyChangedEventArgs(propName));
                }
            }
            return newValue;
        }
    }
}
