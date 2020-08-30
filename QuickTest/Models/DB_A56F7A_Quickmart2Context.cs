using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuickTest.Models
{
    public partial class DB_A56F7A_Quickmart2Context : DbContext
    {
        public DB_A56F7A_Quickmart2Context(DbContextOptions<DB_A56F7A_Quickmart2Context> options) : base(options)
        {

        }



        public virtual DbSet<BrandCategoryPivot> BrandCategoryPivot { get; set; }
        public virtual DbSet<BrandSubcategoryPivot> BrandSubcategoryPivot { get; set; }
        public virtual DbSet<BrandTable> BrandTable { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartProdescriptionPivot> CartProdescriptionPivot { get; set; }
        public virtual DbSet<Categorytable> Categorytable { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecification { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Usertable> Usertable { get; set; }

        // Unable to generate entity type for table 'dbo.cart_pro_description_pivot'. Please see the warning messages.

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Data Source=SQL5052.site4now.net;Initial Catalog=DB_A56F7A_Quickmart2;User Id=DB_A56F7A_Quickmart2_admin;Password=ahmedishtiaq9777;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandCategoryPivot>(entity =>
            {
                entity.HasKey(e => e.SubcategoryId)
                    .HasName("PK_brand_category_pivot");

                entity.ToTable("brand_category_pivot");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandCategoryPivot)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_brand_category_pivot_brand_table");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BrandCategoryPivot)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_brand_category_pivot_categorytable");
            });

            modelBuilder.Entity<BrandSubcategoryPivot>(entity =>
            {
                entity.HasKey(e => e.BrandSubcategoryId)
                    .HasName("PK_brand_subcategory_pivot");

                entity.ToTable("brand_subcategory_pivot");

                entity.Property(e => e.BrandSubcategoryId)
                    .HasColumnName("brand_subcategory_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BrandId)
                    .HasColumnName("brand_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandSubcategoryPivot)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_brand_subcategory_pivot_brand_table");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.BrandSubcategoryPivot)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_brand_subcategory_pivot_sub_category");
            });

            modelBuilder.Entity<BrandTable>(entity =>
            {
                entity.HasKey(e => e.BrandId)
                    .HasName("PK_brand_table");

                entity.ToTable("brand_table");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.ProductDesId).HasColumnName("product_des_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.ProductDes)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.ProductDesId)
                    .HasConstraintName("FK_cart_product_specification");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_cart_product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_cart_Usertable");
            });

            modelBuilder.Entity<CartProdescriptionPivot>(entity =>
            {
                entity.HasKey(e => e.CartProdesId)
                    .HasName("PK_cart_prodescription_pivot");

                entity.ToTable("cart_prodescription_pivot");

                entity.Property(e => e.CartProdesId).HasColumnName("cart_prodes_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");
                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartProdescriptionPivot)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_cart_prodescription_pivot_cart");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartProdescriptionPivot)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_cart_prodescription_pivot_product");

                entity.HasOne(d => d.Specification)
                    .WithMany(p => p.CartProdescriptionPivot)
                    .HasForeignKey(d => d.SpecificationId)
                    .HasConstraintName("FK_cart_prodescription_pivot_product_specification");
            });

            modelBuilder.Entity<Categorytable>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_categorytable");

                entity.ToTable("categorytable");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50);
                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_order_Usertable");
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PK_order_items");

                entity.ToTable("order_items");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProDesId).HasColumnName("pro_des_id");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.SellerId).HasColumnName("seller_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.unitTotal).HasColumnName("unit_total");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_order_items_cart");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_order_items_order");

                entity.HasOne(d => d.Pro)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_order_items_product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductImage).HasColumnName("product_image");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_product_Usertable");
            });

            modelBuilder.Entity<ProductSpecification>(entity =>
            {
                entity.HasKey(e => e.SpecificationId)
                    .HasName("PK_product_specification");

                entity.ToTable("product_specification");

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");

                entity.Property(e => e.ProductColor)
                    .HasColumnName("product_color")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductSize)
                    .HasColumnName("product_size")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSpecification)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_product_specification_product");
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.Property(e => e.ReviewId).HasColumnName("review_id");

                entity.Property(e => e.Feedback).HasColumnName("feedback");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.RatingStars).HasColumnName("rating_stars");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("sub_category");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_sub_category_categorytable1");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.SpecId).HasColumnName("spec_id");
                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_transaction_categorytable");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_transaction_order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_transaction_product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_transaction_Usertable");
            });

            modelBuilder.Entity<Usertable>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Usertable");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.Cnic)
                    .HasColumnName("cnic")
                    .HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.IsBlocked)
                .HasColumnName("is_blocked");



                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("decimal");

                entity.Property(e => e.Logo).HasColumnName("logo");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("decimal");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNo)
                    .HasColumnName("phone_no")
                    .HasMaxLength(50);

                entity.Property(e => e.ShopName)
                    .HasColumnName("shop_name")
                    .HasMaxLength(50);

                entity.Property(e => e.ShippingDetail).HasColumnName("shipping_detail");

                entity.Property(e => e.UserType)
                    .HasColumnName("user_type")
                    .HasMaxLength(50);
            });
        }
    }
}