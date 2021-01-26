using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RutotecaWeb.Models;

namespace RutotecaWeb.DataContext
{
    public class AppContext : DbContext
    {
        public AppContext() { }
        public AppContext(DbContextOptions<AppContext> options) : base(options) { }
        public DbSet<RutotecaWeb.Models.ElementoDTO> ElementoDTO { get; set; }
        public DbSet<RutotecaWeb.Models.LocalidadDTO> LocalidadDTO { get; set; }
    }
}
