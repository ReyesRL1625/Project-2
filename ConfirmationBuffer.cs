using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project_2
{
    class ConfirmationBuffer
    {
        public int counter;
        public string buffer;
        public static Semaphore _pool;
        ReaderWriterLock rwlock = new ReaderWriterLock();

        public ConfirmationBuffer()
        {
            counter = 1;
            buffer = "";
            _pool = new Semaphore(1, 1);
        }

        public void setOneCell(string confirmation)
        {
            _pool.WaitOne();
            rwlock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                //while there is no room in the buffer
                while(counter == 0)
                {
                    Monitor.Wait(this);
                }
                //adds the confirmation to the buffer
                buffer = confirmation;
                counter--;
                _pool.Release();
            }
            finally
            {
                rwlock.ReleaseWriterLock();
                Monitor.Pulse(this);
            }

        }

        public string getOneCell()
        {
            _pool.WaitOne();
            string result = "";
            rwlock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                //while there is nothing to read in the buffer
                while(counter == 1)
                {
                    //wait for pulse
                    Monitor.Wait(this);
                }
                result = buffer;
                counter++;
                _pool.Release();
            }
            finally
            {
                rwlock.ReleaseReaderLock();
                Monitor.Pulse(this);
            }
            return result;
        }
    }
}
