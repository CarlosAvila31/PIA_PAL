using System;
using System.Text;
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

        private Pelicula? _peliculaActual;
        private void CargarDatos(Pelicula p)
        {
            _peliculaActual = p;

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
                    
                }
            }

            // Título
            TituloText.Text = p.Titulo;

            //Estrellas
            if (p.Calificacion > 0)
            {
                EstrellasText.Text = GenerarEstrellas(p.Calificacion);
                CalificacionText.Text = $"{p.Calificacion:0.0} ";
            }
            else
            {
                EstrellasText.Text = "Sin calificación aún";
                CalificacionText.Text = "";
            }

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

            // ── Botón comprar ────────────────────────────────────────
            if (p.EsHoy)
            {
                BtnComprar.IsEnabled = true;
                BtnComprar.Content = "Comprar boletos";
                BtnComprar.Background = new SolidColorBrush(Color.FromArgb(255, 245, 200, 66));
                BtnComprar.Foreground = new SolidColorBrush(Color.FromArgb(255, 13, 13, 13));
            }
            else
            {
                BtnComprar.IsEnabled = false;
                BtnComprar.Content = "Próximamente";
                BtnComprar.Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                BtnComprar.Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90));
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

            // Trailer
            if (!string.IsNullOrEmpty(p.TrailerUrl))
            {
                BtnTrailer.IsEnabled = true;
                BtnTrailer.Opacity = 1;
            }
            else
            {
                BtnTrailer.IsEnabled = false;
                BtnTrailer.Opacity = 0.6;
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

        // ── Genera ★ sobre 5 estrellas a partir de calificación sobre 10 ──
        private static string GenerarEstrellas(double calificacion)
        {
            double sobre5 = calificacion / 2.0;
            var sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                if (sobre5 >= i) sb.Append('★');
                else if (sobre5 >= i - 0.5) sb.Append('½');
                else sb.Append('☆');
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
            if (_peliculaActual != null)
                Frame.Navigate(typeof(HorarioPage), _peliculaActual.Id);
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

        // ── Extrae el embed URL de YouTube ──
        private static string? GetEmbedUrl(string trailerUrl)
        {
            if (string.IsNullOrEmpty(trailerUrl)) return null;
            try
            {
                var uri = new Uri(trailerUrl);

                // youtube.com/watch?v=XXXX
                if (uri.Host.Contains("youtube.com"))
                {
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var id = query["v"];
                    return id != null ? $"https://www.youtube.com/watch?v={id}" : null;
                }

                // youtu.be/XXXX
                if (uri.Host.Contains("youtu.be"))
                {
                    var id = uri.AbsolutePath.TrimStart('/').Split('?')[0];
                    return $"https://www.youtube.com/watch?v={id}";
                }
            }
            catch { }
            return null;
        }

        // ── Abre el trailer en un dialog con WebView2 ──
        private async void BtnTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (_peliculaActual == null) return;

            var embedUrl = GetEmbedUrl(_peliculaActual.TrailerUrl);
            if (embedUrl == null) return;

            // ── Overlay oscuro de fondo ──
            var overlay = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // ── WebView2 ──
            var webView = new Microsoft.UI.Xaml.Controls.WebView2
            {
                Width = 1100,
                Height = 619  // ratio 16:9
            };

            // ── Botón cerrar ──
            var btnCerrar = new Button
            {
                Content = "✕  Cerrar",
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16, 8, 16, 8),
                Margin = new Thickness(0, 0, 0, 10)
            };

            // ── Contenedor centrado ──
            var contenedor = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 0
            };
            contenedor.Children.Add(btnCerrar);
            contenedor.Children.Add(new Border
            {
                CornerRadius = new CornerRadius(12),
                Child = webView
            });

            // ── Grid que apila overlay + contenedor ──
            var grid = new Grid();
            grid.Children.Add(overlay);
            grid.Children.Add(contenedor);

            // ── Popup fullscreen ──
            var popup = new Microsoft.UI.Xaml.Controls.Primitives.Popup
            {
                Child = grid,
                IsOpen = false
            };

            // Tamaño del popup = tamaño de la ventana
            grid.Width = this.ActualWidth;
            grid.Height = this.ActualHeight;

            // Cerrar al hacer click en overlay o botón
            btnCerrar.Click += (s, args) =>
            {
                webView.CoreWebView2?.Navigate("about:blank");
                popup.IsOpen = false;
            };
            overlay.Tapped += (s, args) =>
            {
                webView.CoreWebView2?.Navigate("about:blank");
                popup.IsOpen = false;
            };

            // Agregar al árbol visual y abrir
            (this.Content as Grid)?.Children.Add(popup);
            popup.IsOpen = true;

            // Inicializar WebView
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.Settings.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/124.0.0.0 Safari/537.36";
            webView.CoreWebView2.Navigate(embedUrl);
        }
    }
}
