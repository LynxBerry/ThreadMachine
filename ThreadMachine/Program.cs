using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThreadMachine
{
    class Program
    {
        public static IList<string> ReadFileToServerList(string strFile)
        {
            List<string> lServerList = new List<string>();


                using (StreamReader file = new StreamReader(strFile))
                {

                    string line = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        string strServerName = line.Trim();
                        
                        lServerList.Add(strServerName);
                        

                    }
                }


            lServerList = new List<string>(lServerList.Distinct<string>());
            return lServerList;
        }
        static void Main(string[] args)
        {
            ThreadMachine<string> thMachine = new ThreadMachine<string>();
            //string[] lNames = { "abc", "efg", "hig"};
            IList<string> lItems = ReadFileToServerList(@"c:\users\stevensh\list.txt");
            foreach (string lname in lItems)
            {
                thMachine.QueueItem(new WorkItem(lname).DoWork);
            }

            thMachine.InvokeMultiThread();
            foreach (string strItem in thMachine.GetResults()) {
                Console.WriteLine(strItem);
            };
        }
    }
}
