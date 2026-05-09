using EXPERIMENTO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EXPERIMENTO.Data
{
    public static class SalasData
    {
        // ── Layout de cada sala ──────────────────────────────────────────────
        private static readonly Dictionary<string, SalaLayout> Salas = new()
        {
            ["Sala 1"] = new SalaLayout { NombreSala = "Sala 1", NumFilas = 8, AsientosPorFila = 10 },
            ["Sala 3"] = new SalaLayout { NombreSala = "Sala 3", NumFilas = 7, AsientosPorFila = 8 },
            ["Sala 6"] = new SalaLayout { NombreSala = "Sala 6", NumFilas = 6, AsientosPorFila = 8 },
            ["Sala 9 IMAX"] = new SalaLayout { NombreSala = "Sala 9 IMAX", NumFilas = 10, AsientosPorFila = 14 },
        };

        // ── Ruta del archivo de ocupados ─────────────────────────────────────
        private static readonly string RutaJson =
            Path.Combine(AppContext.BaseDirectory, "Data", "ocupados.json");

        // ── Clave única por función ──────────────────────────────────────────
        // Formato: "Sala_fecha_formato"
        private static string GetClave(Funcion funcion)
            => $"{funcion.Sala}_{funcion.FechaHora:yyyyMMddHHmm}_{funcion.Formato}";

        // ── Obtener layout de una sala ───────────────────────────────────────
        public static SalaLayout? GetLayout(string nombreSala)
            => Salas.TryGetValue(nombreSala, out var layout) ? layout : null;

        // ── Cargar ocupados desde archivo ────────────────────────────────────
        public static Dictionary<string, List<string>> CargarOcupados()
        {
            if (!File.Exists(RutaJson))
                return new Dictionary<string, List<string>>();

            try
            {
                string json = File.ReadAllText(RutaJson);
                return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json)
                       ?? new Dictionary<string, List<string>>();
            }
            catch
            {
                return new Dictionary<string, List<string>>();
            }
        }

        // ── Guardar ocupados en archivo ──────────────────────────────────────
        private static void GuardarOcupados(Dictionary<string, List<string>> ocupados)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaJson)!);
                var opciones = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(RutaJson, JsonSerializer.Serialize(ocupados, opciones));
            }
            catch { }
        }

        /// <summary>Agrega asientos vendidos a una función y los persiste.</summary>
        public static void MarcarVendidos(Funcion funcion, List<string> asientosVendidos)
        {
            var ocupados = CargarOcupados();
            string clave = GetClave(funcion);

            if (!ocupados.ContainsKey(clave))
                ocupados[clave] = new List<string>();

            foreach (string asiento in asientosVendidos)
            {
                if (!ocupados[clave].Contains(asiento))
                    ocupados[clave].Add(asiento);
            }

            GuardarOcupados(ocupados);
        }

        // ── Obtener lista de ocupados para una función ───────────────────────
        public static List<string> GetOcupados(Funcion funcion)
        {
            var ocupados = CargarOcupados();
            string clave = GetClave(funcion);
            return ocupados.ContainsKey(clave) ? ocupados[clave] : new List<string>();
        }
    }
}
