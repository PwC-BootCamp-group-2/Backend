﻿using System;
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

        public DbSet<Booking> Booking { get; set; } = default!;

        public DbSet<Space>? Space { get; set; }
    }
}