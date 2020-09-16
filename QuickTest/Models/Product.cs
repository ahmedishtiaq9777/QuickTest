using QuickTest.Models.SellerViewModels;
using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Product
    {
        public Product()
        {
            Cart = new HashSet<Cart>();
            CartProdescriptionPivot = new HashSet<CartProdescriptionPivot>();
            OrderItems = new HashSet<OrderItems>();
            ProductSpecification = new HashSet<ProductSpecification>();
            Transaction = new HashSet<Transaction>();
        }

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

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<CartProdescriptionPivot> CartProdescriptionPivot { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ProductSpecification> ProductSpecification { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual Usertable User { get; set; }
    }
}
