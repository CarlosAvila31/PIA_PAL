using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Data
{
    public static class PeliculasData
    {
        public static List<Pelicula> Todas { get; } = new()
        {
            new Pelicula
            {
                Id = 1,
                Titulo = "Scream 7",
                RutaPoster = "// ms-appx:///Assets/Posters/scream7",
                Generos = new[] { "Terror", "Slasher" },
                DuracionMinutos = 114,
                Clasificacion = "B15",
                FechaEstreno = "2026/04/12",
                Director = "Christopher Landon",
                Reparto = "Melissa Barrera, Jenna Ortega",
                Sinopsis = "La pandilla de Woodsboro enfrenta a un Ghostface más despiadado que nunca, en la entrega más sangrienta de la saga."
            },
            new Pelicula
            {
                Id = 2,
                Titulo = "Frankenstein",
                RutaPoster = "#0F2E0F",
                Generos = new[] { "Drama", "Terror" },
                DuracionMinutos = 150,
                Clasificacion = "B15",
                FechaEstreno = "2026/04/04",
                Director = "Guillermo del Toro",
                Reparto = "Oscar Isaac, Mia Goth, Andrew Scott",
                Sinopsis = "Una nueva visión del clásico de Mary Shelley. El doctor Frankenstein lleva su obsesión por vencer a la muerte más allá de todo límite ético."
            },
            new Pelicula
            {
                Id = 3,
                Titulo = "Cumbres Borrascosas",
                RutaPoster = "#10102E",
                Generos = new[] { "Romance", "Drama", "B15" },
                DuracionMinutos = 136,
                Clasificacion = "B15",
                FechaEstreno = "2026/04/18",
                Director = "Emerald Fennell",
                Reparto = "Jacob Elordi, Margot Robbie",
                Sinopsis = "La tempestuosa historia de amor entre Heathcliff y Catherine cobra vida en esta adaptación visualmente deslumbrante del clásico de Emily Brontë."
            },
            new Pelicula
            {
                Id = 4,
                Titulo = "Proyecto Génesis",
                RutaPoster = "#1E1E0A",
                Generos = new[] { "Sci-Fi", "Acción" },
                DuracionMinutos = 128,
                Clasificacion = "A",
                FechaEstreno = "2026/03/29",
                Director = "Denis Villeneuve",
                Reparto = "Timothée Chalamet, Zendaya, Florence Pugh",
                Sinopsis = "En un futuro cercano, una inteligencia artificial desarrolla conciencia propia y debe decidir entre la lealtad a sus creadores o la supervivencia de la humanidad."
            },
            new Pelicula
            {
                Id = 5,
                Titulo = "El Rey León 2",
                RutaPoster = "#211A0A",
                Generos = new[] { "Animación", "Familiar" },
                DuracionMinutos = 105,
                Clasificacion = "AA",
                FechaEstreno = "2026/06/10",
                Director = "Barry Jenkins",
                Reparto = "Donald Glover, Beyoncé, Billy Eichner",
                Sinopsis = "Simba y Nala guían a la siguiente generación mientras una nueva amenaza surge en las Tierras del Recuerdo, poniendo a prueba el legado de Mufasa."
            },
            new Pelicula
            {
                Id = 6,
                Titulo = "Spider-Man: Nexus",
                RutaPoster = "#180E22",
                Generos = new[] { "Acción", "Superhéroes" },
                DuracionMinutos = 145,
                Clasificacion = "B15",
                FechaEstreno = "2026/06/12",
                Director = "Joaquim Dos Santos",
                Reparto = "Tom Holland, Zendaya, Andrew Garfield",
                Sinopsis = "Peter Parker se adentra en el Nexus cuántico para enfrentar una amenaza que podría colapsar todos los universos conocidos del multiverso Marvel."
            },
            new Pelicula
            {
                Id = 7,
                Titulo = "Interestelar 2",
                RutaPoster = "#0C1620",
                Generos = new[] { "Sci-Fi", "Drama" },
                DuracionMinutos = 180,
                Clasificacion = "A",
                FechaEstreno = "2026/07/08",
                Director = "Christopher Nolan",
                Reparto = "Matthew McConaughey, Anne Hathaway, Fionn Whitehead",
                Sinopsis = "Décadas después de los eventos de la primera entrega, una nueva tripulación debe atravesar el agujero de gusano para rescatar a Cooper y completar la misión."
            },
            new Pelicula
            {
                Id = 8,
                Titulo = "Dune: Parte III",
                RutaPoster = "#201808",
                Generos = new[] { "Aventura", "Sci-Fi" },
                DuracionMinutos = 160,
                Clasificacion = "B15",
                FechaEstreno = "2026/08/28",
                Director = "Denis Villeneuve",
                Reparto = "Timothée Chalamet, Zendaya, Austin Butler",
                Sinopsis = "Paul Atreides, convertido en el mesías Muad'Dib, enfrenta las consecuencias de su ascenso al poder mientras el universo se desmorona a su alrededor."
            }
        };
        public static void GuardarPeliculas()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "Data", "peliculas.json");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string json = JsonSerializer.Serialize(Todas, options);
            Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
            File.WriteAllText(ruta, json); 
        }

        public static List<Pelicula> CargarPeliculas()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "Data", "peliculas.json");
            if (!File.Exists(ruta))
                return new List<Pelicula>();
            string json = File.ReadAllText(ruta);
            var peliculas = JsonSerializer.Deserialize<List<Pelicula>>(json);
            return peliculas ?? new List<Pelicula>();
        }
        
        public static Pelicula? GetById(int id)
        {
            var peliculas = CargarPeliculas();
            return peliculas.Find(p => p.Id == id);
        }

    }
}