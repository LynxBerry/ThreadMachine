using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadMachine
{
    public class ThreadItem<T>
    {
        private ThreadMachine<T> threadMachine;
        private Func<T> doWork;

        internal ThreadItem(Func<T> doWork) //customer has no need to know.
        {
            this.doWork = doWork;
        }

        internal void setThreadMachine(ThreadMachine<T> threadMachine)
        {
            this.threadMachine = threadMachine;
        }

        /*
         * WorkProc is a wrapper for <object of your customized class>.DoWork().
         * The reason for this wrapper is to make sure all the **multi-thread** related work would be done.
         */
        internal void WorkProc(Object state) // The param here is just for aligning with signature of thread's delegate.
        {
            // To Do: Need to catch exception for doWork()
            threadMachine.WriteShareData(doWork()); // Access the shared storage to store result of doWork() safely under multi-thread.
            threadMachine.DoneWork(); // Notify threadMachine that the work has been done.
        }

    }
    public class ThreadMachine<T>
    {
        private AutoResetEvent objFinishEvent = new AutoResetEvent(false); // For thread to wait upon.
        private int nItems = 0; // Store total num of threads to be started.
        private List<ThreadItem<T>> lItems = new List<ThreadItem<T>>(); //Actual work items.
        private int nItemProccessed = 0; // The number of finished work items.
        private object objLock = new Object(); // used to lock critical section.
        private List<T> dictResults = new List<T>(); // for collecting results. 


        public ThreadMachine()
        {
            //nothing to do
        }

        public List<T> GetResults()
        {
            return dictResults;
        }

        internal void WriteShareData(T obj) //avoid client from accessing this method.
        {
            lock (objLock) //maybe unecessary.
            {

                //do something
                dictResults.Add(obj);
            }
        }

        internal void DoneWork()
        {
            if (Interlocked.Increment(ref nItemProccessed) == nItems)
            {
                objFinishEvent.Set();
            }
        }

        public void QueueItem(Func<T> doWork)
        {
            var handle = new ThreadItem<T>(doWork);
            handle.setThreadMachine(this);
            lItems.Add(handle);
            nItems++;

        }

        public void InvokeMultiThread()
        {
            int count = 0;
            foreach (ThreadItem<T> threadItem in lItems)
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
