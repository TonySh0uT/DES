using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DES
{
    class Program
    {
        static private byte[] buffer = new byte[20480];
        
        static void ReadAndWrite(string readFile, string writeFile, bool mode)
        {
            var des = new DES(152154, DES.ShiftType.CBC);
            using FileStream rstream = new(readFile, FileMode.Open);
            using FileStream wstream = new(writeFile, FileMode.Create);
            Task task;
            var numbytes = rstream.Length;
            while (numbytes > 0)
            {
                rstream.Read(buffer, 0, buffer.Length);
                if (mode)
                {
                    des.Shifr(ref buffer);
                }
                else
                {
                    des.DeShifr(ref buffer);
                }
                numbytes -= buffer.Length;
                task = WriteAsync(wstream);
                task.Wait();
            }
        }

        static internal async Task WriteAsync(FileStream wstream)
        {
            await wstream.WriteAsync(buffer, 0, buffer.Length);
        }


        static void Main(string[] args)
        {
            ReadAndWrite("C:/Users/eboni/source/repos/DES/test.mp4", "/Users/eboni/source/repos/DES/test2.mp4", true);
            ReadAndWrite("/Users/eboni/source/repos/DES/test2.mp4", "/Users/eboni/source/repos/DES/test3.mp4", false);
        }
    }
}