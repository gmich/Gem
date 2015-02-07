using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Other
{
    public class RandomGenerator
    {

        /// <summary>
        /// Fills an array with random bytes
        /// </summary>
        /// <param name="arrayLength">The array's length</param>
        /// <returns>An array of random bytes</returns>
        public static Byte[] GetRandomBytesArray(int arrayLength)
        {
            Guard.That(arrayLength, "Array length should be between 1 - " + byte.MaxValue).IsInRange(1, byte.MaxValue);

            Random rand = new Random();
            Byte[] bytes = new Byte[arrayLength];

            for (int i = 0; i < arrayLength; )
            {
                Byte[] b = new Byte[1];
                rand.NextBytes(b);
                if (!bytes.Contains(b[0]))
                {
                    bytes[i] = b[0];
                    i++;
                }
            }
            return bytes;
        }

        /// <summary>
        /// Returns a random byte
        /// </summary>
        /// <returns>A byte</returns>
        public static Byte GetRandomByte()
        {
            Random rand = new Random();
            Byte[] bytes = new Byte[1];

            rand.NextBytes(bytes);

            return bytes[0];
        }
    }
}
