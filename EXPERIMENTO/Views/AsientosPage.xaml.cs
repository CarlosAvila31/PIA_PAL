using EXPERIMENTO.Data;
using EXPERIMENTO.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;


namespace EXPERIMENTO.Views
{
    public sealed partial class AsientosPage : Page
    {
        private const double PrecioPorAsiento = 80.0;

        private Funcion? _funcion;
        private readonly List<string> _seleccionados = new();
        private readonly Dictionary<string, Button> _botonesAsiento = new();

        public AsientosPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is not Funcion funcion) return;
            _funcion = funcion;

            InfoFuncionText.Text =
                $"{funcion.Sala}  |  {funcion.FechaHora:dddd d 'de' MMMM}  |  {funcion.HoraFormateada}  |  {funcion.Formato}";

            var layout = SalasData.GetLayout(funcion.Sala);
            if (layout == null) return;

            var ocupados = SalasData.GetOcupados(funcion);
            GenerarMapa(layout, ocupados);
        }

        // ── Genera el mapa visual fila por fila ──────────────────────────────
        private void GenerarMapa(SalaLayout layout, List<string> ocupados)
        {
            MapaAsientos.Children.Clear();
            _botonesAsiento.Clear();

            for (int f = 0; f < layout.NumFilas; f++)
            {
                char letraFila = (char)('A' + f);
                var fila = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 5
                };

                // Etiqueta de fila
                fila.Children.Add(new TextBlock
                {
                    Text = letraFila.ToString(),
                    Width = 20,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                for (int n = 1; n <= layout.AsientosPorFila; n++)
                {
                    string codigo = $"{letraFila}{n}";
                    bool estaOcupado = ocupados.Contains(codigo);

                    var btn = CrearBotonAsiento(codigo, estaOcupado);
                    _botonesAsiento[codigo] = btn;
                    fila.Children.Add(btn);
                }

                // Etiqueta de fila al final también
                fila.Children.Add(new TextBlock
                {
                    Text = letraFila.ToString(),
                    Width = 20,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                MapaAsientos.Children.Add(fila);
            }

            // Números de columna abajo
            var numeracion = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                Margin = new Thickness(0, 6, 0, 0)
            };
            numeracion.Children.Add(new TextBlock { Width = 20 }); // espacio para etiqueta fila
            for (int n = 1; n <= layout.AsientosPorFila; n++)
            {
                numeracion.Children.Add(new TextBlock
                {
                    Text = n.ToString(),
                    Width = 32,
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = Microsoft.UI.Xaml.TextAlignment.Center
                });
            }
            MapaAsientos.Children.Add(numeracion);
        }

        // ── Crea un botón de asiento con su estado visual ────────────────────
        private Button CrearBotonAsiento(string codigo, bool ocupado)
        {
            var btn = new Button
            {
                Width = 32,
                Height = 32,
                Padding = new Thickness(0),
                CornerRadius = new CornerRadius(4),
                Tag = codigo,
                Content = new TextBlock
                {
                    Text = "",   // sin texto, el color comunica el estado
                    FontSize = 9
                }
            };

            // Aplica estilo según estado
            AplicarEstilo(btn, ocupado ? EstadoAsiento.Ocupado : EstadoAsiento.Disponible);

            if (ocupado)
            {
                // Mantén el botón habilitado para que se vea rojo
                btn.IsEnabled = true;
                // Pero quita el evento de click para que no se pueda seleccionar
                btn.Click -= Asiento_Click;
            }
            else
            {
                btn.IsEnabled = true;
                btn.Click += Asiento_Click;
            }

            return btn;
        }


        // ── Click en un asiento ──────────────────────────────────────────────
        private void Asiento_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not string codigo) return;

            if (_seleccionados.Contains(codigo))
            {
                _seleccionados.Remove(codigo);
                AplicarEstilo(btn, EstadoAsiento.Disponible);
            }
            else
            {
                _seleccionados.Add(codigo);
                AplicarEstilo(btn, EstadoAsiento.Seleccionado);
            }

            ActualizarResumen();
        }

        // ── Aplica colores según el estado ───────────────────────────────────
        private static void AplicarEstilo(Button btn, EstadoAsiento estado)
        {
            btn.Background = estado switch
            {
                EstadoAsiento.Disponible => new SolidColorBrush(Color.FromArgb(255, 42, 42, 42)),
                EstadoAsiento.Seleccionado => new SolidColorBrush(Color.FromArgb(255, 245, 200, 66)),
                EstadoAsiento.Ocupado => new SolidColorBrush(Color.FromArgb(255, 200, 50, 50)), // rojo
                _ => new SolidColorBrush(Color.FromArgb(255, 42, 42, 42))
            };

            btn.BorderBrush = estado switch
            {
                EstadoAsiento.Disponible => new SolidColorBrush(Color.FromArgb(255, 58, 58, 58)),
                EstadoAsiento.Seleccionado => new SolidColorBrush(Color.FromArgb(255, 245, 200, 66)),
                EstadoAsiento.Ocupado => new SolidColorBrush(Color.FromArgb(255, 200, 50, 50)),
                _ => new SolidColorBrush(Color.FromArgb(255, 58, 58, 58))
            };

            btn.BorderThickness = new Thickness(1);
        }


        // ── Actualiza el panel lateral ───────────────────────────────────────
        private void ActualizarResumen()
        {
            int cantidad = _seleccionados.Count;
            double total = cantidad * PrecioPorAsiento;

            AsientosSeleccionadosText.Text = cantidad == 0
                ? "Ninguno"
                : string.Join(", ", _seleccionados.OrderBy(s => s));

            CantidadText.Text = cantidad.ToString();
            TotalText.Text = $"${total:N2}";
            BtnConfirmar.IsEnabled = cantidad > 0;
        }

        // ── Confirmar compra: persiste los asientos y navega ─────────────────
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (_funcion == null || _seleccionados.Count == 0) return;

            // Navegar a ConfirmarPage pasando los datos de la compra
            Frame.Navigate(typeof(ConfirmarPage), new DatosCompra
            {
                Funcion = _funcion,
                AsientosElegidos = new List<string>(_seleccionados),
                Total = _seleccionados.Count * PrecioPorAsiento
            });
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }

    // ── Objeto que se pasa a ConfirmarPage ───────────────────────────────────
    public class DatosCompra
    {
        public Funcion Funcion { get; set; } = null!;
        public List<string> AsientosElegidos { get; set; } = new();
        public double Total { get; set; }
    }
}

