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
        private string orderAirlineName;

        //airline constructor with two buffers
        public Airline(MultiCellBuffer newBuffer, ConfirmationBuffer newCBufffer, string newAirlineName)
        {
            aBuffer = newBuffer;
            aConfirmBuffer = newCBufffer;
            airlineName = newAirlineName;
            willProcessOrder = false;
            orderAirlineName = "";
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
                    //Console.WriteLine("{0} had a price cut", Thread.CurrentThread.Name);
                }
            }
            ticketPrice = price;
        }

        public void airlineFunc()
        {
            //keep the airline thread running until 20 price cuts have passed
            while (numberOfPriceCuts <= 20)
            {

                if (willProcessOrder)
                {
                    processOrder();
                    willProcessOrder = false;
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
                    Thread.Sleep(1000);
                    //calculate a new price using the pricing model 
                    Int32 newPrice = pricingModel(currentDay);
                    //call the private method to change the price
                    changePrice(newPrice);
                    Console.WriteLine("{0} has price of ${1}", Thread.CurrentThread.Name, ticketPrice);
                    Thread.Sleep(1000);
                }
               
            }
            Console.WriteLine("{0} Thread ended", Thread.CurrentThread.Name);
        }

        public Int32 pricingModel(Int32 currentDay)
        {
            Int32 p = pricesForWeek[currentDay];
            return p;
        }
        public void orderAvailable(string newOrderAirlineName)
        {
            willProcessOrder = true;
            orderAirlineName = newOrderAirlineName;
        }
        public void processOrder()
        {
            //receiving order object from the multicell buffer
            Order order = aBuffer.getOneCell();
            if (order == null)
            {
                //Console.WriteLine("Order not intended for {0}", Thread.CurrentThread.Name);
                return;
            }

            //Console.WriteLine("{0} was able to fetch the order from {1} for price of {2}", this.airlineName, order.getSenderId(), order.getUnitPrice());
            /*
            Order copyOfOrder = new Order();
            copyOfOrder.setAmount(order.getAmount());
            copyOfOrder.setCardNo(order.getCardNo());
            copyOfOrder.setReceiverID(order.getReceiverID());
            copyOfOrder.setSenderId(order.getSenderId());
            copyOfOrder.setUnitPrice(order.getUnitPrice());
            copyOfOrder.setTimeStamp(order.getTimestamp());
            */        
            //creating new order processing thread to process the order
            OrderProcessing orderProcessing = new OrderProcessing(order);
            Thread newOrder = new Thread(new ThreadStart(orderProcessing.processOrder));
            newOrder.Start();
            //Console.WriteLine("{0} started to process order", this.airlineName);

            if (order.getReceiverID().CompareTo("Southwest") == 0)
            {
                MyApplication.orderreceivedSouthwest.Set();
            }
            else
            {
                MyApplication.orderreceivedDelta.Set();
            }

            MyApplication.multiCellBufferPool.Release();

        }
    }
}