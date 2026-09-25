namespace KMC_API.Data
{
    using KMC_API.Model;
    using Microsoft.EntityFrameworkCore;

    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options) { }

        //For each model do as below
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketClass> TicketClasses { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Opt1 - for some columns ask EF to use the DB types as we select
            modelBuilder.Entity<TicketClass>().Property(t => t.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Registration>().Property(r => r.AmountPaid).HasColumnType("decimal(18,2)");

            //Relationships
            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(o => o.EventList)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketClass>()
                .HasOne(t => t.ForEvent)
                .WithMany(e => e.TicketClasses)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.ForEvent)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.BookedClass)
                .WithMany(t => t.Registrations)
                .HasForeignKey(r => r.TicketClassId)
                .OnDelete(DeleteBehavior.Restrict);

            //Opt2 - allow EF to decide all the data types with the SQL server
            modelBuilder.Entity<Organizer>();
            modelBuilder.Entity<Category>().HasData(
       new Category { CategoryId = 1, Name = "Cultural" },
       new Category { CategoryId = 2, Name = "Music" },
       new Category { CategoryId = 3, Name = "Sports" },
       new Category { CategoryId = 4, Name = "Workshop" },
       new Category { CategoryId = 5, Name = "Exhibition" },
       new Category { CategoryId = 6, Name = "Religious" },
       new Category { CategoryId = 7, Name = "Community" },
       new Category { CategoryId = 8, Name = "Food & Trade Fair" },
       new Category { CategoryId = 9, Name = "Other" }
   );
            modelBuilder.Entity<Manager>().HasData(
      new Manager
      {
          ManagerId = 1,
          FullName = "KMC Admin",
          Username = "admin",
          PasswordHash = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9",
          CreatedDate = new DateTime(2026, 1, 1)   // fixed static date - DateTime.Now vaikama
      }
  );
        }
    }
}
