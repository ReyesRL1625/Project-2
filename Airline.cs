using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    public delegate void priceCutEvent(Int32 p);
    class Airline
    {
        public static event priceCutEvent priceCut;
        private static Int32 ticketPrice = 200;
        public Int32 counter = 0;
        public Int32 getPrice() { return ticketPrice; }
        public static void changePrice(Int32 price)
        {
            if (price < ticketPrice)
                if (priceCut != null)
                    priceCut(price);
            ticketPrice = price;
        }

        public void airlineFunc()
        {
            //TODO: CHANGE ITERATION NUMBER
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(500);
                ticketPrice = pricingModel();
                changePrice(ticketPrice);
            }
        }

        public Int32 pricingModel()
        {

            return 200;
        }

        public void OrderProcessing()
        {

        }
    }
}
