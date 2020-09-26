using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class ProductSpecification
    {
        public ProductSpecification()
        {
            Cart = new HashSet<Cart>();
            CartProdescriptionPivot = new HashSet<CartProdescriptionPivot>();
        }

        public int SpecificationId { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<CartProdescriptionPivot> CartProdescriptionPivot { get; set; }
        public virtual Product Product { get; set; }
    }
}
