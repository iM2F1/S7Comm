using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace S7ConnectionTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // initial
            var plc = new Plc(CpuType.S71500, "127.0.0.1", 105, 0, 1);
            //var plc = new Plc(CpuType.S71500, "192.168.1.176", 102, 0, 1);

            //1.連線
            plc.Open();
            // Initial Test Environment
            var DBCluster = new List<int>();
            DBCluster.Add(1);
            DBCluster.Add(2);
            DBCluster.Add(3);
            DBCluster.Add(4);
            DBCluster.Add(5);
            DBCluster.Add(6);
            DBCluster.Add(7);
            DBCluster.Add(8);
            DBCluster.Add(9);
            DBCluster.Add(10);

            Console.WriteLine("S7-1500 PLC 性能測試");
            var testtimes = 100;
            var testSize = 300;

            byte[] ret=new byte[testSize];
            foreach (int DBid in DBCluster)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine($"測試讀取 db{DBid} 長度:{testSize} 次數:{testtimes} 所需時間");
                Console.WriteLine("enter 測試開始");
                Console.ReadLine();

                List<byte> dummylist = new List<byte>();

                Console.WriteLine($"DB{DBid} 寫入測試資料");

                #region write Dummy Data

                ushort salt = ushort.Parse(DateTime.Now.Second.ToString());

                var saltDataMax = testSize + salt;
                for (ushort i = salt; i < saltDataMax; i++)
                {
                    dummylist.Add((byte)i);
                }

                plc.WriteBytes(DataType.DataBlock, DBid, 0, dummylist.ToArray());

                #endregion

                #region Read Performance

                var t1 = DateTime.Now;
                for (int i = 0; i < testtimes; i++)
                {
                    ret = plc.ReadBytes(DataType.DataBlock, DBid, 0, testSize);
                }

                var t2 = DateTime.Now;
                var timediff = t2.Subtract(t1).TotalMilliseconds;
                Console.WriteLine($"總共費時: {timediff}ms");

                #endregion

                // display dummy data
                foreach (var retvalue in ret)
                {
                    Console.Write($"{(ushort)retvalue} ");
                }
                Console.WriteLine();
            }

            //5.斷線
            plc.Close();
        }
    }
}
