using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class BrandCategoryPivot
    {
        public int SubcategoryId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public virtual BrandTable Brand { get; set; }
        public virtual Categorytable Category { get; set; }
    }
}
