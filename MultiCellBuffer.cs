using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class MultiCellBuffer
    {
        Order order1 = new Order();
        Order order2 = new Order();
        Order order3 = new Order();
        private Order[] buffer;
        private static Semaphore _pool;
        ReaderWriterLock rwlock = new ReaderWriterLock();
        public MultiCellBuffer()
        {
            buffer = new Order[3];
        }
        public void setOneCell(Order order)
        {
            rwlock.AcquireWriterLock(Timeout.Infinite);
            
        }

        public Order getOneCell()
        {
            return order1;
        }
    }
}
