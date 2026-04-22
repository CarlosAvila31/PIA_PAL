namespace EXPERIMENTO.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string RutaPoster { get; set; } = "";
        public string[] Generos { get; set; } = [];
        public int DuracionMinutos { get; set; } = 0;
        public string Clasificacion { get; set; } = "";
        public string FechaEstreno { get; set; } = "";
        public string Director { get; set; } = "";
        public string Reparto { get; set; } = "";
        public string Sinopsis { get; set; } = "";
    }
}
