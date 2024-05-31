using LotoSpain_API.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LotoSpain_API.Datos
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Bonoloto> bonoloto_res { get; set; }
        public DbSet<Euromillones> euromillones_res { get; set; }
        public DbSet<Eurodreams> eurodreams_res { get; set; }
        public DbSet<Gordo> gordo_res { get; set; }
        public DbSet<Lototurf> lototurf_res { get; set; }
        public DbSet<Nacional> nacional_res { get; set; }
        public DbSet<Primitiva> primitiva_res { get; set; }
        public DbSet<Quiniela> quiniela_res { get; set; }
        public DbSet<Quinigol> quinigol_res { get; set; }
        public DbSet<Quintuple> quintuple_res { get; set; }
        public DbSet<Bote> botes_res { get; set; }
    }
}
