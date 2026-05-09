using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EXPERIMENTO.Models;

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
        // Formato: "peliculaId_sala_yyyy-MM-dd_HH:mm"
        public static string GetClave(Funcion f)
            => $"{f.PeliculaId}_{f.Sala}_{f.FechaHora:yyyy-MM-dd_HH:mm}";

        // ── Obtener layout de una sala ───────────────────────────────────────
        public static SalaLayout? GetLayout(string nombreSala)
            => Salas.TryGetValue(nombreSala, out var layout) ? layout : null;

        // ── Cargar todos los ocupados del JSON ───────────────────────────────
        private static Dictionary<string, List<string>> CargarOcupados()
        {
            try
            {
                if (!File.Exists(RutaJson)) return new();
                string json = File.ReadAllText(RutaJson);
                return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new();
            }
            catch { return new(); }
        }

        // ── Guardar ocupados al JSON ─────────────────────────────────────────
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

        // ── API pública ──────────────────────────────────────────────────────

        /// <summary>Devuelve los códigos de asientos ocupados para una función.</summary>
        public static List<string> GetOcupados(Funcion funcion)
        {
            var ocupados = CargarOcupados();
            string clave = GetClave(funcion);
            return ocupados.TryGetValue(clave, out var lista) ? lista : new();
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
    }
}