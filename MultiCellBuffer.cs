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
        private bool Cell2Writeable;
        private bool Cell3Writeable;


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

        public void setOneCell(Order order)
        {
            //enter the set one cell method
            Console.WriteLine("Order sent by {0} to {1}", order.getSenderId(), order.getReceiverID());

            //attempt to use cell 1, if not available, move to cell 2
            if (Monitor.TryEnter(buffer[0]) && Cell1Writeable)
            {
                try
                {
                    Console.WriteLine("Adding to the buffer at index 0");
                    //adds the order passed to the buffer
                    buffer[0].setAmount(order.getAmount());
                    buffer[0].setCardNo(order.getCardNo());
                    buffer[0].setReceiverID(order.getReceiverID());
                    buffer[0].setSenderId(order.getSenderId());
                    buffer[0].setUnitPrice(order.getUnitPrice());
                    Cell1Writeable = false;
                }
                finally
                {
                    Monitor.Exit(buffer[0]);
                }
                
                
            }
            else if (Monitor.TryEnter(buffer[1]) && Cell2Writeable)
            {
                try
                {
                    Console.WriteLine("Adding to the buffer at index 1");
                    //adds the order passed to the buffer
                    buffer[1].setAmount(order.getAmount());
                    buffer[1].setCardNo(order.getCardNo());
                    buffer[1].setReceiverID(order.getReceiverID());
                    buffer[1].setSenderId(order.getSenderId());
                    buffer[1].setUnitPrice(order.getUnitPrice());
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
                    if (Cell3Writeable)
                    {
                        Console.WriteLine("Adding to the buffer at index 2");
                        //adds the order passed to the buffer
                        buffer[2].setAmount(order.getAmount());
                        buffer[2].setCardNo(order.getCardNo());
                        buffer[2].setReceiverID(order.getReceiverID());
                        buffer[2].setSenderId(order.getSenderId());
                        buffer[2].setUnitPrice(order.getUnitPrice());
                        Cell3Writeable = false;
                    }
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
                Monitor.Enter(buffer[0]);
                try
                {
                    Console.WriteLine("Getting a cell at index 0");
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
                Monitor.Enter(buffer[1]);
                try
                {
                    Console.WriteLine("Getting a cell at index 1");
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
                //if no other cell is available force the airline to wait for one of the cells to read
                Monitor.Enter(buffer[2]);
                try
                {
                    Console.WriteLine("Getting a cell at index 2");
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
