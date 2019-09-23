using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class Order
    {
        private string senderId; //= Thread.CurrentThread.Name; of travel agency
        private Int32 cardNo;
        private string receiverID;//thread name of airline
        private Int32 amount;
        private double unitPrice; //price for the bulk of tickets received from airline

        public string getSenderId()
        {
            return senderId;
        }
        public Int32 getCardNo()
        {
            return cardNo;
        }
        public string getReceiverID()
        {
            return receiverID;
        }
        public Int32 getAmount()
        {
            return amount;
        }
        public double getUnitPrice()
        {
            return unitPrice;
        }
        public void setSenderId(string newsenderid)
        {
            senderId = newsenderid;
        }
        public void setCardNo(Int32 newcardno)
        {
            cardNo = newcardno;
        }
        public void setReceiverID(string newreceiverid)
        {
            receiverID = newreceiverid;
        }
        public void setAmount(Int32 newamount)
        {
            amount = newamount;
        }
        public void setUnitPrice(double newunitprice)
        {
            unitPrice = newunitprice;
        }


    }
}
