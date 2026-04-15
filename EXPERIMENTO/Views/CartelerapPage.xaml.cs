using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace EXPERIMENTO.Views
{
    public sealed partial class CartelerapPage : Page
    {
        public CartelerapPage()
        {
            InitializeComponent();
        }

        private void MovieCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string titulo)
            {
                // Navegar a la página de detalles pasando el título
                Frame.Navigate(typeof(DetallesPeliculaPage), titulo);
            }
        }
    }
}
