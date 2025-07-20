using ProyectoTareas.Data;
using ProyectoTareas.Models;
using ProyectoTareas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoTareas.Business
{
    public class TareaWorker
    {
        private readonly TareaBusiness tareaBusiness;
        private bool isRunning;


        public TareaWorker()
        {
            tareaBusiness = new TareaBusiness();
            isRunning = false;
            var context = new AppDbContext();
        }

        public void Start()
        {
            isRunning = true;

            new Thread(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        ProcesarTareas();
                        Thread.Sleep(5000); // Esperar 5 segundos 
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar tareas: {ex.Message}");
                    }
                }
            }).Start();

        }
        public void Stop()
        {
            isRunning = false;
        }

        public void ProcesarTareas()
        {
            var tareasPendientes = tareaBusiness.ObtenerTodas()
                .Where(t => t.Estado == "Pendiente")
                .OrderByDescending(t => t.Prioridad)
                .ThenBy(t => t.FechaEjecucion)
                .ToList();

            foreach (var tarea in tareasPendientes)
            {
                tarea.Estado = "En Proceso";
                tareaBusiness.ActualizarTarea(tarea);

                try
                {
                    using (var client = new WebClient())
                    {
                        string nombreArchivo = System.IO.Path.GetFileName(new Uri(tarea.UrlArchivo).AbsolutePath);
                        string rutaCarpeta = @"C:\Descargas\";

                        if (!System.IO.Directory.Exists(rutaCarpeta))
                        {
                            System.IO.Directory.CreateDirectory(rutaCarpeta);
                        }

                        string ruta = rutaCarpeta + nombreArchivo;

                        client.DownloadFile(tarea.UrlArchivo, ruta);
                    }

                    tarea.Estado = "Finalizada";
                    tarea.MensajeLog = "Tarea Completada.";
                }
                catch (Exception ex)
                {
                    tarea.Estado = "Fallida";
                    tarea.MensajeLog = $"Error: {ex.Message}";
                }

                // Actualiza el estado Finalizada o Fallida
                tareaBusiness.ActualizarTarea(tarea);

            }
        }
    }
}

