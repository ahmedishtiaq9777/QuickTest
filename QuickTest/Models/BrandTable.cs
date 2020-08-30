using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class BrandTable
    {
        public BrandTable()
        {
            BrandCategoryPivot = new HashSet<BrandCategoryPivot>();
            BrandSubcategoryPivot = new HashSet<BrandSubcategoryPivot>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BrandCategoryPivot> BrandCategoryPivot { get; set; }
        public virtual ICollection<BrandSubcategoryPivot> BrandSubcategoryPivot { get; set; }
    }
}
