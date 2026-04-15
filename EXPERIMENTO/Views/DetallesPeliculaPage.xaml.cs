using System;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI;
using EXPERIMENTO.Data;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Views
{
    public sealed partial class DetallesPeliculaPage : Page
    {
        public DetallesPeliculaPage()
        {
            InitializeComponent();
        }

        // ── Recibe el título como parámetro, busca el objeto Pelicula y llena la UI ──
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string titulo = e.Parameter as string ?? "";
            Pelicula? peli = PeliculasData.GetByTitulo(titulo);

            if (peli != null)
                CargarDatos(peli);
        }

        private void CargarDatos(Pelicula p)
        {
            // Poster
            PosterEmoji.Text = p.Emoji;
            PosterImage.Opacity = 0; // oculto hasta que cargue bien

            if (!string.IsNullOrEmpty(p.RutaPoster))
            {
                try
                {
                    PosterImage.Source = new BitmapImage(new Uri(p.RutaPoster));
                    // OnImageOpened lo hace visible; OnImageFailed deja el emoji
                    PosterImage.ImageOpened += (s, e) => PosterImage.Opacity = 1;
                }
                catch
                {
                    // Ruta mal formada: el emoji queda visible
                }
            }

            // Título
            TituloText.Text = p.Titulo;

            // Estrellas y calificación
            if (p.Calificacion > 0)
            {
                EstrellasText.Text  = GenerarEstrellas(p.Calificacion);
                CalificacionText.Text = $"{p.Calificacion:0.0}  ·  {p.NumResenas:N0} reseñas";
            }
            else
            {
                EstrellasText.Text    = "Sin calificación aún";
                CalificacionText.Text = "";
            }

            // Chips de géneros
            GenerosPanel.Children.Clear();
            foreach (var genero in p.Generos)
            {
                var chip = new Border
                {
                    Background    = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    BorderBrush   = new SolidColorBrush(Color.FromArgb(255, 58, 58, 58)),
                    BorderThickness = new Thickness(1),
                    CornerRadius  = new CornerRadius(16),
                    Padding       = new Thickness(12, 5, 12, 5),
                    Child         = new TextBlock
                    {
                        Text       = genero,
                        FontSize   = 12,
                        Foreground = new SolidColorBrush(Colors.White)
                    }
                };
                GenerosPanel.Children.Add(chip);
            }

            // Metadata
            DuracionText.Text      = $"{p.DuracionMinutos} min";
            ClasificacionText.Text = p.Clasificacion;
            EstrenoText.Text       = p.FechaEstreno;

            // Equipo
            DirectorText.Text = p.Director;
            RepartoText.Text  = p.Reparto;

            // Sinopsis
            SinopsisText.Text = p.Sinopsis;

            // Salas y audio
            SalasText.Text = p.Salas;
            AudioText.Text = p.Audio;

            // Deshabilitar "Comprar" si no está en cartelera todavía
            if (!p.EsHoy)
            {
                BtnComprar.IsEnabled = false;
                BtnComprar.Content   = $"Disponible el {p.FechaProxima}";
            }
        }

        // ── Convierte "#RRGGBB" a Windows.UI.Color ──
        private static Color ParseColor(string hex)
        {
            hex = hex.TrimStart('#');
            return Color.FromArgb(
                255,
                Convert.ToByte(hex[0..2], 16),
                Convert.ToByte(hex[2..4], 16),
                Convert.ToByte(hex[4..6], 16)
            );
        }

        // ── Genera cadena de estrellas ★ / ½ / ☆ para una calificación sobre 10 ──
        private static string GenerarEstrellas(double calificacion)
        {
            // Convierte a escala de 5 estrellas
            double sobre5 = calificacion / 2.0;
            var sb = new System.Text.StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                if (sobre5 >= i)
                    sb.Append('★');
                else if (sobre5 >= i - 0.5)
                    sb.Append('½');
                else
                    sb.Append('☆');
            }
            return sb.ToString();
        }

        // ── Navegación ──
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            // Aquí navegaremos a HorarioPage cuando esté lista
        }

        // ── Handlers añadidos para coincidir con DetallesPeliculaPage.g.cs (XAML) ──
        private void PosterImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            PosterImage.Opacity = 1;
            PosterEmoji.Visibility = Visibility.Collapsed;
        }

        private void PosterImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            PosterImage.Opacity = 0;
            PosterEmoji.Visibility = Visibility.Visible;
        }
    }
}
