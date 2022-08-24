using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using spacemeet.Models;

namespace spacemeet.Data
{
    public class spacemeetContext : DbContext
    {
        public spacemeetContext (DbContextOptions<spacemeetContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Booking { get; set; } = default!;

        public DbSet<Space>? Space { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<spacemeet.Models.Review>? Review { get; set; }
        public DbSet<spacemeet.Models.Merchant>? Merchant { get; set; }
        public DbSet<spacemeet.Models.Individual>? Individual { get; set; }
    }
}
