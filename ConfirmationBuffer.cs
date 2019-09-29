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
        
        ReaderWriterLock rwlock = new ReaderWriterLock();

        public ConfirmationBuffer()
        {
            counter = 1;
            buffer = "";
            //multicellbuffer_pool = new Semaphore(1, 1);
        }

        public void setOneCell(string confirmation)
        {
            //multicellbuffer_pool.WaitOne();
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
                //multicellbuffer_pool.Release();
            }
            finally
            {
                rwlock.ReleaseWriterLock();
                Monitor.Pulse(this);
            }

        }

        public string getOneCell()
        {
            //multicellbuffer_pool.WaitOne();
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
                //multicellbuffer_pool.Release();
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
