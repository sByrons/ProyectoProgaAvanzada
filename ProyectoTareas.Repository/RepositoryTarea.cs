using ProyectoTareas.Data;
using ProyectoTareas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTareas.Repository
{
    public interface IRepositoryTarea : IRepositoryBase<Tarea>
    {
    }

    public class RepositoryTarea : RepositoryBase<Tarea>, IRepositoryTarea
    {
        public RepositoryTarea(AppDbContext context) : base(context)
        {
        }

    }
}
