using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using EXPERIMENTO.Data;

namespace EXPERIMENTO.Views
{
    public sealed partial class MiCuentaPage : Page
    {
        private bool _modoRegistro = false;

        public MiCuentaPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ActualizarVista();
        }

        // ── Decide qué panel mostrar ─────────────────────────────────────────
        private void ActualizarVista()
        {
            if (SessionService.EstaLogueado)
            {
                var usuario = SessionService.UsuarioActual!;
                var boletos = SessionService.GetBoletos();

                // Avatar: primera letra del nombre
                AvatarText.Text = usuario.Nombre.Length > 0
                    ? usuario.Nombre[0].ToString().ToUpper()
                    : "?";

                NombrePerfilText.Text = usuario.Nombre;
                EmailPerfilText.Text = usuario.Email;

                int totalBoletos = 0;
                double totalGastado = 0;
                foreach (var b in boletos)
                {
                    totalBoletos += b.Asientos.Count;
                    totalGastado += b.Total;
                }

                TotalBoletosText.Text = totalBoletos.ToString();
                TotalGastadoText.Text = $"${totalGastado:N0}";

                PanelAuth.Visibility = Visibility.Collapsed;
                PanelPerfil.Visibility = Visibility.Visible;
            }
            else
            {
                PanelAuth.Visibility = Visibility.Visible;
                PanelPerfil.Visibility = Visibility.Collapsed;
                LimpiarCampos();
            }
        }

        // ── Botón principal: Login o Registro ───────────────────────────────
        private void BtnPrincipal_Click(object sender, RoutedEventArgs e)
        {
            OcultarMensaje();

            if (_modoRegistro)
            {
                var (ok, msg) = SessionService.Registrar(
                    NombreBox.Text.Trim(),
                    EmailBox.Text.Trim(),
                    PasswordBox.Password);

                if (ok)
                    ActualizarVista();
                else
                    MostrarError(msg);
            }
            else
            {
                var (ok, msg) = SessionService.Login(
                    EmailBox.Text.Trim(),
                    PasswordBox.Password);

                if (ok)
                    ActualizarVista();
                else
                    MostrarError(msg);
            }
        }

        // ── Cambiar entre Login y Registro ───────────────────────────────────
        private void BtnCambiarModo_Click(object sender, RoutedEventArgs e)
        {
            _modoRegistro = !_modoRegistro;
            OcultarMensaje();

            if (_modoRegistro)
            {
                TituloAuthText.Text = "Crear cuenta";
                SubtituloAuthText.Text = "Regístrate para guardar tus boletos";
                BtnPrincipal.Content = "Crear cuenta";
                BtnCambiarModoText.Text = "¿Ya tienes cuenta? Inicia sesión";
                CampoNombrePanel.Visibility = Visibility.Visible;
            }
            else
            {
                TituloAuthText.Text = "Iniciar sesión";
                SubtituloAuthText.Text = "Accede a tu cuenta para ver tus boletos";
                BtnPrincipal.Content = "Iniciar sesión";
                BtnCambiarModoText.Text = "¿No tienes cuenta? Regístrate";
                CampoNombrePanel.Visibility = Visibility.Collapsed;
            }
        }

        // ── Cerrar sesión ────────────────────────────────────────────────────
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionService.Logout();
            _modoRegistro = false;
            ActualizarVista();
        }

        // ── Helpers UI ───────────────────────────────────────────────────────
        private void MostrarError(string mensaje)
        {
            MensajeText.Text = mensaje;
            MensajeText.Visibility = Visibility.Visible;
        }

        private void OcultarMensaje()
        {
            MensajeText.Visibility = Visibility.Collapsed;
        }

        private void LimpiarCampos()
        {
            NombreBox.Text = "";
            EmailBox.Text = "";
            PasswordBox.Password = "";
        }
    }
}