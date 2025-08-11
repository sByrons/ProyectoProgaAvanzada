using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTareas.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")] // valida que no  esté vacío
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La url es obligatorio")] // valida que no  esté vacío
        [Url(ErrorMessage = "La URL debe ser válida.")] // valida que sea una URL válida
        [Display(Name = "URL del archivo")] // muestra un nombre más amigable en la interfaz
        public string UrlArchivo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la prioridad.")] // valida que no esté vacío
        public string Prioridad { get; set; } // puede ser baja, media o alta

        [Required(ErrorMessage = "Debe indicar la fecha de ejecucion.")] // valida que no esté vacío
        [Display(Name = "Fecha de ejecución")] // muestra un nombre más amigable en la interfaz
        public DateTime FechaEjecucion { get; set; } = DateTime.Now; // fecha de ejecución de la tarea

        public string Estado { get; set; } = "Pendiente"; // estado de la tarea, por defecto es "Pendiente"

        [Display(Name = "Mensaje de log")] // muestra un nombre más amigable en la interfaz
        public string MensajeLog { get; set; } // resultado de la ejecucion

        [Display(Name = "Fecha de creación")] // muestra un nombre más amigable en la interfaz
        public DateTime FechaCreacion { get; set; } = DateTime.Now; // fecha de creación de la tarea, por defecto es la fecha actual



    }
}
