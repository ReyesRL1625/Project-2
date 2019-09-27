using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    public delegate void priceCutEvent(Int32 p);

    class Airline
    {
        public Int32[] pricesForWeek;
        //static Random rng = new Random(); //to generate random ticket prices
        public static event priceCutEvent priceCut;
        private static Int32 ticketPrice = 200;
        public Int32 availableTickets = 500;
        public static Int32 numberOfPriceCuts = 0;

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
            Int32[] pricesForWeek = new Int32[7];
            pricesForWeek[0] = 180; //Sunday
            pricesForWeek[1] = 100; //Monday
            pricesForWeek[2] = 100; //Tuesday
            pricesForWeek[3] = 90;  //Wednesday
            pricesForWeek[4] = 80; //Thursday
            pricesForWeek[5] = 120; //Friday
            pricesForWeek[6] = 200; //Saturday
            Int32 currentDay = 0; //represents day of the week

            while (numberOfPriceCuts <= 20)
            {
                if (currentDay > 6)
                {
                    currentDay = 0;
                }
                else
                {
                    currentDay++;
                }

                Thread.Sleep(500);
                ticketPrice = pricingModel(currentDay);
                Airline.changePrice(ticketPrice);
                //receiving order object from the multicell buffer
                Order order = MyApplication.buffer.getOneCell();
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