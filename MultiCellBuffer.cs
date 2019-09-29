using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class MultiCellBuffer
    {
        //creates new variables
        private Order order;
        private Order[] buffer;
        //constructor
        public MultiCellBuffer()
        {
            order = new Order();
            buffer = new Order[3];
            //fills the buffer with a new order at every index
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Order();
            }
        }
        public void setOneCell(Order order)
        {
            Console.WriteLine("Setting a cell in the buffer");
            //use the writer lock that won't time out
            lock(this)
            {
                Console.WriteLine("Inside of the lock");
                //loop through the buffer
                for(int i = 0; i < buffer.Length; i++)
                {
                    Console.WriteLine("In the buffer at index {0}", i);
                    //find an empty spot
                    if(buffer[i].getIsEmpty())
                    {
                        Console.WriteLine("Adding to the buffer");
                        //adds the order passed to the buffer
                        buffer[i] = order;
                        //breaks from the loop
                        break;
                    }
                }
            }
            
        }

        public Order getOneCell()
        {
            Console.WriteLine("Getting the cell buffer");
            //acquires lock
            lock(this)
            {
                Console.WriteLine("Inside of the lock");
                //loop that does not stop until the order has been added to the buffer
                for(int i = 0; i < buffer.Length; i++)
                {
                    //makes sure that the order at that index is not empty
                    if(!buffer[i].getIsEmpty())
                    {
                        Console.WriteLine("Getting the order at index {0}", i);
                        //gets the order at that index
                        order = buffer[i];
                        //adds a new order to the buffer at that index
                        buffer[i] = new Order();
                        //breaks from the loop
                        break;
                    }
                }
            }
            //returns the order*/
            return order;
        }
    }
}
