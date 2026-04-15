using System.Collections.Generic;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Data
{
    public static class PeliculasData
    {
        public static List<Pelicula> Todas { get; } = new()
        {
            // ── EN CARTELERA ────────────────────────────────────────────
            new Pelicula
            {
                Titulo          = "Scream 7",
                Emoji           = "😱",
                RutaPoster      = "// ms-appx:///Assets/Posters/scream7",
                Calificacion    = 8.2,
                NumResenas      = 4820,
                Generos         = ["Terror", "Slasher", "B15"],
                DuracionMinutos = 114,
                Clasificacion   = "B15",
                FechaEstreno    = "4 Abr 2025",
                Director        = "Christopher Landon",
                Reparto         = "Melissa Barrera, Jenna Ortega",
                Sinopsis        = "La pandilla de Woodsboro enfrenta a un Ghostface más despiadado que nunca, en la entrega más sangrienta de la saga.",
                Salas           = "Salas 1, 2, 4, 7",
                Audio           = "Dolby Atmos",
                EsHoy           = true
            },
            new Pelicula
            {
                Titulo          = "Frankenstein",
                Emoji           = "🧟",
                RutaPoster      = "#0F2E0F",
                Calificacion    = 7.8,
                NumResenas      = 3150,
                Generos         = ["Drama", "Terror", "B15"],
                DuracionMinutos = 150,
                Clasificacion   = "B15",
                FechaEstreno    = "4 Abr 2025",
                Director        = "Guillermo del Toro",
                Reparto         = "Oscar Isaac, Mia Goth, Andrew Scott",
                Sinopsis        = "Una nueva visión del clásico de Mary Shelley. El doctor Frankenstein lleva su obsesión por vencer a la muerte más allá de todo límite ético.",
                Salas           = "Salas 3, 5",
                Audio           = "Dolby Atmos",
                EsHoy           = true
            },
            new Pelicula
            {
                Titulo          = "Cumbres Borrascosas",
                Emoji           = "🌪️",
                RutaPoster      = "#10102E",
                Calificacion    = 7.5,
                NumResenas      = 2200,
                Generos         = ["Romance", "Drama", "B15"],
                DuracionMinutos = 136,
                Clasificacion   = "B15",
                FechaEstreno    = "4 Abr 2025",
                Director        = "Emerald Fennell",
                Reparto         = "Jacob Elordi, Margot Robbie",
                Sinopsis        = "La tempestuosa historia de amor entre Heathcliff y Catherine cobra vida en esta adaptación visualmente deslumbrante del clásico de Emily Brontë.",
                Salas           = "Salas 6, 8",
                Audio           = "Estéreo",
                EsHoy           = true
            },
            new Pelicula
            {
                Titulo          = "Proyecto Génesis",
                Emoji           = "🤖",
                RutaPoster      = "#1E1E0A",
                Calificacion    = 8.5,
                NumResenas      = 5100,
                Generos         = ["Sci-Fi", "Acción", "A"],
                DuracionMinutos = 128,
                Clasificacion   = "A",
                FechaEstreno    = "4 Abr 2025",
                Director        = "Denis Villeneuve",
                Reparto         = "Timothée Chalamet, Zendaya, Florence Pugh",
                Sinopsis        = "En un futuro cercano, una inteligencia artificial desarrolla conciencia propia y debe decidir entre la lealtad a sus creadores o la supervivencia de la humanidad.",
                Salas           = "Salas 1, 3, 9",
                Audio           = "Dolby Atmos, IMAX",
                EsHoy           = true
            },

            // ── PRÓXIMAMENTE ────────────────────────────────────────────
            new Pelicula
            {
                Titulo          = "El Rey León 2",
                Emoji           = "🦁",
                RutaPoster      = "#211A0A",
                Calificacion    = 0,
                NumResenas      = 0,
                Generos         = ["Animación", "Familiar", "AA"],
                DuracionMinutos = 105,
                Clasificacion   = "AA",
                FechaEstreno    = "25 Jul 2025",
                Director        = "Barry Jenkins",
                Reparto         = "Donald Glover, Beyoncé, Billy Eichner",
                Sinopsis        = "Simba y Nala guían a la siguiente generación mientras una nueva amenaza surge en las Tierras del Recuerdo, poniendo a prueba el legado de Mufasa.",
                Salas           = "Por confirmar",
                Audio           = "Por confirmar",
                EsHoy           = false,
                FechaProxima    = "25 Jul"
            },
            new Pelicula
            {
                Titulo          = "Spider-Man: Nexus",
                Emoji           = "🕷️",
                RutaPoster      = "#180E22",
                Calificacion    = 0,
                NumResenas      = 0,
                Generos         = ["Acción", "Superhéroes", "B15"],
                DuracionMinutos = 145,
                Clasificacion   = "B15",
                FechaEstreno    = "3 Ago 2025",
                Director        = "Joaquim Dos Santos",
                Reparto         = "Tom Holland, Zendaya, Andrew Garfield",
                Sinopsis        = "Peter Parker se adentra en el Nexus cuántico para enfrentar una amenaza que podría colapsar todos los universos conocidos del multiverso Marvel.",
                Salas           = "Por confirmar",
                Audio           = "Dolby Atmos, IMAX",
                EsHoy           = false,
                FechaProxima    = "3 Ago"
            },
            new Pelicula
            {
                Titulo          = "Interestelar 2",
                Emoji           = "🚀",
                RutaPoster      = "#0C1620",
                Calificacion    = 0,
                NumResenas      = 0,
                Generos         = ["Sci-Fi", "Drama", "A"],
                DuracionMinutos = 180,
                Clasificacion   = "A",
                FechaEstreno    = "15 Ago 2025",
                Director        = "Christopher Nolan",
                Reparto         = "Matthew McConaughey, Anne Hathaway, Fionn Whitehead",
                Sinopsis        = "Décadas después de los eventos de la primera entrega, una nueva tripulación debe atravesar el agujero de gusano para rescatar a Cooper y completar la misión.",
                Salas           = "Por confirmar",
                Audio           = "Dolby Atmos, IMAX",
                EsHoy           = false,
                FechaProxima    = "15 Ago"
            },
            new Pelicula
            {
                Titulo          = "Dune: Parte III",
                Emoji           = "🧙",
                RutaPoster      = "#201808",
                Calificacion    = 0,
                NumResenas      = 0,
                Generos         = ["Aventura", "Sci-Fi", "B15"],
                DuracionMinutos = 160,
                Clasificacion   = "B15",
                FechaEstreno    = "22 Ago 2025",
                Director        = "Denis Villeneuve",
                Reparto         = "Timothée Chalamet, Zendaya, Austin Butler",
                Sinopsis        = "Paul Atreides, convertido en el mesías Muad'Dib, enfrenta las consecuencias de su ascenso al poder mientras el universo se desmorona a su alrededor.",
                Salas           = "Por confirmar",
                Audio           = "Dolby Atmos, IMAX",
                EsHoy           = false,
                FechaProxima    = "22 Ago"
            },
        };

        // Búsqueda rápida por título
        public static Pelicula? GetByTitulo(string titulo)
            => Todas.Find(p => p.Titulo == titulo);
    }
}
