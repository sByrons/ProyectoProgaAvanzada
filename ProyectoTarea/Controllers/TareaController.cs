using ProyectoTareas.Business;
using ProyectoTareas.Models;
using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace ProyectoTareas.Mvc.Controllers
{
    public class TareaController : Controller
    {
        private readonly TareaBusiness tareaBusiness = new TareaBusiness();
        public ActionResult Index()
        {
            var tareas = tareaBusiness.ObtenerTodas();
            ViewBag.ContadorPendientes = tareas.Count(t => t.Estado == "Pendiente");
            ViewBag.ContadorEnProceso = tareas.Count(t => t.Estado == "EnProceso");
            ViewBag.ContadorFinalizadas = tareas.Count(t => t.Estado == "Finalizada");
            ViewBag.ContadorFallidas = tareas.Count(t => t.Estado == "Fallida");
            return View(tareas);
        }

        public ActionResult Create()
        {
            ViewBag.Prioridades = new SelectList(new[] {"Alta", "Media", "Baja"});
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                tarea.Estado = "Pendiente";
                tarea.FechaCreacion = DateTime.Now;
                tarea.MensajeLog = string.Empty;
                tarea.FechaEjecucion = DateTime.Now;
                tareaBusiness.CrearTarea(tarea);
                return RedirectToAction("Index");
            }
            ViewBag.Prioridades = new SelectList(new[] { "Alta", "Media", "Baja" }, tarea?.Prioridad);
            return View(tarea);
        }

        public ActionResult Edit(int id)
        {
            
            var tarea = tareaBusiness.ObtenerPorId(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Prioridades = new SelectList(new[] {"Alta", "Media", "Baja"}, tarea.Prioridad);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                tareaBusiness.ActualizarTarea(tarea);
                return RedirectToAction("Index");
            }
            ViewBag.Prioridades = new SelectList(new[] {"Alta", "Media", "Baja"}, tarea.Prioridad);
            return View(tarea);
        }

        public ActionResult Delete(int id)
        {
            tareaBusiness.EliminarTarea(id);
            return RedirectToAction("Index");
        }

    }
}

