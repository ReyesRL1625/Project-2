using System;
using System.Threading;

namespace Project_2
{
    class MyApplication
    {
        public static MultiCellBuffer buffer;
        public static Int32 NumberOfTravelAgencies = 5;

        static void Main(string[] args)
        {
            //create a multicell buffer
            buffer = new MultiCellBuffer();
            Console.WriteLine("Creating Two Airline Objects");
            Airline airline1 = new Airline();
            Airline airline2 = new Airline();
            Console.WriteLine("Finished Creating The Two Airline Objects");

            Console.WriteLine("Creating Two Airline Threads");
            Thread airline1T = new Thread(new ThreadStart(airline1.airlineFunc));
            Thread airline2T = new Thread(new ThreadStart(airline2.airlineFunc));

            Console.WriteLine("Starting Two Airline Threads");
            airline1T.Start();
            airline2T.Start();

            //5 TravelAgency Threads
            Console.WriteLine("Creating a travel agency object");
            TravelAgency agency = new TravelAgency();
            Console.WriteLine("Subscribing the travel agency object to the event");
            Airline.priceCut += new priceCutEvent(agency.ticketsOnSale);
            Console.WriteLine("Creating 5 travel agency threads and starting them");
            Thread[] travelAgency = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                travelAgency[i] = new Thread(new ThreadStart(agency.travelAgencyFunc));
                travelAgency[i].Name = (i + 1).ToString();
                travelAgency[i].Start();
                Console.WriteLine("Travel Agency {0} started", travelAgency[i].Name);
            }
        }
        
        
            
        
    }
}
