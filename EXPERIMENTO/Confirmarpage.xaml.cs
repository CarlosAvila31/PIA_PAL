using EXPERIMENTO.Data;
using EXPERIMENTO.Models;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EXPERIMENTO.Views
{
    public sealed partial class ConfirmarPage : Page
    {
        private DatosCompra? _datos;
        public ConfirmarPage() => InitializeComponent();


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is not DatosCompra datos) return;
            _datos = datos;

            // Mostrar info
            FuncionText.Text =
                $"{datos.Funcion.Sala} | {datos.Funcion.FechaHora:dddd d 'de' MMMM} | {datos.Funcion.HoraFormateada} | {datos.Funcion.Formato}";

            AsientosText.Text = string.Join(", ", datos.AsientosElegidos.OrderBy(a => a));
            TotalText.Text = $"${datos.Total:N2}";
        }

        // ── Confirmar compra: aquí sí se guardan los ocupados ────────────────
        private void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (_datos == null) return;

            // Marca los asientos como vendidos y persiste en JSON
            SalasData.MarcarVendidos(_datos.Funcion, _datos.AsientosElegidos);

            {
                Frame.Navigate(typeof(CarteleraPage));
            };
        }




        // ── Volver: solo navega atrás, NO guarda nada ───────────────────────
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}