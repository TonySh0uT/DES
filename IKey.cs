using System;
namespace DES
{
	public interface IKey
	{
		UInt64[] KeyGen(UInt64 value);
	}
}