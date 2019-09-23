using System;
using System.Threading;

namespace Project_2
{
    class MyApplication
    {
        static void Main(string[] args)
        {
            Airline airline1 = new Airline();
            MultiCellBuffer buffer = new MultiCellBuffer();
            //5 TravelAgency Threads
            Thread[] travelAgency = new Thread[5];
        }
    }
}
