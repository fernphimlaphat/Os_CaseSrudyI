using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Problem01
{
    class Program
    {
        static int myThread = 10;
        static int numData = 1000000000;
        static byte[] Data_Global = new byte[numData];
        static long Sum_Global = 0;
        static int G_index = numData/myThread;
        
        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        
        static void sum(object p)
        {
            int z = (int) p;
            int min = G_index*z;
            int max = G_index*(z+1)-1;
            long sum_i = 0;
            for (int index = min; index <= max; index++)
            {
                if (Data_Global[index] % 2 == 0)
                {
                    sum_i -= Data_Global[index];
                }
                else if (Data_Global[index] % 3 == 0)
                {
                    sum_i += (Data_Global[index]*2);
                }
                else if (Data_Global[index] % 5 == 0)
                {
                    sum_i += (Data_Global[index] / 2);
                }
                else if (Data_Global[index] %7 == 0)
                {
                    sum_i += (Data_Global[index] / 3);
                }
                Data_Global[index] = 0;  
            }
            Sum_Global += sum_i;
        }
        
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;
            Thread[] myth = new Thread[myThread];
            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();
            for(i = 0;i < myThread;i++){
                myth[i] = new Thread(sum);
                myth[i].Priority = ThreadPriority.Highest;
                myth[i].Start(i);
            }
            for(i = 0;i < myThread;i++)
                myth[i].Join();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}