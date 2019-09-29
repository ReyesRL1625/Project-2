using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class MultiCellBuffer
    {
        //creates new variables
        private Int32 counter;
        private Order order;
        private Order[] buffer;
        private static Semaphore _pool;
        Random rnd = new Random();
        //constructor
        public MultiCellBuffer()
        {
            //instantiating different variables
            counter = 0;
            order = new Order();
            _pool = new Semaphore(3, 3);
            buffer = new Order[3];
            //fills the buffer with a new order at every index
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Order();
            }
        }
        public void setOneCell(Order order)
        {
            //decreases the semaphore by 1 to indicate it is being used
            _pool.WaitOne();
            //use the writer lock that won't time out
            lock(buffer)
            {
                //while there is no room in the buffer
                if (counter == 3)
                {
                    //Makes the operation wait until a pulse signal
                    Monitor.Wait(this);
                }
                //loop through the buffer
                for(int i = 0; i < buffer.Length; i++)
                {
                    //find an empty spot
                    if(buffer[i].getIsEmpty())
                    {
                        //adds the order passed to the buffer
                        buffer[i] = order;
                        //increments the counter
                        counter++;
                        //breaks from the loop
                        break;
                    }
                }
                //release the _pool
                _pool.Release();
                //sends the pulse signal back to anything waiting
                Monitor.Pulse(this);
            }
            
        }

        public Order getOneCell()
        {
            //decreases the semaphore by 1 to indicate it is being used
            _pool.WaitOne();
            //acquires lock
            lock(buffer)
            {
                //while there are no orders available
                if (counter == 0)
                {
                    //wait to continue until there is a pulse 
                    //this signals that there is room in the counter
                    Monitor.Wait(buffer);
                }
                //loop that does not stop until the order has been added to the buffer
                while(true)
                {
                    //gets a random number
                    Int32 i = rnd.Next(0, buffer.Length);
                    //makes sure that the order at that index is not empty
                    if(!buffer[i].getIsEmpty())
                    {
                        //gets the order at that index
                        order = buffer[i];
                        //adds a new order to the buffer at that index
                        buffer[i] = new Order();
                        //breaks from the loop
                        break;
                    }
                }
                //releases the semaphore
                _pool.Release();
                //sends a pulse signal
                Monitor.Pulse(buffer);
            }
            //returns the order
            return order;
        }
    }
}
