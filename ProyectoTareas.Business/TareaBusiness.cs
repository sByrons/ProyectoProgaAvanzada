using ProyectoTareas.Data;
using ProyectoTareas.Models;
using ProyectoTareas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTareas.Business
{
    public class TareaBusiness
    {
        private readonly RepositoryTarea repositoryTarea;

        public TareaBusiness()
        {
            var context = new AppDbContext();
            repositoryTarea = new RepositoryTarea(context);
        }

        public List<Tarea> ObtenerTodas()
        {
            return repositoryTarea.GetAll().ToList();
        }

        public Tarea ObtenerPorId(int id)
        {
            return repositoryTarea.GetById(id);
        }

        public void CrearTarea(Tarea tarea)
        {
            repositoryTarea.Add(tarea);
            repositoryTarea.Save();
        }

        public void ActualizarTarea(Tarea tarea)
        {
            repositoryTarea.Update(tarea);
            repositoryTarea.Save();
        }

        public void EliminarTarea(int id)
        {
            repositoryTarea.Delete(id);
            repositoryTarea.Save();
        }
    }
}
