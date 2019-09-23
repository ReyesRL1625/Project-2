using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    public delegate void priceCutEvent(Int32 p);

    class Airline
    {
        static Random rng = new Random();
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
            /*
            Int32[] pricesForWeek = new Int32[7];
            pricesForWeek[0] = 180; //Sunday
            pricesForWeek[1] = 100; //Monday
            pricesForWeek[2] = 100; //Tuesday
            pricesForWeek[3] = 90;  //Wednesday
            pricesForWeek[4] = 80; //Thursday
            pricesForWeek[5] = 120; //Friday
            pricesForWeek[6] = 200; //Saturday
            Int32 currentDay = 0; //represents day of the week
            */

            //TODO: CHANGE ITERATION NUMBER
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(500);
                ticketPrice = pricingModel();
                /*
                if (currentDay > 6)
                {
                    currentDay = 0;
                }
                else
                {
                    currentDay++;
                }
                */
                changePrice(ticketPrice);
            }
        }

        public Int32 pricingModel()
        {

            Int32 p = rng.Next(50, 200);
            return p;
        }

        public void OrderProcessing()
        {

        }
    }
}
