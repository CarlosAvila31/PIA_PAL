using System;

namespace EXPERIMENTO.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string RutaPoster { get; set; } = "";
        public string[] Generos { get; set; } = System.Array.Empty<string>();
        public int DuracionMinutos { get; set; } = 0;
        public string Clasificacion { get; set; } = "";
        public string FechaEstreno { get; set; } = ""; // IMPORTANTE formato yyyy-MM-dd
        public string Director { get; set; } = "";
        public string Reparto { get; set; } = "";
        public string Sinopsis { get; set; } = "";
        public double Calificacion { get; set; } = 0.0;

        // URL al trailer (YouTube u otra URL de video). Dejar vacío si no hay trailer.
        public string TrailerUrl { get; set; } = "";

        public bool EsHoy => DateTime.TryParse(FechaEstreno, out var fecha) && fecha <= DateTime.Today;

    }
}
