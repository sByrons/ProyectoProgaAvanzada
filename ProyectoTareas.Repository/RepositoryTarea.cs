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
        void UpdateSinTrackeo(Tarea entity);
        
    }
    
    public class RepositoryTarea : RepositoryBase<Tarea>, IRepositoryTarea
    {
        private readonly AppDbContext _context;
        public RepositoryTarea(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateSinTrackeo(Tarea entity)
        {
            var existingEntity = _context.Tareas.Find(entity.Id);

            if (existingEntity != null)
            {
                existingEntity.Nombre = entity.Nombre;
                existingEntity.UrlArchivo = entity.UrlArchivo;
                existingEntity.Prioridad = entity.Prioridad;
                existingEntity.Estado = entity.Estado;
                existingEntity.MensajeLog = entity.MensajeLog;
                existingEntity.FechaEjecucion = entity.FechaEjecucion;
                existingEntity.FechaCreacion = entity.FechaCreacion;
            }
        }
    }
}
