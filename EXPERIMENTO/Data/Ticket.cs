using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using EXPERIMENTO.Models;

namespace EXPERIMENTO.Data
{
    public static class TicketGenerator
    {
        // Ruta donde está el script Python (junto al exe)
        private static string ScriptPath =>
            Path.Combine(AppContext.BaseDirectory, "Scripts", "generar_ticket.py");

        // Carpeta donde se guardan los PDFs de tickets
        private static string TicketsFolder =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CineLobo", "Tickets");

        public static async Task<string?> GenerarTicketAsync(BoletoGuardado boleto)
        {
            try
            {
                Directory.CreateDirectory(TicketsFolder);

                // Nombre único del archivo
                string folio = $"CNL-{DateTime.Now:yyyyMMdd}-{boleto.GetHashCode() & 0xFFFF:X4}";
                string outputPath = Path.Combine(TicketsFolder, $"{folio}.pdf");

                string asientos = string.Join(", ", boleto.Asientos);

                var psi = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = $"\"{ScriptPath}\" \"{outputPath}\" " +
                                $"\"{Esc(boleto.PeliculaTitulo)}\" " +
                                $"\"{Esc(boleto.Sala)}\" " +
                                $"\"{Esc(boleto.FechaHora)}\" " +
                                $"\"{Esc(asientos)}\" " +
                                $"\"{Esc(boleto.Formato)}\" " +
                                $"\"{boleto.Total:N2}\" " +
                                $"\"{folio}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi)!;
                string stdout = await proc.StandardOutput.ReadToEndAsync();
                await proc.WaitForExitAsync();

                if (proc.ExitCode == 0 && stdout.StartsWith("OK:"))
                    return outputPath;

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static void AbrirPdf(string path)
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        private static string Esc(string s) => s.Replace("\"", "'");
    }
}