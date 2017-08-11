using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMachine
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
            Console.WriteLine(this.strID + ":hello");
            return this.strID + ":Say Something";
        }
    }
}
