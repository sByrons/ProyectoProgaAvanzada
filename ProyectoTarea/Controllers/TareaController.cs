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
        private readonly TareaWorker tareaWorker = new TareaWorker();

        private static TareaWorker _worker = new TareaWorker();

        public ActionResult IniciarWorker()
        {
            if (!_worker.IsRunning)
            {
                _worker.Start();
                TempData["MensajeWorker"] = "Worker iniciado correctamente.";
            }
            else
            {
                TempData["MensajeWorker"] = "El Worker ya está en ejecución.";
            }

            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            var tareas = tareaBusiness.ObtenerTodasOrdenadas();  
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
                tareaWorker.EncolarTarea(tarea);
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

        public ActionResult Reintentar(int id)
        {
            var tarea = tareaBusiness.ObtenerPorId(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            if (tarea.Estado == "Fallida")
            {
                tarea.Estado = "Pendiente";
                tarea.MensajeLog = string.Empty;
                tareaBusiness.ActualizarTarea(tarea);
                tareaWorker.EncolarTarea(tarea);
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult TablaTareas()
        {
            var tareas = tareaBusiness.ObtenerTodasOrdenadas();
            return PartialView("_TablaTareas", tareas);
        }

        public PartialViewResult ResumenTareas()
        {
            var tareas = tareaBusiness.ObtenerTodas();       
            ViewBag.ContadorPendientes = tareas.Count(t => t.Estado == "Pendiente");
            ViewBag.ContadorEnProceso = tareas.Count(t => t.Estado == "EnProceso");
            ViewBag.ContadorFinalizadas = tareas.Count(t => t.Estado == "Finalizada");
            ViewBag.ContadorFallidas = tareas.Count(t => t.Estado == "Fallida");
            return PartialView("_ResumenTareas");
        }

    }
}

