using System;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI;
using EXPERIMENTO.Data;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Views
{
    public sealed partial class HorarioPage : Page
    {
        private List<GrupoFunciones> _gruposFunciones = new();
        private StackPanel _horariosPanel = new();
        public HorarioPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Recibe el Id de la película
            int peliculaId = e.Parameter is int i ? i : -1;
            var pelicula = PeliculasData.GetById(peliculaId);

            if (pelicula == null) return;

            SubtituloText.Text = pelicula.Titulo;

            var grupos = FuncionesData.GetFuncionesPorPelicula(peliculaId, diasAdelante: 7);

            if (grupos.Count == 0)
            {
                SinFuncionesPanel.Visibility = Visibility.Visible;
                return;
            }

            _gruposFunciones = grupos;

            // PANEL FECHAS
            var fechasPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 12
            };

            // PANEL HORARIOS
            _horariosPanel = new StackPanel
            {
                Spacing = 28,
                Margin = new Thickness(0, 24, 0, 0)
            };

            foreach (var grupo in grupos)
            {
                fechasPanel.Children.Add(CrearBotonFecha(grupo));
            }

            // Agrega primero las fechas
            GruposPanel.Children.Add(fechasPanel);

            // Luego horarios
            GruposPanel.Children.Add(_horariosPanel);

            // Mostrar automáticamente el primero
            MostrarHorarios(grupos[0]);
        }

        // ── Crea la sección de un día con sus botones de horario ─────────────
        private StackPanel CrearGrupo(GrupoFunciones grupo)
        {
            var panel = new StackPanel
            {
                Spacing = 18
            };

            // Título
            panel.Children.Add(new TextBlock
            {
                Text = grupo.FechaFormateada,
                FontSize = 20,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            });

            // Wrap visual
            var fila = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 14
            };

            foreach (var funcion in grupo.Funciones)
            {
                fila.Children.Add(CrearBotonFuncion(funcion));
            }

            panel.Children.Add(fila);

            return panel;
        }

        private Button CrearBotonFecha(GrupoFunciones grupo)
        {
            var btn = new Button
            {
                Content = new StackPanel
                {
                    Spacing = 2,
                    Children =
            {
                new TextBlock
                {
                    Text = grupo.Fecha.ToString("ddd").ToUpper(),
                    FontSize = 11,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.Black)
                },

                new TextBlock
                {
                    Text = grupo.Fecha.ToString("dd MMM").ToUpper(),
                    FontSize = 13,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.Black)
                }
            }
                },

                Background = new SolidColorBrush(Color.FromArgb(255, 245, 200, 66)),
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(18, 10, 18, 10),
                Tag = grupo
            };

            btn.Click += BtnFecha_Click;

            return btn;
        }

        private void BtnFecha_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn &&
                btn.Tag is GrupoFunciones grupo)
            {
                MostrarHorarios(grupo);
            }
        }

        private void MostrarHorarios(GrupoFunciones grupo)
        {
            _horariosPanel.Children.Clear();

            _horariosPanel.Children.Add(CrearGrupo(grupo));
        }

        // ── Crea un botón individual de función ──────────────────────────────
        private Button CrearBotonFuncion(Funcion funcion)
        {
            var contenido = new StackPanel
            {
                Spacing = 6,
                Padding = new Thickness(18, 14, 18, 14)
            };

            contenido.Children.Add(new TextBlock
            {
                Text = funcion.HoraFormateada,
                FontSize = 20,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            contenido.Children.Add(new Border
            {
                Background = GetFormatoBrush(funcion.Formato),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(8, 3, 8, 3),
                HorizontalAlignment = HorizontalAlignment.Center,

                Child = new TextBlock
                {
                    Text = funcion.Formato,
                    FontSize = 10,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                }
            });

            contenido.Children.Add(new TextBlock
            {
                Text = funcion.Sala,
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 160, 160, 160)),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            var btn = new Button
            {
                Content = contenido,
                Background = new SolidColorBrush(Color.FromArgb(255, 26, 26, 26)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(0),
                MinWidth = 130,
                MinHeight = 100,
                Tag = funcion
            };

            btn.Click += BtnFuncion_Click;

            return btn;
        }

        // ── Color del badge según el formato ────────────────────────────────
        private static SolidColorBrush GetFormatoBrush(string formato) => formato switch
        {
            "IMAX" => new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),   // azul
            "Dolby" => new SolidColorBrush(Color.FromArgb(255, 180, 30, 30)),   // rojo
            "3D" => new SolidColorBrush(Color.FromArgb(255, 30, 150, 80)),   // verde
            _ => new SolidColorBrush(Color.FromArgb(255, 80, 80, 80))     // gris (2D)
        };

        // ── Al seleccionar una función navega a AsientosPage ─────────────────
        private void BtnFuncion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Funcion funcion)
                Frame.Navigate(typeof(AsientosPage), funcion);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}