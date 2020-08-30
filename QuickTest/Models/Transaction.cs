using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Qty { get; set; }
        public int? CartId { get; set; }
        public int? SpecId { get; set; }
        public int? SellerId { get; set; }

        public virtual Categorytable Cart { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Usertable User { get; set; }
    }
}
