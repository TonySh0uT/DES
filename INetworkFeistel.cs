using System;
namespace DES
{
	public interface INetworkFeistel
	{
		public UInt64 Encrypt(UInt64 value);
		public UInt64 Decrypt(UInt64 value);
	}
}
