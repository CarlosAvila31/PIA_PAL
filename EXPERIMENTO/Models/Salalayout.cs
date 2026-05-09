using System.Collections.Generic;

namespace EXPERIMENTO.Models
{
    public class SalaLayout
    {
        public string NombreSala { get; set; } = "";
        public int NumFilas { get; set; }
        public int AsientosPorFila { get; set; }

        // Genera la lista de códigos de asiento: A1, A2... H10
        public List<string> GenerarCodigos()
        {
            var codigos = new List<string>();
            for (int f = 0; f < NumFilas; f++)
            {
                char fila = (char)('A' + f);
                for (int n = 1; n <= AsientosPorFila; n++)
                    codigos.Add($"{fila}{n}");
            }
            return codigos;
        }
    }

    public class AsientoVM
    {
        public string Codigo { get; set; } = "";     // A1, B3, etc.
        public char Fila { get; set; }
        public int Numero { get; set; }
        public EstadoAsiento Estado { get; set; } = EstadoAsiento.Disponible;
    }

    public enum EstadoAsiento
    {
        Disponible,
        Ocupado,
        Seleccionado
    }
}