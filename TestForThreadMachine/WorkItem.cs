using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForThreadMachine
{
    //Example of consumer
    public class WorkItem
    {
        private string strID;
        public WorkItem(string strID)
        {
            this.strID = strID; 
        }
        public string DoWork()
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + "Test Case>>Do Work Started for: " + this.strID);
            return this.strID + ":Say Something";
        }
    }
}
