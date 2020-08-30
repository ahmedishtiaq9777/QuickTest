using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItems>();
            Transaction = new HashSet<Transaction>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public double? Total { get; set; }
        public int? SellerId { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual Usertable User { get; set; }
    }
}
