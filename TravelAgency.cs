using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class TravelAgency
    {
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
            //create an airline object
            Airline airline = new Airline(tBuffer, tConfirmBuffer);
            while (ticketsBought < ticketsNeeded)
            {
                Thread.Sleep(1000);
                Int32 p = airline.getPrice();
                //buy tickets
                //create an order
                Console.WriteLine("{0} has everyday low price: ${1} each", Thread.CurrentThread.Name, p);
                //Console.WriteLine("Travel agency {0} has bought {0} tickets", Thread.CurrentThread.Name, ticketsBought);
                
            }
        }
        public void ticketsOnSale(Int32 p)
        {
            ticketsBought++;
            Order order = new Order();
            if(p < 100)
            {
                order.setAmount(20);
            }
            else
            {
                order.setAmount(10);
            }
            Int32 rand = rnd.Next(4000, 8000);
            order.setCardNo(rand);
            order.setUnitPrice(p);
            
            order.setReceiverID(Thread.CurrentThread.Name);
            tBuffer.setOneCell(order);
            Console.WriteLine("{0} is selling good priced tickets, it's thread id is {1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
        }

    }
}