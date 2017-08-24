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
        public string DoWork() // Normal Work
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + "Test Case>>Do Work Started for: " + this.strID);
            return this.strID + ":Say Something";
        }

        public string DoWorkWithError() // Work Will raise Exception
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + ":Test Case With Error>>Do Work Started for: " + this.strID);
            throw new Exception("error" + this.strID);
            return this.strID + ":Say Something";
        }
    }
}
