using System.Diagnostics.Contracts;

namespace Gem.Infrastructure.Functional
{
    public struct Maybe<T>
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                Contract.Requires(HasValue);

                return _value;
            }
        }

        public bool HasValue
        {
            get { return _value != null; }
        }

        public bool HasNoValue
        {
            get { return !HasValue; }
        }

        private Maybe(T value)
        {
            _value = value;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }
    }
}
