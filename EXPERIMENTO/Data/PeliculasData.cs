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
                Titulo = "La Princesa Mononoke",
                RutaPoster = "ms-appx:///Assets/Posters/Princesa_Mononoke.jpg",
                Generos = new[] { "Fantasía", "Anime", "Cine Familiar" },
                DuracionMinutos = 133,
                Clasificacion = "AA",
                FechaEstreno = "2026/04/12",
                Director = "Hayao Miyazaki",
                Reparto = "Akihiro Miwa, Yuko Tanaka",
                Sinopsis = "Un príncipe se ve involucrado en un conflicto entre una princesa del bosque y el abuso de la mecanización.",
                Calificacion = 8.7

            },
            new Pelicula
            {
                Id = 2,
                Titulo = "Super Mario Galaxy",
                RutaPoster = "ms-appx:///Assets/Posters/SMG.jpg",
                Generos = new[] { "Infantil", "Aventura" },
                DuracionMinutos = 98,
                Clasificacion = "AA",
                FechaEstreno = "2026/04/01",
                Director = "Michael Jelenic",
                Reparto = "Brie Larson, Jack Black, Chriss Pratt",
                Sinopsis = "Mario viaja por el espacio, explorando pequeños planetas con gravedad propia para rescatar a la Princesa Peach de Bowser, quien busca gobernar el universo.",
                Calificacion = 6.7
            },
            new Pelicula
            {
                Id = 3,
                Titulo = "Whiplash",
                RutaPoster = "ms-appx:///Assets/Posters/whiplash.jpg",
                Generos = new[] { "Independiente", "Drama", "B15" },
                DuracionMinutos = 106,
                Clasificacion = "B15",
                FechaEstreno = "2026/04/18",
                Director = "Damien Chazelle",
                Reparto = "Miles Teller, J. K. Simmons",
                Sinopsis = "Andrew Neiman es un joven y ambicioso baterista de jazz. Marcado por el fracaso de la carrera literaria de su padre, está obsesionado con alcanzar la cima dentro del elitista conservatorio de música de la Costa Este en el que estudia.",
                Calificacion = 9.2
            },
            new Pelicula
            {
                Id = 4,
                Titulo = "Akira",
                RutaPoster = "ms-appx:///Assets/Posters/akira.jpg",
                Generos = new[] { "Sci-Fi", "Acción" },
                DuracionMinutos = 124,
                Clasificacion = "R",
                FechaEstreno = "2026/03/29",
                Director = "Katsuhiro Ôtomo",
                Reparto = "Johnny Yong Bosch, Joshua Seth, Simon Prescott",
                Sinopsis = "Un joven telépata deambula por las calles de Tokio al darse cuenta de que los poderes que posee son asombrosos.",
                Calificacion = 8.8
            },
            new Pelicula
            {
                Id = 5,
                Titulo = "Avatar The Last Airbender",
                RutaPoster = "ms-appx:///Assets/Posters/Avatar.jpg",
                Generos = new[] { "Animación", "Familiar", "Fantasía" },
                DuracionMinutos = 98,
                Clasificacion = "AA",
                FechaEstreno = "2026/06/10",
                Director = "Lauren Montgomery",
                Reparto = "Eric Nam, Dave Bautista, Steven Yeun",
                Sinopsis = "Avatar Aang, el último Airbender del mundo, se entera de un antiguo poder que podría salvar a su cultura de la extinción. Con la ayuda de sus amigos, se embarca en una búsqueda global para encontrarlo antes de que caiga en las manos equivocadas."
            },
            new Pelicula
            {
                Id = 6,
                Titulo = "Baby Driver",
                RutaPoster = "ms-appx:///Assets/Posters/baby_driver.jpg",
                Generos = new[] { "Acción", "Crimen" },
                DuracionMinutos = 115,
                Clasificacion = "R",
                FechaEstreno = "2026/06/12",
                Director = "Edgar Wright",
                Reparto = "Lily James, Jon Hamm, Kevin Spacey, Jamie Foxx",
                Sinopsis = "Baby es un chofer especializado en fugas que, enamorado, pretende dejar la vida criminal y empezar de cero con la mujer que ama. Cuando el jefe de una banda de gánsteres le obliga a trabajar para él y la operación fracasa, su vida y la de su chica pasan a estar en peligro."
            },
            new Pelicula
            {
                Id = 7,
                Titulo = "Backrooms",
                RutaPoster = "ms-appx:///Assets/Posters/backrooms.jpg",
                Generos = new[] { "Sci-Fi", "Terror" },
                DuracionMinutos = 105,
                Clasificacion = "A",
                FechaEstreno = "2026/07/08",
                Director = "Kane Parsons",
                Reparto = "Chiwetel Ejiofor, Renate Reinsve, Finn Bennett",
                Sinopsis = "La historia de una terapeuta que se adentra en una peligrosa dimensión paralela, conocida como \"los Backrooms\", para salvar a su paciente desaparecido."
            },
            new Pelicula
            {
                Id = 8,
                Titulo = "Before Sunrise",
                RutaPoster = "ms-appx:///Assets/Posters/before_sunrise.jpg",
                Generos = new[] { "Romance", "Drama" },
                DuracionMinutos = 101,
                Clasificacion = "B15",
                FechaEstreno = "2026/08/28",
                Director = "Richar Linklater",
                Reparto = "Ethan Hawke, Julie Delpy",
                Sinopsis = "Dos viajeros, un joven estadounidense y una mujer francesa, se conocen en un tren y pasan un día romántico en Viena, Austria."
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
            Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
            File.WriteAllText(ruta, JsonSerializer.Serialize(Todas, options)); ;
        }

        public static List<Pelicula> CargarPeliculas()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "Data", "peliculas.json");
            if (!File.Exists(ruta))
                return Todas;
            string json = File.ReadAllText(ruta);
            var peliculas = JsonSerializer.Deserialize<List<Pelicula>>(json);
            return JsonSerializer.Deserialize<List<Pelicula>>(json) ?? Todas;
        }

        public static Pelicula? GetById(int id)
            => Todas.Find(p => p.Id == id);


    }
}