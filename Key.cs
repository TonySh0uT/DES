using System;

namespace DES
{
	internal class Key : IKey
	{
		public UInt64[] KeyGen(UInt64 value)
		{
			UInt64[] keyArray = new UInt64[16];
			UInt64 blockC, blockD;
			int shift;
			value = PBlock.PBlockEtap(value, PBlock.KeyGenPBlock);
			blockC = value >> 28;
			blockD = value & ((1 << 28) - 1);
			for (int i = 0; i < 16; i++)
			{
				shift = (i == 0 || i == 1 || i == 8 || i == 15) ? 1 : 2;
				blockC = ((blockC << shift) | (blockC >> (28 - shift))) & ((1 << 28) - 1);
				blockD = ((blockD << shift) | (blockD >> (28 - shift))) & ((1 << 28) - 1);
				keyArray[i] = (blockC << 28) | blockD;
			}
			return keyArray;
		}
	}
}