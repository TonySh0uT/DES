using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public class DES
    {
        public enum ShiftType
        {
            ECB,
            CBC,
            CFB,
            OFB,
            CTR,
            RD,
            RDH
        };

        private NetworkFeistel _networkFeistel;
        private ShiftType _type;
        private UInt64 _initVector;

        internal DES(UInt64 key, ShiftType shiftType, UInt64 initvector = 92424142112143524, params object[] optionals)
        {
            _type = shiftType;
            _initVector = initvector;
            _networkFeistel = new NetworkFeistel(new Key(), new FuncFeistel(), key);

        }

        internal void Shifr(ref byte[] value)
        {
            if (value.Length % 8 != 0)
            {
                Array.Resize(ref value, value.Length + (8 - (value.Length % 8)));
                value[value.Length - 1] = (byte)(8 - (value.Length % 8));
            }
            UInt64[] massive = new UInt64[value.Length / 8];
            for (int i = 0; i <= value.Length / 8 - 1; i++)
            {
                massive[i] = BitConverter.ToUInt64(value, i * 8);
            }

            switch (_type)
            {
                case ShiftType.ECB:
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] = _networkFeistel.Encrypt(massive[i]);
                    });
                    break;

                case ShiftType.CBC:
                    massive[0] ^= _initVector;
                    massive[0] = _networkFeistel.Encrypt(massive[0]);
                    for (int i = 1; i < massive.Length; i++)
                    {
                        massive[i] ^= massive[i - 1];
                        massive[i] = _networkFeistel.Encrypt(massive[i]);
                    }
                    break;

                case ShiftType.CFB:
                    massive[0] ^= _networkFeistel.Encrypt(_initVector);
                    for (int i = 1; i < massive.Length; i++)
                    {
                        massive[i] ^= _networkFeistel.Encrypt(massive[i - 1]);
                    }
                    break;

                case ShiftType.OFB:
                    var copyVectorInit = _initVector;
                    for (int i = 0; i < massive.Length; i++)
                    {
                        copyVectorInit = _networkFeistel.Encrypt(copyVectorInit);
                        massive[i] ^= copyVectorInit;
                    }
                    break;

                case ShiftType.CTR:
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector ^ (UInt64)i);
                    });
                    break;

                case ShiftType.RD:
                    var delta = _initVector >> (BitConverter.GetBytes(_initVector).Length * 8 / 2);
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector + delta * (UInt64)(i + 1));
                    });
                    break;

                case ShiftType.RDH:
                    delta = _initVector >> (BitConverter.GetBytes(_initVector).Length * 8 / 2);
                    UInt64 hash = (UInt64)_initVector.GetHashCode();
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector + delta * (UInt64)(i + 2));
                    });
                    break;
            }
            Buffer.BlockCopy(massive, 0, value, 0, value.Length);

        }

        internal void DeShifr(ref byte[] value)
        {
            UInt64[] massive = new UInt64[value.Length / 8];
            for (int i = 0; i <= value.Length / 8 - 1; i++)
            {
                massive[i] = BitConverter.ToUInt64(value, i * 8);
            }

            switch (_type)
            {
                case ShiftType.ECB:
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] = _networkFeistel.Decrypt(massive[i]);
                    });
                    break;

                case ShiftType.CBC:
                    var copy = new UInt64[massive.Length];
                    massive.CopyTo(copy, 0);
                    Parallel.For(1, massive.Length, i =>
                    {
                        massive[i] = _networkFeistel.Decrypt(massive[i]);
                        massive[i] ^= copy[i - 1];
                    });
                    massive[0] = _networkFeistel.Decrypt(massive[0]);
                    massive[0] ^= _initVector;
                    break;

                case ShiftType.CFB:
                    copy = new UInt64[massive.Length];
                    massive.CopyTo(copy, 0);
                    massive[0] ^= _networkFeistel.Encrypt(_initVector);
                    Parallel.For(1, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(copy[i - 1]);
                    });
                    break;

                case ShiftType.OFB:
                    var copyVectorInit = _initVector;
                    for (int i = 0; i < massive.Length; i++)
                    {
                        copyVectorInit = _networkFeistel.Encrypt(copyVectorInit);
                        massive[i] ^= copyVectorInit;
                    }
                    break;

                case ShiftType.CTR:
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector ^ (UInt64)i);
                    });
                    break;

                case ShiftType.RD:
                    var delta = _initVector >> (BitConverter.GetBytes(_initVector).Length * 8 / 2);
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector + delta * (UInt64)(i + 1));
                    });
                    break;

                case ShiftType.RDH:
                    delta = _initVector >> (BitConverter.GetBytes(_initVector).Length * 8 / 2);
                    UInt64 hash = (UInt64)_initVector.GetHashCode();
                    Parallel.For(0, massive.Length, i =>
                    {
                        massive[i] ^= _networkFeistel.Encrypt(_initVector + delta * (UInt64)(i + 2));
                    });
                    break;
            }
            Buffer.BlockCopy(massive, 0, value, 0, value.Length);
        }
    }
}