using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ApplicationDbContext()
        {
            
        }

        #region DbSets

        public DbSet<Investment> Investments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        #endregion

        #region Override methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para Investment -> Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Investment)
                .WithMany()
                .HasForeignKey("InvestmentId");

            // Configuração para Customer -> Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Transactions)
                .HasForeignKey("CustomerId");
        }

        #endregion


    }
}
