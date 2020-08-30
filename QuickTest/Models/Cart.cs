using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Cart
    {
        public Cart()
        {

            CartProdescriptionPivot = new HashSet<CartProdescriptionPivot>();
            OrderItems = new HashSet<OrderItems>();
        }

        public int CartId { get; set; }
        public int? UserId { get; set; }
        public double? Total { get; set; }
        public int? ProductId { get; set; }
        public int? ProductDesId { get; set; }

        public virtual ICollection<CartProdescriptionPivot> CartProdescriptionPivot { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ProductSpecification ProductDes { get; set; }
        public virtual Product Product { get; set; }
        public virtual Usertable User { get; set; }
    }
}
