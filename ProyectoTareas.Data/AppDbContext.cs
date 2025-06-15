using ProyectoTareas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTareas.Data
{
    public class AppDbContext :DbContext
    {
       public AppDbContext() : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
        }
        
        public DbSet<Tarea> Tareas { get; set; }

    }
}
