using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class MultiCellBuffer
    {
        //array of order objects that will be of length 3
        private Order[] buffer;

        //booleans used to see if each cell is writeable
        private bool Cell1Writeable;
        private bool Cell1ForSouthwest; //used to determine who the order is for
        private bool Cell2Writeable;
        private bool Cell2ForSouthwest; //used to determine who the order is for
        private bool Cell3Writeable;
        private bool Cell3ForSouthwest; //used to determine who the order is for


        //multicellbuffer constructor
        public MultiCellBuffer()
        {
            //initialize all cells as writeable
            Cell1Writeable = true;
            Cell2Writeable = true;
            Cell3Writeable = true;

            //initialize array of order objects
            buffer = new Order[3];

            //fills the buffer with a new order at every index
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Order();
            }
        }

        public void setOneCell(Int32 newAmount, Int32 newCardNo, string newReceiverId, string newSenderId, Int32 newUnitPrice)
        {
            //enter the set one cell method
            Console.WriteLine("Order sent by {0} to {1} for the price of {2}", newSenderId, newReceiverId, newUnitPrice);

            //attempt to use cell 1, if not available, move to cell 2
            if (Monitor.TryEnter(buffer[0]))
            {
                try
                {
                    //Console.WriteLine("Adding to the buffer at index 0");
                    //adds the order passed to the buffer
                    buffer[0].setAmount(newAmount);
                    buffer[0].setCardNo(newCardNo);
                    buffer[0].setReceiverID(newReceiverId);
                    buffer[0].setSenderId(newSenderId);
                    buffer[0].setUnitPrice(newUnitPrice);
                    if(newReceiverId.CompareTo("Southwest") == 0)
                    {
                        Cell1ForSouthwest = true;
                    }
                    else
                    {
                        Cell1ForSouthwest = false;
                    }
                    Cell1Writeable = false;
                }
                finally
                {
                    Monitor.Exit(buffer[0]);
                }
                
                
            }
            else if (Monitor.TryEnter(buffer[1]))
            {
                try
                {
                    //Console.WriteLine("Adding to the buffer at index 1");
                    //adds the order passed to the buffer
                    buffer[1].setAmount(newAmount);
                    buffer[1].setCardNo(newCardNo);
                    buffer[1].setReceiverID(newReceiverId);
                    buffer[1].setSenderId(newSenderId);
                    buffer[1].setUnitPrice(newUnitPrice);
                    if (newReceiverId.CompareTo("Southwest") == 0)
                    {
                        Cell2ForSouthwest = true;
                    }
                    else
                    {
                        Cell2ForSouthwest = false;
                    }
                    Cell2Writeable = false;
                }
                finally
                {
                    Monitor.Exit(buffer[1]);
                }
                
            }
            else
            {
                Monitor.Enter(buffer[2]);
                try
                {
                    //Console.WriteLine("Adding to the buffer at index 2");
                    //adds the order passed to the buffer
                    buffer[2].setAmount(newAmount);
                    buffer[2].setCardNo(newCardNo);
                    buffer[2].setReceiverID(newReceiverId);
                    buffer[2].setSenderId(newSenderId);
                    buffer[2].setUnitPrice(newUnitPrice);
                    if (newReceiverId.CompareTo("Southwest") == 0)
                    {
                        Cell3ForSouthwest = true;
                    }
                    else
                    {
                        Cell3ForSouthwest = false;
                    }
                    Cell3Writeable = false;
                }
                finally
                {
                    Monitor.Exit(buffer[2]);
                }
            
            }
            
        }

        public Order getOneCell()
        {
            if (!Cell1Writeable)
            {
                //if order is not corresponding to the thread trying to get the order, don't let it get a cell 
                if (!((Thread.CurrentThread.Name.CompareTo("Southwest") == 0 && Cell1ForSouthwest) || (Thread.CurrentThread.Name.CompareTo("Southwest") != 0 && !Cell1ForSouthwest)))
                {
                    return null;
                }
                Monitor.Enter(buffer[0]);
                try
                {
                   // Console.WriteLine("Getting a cell at index 0");
                    Cell1Writeable = true;
                    return buffer[0];
                }
                finally
                {
                    Monitor.Exit(buffer[0]);
                }
            }
            else if (!Cell2Writeable)
            {
                //if order is not corresponding to the thread trying to get the order, don't let it get a cell 
                if (!((Thread.CurrentThread.Name.CompareTo("Southwest") == 0 && Cell2ForSouthwest) || (Thread.CurrentThread.Name.CompareTo("Southwest") != 0 && !Cell2ForSouthwest)))
                {
                    return null;
                }
                Monitor.Enter(buffer[1]);
                try
                {
                    //Console.WriteLine("Getting a cell at index 1");
                    Cell2Writeable = true;
                    return buffer[1];
                }
                finally
                {
                    Monitor.Exit(buffer[1]);
                }
            }

            else 
            {
                //if order is not corresponding to the thread trying to get the order, don't let it get a cell 
                if (!((Thread.CurrentThread.Name.CompareTo("Southwest") == 0 && Cell3ForSouthwest) || (Thread.CurrentThread.Name.CompareTo("Southwest") != 0 && !Cell3ForSouthwest)))
                {
                    return null;
                }
                    //if no other cell is available force the airline to wait for one of the cells to read
                Monitor.Enter(buffer[2]);
                try
                {
                    //Console.WriteLine("Getting a cell at index 2");
                    Cell3Writeable = true;
                    return buffer[2];
                }
                finally
                {
                    Monitor.Exit(buffer[2]);
                }
            }
        }
    }
}
