using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest.Models.SellerViewModels
{
    public partial class ProductViewModel :Basedashboard
    {

        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public double? Price { get; set; }
        public string ProductImage { get; set; }
        public int? UserId { get; set; }
        public string Category { get; set; }
        public int? Quantity { get; set; }
        public double? AvgRating { get; set; }
        public int OrderId { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public int  specificationid { get; set; }
        
    }
}
