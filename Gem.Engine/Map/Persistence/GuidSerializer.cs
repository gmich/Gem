using Gem.Engine.Database;
using System;
namespace Gem.Map.Persistence
{
    public class GuidSerializer : ISerializer<Guid>
    {
        public byte[] Serialize(Guid value)
        {
            return value.ToByteArray();
        }

        public Guid Deserialize(byte[] buffer, int offset, int length)
        {
            if (length != 16)
            {
                throw new ArgumentException("length");
            }

            return BufferHelper.ReadBufferGuid(buffer, offset);
        }

        public bool IsFixedSize { get; } = true;

        public int Length { get; } = 16;

    }
}
