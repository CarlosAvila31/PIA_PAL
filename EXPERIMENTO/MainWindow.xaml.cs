using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using EXPERIMENTO.Views;

namespace EXPERIMENTO
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Navegar a Cartelera al iniciar
            ContentFrame.Navigate(typeof(CarteleraPage));
        }

        // ─── Helpers para manejar el estado "activo" del sidebar ───

        private void SetAllInactive()
        {
            var inactive = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));   // Transparent
            var inactiveFg = new SolidColorBrush(Color.FromArgb(255, 138, 138, 138)); // #8A8A8A

            BtnCartelera.Background = inactive;
            BtnMisBoletos.Background = inactive;
            BtnMiCuenta.Background = inactive;
            BtnAjustes.Background = inactive;

            SetButtonForeground(BtnCartelera, inactiveFg);
            SetButtonForeground(BtnMisBoletos, inactiveFg);
            SetButtonForeground(BtnMiCuenta, inactiveFg);
            SetButtonForeground(BtnAjustes, inactiveFg);
        }

        private void SetActive(Button btn)
        {
            btn.Background = new SolidColorBrush(Color.FromArgb(255, 42, 34, 0));   // #2A2200
            SetButtonForeground(btn, new SolidColorBrush(Color.FromArgb(255, 245, 200, 66))); // #F5C842
        }

        private void SetButtonForeground(Button btn, Brush brush)
        {
            btn.Foreground = brush;
        }

        // ─── Handlers de navegación ───────────────────────────────

        private void BtnCartelera_Click(object sender, RoutedEventArgs e)
        {
            SetAllInactive();
            SetActive(BtnCartelera);
            ContentFrame.Navigate(typeof(CarteleraPage));
        }

        private void BtnMisBoletos_Click(object sender, RoutedEventArgs e)
        {
            SetAllInactive();
            SetActive(BtnMisBoletos);
            ContentFrame.Navigate(typeof(MisBoletosPage));
        }

        private void BtnMiCuenta_Click(object sender, RoutedEventArgs e)
        {
            SetAllInactive();
            SetActive(BtnMiCuenta);
            ContentFrame.Navigate(typeof(MiCuentaPage));
        }

        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            SetAllInactive();
            SetActive(BtnAjustes);
            ContentFrame.Navigate(typeof(AjustesPage));
        }
    }
}