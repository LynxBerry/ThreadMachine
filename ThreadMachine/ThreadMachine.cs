using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadMachine
{
    public class ThreadItem
    {
        private ThreadMachine threadMachine;
        private IWorkItem workItem;

        public ThreadItem(IWorkItem workItem)
        {
            this.workItem = workItem;
        }

        public void setThreadMachine(ThreadMachine threadMachine)
        {
            this.threadMachine = threadMachine;
        }

        public void setWorkItem(IWorkItem workItem)
        {
            this.workItem = workItem;
        }
        public void WorkProc(Object state)
        {
            threadMachine.WriteShareData(workItem.DoWork());
            threadMachine.DoneWork();
        }

    }
    public class ThreadMachine
    {
        private AutoResetEvent objFinishEvent = new AutoResetEvent(false);
        private int nItems = 0;
        private List<ThreadItem> lItems = new List<ThreadItem>();
        private int nItemProccessed = 0;
        private object objLock = new Object();
        private List<Object> dictResults = new List<Object>();


        public ThreadMachine()
        {
            
        }

        public List<Object> GetResults()
        {
            return dictResults;
        }

        public void WriteShareData(Object obj)
        {
            lock (objLock) //maybe unecessary.
            {

                //do something
                dictResults.Add(obj);
            }
        }

        public void DoneWork()
        {
            if (Interlocked.Increment(ref nItemProccessed) == nItems)
            {
                objFinishEvent.Set();
            }
        }

        public void QueueItem(ThreadItem threadItem)
        {
            threadItem.setThreadMachine(this);
            lItems.Add(threadItem);
            nItems++;

        }

        public void InvokeMultiThread()
        {
            int count = 1;
            foreach (ThreadItem threadItem in lItems)
            {
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadItem.WorkProc));
                
                if ((count % 20) == 0)
                {

                    Console.WriteLine("sleep for a while");
                    //sleep 20 seconds
                    Thread.Sleep(20000);
                }
                count++;
            }

            this.objFinishEvent.WaitOne();

        }
    }
}
