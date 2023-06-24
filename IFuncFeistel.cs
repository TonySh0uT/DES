using System;
namespace DES
{
	public interface IFuncFeistel
	{
		public UInt64 Encrypt(UInt64 value, UInt64 key);
	}
}