using System;
using System.IO;

namespace DES
{
    public class PBlock
    {
        internal static UInt64 PBlockEtap(UInt64 value, in int[] rule)
        {
            UInt64 itog = 0;
            int limit = rule.Length - 1;
            for (int j = limit; j > -1; j--)
            {
                itog |= ((value >> (rule[j] - 1)) & 1) << (limit - j);
            }
            return itog;
        }


        internal static int[] ShifrPBlock = ShifrPblockInit();
        internal static int[] DeShifrPBlock = DeShifrPblockInit();

        internal static int[] P =
            {16, 7, 20, 21, 29, 12,  28, 17,
            1,  15, 23, 26, 5, 18, 31, 10,
            2,  8,  24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };


        internal static int[] KeyGenPBlock =
            {57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
        };

        internal static int[] KeyEndPBlock =
        {
            14, 17, 11, 24, 1 , 5 , 3 , 28, 15, 6 , 21, 10, 23, 19, 12, 4 ,
            26, 8 , 16, 7 , 27, 20, 13, 2 , 41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        internal static int[] Extension =
            {32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        private static int[] ShifrPblockInit()
        {
            var pBlock = new int[64];
            int byteNumber = 56;
            for (int count = 0; count < 64; count++)
            {
                if (count == 32)
                {
                    byteNumber = 55;
                }
                if ((count % 8) == 0)
                {
                    byteNumber += 2;
                }
                pBlock[count] = byteNumber - 8 * (count % 8);
            }
            return pBlock;
        }

        private static int[] DeShifrPblockInit()
        {
            {
                int[] pBlock = new int[64];
                pBlock[0] = 40;
                for (int count = 1; count < 64; count++)
                {
                    int t = count - 1;
                    if ((count % 2) == 1)
                    {
                        pBlock[count] = (pBlock[t] - 32) % 65;
                    }
                    else
                    {
                        pBlock[count] = (pBlock[t] + 40) % 65;
                    }
                    if ((count % 8) == 0)
                    {
                        pBlock[count] = pBlock[t] + 7;
                    }
                }
                return pBlock;
            }
        }

    }
}