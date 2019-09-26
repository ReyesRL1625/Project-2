using System;
using System.Collections.Generic;
using System.Text;

namespace Project_2
{
    class OrderProcessing
    {
        public const double TAX = 4.5;
        public const Int32 locationFee = 10;
        public double amount;
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
                Console.WriteLine("Invalid card number.");
            }
            else
            {
                //valid card
                //calculates the amount based on different factors
                double basePrice = order.getUnitPrice() * order.getAmount();
                amount = basePrice + (basePrice * 4.50) + 10;
            }
        }

        public void confirmation()
        {
            Console.WriteLine("Ther order from Travel Agency{0} to Airline{1} has been approved for ${2}", order.getSenderId(), order.getReceiverID(), amount);
        }
    }