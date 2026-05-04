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

            foreach (var grupo in grupos)
                GruposPanel.Children.Add(CrearGrupo(grupo));
        }

        // ── Crea la sección de un día con sus botones de horario ─────────────
        private StackPanel CrearGrupo(GrupoFunciones grupo)
        {
            var panel = new StackPanel { Spacing = 12 };

            // Encabezado del día
            panel.Children.Add(new TextBlock
            {
                Text = grupo.FechaFormateada,
                FontSize = 14,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Colors.White)
            });

            // Fila de botones de horario
            var fila = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10
            };

            foreach (var funcion in grupo.Funciones)
                fila.Children.Add(CrearBotonFuncion(funcion));

            panel.Children.Add(fila);
            return panel;
        }

        // ── Crea un botón individual de función ──────────────────────────────
        private Button CrearBotonFuncion(Funcion funcion)
        {
            // Contenido del botón: hora arriba, sala + formato abajo
            var contenido = new StackPanel { Spacing = 4, Padding = new Thickness(16, 12, 16, 12) };

            contenido.Children.Add(new TextBlock
            {
                Text = funcion.HoraFormateada,
                FontSize = 18,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            contenido.Children.Add(new TextBlock
            {
                Text = funcion.Sala,
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 138, 138)),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            // Badge de formato (2D, 3D, IMAX, Dolby)
            var badge = new Border
            {
                Background = GetFormatoBrush(funcion.Formato),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Child = new TextBlock
                {
                    Text = funcion.Formato,
                    FontSize = 10,
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 13, 13, 13))
                }
            };
            contenido.Children.Add(badge);

            var btn = new Button
            {
                Content = contenido,
                Background = new SolidColorBrush(Color.FromArgb(255, 26, 26, 26)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 42, 42, 42)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(0),
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