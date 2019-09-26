using System;
using System.Threading;

namespace Project_2
{
    class MyApplication
    {
        public static MultiCellBuffer buffer;
        static void Main(string[] args)
        {
            Airline airline1 = new Airline();
            buffer = new MultiCellBuffer();
            //5 TravelAgency Threads
            Thread[] travelAgency = new Thread[5];
        }
    }
}
