using System.Data.Entity;

namespace hazi.Models
{
    public class BejelentesContext : DbContext
    {
        public BejelentesContext()
            : base("hazi")
        { }
        public DbSet<Jogcim> Jogcimek { get; set; }
        public DbSet<IdoBejelentes> IdoBejelentesek { get; set; }
    }
}