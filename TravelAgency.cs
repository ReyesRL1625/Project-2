using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    //declaring a delegate container for the orderPladcedDelegate signature
    public delegate void orderPlacedDelegate();
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

        //constructor for travel agency with two buffers and the travel agency ID
        public TravelAgency(MultiCellBuffer newMBuffer, ConfirmationBuffer newCBuffer, Int32 newTravelAgencyID)
        {
            tBuffer = newMBuffer;
            tConfirmBuffer = newCBuffer;
            ticketsBought = 0;
            travelAgencyID = newTravelAgencyID;
        }

        //method that will be running as a thread
        public void travelAgencyFunc()
        {   
            //the thread will keep running until the airline threads terminate
            while (MyApplication.airline1T.IsAlive || MyApplication.airline2T.IsAlive)
            {
                Thread.Sleep(500);
                //Console.WriteLine("{0} is waiting for a price cut to buy tickets", Thread.CurrentThread.Name);   
            }
            Console.WriteLine("{0} terminating", Thread.CurrentThread.Name);
        }
        public void ticketsOnSale(Int32 p)
        {
            //create a new order object
            Order order = new Order();
            lock(order)
            //if the price that was passed in is less than 100, purchase a bulk of 20, otherwise purchase 10
            if(p < 100)
            {
                order.setAmount(20);
            }
            else
            {
                order.setAmount(10);
            }

            //generate a random number between 4000 and 8000 to simulate valid and invalid credit cards
            Int32 rand = rnd.Next(4000, 8000);

            //set the order attributes
            order.setCardNo(rand);
            order.setUnitPrice(p);
            order.setSenderId(this.travelAgencyID.ToString());
            order.setReceiverID(Thread.CurrentThread.Name);
            MyApplication.multiCellBufferPool.WaitOne();
            tBuffer.setOneCell(order);

            //emit an event when an order has been placed
            if(orderPlaced != null)
            {
                orderPlaced();
            }
            //Console.WriteLine("{0} sent an order", this.travelAgencyID);
        }

    }
}