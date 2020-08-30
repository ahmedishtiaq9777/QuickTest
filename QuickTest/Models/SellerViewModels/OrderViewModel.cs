using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.SellerViewModels
{
    public partial class OrderViewModel
    {





        public int Orderid { get; set; }
        public int userid   {get;set;}
        public string user_name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public double total { get; set; }
        public string status { get; set; }
        public DateTime orderdate { get; set; }
        public string image { get; set; }


        public int pid { get; set; }

        public List<Title_proid> products { get; set; }






    }
}
