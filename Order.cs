using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class Order
    {
        private string senderId; //= Thread.CurrentThread.Name; of travel agency
        private Int32 cardNo;   //credit card number
        private string receiverID;//thread name of airline
        private Int32 amount; //number of tickets to order
        private double unitPrice; //price for the bulk of tickets received from airline
        private string timestamp;
        private string timestampReturn;
        private double totalprice;

        public Order()
        {
            senderId = "";
            cardNo = 0;
            receiverID = "";
            amount = 0;
            unitPrice = 0;
            timestamp = "";
            timestampReturn = "";
            totalprice = 0;
        }
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
        public string getTimestamp()
        {
            return timestamp;
        }
        public string getTimestampReturn()
        {
            return timestampReturn;
        }
        public double getTotalPrice()
        {
            return totalprice;
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
        public void setTimeStamp(string newTimestamp)
        {
            timestamp = newTimestamp;
        }
        public void setTimeStampReturn(string newTimestampReturn)
        {
            timestampReturn = newTimestampReturn;
        }
        public void setTotalPrice(double newTotalPrice)
        {
            totalprice = newTotalPrice;
        }
    }
}
