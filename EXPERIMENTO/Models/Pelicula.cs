namespace EXPERIMENTO.Models
{
    public class Pelicula
    {
        public string Titulo { get; set; } = "";
        public string Emoji { get; set; } = "🎬";
        public string RutaPoster { get; set; } = "";        // ms-appx:///Assets/Posters/scream7.jpg
        public double Calificacion { get; set; } = 0.0;
        public int NumResenas { get; set; } = 0;
        public string[] Generos { get; set; } = [];
        public int DuracionMinutos { get; set; } = 0;
        public string Clasificacion { get; set; } = "";
        public string FechaEstreno { get; set; } = "";
        public string Director { get; set; } = "";
        public string Reparto { get; set; } = "";
        public string Sinopsis { get; set; } = "";
        public string Salas { get; set; } = "";
        public string Audio { get; set; } = "";
        public bool EsHoy { get; set; } = false;
        public string FechaProxima { get; set; } = "";           // vacío si es hoy
    }
}
