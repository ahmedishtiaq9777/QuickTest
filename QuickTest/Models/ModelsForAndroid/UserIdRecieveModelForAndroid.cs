using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.ModelsForAndroid
{
    public partial class UserIdRecieveModelForAndroid
    {
        public int userid { get; set; }
        public int sellerid { get; set; }
        public int proid { get; set; }
        public int quantity { get; set; }
        public int orderid { get; set; }
        public double total { get; set; }
        public string bitmapstr { get; set; }
        public string filename { get; set; }
        public string shippingdetail { get; set; }
        public string orderedproducts { get; set; }
        
    }
}
