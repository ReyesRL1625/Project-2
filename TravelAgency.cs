using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    //declaring a delegate container for the orderPladcedDelegate signature
    public delegate void orderPlacedDelegate(string orderAirlineName);
    class TravelAgency
    {
        //define an order placed event named orderPlaced
        public static event orderPlacedDelegate orderPlaced;

        //total number of tickets that will be needed by each travel agency thread
        private const Int32 ticketsNeeded = 200;

        //current number of tickets bought
        private Int32 ticketsBought;

        // random object used to generate a random number
        Random rnd = new Random();

        //buffer for initial orders
        MultiCellBuffer tBuffer;

        //confirmation buffer used for returning confirmed orders to travel agency
        ConfirmationBuffer tConfirmBuffer;

        //ID used to differentiate different travel agency threads
        private Int32 travelAgencyID;

        private bool willOrder;
        private Int32 salePrice;
        private string saleAirline;

        //constructor for travel agency with two buffers and the travel agency ID
        public TravelAgency(MultiCellBuffer newMBuffer, ConfirmationBuffer newCBuffer, Int32 newTravelAgencyID)
        {
            tBuffer = newMBuffer;
            tConfirmBuffer = newCBuffer;
            ticketsBought = 0;
            travelAgencyID = newTravelAgencyID;
            willOrder = false;
            salePrice = 0;
        }

        //method that will be running as a thread
        public void travelAgencyFunc()
        {   
            //the thread will keep running until the airline threads terminate
            while (MyApplication.airline1T.IsAlive || MyApplication.airline2T.IsAlive)
            {
                Thread.Sleep(500);
                if(willOrder)
                {
                    MyApplication.multiCellBufferPool.WaitOne();
                    placeOrder(salePrice);
                    //Console.WriteLine("{0} is ready for next pricecut", Thread.CurrentThread.Name);
                    willOrder = false;
                }
                //Console.WriteLine("{0} is waiting for a price cut to buy tickets", Thread.CurrentThread.Name);   
            }
            Console.WriteLine("{0} terminating", Thread.CurrentThread.Name);
        }
        public void ticketsOnSale(Int32 p)
        {
            willOrder = true;
            salePrice = p;
            saleAirline = Thread.CurrentThread.Name;
        }
        public void placeOrder(Int32 p)
        {
            Int32 amountOfTickets = 0;
            //if the price that was passed in is less than 100, purchase a bulk of 20, otherwise purchase 10
            if (p < 100)
            {
                amountOfTickets = 20;
            }
            else
            {
                amountOfTickets = 10;
            }

            //generate a random number between 4000 and 8000 to simulate valid and invalid credit cards
            Int32 rand = rnd.Next(4000, 8000);
            //Gets the time stamp
            string timeStamp = getTimestamp();
            Thread.Sleep(2000);
            tBuffer.setOneCell(amountOfTickets, rand, saleAirline, Thread.CurrentThread.Name, p, timeStamp);

            //emit an event when an order has been placed
            if (orderPlaced != null)
            {
                orderPlaced(saleAirline);
            }
            //Console.WriteLine("{0} sent an order", this.travelAgencyID);

            if(saleAirline.CompareTo("Southwest") == 0)
            {
                MyApplication.orderreceivedSouthwest.WaitOne();
            }
            else
            {
                MyApplication.orderreceivedDelta.WaitOne();
            }
                

        }

        //method in charge of getting the time stamp
        public string getTimestamp()
        {
            return string.Format("{0:yyyy-MM-dd  hh:mm:ss}", DateTime.Now);
        }

    }
}