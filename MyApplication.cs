using System;
using System.Threading;

namespace Project_2
{
    class MyApplication
    {
        //buffer to be used for sending and recieving initial orders
        public static MultiCellBuffer buffer;

        //buffer used for confirming orders
        public static ConfirmationBuffer confirmBuffer;

        //constant number of travel agency threads running
        public const Int32 NumberOfTravelAgencies = 5;

        public static Semaphore _pool = new Semaphore(3, 3);

        static void Main(string[] args)
        {
            //create a multicell buffer
            buffer = new MultiCellBuffer();
            confirmBuffer = new ConfirmationBuffer();

            //create two airline objects and pass in the same multicellbuffer to be shared for receiving orders
            Console.WriteLine("Creating Two Airline Objects");
            Airline airline1 = new Airline(buffer, confirmBuffer);
            Airline airline2 = new Airline(buffer, confirmBuffer);

            //create two airline threads, name them, and start their airlinefunc running as a thread
            Console.WriteLine("Creating Two Airline Threads");
            Thread airline1T = new Thread(new ThreadStart(airline1.airlineFunc));
            Thread airline2T = new Thread(new ThreadStart(airline2.airlineFunc));
            airline1T.Name = "Airline1";
            airline2T.Name = "Airline2";
            Console.WriteLine("Starting Two Airline Threads");
            airline1T.Start();
            airline2T.Start();

            //this array will be used to store travel agency threads
            Thread[] travelAgency = new Thread[5];

            //for loop will run 5 times to create five travel agency objects, assign it's id, subscribe it to the price cut event
            //and start a thread for it
            TravelAgency agency;
            for (int i = 0; i < 5; i++)
            {
                //Creating five TravelAgency Threads using the same travel agency object
                agency = new TravelAgency(buffer, confirmBuffer, (i+1));

                //Subscribing the travel agency objects to the event
                Airline.priceCut += new priceCutEvent(agency.ticketsOnSale);
                travelAgency[i] = new Thread(new ThreadStart(agency.travelAgencyFunc));
                travelAgency[i].Name = "Travel Agency " + (i + 1).ToString();
                travelAgency[i].Start();
                Console.WriteLine("{0} started", travelAgency[i].Name);
            }
        }
    }
}
