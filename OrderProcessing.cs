using System;

namespace Project_2
{
    public delegate void orderConfirmationDelegate(Int32 amount, Int32 cardNo, string receiverId, string senderId, double unitPrice, string timestamp);
    class OrderProcessing
    {
        public static event orderConfirmationDelegate orderConfirmed;
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
                Console.WriteLine("Order failed due to invalid card number: {0}", order.getCardNo());
            }
            else
            {
                //valid card
                //calculates the amount based on different factors
                double basePrice = order.getUnitPrice() * order.getAmount();
                double amount = basePrice + (basePrice * 4.50) + 10;
                order.setTotalPrice(amount);
                orderConfirmed(order.getAmount(), order.getCardNo(), order.getReceiverID(), order.getSenderId(), order.getUnitPrice(), order.getTimestamp());
            }
        }
    }
}