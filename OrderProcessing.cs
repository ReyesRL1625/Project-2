﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Project_2
{
    class OrderProcessing
    {
        public const double TAX = 4.5;
        public const Int32 locationFee = 10;
        private static Order order;
        private ConfirmationBuffer confirmationBuffer;
        public OrderProcessing(Order newOrder)
        {
            //instantiates thew order object
            order = newOrder;
            //confirmationBuffer = new ConfirmationBuffer();
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
                double amount = basePrice + (basePrice * 4.50) + 10;
                //this.confirmation(amount);
            }
        }

        public void confirmation(double amount)
        {

        }
    }
}