using System;
namespace DES
{
	internal class NetworkFeistel : INetworkFeistel
	{
		private Key _key;
		private FuncFeistel _funcFeistel;
		private UInt64[] _keyArray;

		public NetworkFeistel(Key key, FuncFeistel funcFeistel, UInt64 keyValue)
		{
			_key = key;
			_funcFeistel = funcFeistel;
			_keyArray = _key.KeyGen(keyValue);
		}

		public UInt64 Encrypt(UInt64 value)
		{
			value = PBlock.PBlockEtap(value, PBlock.ShifrPBlock);
			var left = value >> 32;
			var right = value & (((UInt64)(1) << 32) - 1);
			UInt64 copy;
			for (int i = 0; i < 16; i++)
			{
				copy = right;
				right = left ^ _funcFeistel.Encrypt(right, _keyArray[i]);
				left = copy;
			}
			value = (left << 32) | right;
			value = PBlock.PBlockEtap(value, PBlock.DeShifrPBlock);
			return value;
		}

		public UInt64 Decrypt(UInt64 value)
		{
			value = PBlock.PBlockEtap(value, PBlock.ShifrPBlock);
			var left = value >> 32;
			var right = value & (((UInt64)(1) << 32) - 1);
			UInt64 copy;
			for (int i = 15; i > -1; i--)
			{
				copy = left;
				left = right ^ _funcFeistel.Encrypt(left, _keyArray[i]);
				right = copy;
			}
			value = (left << 32) | right;
			value = PBlock.PBlockEtap(value, PBlock.DeShifrPBlock);
			return value;
		}
	}
}