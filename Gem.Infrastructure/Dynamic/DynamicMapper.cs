using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Gem.Infrastructure.Dynamic
{
    /// <summary>
    /// Utility class for creating runtime mappers
    /// </summary>
    public class DynamicMapper
    {
        /// <summary>
        /// Creates a runtime delegate that maps two objects properties
        /// </summary>
        /// <typeparam name="TSource">The object's source</typeparam>
        /// <typeparam name="TDest">The object's destination</typeparam>
        /// <returns>A delegate that maps two objects properties</returns>
        public static Func<TSource, TDest> CreateMapper<TSource, TDest>()
        {
            var source = Expression.Parameter(typeof(TSource), "source");
            var dest = Expression.Variable(typeof(TDest), "dest");

            var assignments = from srcProp in
                                  typeof(TSource)
                                  .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                              where srcProp.CanRead
                              let destProp = typeof(TDest).
                                             GetProperty(srcProp.Name,
                                                         BindingFlags.Public | BindingFlags.Instance)
                              where (destProp != null) && (destProp.CanWrite)
                              select Expression.Assign(Expression.Property(dest, destProp),
                                                       Expression.Property(source, srcProp));

            var body = new List<Expression>();
            body.Add(Expression.Assign(dest, Expression.New(typeof(TDest))));
            body.AddRange(assignments);
            body.Add(dest);
            var expr = Expression.Lambda<Func<TSource, TDest>>(Expression.Block(new[] { dest },
                                                               body.ToArray()),
                                                               source);

            return expr.Compile();
        }
    }
}

