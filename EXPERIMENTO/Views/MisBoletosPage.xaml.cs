using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;
using EXPERIMENTO.Data;

namespace EXPERIMENTO.Views
{
    public sealed partial class MisBoletosPage : Page
    {
        public MisBoletosPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CargarBoletos();
        }

        private void CargarBoletos()
        {
            // Ocultar todo
            PanelSinSesion.Visibility = Visibility.Collapsed;
            PanelSinBoletos.Visibility = Visibility.Collapsed;
            PanelBoletos.Visibility = Visibility.Collapsed;

            if (!SessionService.EstaLogueado)
            {
                PanelSinSesion.Visibility = Visibility.Visible;
                return;
            }

            var boletos = SessionService.GetBoletos();

            if (boletos.Count == 0)
            {
                PanelSinBoletos.Visibility = Visibility.Visible;
                return;
            }

            // Hay boletos: mostrar lista
            SubtituloText.Text = $"{SessionService.UsuarioActual!.Nombre} · {boletos.Count} compra(s)";

            ListaBoletos.Children.Clear();

            foreach (var boleto in boletos)
            {
                ListaBoletos.Children.Add(CrearCardBoleto(boleto));
            }

            PanelBoletos.Visibility = Visibility.Visible;
        }

        // ── Construye la card visual de un boleto ────────────────────────────
        private static Border CrearCardBoleto(BoletoGuardado boleto)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 42, 42, 42)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(20, 18, 20, 18)
            };

            var contenido = new StackPanel { Spacing = 0 };

            // Título de la película
            contenido.Children.Add(new TextBlock
            {
                Text = boleto.PeliculaTitulo,
                FontSize = 17,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                Margin = new Thickness(0, 0, 0, 4)
            });

            // Badge de formato
            var badgePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 14)
            };

            badgePanel.Children.Add(new Border
            {
                Background = GetFormatoBrush(boleto.Formato),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(8, 3, 8, 3),
                Child = new TextBlock
                {
                    Text = boleto.Formato,
                    FontSize = 10,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.White)
                }
            });

            contenido.Children.Add(badgePanel);

            // Separador
            contenido.Children.Add(new Rectangle
            {
                Height = 1,
                Fill = new SolidColorBrush(Color.FromArgb(255, 42, 42, 42)),
                Margin = new Thickness(0, 0, 0, 14)
            });

            // Info: sala, fecha/hora, asientos
            contenido.Children.Add(InfoFila("🎬 Sala", boleto.Sala));
            contenido.Children.Add(InfoFila("📅 Función", boleto.FechaHora));
            contenido.Children.Add(InfoFila("💺 Asientos", string.Join(", ", boleto.Asientos)));

            // Separador
            contenido.Children.Add(new Rectangle
            {
                Height = 1,
                Fill = new SolidColorBrush(Color.FromArgb(255, 42, 42, 42)),
                Margin = new Thickness(0, 14, 0, 14)
            });

            // Total
            var filaTotal = new Grid();
            filaTotal.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            filaTotal.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var lblTotal = new TextBlock
            {
                Text = "Total pagado",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 138, 138))
            };

            var valorTotal = new TextBlock
            {
                Text = $"${boleto.Total:N2}",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 245, 200, 66))
            };
            Grid.SetColumn(valorTotal, 1);

            filaTotal.Children.Add(lblTotal);
            filaTotal.Children.Add(valorTotal);
            contenido.Children.Add(filaTotal);

            // Fecha de compra (pequeña)
            contenido.Children.Add(new TextBlock
            {
                Text = $"Comprado el {boleto.FechaCompra:dd/MM/yyyy}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 58, 58, 58)),
                Margin = new Thickness(0, 6, 0, 0)
            });

            // Botón Ver boleto
            var btnVerPdf = new Button
            {
                Content = "🎟  Ver boleto en PDF",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 58, 58, 58)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(0, 10, 0, 10),
                Margin = new Thickness(0, 14, 0, 0)
            };

            btnVerPdf.Click += async (s, args) =>
            {
                btnVerPdf.IsEnabled = false;
                btnVerPdf.Content = "Generando...";

                string? path = await TicketGenerator.GenerarTicketAsync(boleto);
                if (path != null)
                    TicketGenerator.AbrirPdf(path);

                btnVerPdf.Content = "🎟  Ver boleto en PDF";
                btnVerPdf.IsEnabled = true;
            };

            contenido.Children.Add(btnVerPdf);

            card.Child = contenido;
            return card;
        }

        // ── Fila etiqueta + valor ─────────────────────────────────────────────
        private static Grid InfoFila(string etiqueta, string valor)
        {
            var grid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new TextBlock
            {
                Text = etiqueta,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90))
            });

            var val = new TextBlock
            {
                Text = valor,
                FontSize = 13,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                TextWrapping = TextWrapping.Wrap
            };
            Grid.SetColumn(val, 1);
            grid.Children.Add(val);

            return grid;
        }

        // ── Color del badge de formato ────────────────────────────────────────
        private static SolidColorBrush GetFormatoBrush(string formato) => formato switch
        {
            "IMAX" => new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),
            "Dolby" => new SolidColorBrush(Color.FromArgb(255, 180, 30, 30)),
            "3D" => new SolidColorBrush(Color.FromArgb(255, 30, 150, 80)),
            _ => new SolidColorBrush(Color.FromArgb(255, 80, 80, 80))
        };
    }
}