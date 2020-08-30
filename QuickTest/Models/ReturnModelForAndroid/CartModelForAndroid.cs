using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.ReturnModelForAndroid
{
    public  partial class CartModelForAndroid
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductImage { get; set; }
        public double? Price { get; set; }
        public int? UserQuantity { get; set; }
        public int? SellerQuantity { get; set; }
        public int? SellerId { get; set; }



    }
}
