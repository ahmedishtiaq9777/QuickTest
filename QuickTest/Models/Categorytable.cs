using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Categorytable
    {
        public Categorytable()
        {
            BrandCategoryPivot = new HashSet<BrandCategoryPivot>();
            SubCategory = new HashSet<SubCategory>();
            Transaction = new HashSet<Transaction>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BrandCategoryPivot> BrandCategoryPivot { get; set; }
        public virtual ICollection<SubCategory> SubCategory { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
