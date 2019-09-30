using System;
using System.Collections.Generic;
using System.Text;

namespace Project_2
{
    class OrderProcessing
    {
        public const double TAX = 4.5;
        public const Int32 locationFee = 10;
        private static Order order;
        public OrderProcessing(Order newOrder)
        {
            //instantiates thew order object
            order = newOrder;
        }

        public void processOrder()
        {
            //checking the credit card number
            if (!(order.getCardNo() >= 5000) && !(order.getCardNo() <= 7000))
            {
                //invalid card
                Console.WriteLine("Order failed due to invalid card number: {0}", order.getCardNo());
            }
            else
            {
                //valid card
                //calculates the amount based on different factors
                double basePrice = order.getUnitPrice() * order.getAmount();
                double amount = basePrice + (basePrice * 4.50) + 10;
                Console.WriteLine("[{0}]: {1} has successfully purchased {2} tickets from {3} for ${4}", order.getTimestamp(), order.getSenderId(), 
                    order.getAmount(), order.getReceiverID(), amount);
            }
        }
    }
}