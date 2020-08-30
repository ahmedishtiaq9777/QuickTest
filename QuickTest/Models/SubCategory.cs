using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            BrandSubcategoryPivot = new HashSet<BrandSubcategoryPivot>();
        }

        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<BrandSubcategoryPivot> BrandSubcategoryPivot { get; set; }
        public virtual Categorytable Category { get; set; }
    }
}
