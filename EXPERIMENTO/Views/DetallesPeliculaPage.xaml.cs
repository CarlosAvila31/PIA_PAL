using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI;
using EXPERIMENTO.Data;
using EXPERIMENTO.Models;
using Microsoft.UI;

namespace EXPERIMENTO.Views
{
    public sealed partial class DetallesPeliculaPage : Page
    {
        public DetallesPeliculaPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int id = e.Parameter is int ? (int)e.Parameter : -1;
            var peli = PeliculasData.GetById(id);

            if (peli != null)
                CargarDatos(peli);
        }

        private void CargarDatos(Pelicula p)
        {
            // Poster
            PosterImage.Opacity = 0; // oculto hasta que cargue bien

            if (!string.IsNullOrEmpty(p.RutaPoster))
            {
                try
                {
                    PosterImage.Source = new BitmapImage(new Uri(p.RutaPoster));
                    PosterImage.ImageOpened += (s, e) => PosterImage.Opacity = 1;
                }
                catch
                {
                    // Ruta mal formada: el emoji queda visible
                }
            }

            // Título
            TituloText.Text = p.Titulo;

            // Chips de géneros
            GenerosPanel.Children.Clear();
            foreach (var genero in p.Generos)
            {
                var chip = new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 58, 58, 58)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(16),
                    Padding = new Thickness(12, 5, 12, 5),
                    Child = new TextBlock
                    {
                        Text = genero,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Colors.White)
                    }
                };
                GenerosPanel.Children.Add(chip);
            }

            // Metadata
            DuracionText.Text = $"{p.DuracionMinutos} min";
            ClasificacionText.Text = p.Clasificacion;
            EstrenoText.Text = p.FechaEstreno;

            // Equipo
            DirectorText.Text = p.Director;
            RepartoText.Text = p.Reparto;

            // Sinopsis
            SinopsisText.Text = p.Sinopsis;
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

        // ── Navegación ──
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            // Aquí navegaremos a HorarioPage cuando esté lista
        }

        // ── Handlers para imagen ──
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
