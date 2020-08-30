using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.SellerViewModels
{
    public class DashboardViewModel
    {
        public int Totalorders { get; set; }

        public int Totalproducts { get; set; }
        public double Totalearnings { get; set; }
        public int Salesitems  { get; set;}
    }
}
