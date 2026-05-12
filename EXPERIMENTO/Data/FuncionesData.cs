using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Data
{
    public class FuncionesData
    {

        // ── Horarios base por película ───────────────────────────────────────
        // Define en qué días y a qué horas se proyecta cada película.
        // Es como PeliculasData pero pues de las funciones va

        private static readonly List<HorarioPelicula> Horarios = new()
        { 
            //Funciones de La Princesa Mononoke
            //Sala 1 en 2D (Lun, Mié, Vie, Sáb, Dom)
            new HorarioPelicula
            {
                PeliculaId = 1,
                Dias       = [DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday,
                              DayOfWeek.Saturday, DayOfWeek.Sunday],
                Horas      = ["14:30", "17:00", "20:15", "22:30"],
                Sala       = "Sala 1",
                Formato    = "2D"
            },

            //Funciones de Super Mario Galaxy
            //Sala 3 en 2D (Mar, Jue, Sáb, Dom)
            new HorarioPelicula
            {
                PeliculaId = 2,
                Dias       = [DayOfWeek.Tuesday, DayOfWeek.Thursday,
                              DayOfWeek.Saturday, DayOfWeek.Sunday],
                Horas      = ["15:00", "18:30"],
                Sala       = "Sala 3",
                Formato    = "2D"
            },

            //Funciones de Whiplash
            //Sala 6 (todos los días)
            new HorarioPelicula
            {
                PeliculaId = 3,
                Dias       = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                              DayOfWeek.Thursday, DayOfWeek.Friday,
                              DayOfWeek.Saturday, DayOfWeek.Sunday],
                Horas      = ["16:00", "19:00"],
                Sala       = "Sala 6",
                Formato    = "2D"
            },

            //Sala 9 IMAX (todos los días)
            new HorarioPelicula
            {
                PeliculaId = 3,
                Dias       = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                              DayOfWeek.Thursday, DayOfWeek.Friday,
                              DayOfWeek.Saturday, DayOfWeek.Sunday],
                Horas      = ["13:00", "16:30", "20:00"],
                Sala       = "Sala 9 IMAX",
                Formato    = "IMAX"
            },

            //Funciones de Akira
            //Sala 1 en 3D (Lun-Vie)
            new HorarioPelicula
            {
                PeliculaId = 4,
                Dias       = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                              DayOfWeek.Thursday, DayOfWeek.Friday],
                Horas      = ["18:00"],
                Sala       = "Sala 1",
                Formato    = "3D"
            },



        };

        public static List<GrupoFunciones> GetFuncionesPorPelicula(int peliculaId, int diasAdelante = 7)
        {
            var funciones = GenerarFunciones(peliculaId, diasAdelante);

            return funciones
                .GroupBy(f => f.FechaHora.Date)
                .OrderBy(g => g.Key)
                .Select(g => new GrupoFunciones
                {
                    Fecha = g.Key,
                    Funciones = g.OrderBy(f => f.FechaHora).ToList()
                })
                .ToList();
        }

        // ── Generación dinámica ──────────────────────────────────────────────
        private static List<Funcion> GenerarFunciones(int peliculaId, int diasAdelante)
        {
            var resultado = new List<Funcion>();
            var horariosFiltered = Horarios.Where(h => h.PeliculaId == peliculaId);
            int idCounter = 1;

            for (int dia = 0; dia < diasAdelante; dia++)
            {
                DateTime fecha = DateTime.Today.AddDays(dia);

                foreach (var horario in horariosFiltered)
                {
                    if (!horario.Dias.Contains(fecha.DayOfWeek))
                        continue;

                    foreach (string hora in horario.Horas)
                    {
                        if (!TimeSpan.TryParse(hora, out var ts))
                            continue;

                        var fechaHora = fecha + ts;

                        // No mostrar funciones que ya pasaron hoy
                        if (fechaHora < DateTime.Now)
                            continue;

                        resultado.Add(new Funcion
                        {
                            Id = idCounter++,
                            PeliculaId = peliculaId,
                            Sala = horario.Sala,
                            Formato = horario.Formato,
                            FechaHora = fechaHora
                        });
                    }
                }
            }

            return resultado;
        }
    }

    // ── Clase auxiliar para agrupar funciones por fecha ──────────────────────
    public class GrupoFunciones
    {
        public DateTime Fecha { get; set; }
        public List<Funcion> Funciones { get; set; } = [];

        public string FechaFormateada =>
            Fecha.Date == DateTime.Today
                ? "Hoy"
                : Fecha.Date == DateTime.Today.AddDays(1)
                    ? "Mañana"
                    : Fecha.ToString("dddd d 'de' MMMM",
                        new System.Globalization.CultureInfo("es-MX"));
    }
}

 