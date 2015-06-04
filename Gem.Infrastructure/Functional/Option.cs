using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Functional
{

    /// <summary>
    /// Enumeration with possible alternatives of the Option type
    /// </summary>
    public enum OptionType { Some, None };

    /// <summary>
    /// Represents option value that can be either 'None' or 'Some(v)'
    /// </summary>
    public abstract class Option<T>
    {
        private OptionType tag;

        /// <summary>
        /// Creates the option type and takes the type of the 
        /// created option as an argument.
        /// </summary>
        protected Option(OptionType tag)
        {
            this.tag = tag;
        }

        /// <summary>
        /// Specifies alternative represented by this instance
        /// </summary>
        public OptionType Tag { get { return tag; } }


        /// <summary>
        /// Matches 'None' alternative
        /// </summary>
        /// <returns>Returns true when succeeds</returns>
        public bool MatchNone()
        {
            return Tag == OptionType.None;
        }

        /// <summary>
        /// Matches 'Some' alternative
        /// </summary>
        /// <param name="value">When succeeds sets this parameter to the carried value</param>
        /// <returns>Returns true when succeeds</returns>
        public bool MatchSome(out T value)
        {
            if (Tag == OptionType.Some) value = ((Some<T>)this).Value;
            else value = default(T);
            return Tag == OptionType.Some;
        }
    }

    /// <summary>
    /// Inherited class representing empty option
    /// </summary>
    public class None<T> : Option<T>
    {
        public None()
            : base(OptionType.None)
        {
        }
    }

    /// <summary>
    /// Inherited class representing option with value
    /// </summary>
    public class Some<T> : Option<T>
    {
        public Some(T value)
            : base(OptionType.Some)
        {
            Value = value;
        }

        /// <summary>
        /// Returns value carried by the option
        /// </summary>
        public T Value { get; private set; }
    }

    /// <summary>
    /// Utility class for creating options
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Creates an empty option
        /// </summary>
        public static Option<T> None<T>()
        {
            return new None<T>();
        }

        /// <summary>
        /// Creates option with a value. This method can be
        /// used without type parameters thanks to C# type inference
        /// </summary>
        public static Option<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }
    }


    /// <summary>
    /// Contains utility methods for working with option values
    /// </summary>
    public static class OptionUtils
    {
        /// <summary>
        /// If the 'opt' argument contains a value, the function 'f' is called 
        /// with this value as an argument and the result is returned.
        /// </summary>
        public static Option<R> Bind<T, R>(this Option<T> opt, Func<T, Option<R>> f)
        {
            T value1;
            if (opt.MatchSome(out value1))
                return f(value1); // Just call the function
            else
                return Option.None<R>();
        }

        /// <summary>
        /// If the 'opt' argument contains a value, the value is processed 
        /// using 'f' function and is wrapend into a 'Some' constructor.
        /// </summary>
        public static Option<R> Map<T, R>(this Option<T> opt, Func<T, R> f)
        {
            T value1;
            if (opt.MatchSome(out value1))
                return Option.Some(f(value1)); // Call the function & wrap the result
            else
                return Option.None<R>();
        }
    }
}