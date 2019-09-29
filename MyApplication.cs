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

        //threads are made static so that travel agency know if the airline threads are still alive
        public static Thread airline1T;
        public static Thread airline2T;

        //initialize a semaphore with no resources available, they will get realeased after the buffer is created
        public static Semaphore multiCellBufferPool = new Semaphore(0, 3);

        static void Main(string[] args)
        {
            //create a multicell buffer, a confirmation buffer, and release three resources for the multicellbuffer
            buffer = new MultiCellBuffer();
            confirmBuffer = new ConfirmationBuffer();
            multiCellBufferPool.Release(3);


            //create two airline objects and pass in the same multicellbuffer to be shared for receiving orders
            Airline airline1 = new Airline(buffer, confirmBuffer);
            Airline airline2 = new Airline(buffer, confirmBuffer);

            //create two airline threads, name them, and start their airlinefunc running as a thread
            airline1T = new Thread(new ThreadStart(airline1.airlineFunc));
            airline2T = new Thread(new ThreadStart(airline2.airlineFunc));
            airline1T.Name = "Airline1";
            airline2T.Name = "Airline2";
            airline1T.Start();
            airline2T.Start();

            //this array will be used to store travel agency threads
            Thread[] travelAgency = new Thread[5];

            //Create five travel agency objects, assign it's id, subscribe it to the price cut event
            //and start a thread for it
            TravelAgency agency;
            for (int i = 0; i < 5; i++)
            {
                //create a travel agency object with it's own ID
                agency = new TravelAgency(buffer, confirmBuffer, (i+1));

                //Subscribing the travel agency object to the price cut event
                Airline.priceCut += new priceCutDelegate(agency.ticketsOnSale);

                //create a new thread, rename it, and start it
                travelAgency[i] = new Thread(new ThreadStart(agency.travelAgencyFunc));
                travelAgency[i].Name = "Travel Agency " + (i + 1).ToString();
                travelAgency[i].Start();
                Console.WriteLine("{0} started", travelAgency[i].Name);
            }
        }
    }
}
