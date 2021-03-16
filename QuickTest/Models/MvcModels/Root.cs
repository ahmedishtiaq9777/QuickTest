using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.MvcModels
{
    public class Root
    {
        public int status { get; set; }
        public string type { get; set; }
        public double totalprice { get; set; }
        public int totalgsm { get; set; }
        public double remaincredit { get; set; }
        public List<Message> messages { get; set; }
    }

}
