using System;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBankAccounts.Models
{
    public class BasicEntity
    {
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public BasicEntity()
        {
            this.CreatedAt = System.DateTime.Now;
            this.UpdatedAt = System.DateTime.Now;
        }
    }
    public class BasicDbContext : DbContext
    {
        public BasicDbContext(DbContextOptions options) : base(options)
        {}
        public DbSet<User> Users {get; set;}
        public DbSet<Account> Accounts {get; set;}
        public DbSet<Transaction> Transactions {get; set;}
    }
    
}