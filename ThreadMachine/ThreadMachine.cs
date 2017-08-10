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

        /*
         * WorkProc is a wrapper for workItem.DoWork().
         * The reason for this wrapper is to make sure all the **multi-thread** related work would be done.
         */
        internal void WorkProc(Object state) 
        {
            threadMachine.WriteShareData(workItem.DoWork()); // Access the shared storage to store result of DoWork() safely under multi-thread.
            threadMachine.DoneWork(); // Notify threadMachine that the work has been done.
        }

    }
    public class ThreadMachine
    {
        private AutoResetEvent objFinishEvent = new AutoResetEvent(false); // For thread to wait upon.
        private int nItems = 0; // Store total num of threads to be started.
        private List<ThreadItem> lItems = new List<ThreadItem>(); //Actual work items.
        private int nItemProccessed = 0; // The number of finished work items.
        private object objLock = new Object(); // used to lock critical section.
        private List<Object> dictResults = new List<Object>(); // for collecting results. 


        public ThreadMachine()
        {
            //nothing to do
        }

        public List<Object> GetResults()
        {
            return dictResults;
        }

        internal void WriteShareData(Object obj) //avoid client from accessing this method.
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
            int count = 0;
            foreach (ThreadItem threadItem in lItems)
            {
                count++;

                ThreadPool.QueueUserWorkItem(new WaitCallback(threadItem.WorkProc));
                
                if ((count % 20) == 0)
                {

                    Console.WriteLine("sleep for a while");
                    //sleep 20 seconds
                    Thread.Sleep(20000);
                }
                
            }

            this.objFinishEvent.WaitOne();

        }
    }
}
