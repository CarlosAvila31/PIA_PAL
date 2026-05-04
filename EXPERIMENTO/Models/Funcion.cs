using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPERIMENTO.Models
{
    public class Funcion
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public string Sala {  get; set; }

        public string Formato { get; set; }
        public DateTime FechaHora { get; set; }

        // Lectura para mostrar en la 
        public string FechaFormateada =>
            FechaHora.ToString("dddd  d 'de' MMMM", new System.Globalization.CultureInfo("es-MX"));

        public string HoraFormateada => FechaHora.ToString("HH:mm");
    }

    public class HorarioPelicula
    {
        public int PeliculaId { get; set; }

        //Dias de la semana donde se proyecta asi bien proyectado me proyecte
        public DayOfWeek[] Dias { get; set; } = [];

        //Horaros del día en formato HH.mm
        public string[] Horas { get; set; } = [];

        public string Sala { get; set; } = "";
        public string Formato { get; set; } = "";

    }
}
