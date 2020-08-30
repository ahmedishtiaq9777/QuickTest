using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class CartProdescriptionPivot
    {
        public int CartProdesId { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public int? SpecificationId { get; set; }
        public int? Quantity { get; set; }
        public int? SellerId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductSpecification Specification { get; set; }
    }
}
