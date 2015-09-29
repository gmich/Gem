using System;

namespace Gem.Engine.Database
{
	public class TreeKeyExistsException : Exception
	{
		public TreeKeyExistsException (object key) : base ("Duplicate key: " + key.ToString())
		{
			
		}
	}

}

