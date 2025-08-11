using ProyectoTareas.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace ProyectoTareas.Business
{
    public class TareaWorker
    {
        private readonly List<Tarea> _colaTareas = new List<Tarea>();
        private readonly AutoResetEvent _despertarWorker = new AutoResetEvent(false);

        private readonly object _lock = new object();
        private readonly TareaBusiness tareaBusiness;
        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
        }
        public TareaWorker()
        {
            tareaBusiness = new TareaBusiness();
            isRunning = false;
        }

        public void EncolarTarea(Tarea tarea)
        {
            lock (_lock)
            {
                _colaTareas.Add(tarea);
            }

            // Despierta al Worker (si estaba dormido)
            _despertarWorker.Set();
        }


        public void Start()
        {
            if (isRunning)
                return;

            isRunning = true;

            // Cargar tareas pendientes de la BD al iniciar
            var tareasPendientes = tareaBusiness.ObtenerTodas()
                                    .Where(t => t.Estado == "Pendiente")
                                    .ToList();

            lock (_lock)
            {
                foreach (var tarea in tareasPendientes)
                {
                    _colaTareas.Add(tarea);
                }
            }

            new Thread(() =>
            {
                while (isRunning)
                {
                    Tarea tareaAProcesar = null;

                    lock (_lock)
                    {
                        if (_colaTareas.Any())
                        {
                            tareaAProcesar = _colaTareas
                                .OrderByDescending(t => ObtenerValorPrioridad(t.Prioridad))
                                .ThenBy(t => t.FechaEjecucion)
                                .First();
                        }
                    }

                    if (tareaAProcesar != null)
                    {
                        ProcesarTarea(tareaAProcesar);

                        // Después de procesar, eliminas la tarea de la cola
                        lock (_lock)
                        {
                            _colaTareas.Remove(tareaAProcesar);
                        }
                    }
                    else
                    {
                        // Si no hay tareas, espera hasta que llegue una nueva
                        _despertarWorker.WaitOne(Timeout.Infinite);
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            isRunning = false;
        }

        private void ProcesarTarea(Tarea tarea)
        {
            try
            {
                tarea.Estado = "EnProceso";
                tareaBusiness.ActualizarTarea(tarea);

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

                Thread.Sleep(20000);

                tarea.Estado = "Finalizada";
                tarea.MensajeLog = "Descarga exitosa.";
            }
            catch (Exception ex)
            {
                tarea.Estado = "Fallida";
                tarea.MensajeLog = $"Error: {ex.Message}";
            }

            tareaBusiness.ActualizarTarea(tarea);
        }


        private int ObtenerValorPrioridad(string prioridad)
        {
            switch (prioridad)
            {
                case "Alta": return 3;
                case "Media": return 2;
                case "Baja": return 1;
                default: return 0;
            }
        }

    }
}

