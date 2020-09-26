using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.MvcModels
{
    public partial class User_Km_Pivot
    {
        public int UserId { get; set; }
        public string ShopName { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public double d_kilometers { get; set; }
        public string contact { get; set; }
        public double rating { get; set; }

    }
}
