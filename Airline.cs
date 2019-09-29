using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    //declaring a delegate container for the priceCutDelegate signature
    public delegate void priceCutDelegate(Int32 p);

    class Airline
    {
        //define a price cut event named priceCut
        public static event priceCutDelegate priceCut;

        //arrray of integers used to simulate prices for different days of the week
        private Int32[] pricesForWeek;

        //current ticket price for each airline
        private static Int32 ticketPrice;

        //current number of available tickets
        private static Int32 availableTickets;

        //total number of priceCuts so far
        public static Int32 numberOfPriceCuts;

        //multicell buffer and confirmation buffer for the orders
        MultiCellBuffer aBuffer;
        ConfirmationBuffer aConfirmBuffer;

        //integer that represents the current day of the week in the range of 0 to 6
        private Int32 currentDay;

        //airline id
        private string airlineName;

        private bool willProcessOrder;

        //airline constructor with two buffers
        public Airline(MultiCellBuffer newBuffer, ConfirmationBuffer newCBufffer, string newAirlineName)
        {
            aBuffer = newBuffer;
            aConfirmBuffer = newCBufffer;
            airlineName = newAirlineName;
            willProcessOrder = false;
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
            availableTickets = 500;
            numberOfPriceCuts = 0;
        }

        //getter for ticket price
        public Int32 getPrice()
        {
            return ticketPrice;
        }

        //getter for available tickets
        public Int32 getAvailableTickets()
        {
            return availableTickets;
        }

        //setter for available tickets
        public void setAvailableTickets(Int32 newNoAvailableTickets)
        {
            availableTickets = newNoAvailableTickets;
        }

        //change price method that is private to each airline object
        private void changePrice(Int32 price)
        {
            //if price is lower than previous time, emmit a price cut event after checking that there is at least one subscribed
            if (price < ticketPrice)
            {
                //verify there is at least a subscriber
                if (priceCut != null)
                {
                    //emit a price cut event to notify the subcribed 
                    priceCut(price);
                    numberOfPriceCuts++;
                    Console.WriteLine("{0} had a price cut", Thread.CurrentThread.Name);
                }
            }
            ticketPrice = price;
        }

        public void airlineFunc()
        {
            //keep the airline thread running until 5 price cuts have passed
            while (numberOfPriceCuts <= 5)
            {

                if (willProcessOrder)
                {
                    processOrder();
                    Thread.Sleep(1000);
                }
                else
                {
                    //if current day is the last index, reset it to 0, otherwise increment the current day index
                    if (currentDay == 6)
                    {
                        currentDay = 0;
                    }
                    else
                    {
                        currentDay++;
                    }
                    //sleep this thread for half a second to allow travel agency threads to start running
                    Thread.Sleep(500);
                    //calculate a new price using the pricing model 
                    Int32 newPrice = pricingModel(currentDay);
                    //call the private method to change the price
                    changePrice(newPrice);
                    Console.WriteLine("{0} has price of ${1}", Thread.CurrentThread.Name, ticketPrice);
                }
               
            }
            Console.WriteLine("{0} Thread ended", Thread.CurrentThread.Name);
        }

        public Int32 pricingModel(Int32 currentDay)
        {
            Int32 p = pricesForWeek[currentDay];
            return p;
        }
        public void orderAvailable()
        {
            willProcessOrder = true;
        }
        public void processOrder()
        {
            //receiving order object from the multicell buffer
            Order order = aBuffer.getOneCell();
            MyApplication.multiCellBufferPool.Release();
            Console.WriteLine("{0} was able to fetch the order", this.airlineName);

            if (order != null)
            {
                //creating new order processing thread to process the order
                OrderProcessing orderProcessing = new OrderProcessing(order);
                Thread newOrder = new Thread(new ThreadStart(orderProcessing.processOrder));
                newOrder.Start();
            }
            Console.WriteLine("{0} started to process order", this.airlineName);
        }
    }
}