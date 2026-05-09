using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Data
{
    // ── Modelo de usuario ────────────────────────────────────────────────────
    public class Usuario
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; // en producción usarías hash
        public List<BoletoGuardado> Boletos { get; set; } = new();
    }

    // ── Boleto que se guarda por usuario ─────────────────────────────────────
    public class BoletoGuardado
    {
        public string PeliculaTitulo { get; set; } = "";
        public string FechaHora { get; set; } = "";
        public string Sala { get; set; } = "";
        public string Formato { get; set; } = "";
        public List<string> Asientos { get; set; } = new();
        public double Total { get; set; }
        public DateTime FechaCompra { get; set; }
    }

    // ── Servicio singleton de sesión ─────────────────────────────────────────
    public static class SessionService
    {
        private static readonly string _rutaJson =
            Path.Combine(AppContext.BaseDirectory, "Data", "usuarios.json");

        private static List<Usuario> _usuarios = new();

        public static Usuario? UsuarioActual { get; private set; }
        public static bool EstaLogueado => UsuarioActual != null;

        // ── Cargar usuarios del JSON ─────────────────────────────────────────
        private static void Cargar()
        {
            if (!File.Exists(_rutaJson)) return;
            try
            {
                var json = File.ReadAllText(_rutaJson);
                _usuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new();
            }
            catch { _usuarios = new(); }
        }

        // ── Guardar usuarios al JSON ─────────────────────────────────────────
        private static void Guardar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaJson)!);
            var json = JsonSerializer.Serialize(_usuarios,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_rutaJson, json);
        }

        // ── Registrar nuevo usuario ──────────────────────────────────────────
        public static (bool ok, string mensaje) Registrar(string nombre, string email, string password)
        {
            Cargar();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return (false, "Completa todos los campos.");

            if (!email.Contains('@'))
                return (false, "Correo inválido.");

            if (_usuarios.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "Ese correo ya está registrado.");

            var nuevo = new Usuario { Nombre = nombre, Email = email, Password = password };
            _usuarios.Add(nuevo);
            Guardar();

            UsuarioActual = nuevo;
            return (true, "Registro exitoso.");
        }

        // ── Iniciar sesión ───────────────────────────────────────────────────
        public static (bool ok, string mensaje) Login(string email, string password)
        {
            Cargar();

            var usuario = _usuarios.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (usuario == null)
                return (false, "Correo o contraseña incorrectos.");

            UsuarioActual = usuario;
            return (true, "Bienvenido.");
        }

        // ── Cerrar sesión ────────────────────────────────────────────────────
        public static void Logout()
        {
            UsuarioActual = null;
        }

        // ── Guardar boleto comprado al usuario activo ────────────────────────
        public static void GuardarBoleto(BoletoGuardado boleto)
        {
            if (UsuarioActual == null) return;

            Cargar();

            // Re-obtener la referencia actualizada del archivo
            var usuarioEnLista = _usuarios.FirstOrDefault(u =>
                u.Email.Equals(UsuarioActual.Email, StringComparison.OrdinalIgnoreCase));

            if (usuarioEnLista == null) return;

            usuarioEnLista.Boletos.Add(boleto);
            UsuarioActual = usuarioEnLista; // sincroniza la referencia en memoria
            Guardar();
        }

        // ── Boletos del usuario actual ───────────────────────────────────────
        public static List<BoletoGuardado> GetBoletos()
        {
            if (UsuarioActual == null) return new();
            Cargar();
            var u = _usuarios.FirstOrDefault(u =>
                u.Email.Equals(UsuarioActual.Email, StringComparison.OrdinalIgnoreCase));
            return u?.Boletos ?? new();
        }
    }
}