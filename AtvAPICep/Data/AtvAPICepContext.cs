using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AtvAPICep.Models;

namespace AtvAPICep.Data
{
    public class AtvAPICepContext : DbContext
    {
        public AtvAPICepContext (DbContextOptions<AtvAPICepContext> options)
            : base(options)
        {
        }

        public DbSet<AtvAPICep.Models.Pessoa> Pessoa { get; set; } = default!;
    }
}
