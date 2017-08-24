using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForThreadMachine
{
    static class TestHelper
    {
        public static List<string> ReadFileToServerList(string strFile)
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
    }
}
