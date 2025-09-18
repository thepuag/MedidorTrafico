using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace MedidorTrafico
{
    public partial class MainForm : Form
    {
        private NetworkInterface interfazSeleccionada;
        private long bytesRecibidosPrevios;
        private long bytesEnviadosPrevios;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener todas las interfaces de red activas
                var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                 ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .ToList();

                // Agregar las interfaces al combo
                comboInterfaces.Items.Clear();
                foreach (var interfaz in interfaces)
                {
                    comboInterfaces.Items.Add(interfaz.Name);
                }

                if (comboInterfaces.Items.Count > 0)
                {
                    comboInterfaces.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No se encontraron interfaces de red activas.",
                                  "Información",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar interfaces: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void comboInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboInterfaces.SelectedItem == null) return;

                var nombreSeleccionado = comboInterfaces.SelectedItem.ToString();
                interfazSeleccionada = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(i => i.Name == nombreSeleccionado);

                if (interfazSeleccionada != null)
                {
                    // Obtener estadísticas iniciales
                    var estadisticas = interfazSeleccionada.GetIPv4Statistics();
                    bytesRecibidosPrevios = estadisticas.BytesReceived;
                    bytesEnviadosPrevios = estadisticas.BytesSent;

                    // Iniciar el timer
                    timer.Stop();
                    timer.Start();

                    // Actualizar labels inmediatamente
                    labelDownload.Text = "Descarga: 0.00 MB/s";
                    labelUpload.Text = "Subida: 0.00 MB/s";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar interfaz: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (interfazSeleccionada == null) return;

                // Verificar si la interfaz sigue activa
                if (interfazSeleccionada.OperationalStatus != OperationalStatus.Up)
                {
                    timer.Stop();
                    labelDownload.Text = "Interfaz desconectada";
                    labelUpload.Text = "Interfaz desconectada";
                    return;
                }

                // Obtener estadísticas actuales
                var estadisticas = interfazSeleccionada.GetIPv4Statistics();
                long bytesRecibidosActuales = estadisticas.BytesReceived;
                long bytesEnviadosActuales = estadisticas.BytesSent;

                // Calcular velocidades (bytes por segundo, ya que el timer es de 1 segundo)
                double bytesDescarga = bytesRecibidosActuales - bytesRecibidosPrevios;
                double bytesSubida = bytesEnviadosActuales - bytesEnviadosPrevios;

                // Convertir a MB/s
                double velocidadDescarga = bytesDescarga / (1024.0 * 1024.0);
                double velocidadSubida = bytesSubida / (1024.0 * 1024.0);

                // Actualizar labels
                labelDownload.Text = $"Descarga: {velocidadDescarga:F2} MB/s";
                labelUpload.Text = $"Subida: {velocidadSubida:F2} MB/s";

                // Cambiar color según actividad
                labelDownload.ForeColor = velocidadDescarga > 0.01 ? Color.Green : Color.Black;
                labelUpload.ForeColor = velocidadSubida > 0.01 ? Color.Blue : Color.Black;

                // Guardar valores actuales para la próxima comparación
                bytesRecibidosPrevios = bytesRecibidosActuales;
                bytesEnviadosPrevios = bytesEnviadosActuales;
            }
            catch (Exception ex)
            {
                timer.Stop();
                MessageBox.Show($"Error al actualizar estadísticas: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}