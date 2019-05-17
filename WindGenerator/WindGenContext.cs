using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindEnergy
{
    public class WindGenContext : DbContext
    {
        public WindGenContext() : base("DbConnection")
        {
            Database.SetInitializer<WindGenContext>(new CreateDatabaseIfNotExists<WindGenContext>());
        }

        public DbSet<WindGenerator> WindGenerators { get; set; }
    }
}
