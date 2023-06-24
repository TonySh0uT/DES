using System;

namespace DES
{
	internal class FuncFeistel : IFuncFeistel
	{
		public UInt64 Encrypt(UInt64 value, UInt64 key)
		{
			value = PBlock.PBlockEtap(value, PBlock.Extension);
			value ^= key;
			value = SBlock.SBlockEtap(value, SBlock.sbox, 6);
			value = PBlock.PBlockEtap(value, PBlock.P);
			return value;
		}
	}
}