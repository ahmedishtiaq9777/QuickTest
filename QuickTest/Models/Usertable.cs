using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickTest.Models
{
    public partial class Usertable
    {
        public Usertable()
        {
            Cart = new HashSet<Cart>();
            Order = new HashSet<Order>();
            Product = new HashSet<Product>();
            Transaction = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage ="This is Required")]
        public string Password { get; set; }
        public DateTime? Dob { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        public string Gender { get; set; }
        public string Cnic { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string ShopName { get; set; }
        public string Logo { get; set; }
        public string ShippingDetail { get; set; }
        public int? IsBlocked { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
