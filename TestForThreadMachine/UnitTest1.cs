using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadMachine;
using System.Collections.Generic;

namespace TestForThreadMachine
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ThreadMachine<string> thMachine = new ThreadMachine<string>();
            List<string> lItems = TestHelper.ReadFileToServerList(@"TestMaterial\sl.txt");
            lItems.ForEach(i => thMachine.QueueItem(new WorkItem(i).DoWork));
            thMachine.InvokeMultiThread();
            foreach (string strItem in thMachine.GetResults())
            {
                Console.WriteLine(strItem);
            };
        }
        [TestMethod]
        public void TestMethod2() //What if Do Work raise exception
        {
            ThreadMachine<string> thMachine = new ThreadMachine<string>();
            List<string> lItems = TestHelper.ReadFileToServerList(@"TestMaterial\sl.txt");
            lItems.ForEach(i => thMachine.QueueItem(new WorkItem(i).DoWorkWithError));
            thMachine.InvokeMultiThread();
            foreach (string strItem in thMachine.GetResults())
            {
                Console.WriteLine(strItem);
            };
        }
    }
}
