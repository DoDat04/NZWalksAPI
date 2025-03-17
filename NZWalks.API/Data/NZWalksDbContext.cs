using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Ease, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("02f2826e-0626-4994-8681-8698d4c41580"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("21fe0ec8-d430-4dbe-a2f7-184b057408db"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("67fdeb1c-f8f8-4c4c-9fba-f412d785c951"),
                    Name = "Hard"
                },
            };

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region()
                {
                    Id = Guid.Parse("151948d0-99d6-481a-98f1-b481eb849fdb"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "akl-img.com"
                },
                new Region()
                {
                    Id = Guid.Parse("0e112dd5-2aba-4225-8f33-90c289826164"),
                    Name = "Korea",
                    Code = "KOR",
                    RegionImageUrl = "kor-img.com"
                },
                new Region()
                {
                    Id = Guid.Parse("2145a429-1ea6-48f0-a99b-7245c110d624"),
                    Name = "VietNam",
                    Code = "VN",
                    RegionImageUrl = "vn-img.com"
                }
            };

            // Seed regions to database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
