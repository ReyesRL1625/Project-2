using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    public delegate void priceCutEvent(Int32 p);

    class Airline
    {
        //define a price cut event named 
        public static event priceCutEvent priceCut;
        public Int32[] pricesForWeek;
        private static Int32 ticketPrice;
        public Int32 availableTickets = 500;
        public static Int32 numberOfPriceCuts = 0;
        MultiCellBuffer aBuffer;
        ConfirmationBuffer aConfirmBuffer;
        private Int32 currentDay;

        public Airline(MultiCellBuffer newBuffer, ConfirmationBuffer newCBufffer)
        {
            aBuffer = newBuffer;
            aConfirmBuffer = newCBufffer;
            ticketPrice = 100;
            pricesForWeek = new Int32[7];
            pricesForWeek[0] = 180; //Sunday
            pricesForWeek[1] = 100; //Monday
            pricesForWeek[2] = 100; //Tuesday
            pricesForWeek[3] = 90;  //Wednesday
            pricesForWeek[4] = 80; //Thursday
            pricesForWeek[5] = 120; //Friday
            pricesForWeek[6] = 200; //Saturday
            currentDay = 0; //represents day of the week
        }

        public Int32 getPrice() { return ticketPrice; }
        public static void changePrice(Int32 price)
        {
            if (price < ticketPrice)
                if (priceCut != null)
                {
                    priceCut(price);
                    //increment counter
                    numberOfPriceCuts++;
                }
            ticketPrice = price;
        }

        public void airlineFunc()
        {
            while (numberOfPriceCuts <= 20)
            {
                if (currentDay == 6)
                {
                    currentDay = 0;
                }
                else
                {
                    currentDay++;
                }

                Thread.Sleep(500);
                Int32 newPrice = pricingModel(currentDay);
                Airline.changePrice(newPrice);
                MyApplication._pool.WaitOne();
                //receiving order object from the multicell buffer
                Order order = aBuffer.getOneCell();
                MyApplication._pool.Release();
                //creating new order processing thread to process the order
                OrderProcessing orderProcessing = new OrderProcessing(order);
                Thread newOrder = new Thread(new ThreadStart(orderProcessing.processOrder));
                newOrder.Start();
            }
        }

        public Int32 pricingModel(Int32 currentDay)
        {
            Int32 p = pricesForWeek[currentDay];
            return p;
        }
    }
}