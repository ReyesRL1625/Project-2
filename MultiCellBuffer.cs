using System;
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

        public void setOneCell(Int32 newAmount, Int32 newCardNo, string newReceiverId, string newSenderId, Int32 newUnitPrice, string timestamp)
        {
            //enter the set one cell method
            Console.WriteLine("Order sent by {0} to {1} for the price of {2}", newSenderId, newReceiverId, newUnitPrice);

            //attempt to use cell 1, if not available, move to cell 2
            if (Cell1Writeable && Monitor.TryEnter(buffer[0]))
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
                    buffer[0].setTimeStamp(timestamp);
                    //if the airline is southwest
                    if (newReceiverId.CompareTo("Southwest") == 0)
                    {
                        //set it to true
                        Cell1ForSouthwest = true;
                    }
                    else
                    {
                        //set southwest to false
                        Cell1ForSouthwest = false;
                    }
                    //indicates the cell is not empty
                    Cell1Writeable = false;
                }
                finally
                {
                    //exit the monitor for the buffer at [0]
                    Monitor.Exit(buffer[0]);
                }


            }
            else if (Cell2Writeable && Monitor.TryEnter(buffer[1]))
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
                    buffer[1].setTimeStamp(timestamp);
                    //if the airline is southwest
                    if (newReceiverId.CompareTo("Southwest") == 0)
                    {
                        //set the cell for southwest to true
                        Cell2ForSouthwest = true;
                    }
                    else
                    {
                        //set it to false
                        Cell2ForSouthwest = false;
                    }
                    //indicate that the cell is not empty
                    Cell2Writeable = false;
                }
                finally
                {
                    //exit the monitor for the buffer at [1]
                    Monitor.Exit(buffer[1]);
                }

            }
            else if (Cell3Writeable)
            {
                //block the thread until the last cell is available
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
                    buffer[2].setTimeStamp(timestamp);
                    //if the airline is southwest
                    if (newReceiverId.CompareTo("Southwest") == 0)
                    {
                        //set the cell for southwest to true
                        Cell3ForSouthwest = true;
                    }
                    else
                    {
                        //set it to false
                        Cell3ForSouthwest = false;
                    }
                    //indicate that the cell is not empty
                    Cell3Writeable = false;
                }
                finally
                {
                    //exit the buffer
                    Monitor.Exit(buffer[2]);
                }
            }

        }

        //method to get a cell from the buffer
        public Order getOneCell()
        {
            //creates a new empty order
            Order temp = null;
            //if the cell is not writeable (not empty)
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
                    //indicates that the cell is empty
                    Cell1Writeable = true;
                    //stores the order in a temporary location
                    temp = buffer[0];
                    //resets the buffer
                    buffer[0] = new Order();
                    //returns the order
                    return temp;
                }
                finally
                {
                    Monitor.Exit(temp);
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
                    //indicates that the cell is empty
                    Cell2Writeable = true;
                    //stores the order in a temporary location
                    temp = buffer[1];
                    //creates a new order in that cell
                    buffer[1] = new Order();
                    //returns the order
                    return temp;
                }
                finally
                {
                    Monitor.Exit(temp);
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
                    //indicates the cell is empty
                    Cell3Writeable = true;
                    //stores the order in a temporary location
                    temp = buffer[2];
                    //creates a new order in that cell
                    buffer[2] = new Order();
                    //returns the order
                    return temp;
                }
                finally
                {
                    Monitor.Exit(temp);
                }
            }
        }
    }
}
