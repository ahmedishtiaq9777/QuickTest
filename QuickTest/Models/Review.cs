using System;
using System.Collections.Generic;

namespace QuickTest.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int? ProductId { get; set; }
        public double? RatingStars { get; set; }
        public string Feedback { get; set; }
        public int? UserId { get; set; }
    }
}
