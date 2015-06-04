using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Functional
{
    public abstract class ValidatorPrimitive<TPrimitive> 
        where TPrimitive : struct
    {
        private readonly TPrimitive _value;

        private ValidatorPrimitive(TPrimitive value)
        {
            _value = value;
        }

        //public abstract Result<TPrimitive> Create(TPrimitive primitive);

        public static implicit operator TPrimitive(ValidatorPrimitive<TPrimitive> primitive)
        {
            return primitive._value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ValidatorPrimitive<TPrimitive>;

            if (ReferenceEquals(other, null))
                return false;

            return _value.Equals((TPrimitive)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
