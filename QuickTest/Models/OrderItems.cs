using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class OrderItems
    {
        public int ItemId { get; set; }
        public int? ProId { get; set; }
        public int? OrderId { get; set; }
        public int? CartId { get; set; }
        public int? Quantity { get; set; }
        public int? SpecificationId { get; set; }
        public int? UserId { get; set; }
        public double? unitTotal { get; set; }
        public int? SellerId { get; set; }
        public int? Viewed { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Pro { get; set; }
    }
}
