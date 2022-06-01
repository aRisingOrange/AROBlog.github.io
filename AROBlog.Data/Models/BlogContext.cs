using Microsoft.EntityFrameworkCore;


namespace AROBlog.Data.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext()
        {

        }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(@"Data Source =MIGINA\SQLEXPRESS; Initial Catalog = BlogTestDb; Persist Security Info = True; User ID = sa; Password = 1246616521;");
        }
        /// <summary>
        /// 关闭级联删除
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);
            //foreach (var fk in foreignKeys)
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            //}
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(c => c.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Article>()
                .HasOne(u=>u.User)
                .WithMany(a=>a.Articles)
                .HasForeignKey(a=>a.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ArticleToCategory>()
                .HasKey(e => new { e.CategoryId, e.ArticleId });
            modelBuilder.Entity<ArticleToCategory>()
                .HasOne(a => a.Article)
                .WithMany(atc => atc.ArticleToCategories)
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleToCategory>()
                .HasOne(c => c.Category)
                .WithMany(atc => atc.ArticleToCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ArticleToCategory> ArticleToCategories { get; set; }
    }
}
