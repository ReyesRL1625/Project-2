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
            order = newOrder;
        }

        public void processOrder()
        {
            if (!(order.getCardNo() >= 5000) && !(order.getCardNo() <= 7000))
            {
                Console.WriteLine("Invalid card number.");
            }
            else
            {
                double basePrice = order.getUnitPrice() * order.getAmount();
                double amount = basePrice + (basePrice * 4.50) + 10;
            }
        }
    }
}
