using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class TravelAgency
    {
        private const Int32 ticketsNeeded = 200;
        private Int32 ticketsBought = 0;
        Random rnd = new Random();
        MultiCellBuffer tBuffer;

        public TravelAgency(MultiCellBuffer newBuffer)
        {
            tBuffer = newBuffer;
        }
        public void travelAgencyFunc()
        {   //for starting thread
            Airline airline = new Airline(tBuffer);
            while (ticketsBought < ticketsNeeded)
            {
                Thread.Sleep(500);
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
            Console.WriteLine("{0} going to purchase tickets", Thread.CurrentThread.Name);
        }

    }
}