using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class TravelAgency
    {
        private Int32 ticketsNeeded = 200;
        private Int32 ticketsBought = 0;
        public void travelAgencyFunc()
        {   //for starting thread
            Airline airline = new Airline();
            while (true)
            {
                Thread.Sleep(500);
                Int32 p = airline.getPrice();
                //buy tickets
                //create an order
                Console.WriteLine("Travel Agency{0} has everyday low price: ${1} each", Thread.CurrentThread.Name, p);
            }
        }
        public void ticketsOnSale(Int32 p)
        {  // Event handler
            //Create an order object
        }

    }
}