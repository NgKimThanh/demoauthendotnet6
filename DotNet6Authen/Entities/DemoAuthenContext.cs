using Microsoft.EntityFrameworkCore;

namespace DotNet6Authen.Entities
{
    public class DemoAuthenContext : DbContext
    {

        public DemoAuthenContext(DbContextOptions<DemoAuthenContext> options) : base(options)
        {

        }

        public DbSet<User>? User { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<GroupOfProduct>? GroupOfProduct { get; set; }
        public DbSet<UserRefreshToken>? UserRefreshToken { get; set; }

        // Định nghĩa mô hình cơ sở dữ liệu (database model) cho một ứng dụng
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // IsUnique() đảm bảo rằng các giá trị trong cột UserName phải là duy nhất
        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.HasIndex(u => u.UserName).IsUnique();
        //    });
        //}
    }
}
