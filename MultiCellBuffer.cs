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
            _pool = new Semaphore(3, 3);
            buffer = new Order[3];
        }
        public void setOneCell(Order order)
        {
            _pool.WaitOne();
            rwlock.AcquireWriterLock(Timeout.Infinite);
            try
            {

                _pool.Release();
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
        }

        public Order getOneCell()
        {
            _pool.WaitOne();
            rwlock.AcquireReaderLock(Timeout.Infinite);
            try
            {

                _pool.Release();
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
            return order1;
        }
    }
}
