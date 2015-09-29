using System;
using Gem.Engine.Database;

namespace Gem.Map.Persistence
{

    public class MapSerializer
    {
        public byte[] Serialize(MapModel model)
        {
            var nameBytes = System.Text.Encoding.UTF8.GetBytes(model.Name);
            var modelData = new byte[
                16 +                   // 16 bytes for Guid id
                4 +                    // 4 bytes indicate the length of the `name` string
                nameBytes.Length +     // z bytes for name 
                4 +                    // 4 bytes for x
                4 +                    // 4 bytes for y
                4 +                    // 4 bytes for sizeX
                4                      // 4 bytes for sizeY
            ];

            // Id
            Buffer.BlockCopy(
                      src: model.Id.ToByteArray(),
                srcOffset: 0,
                      dst: modelData,
                dstOffset: 0,
                    count: 16
            );


            // Name
            Buffer.BlockCopy(
                      src: LittleEndianByteOrder.GetBytes((int)nameBytes.Length),
                srcOffset: 0,
                      dst: modelData,
                dstOffset: 16 + 4,
                    count: 4
            );

            Buffer.BlockCopy(
                      src: nameBytes,
                srcOffset: 0,
                      dst: modelData,
                dstOffset: 16 + 4 + 4 + 4,
                    count: nameBytes.Length
            );


            Buffer.BlockCopy(
                      src: LittleEndianByteOrder.GetBytes((int)model.X),
                srcOffset: 0,
                      dst: modelData,
                dstOffset: 16 + 4 + 4 + nameBytes.Length,
                    count: 4
            );

            return modelData;
        }

        public MapModel Deserializer(byte[] data)
        {
            var model = new MapModel();

            // Read id
            model.Id = BufferHelper.ReadBufferGuid(data, 0);

            // Read breed
            var breedLength = BufferHelper.ReadBufferInt32(data, 16);
            if (breedLength < 0 || breedLength > (16 * 1024))
            {
                throw new Exception("Invalid string length: " + breedLength);
            }

            // Read name
            var nameLength = BufferHelper.ReadBufferInt32(data, 16 + 4 + breedLength);
            if (nameLength < 0 || nameLength > (16 * 1024))
            {
                throw new Exception("Invalid string length: " + nameLength);
            }
            model.Name = System.Text.Encoding.UTF8.GetString(data, 16 + 4 + breedLength + 4, nameLength);

            // Read age
            model.X = BufferHelper.ReadBufferInt32(data, 16 + 4 + breedLength + 4 + nameLength);


            return model;
        }
    }
}


