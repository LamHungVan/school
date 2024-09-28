using Microsoft.EntityFrameworkCore;

namespace Week8_BT1_API_CURD.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.TeacherID);

            modelBuilder.Entity<Class>()
                .HasKey(c => c.ClassID);

            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentUUID);

            // Cấu hình mối quan hệ giữa Student và Class
            modelBuilder.Entity<Student>()
                .HasOne<Class>() // Student có một Class
                .WithMany() // Class có nhiều Student
                .HasForeignKey(s => s.ClassID) // Khóa ngoại
                .OnDelete(DeleteBehavior.Cascade); // Thiết lập xóa cascade cho học sinh


            // Cấu hình mối quan hệ giữa Class và Teacher
            modelBuilder.Entity<Class>()
                .HasOne<Teacher>() // Class có một Teacher
                .WithMany() // Teacher có nhiều Class
                .HasForeignKey(c => c.TeacherID) // Khóa ngoại
                .OnDelete(DeleteBehavior.Cascade); // Thiết lập xóa cascade cho lớp học
        }
    }
}
