using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class BrandSubcategoryPivot
    {
        public int BrandSubcategoryId { get; set; }
        public int BrandId { get; set; }
        public int SubcategoryId { get; set; }

        public virtual BrandTable Brand { get; set; }
        public virtual SubCategory Subcategory { get; set; }
    }
}
