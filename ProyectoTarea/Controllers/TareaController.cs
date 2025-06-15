using ProyectoTareas.Business;
using ProyectoTareas.Models;
using System.Web.Mvc;

namespace ProyectoTareas.Mvc.Controllers
{
    public class TareaController : Controller
    {
        private readonly TareaBusiness tareaBusiness = new TareaBusiness();

        public ActionResult Index()
        {
            var tareas = tareaBusiness.ObtenerTodas();
            return View(tareas);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                tareaBusiness.CrearTarea(tarea);
                return RedirectToAction("Index");
            }
            return View(tarea);
        }
    }
}

