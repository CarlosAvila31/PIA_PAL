using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EXPERIMENTO.Data;
using System;
using System.IO;
using System.Linq;

namespace EXPERIMENTO.Views
{
    public sealed partial class CarteleraPage : Page
    {
        public CarteleraPage()
        {
            InitializeComponent();

            // Crear el archivo JSON si no existe
            string ruta = Path.Combine(AppContext.BaseDirectory, "Data", "peliculas.json");
            if (!File.Exists(ruta))
            {
                PeliculasData.GuardarPeliculas();
            }

            // Ahora sí cargar películas
            var peliculas = PeliculasData.Todas;
            var hoy = DateTime.Today;

            ItemsEnCartelera.ItemsSource = peliculas
                .Where(p => DateTime.TryParse(p.FechaEstreno, out var fecha) && fecha <= hoy)
                .ToList();

            ItemsProximamente.ItemsSource = peliculas
                .Where(p => DateTime.TryParse(p.FechaEstreno, out var fecha) && fecha > hoy)
                .ToList();
        }

        private void MovieCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                // Navegar a la página de detalles pasando el Id
                Frame.Navigate(typeof(DetallesPeliculaPage), id);
            }
        }
    }
}
