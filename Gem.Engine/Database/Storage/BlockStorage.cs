using System;
using System.Collections.Generic;
using System.IO;

namespace Gem.Engine.Database
{
	public class BlockStorage : IBlockStorage
	{
		readonly Stream stream;
		readonly int blockSize;
		readonly int blockHeaderSize;
		readonly int blockContentSize;
		readonly int unitOfWork;
		readonly Dictionary<uint, Block> blocks = new Dictionary<uint, Block> ();

		public int DiskSectorSize {
			get {
				return unitOfWork;
			}
		}

		public int BlockSize {
			get {
				return blockSize;
			}
		}

		public int BlockHeaderSize {
			get {
				return blockHeaderSize;
			}
		}

		public int BlockContentSize {
			get {
				return blockContentSize;
			}
		}

		public BlockStorage (Stream storage, int blockSize = 40960, int blockHeaderSize = 48)
		{
			if (storage == null)
				throw new ArgumentNullException ("storage");

			if (blockHeaderSize >= blockSize) {
				throw new ArgumentException ("blockHeaderSize cannot be " +
					"larger than or equal " +
					"to " + "blockSize");
			}

			if (blockSize < 128) {
				throw new ArgumentException ("blockSize too small");
			}

			this.unitOfWork = ((blockSize >= 4096) ? 4096 : 128);
			this.blockSize = blockSize;
			this.blockHeaderSize = blockHeaderSize;
			this.blockContentSize = blockSize - blockHeaderSize;
			this.stream = storage;
		}

		public IBlock Find (uint blockId)
		{
			if (true == blocks.ContainsKey(blockId))
			{
				return blocks[blockId];
			}

			var blockPosition = blockId * blockSize;
			if ((blockPosition + blockSize) > this.stream.Length)
			{
				return null;
			}

			var firstSector = new byte[DiskSectorSize];
			stream.Position = blockId * blockSize;
			stream.Read (firstSector, 0, DiskSectorSize);

			var block = new Block (this, blockId, firstSector, this.stream);
			OnBlockInitialized (block);
			return block;
		}

		public IBlock CreateNew ()
		{
			if ((this.stream.Length % blockSize) != 0) {
				throw new DataMisalignedException ("Unexpected length of the stream: " + this.stream.Length);
			}

			var blockId = (uint)Math.Ceiling ((double)this.stream.Length / (double)blockSize);

			this.stream.SetLength ((long)((blockId * blockSize) + blockSize));
			this.stream.Flush ();

			var block = new Block (this, blockId, new byte[DiskSectorSize], this.stream);
			OnBlockInitialized (block);
			return block;
		}

		protected virtual void OnBlockInitialized (Block block)
		{
			blocks[block.Id] = block;
			block.Disposed += HandleBlockDisposed;
		}

		protected virtual void HandleBlockDisposed (object sender, EventArgs e)
		{
			var block = (Block)sender;
			block.Disposed -= HandleBlockDisposed;

			blocks.Remove (block.Id);
		}
	}
}
