using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        private double velocidadMaxDescarga = 0;
        private double velocidadMaxSubida = 0;
        private double velocidadActualDescarga = 0;
        private double velocidadActualSubida = 0;
        private Icon iconoDescarga;
        private Icon iconoSubida;
        private Icon iconoInactivo;

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

                // Configurar el NotifyIcon
                notifyIcon.Visible = true;

                // Cargar iconos personalizados
                try
                {
                    iconoDescarga = new Icon(new MemoryStream(Properties.Resources.ArrowDown));
                    iconoSubida = new Icon(new MemoryStream(Properties.Resources.ArrowUp));
                    iconoInactivo = new Icon(new MemoryStream(Properties.Resources.Medidor1));
                    notifyIcon.Icon = iconoInactivo;
                }
                catch
                {
                    // Si no se pueden cargar los iconos personalizados, usar el del sistema
                    iconoDescarga = SystemIcons.Information;
                    iconoSubida = SystemIcons.Information;
                    iconoInactivo = SystemIcons.Application;
                    notifyIcon.Icon = iconoInactivo;
                }

                ActualizarTooltipSystemTray();
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

                    // Reiniciar valores máximos
                    velocidadMaxDescarga = 0;
                    velocidadMaxSubida = 0;

                    // Iniciar el timer
                    timer.Stop();
                    timer.Start();

                    // Actualizar labels inmediatamente
                    labelActualDownload.Text = "Actual: 0.00 MB/s";
                    labelActualUpload.Text = "Actual: 0.00 MB/s";
                    labelMaxDownload.Text = "Máx: 0.00 MB/s";
                    labelMaxUpload.Text = "Máx: 0.00 MB/s";

                    ActualizarTooltipSystemTray();
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
                    labelActualDownload.Text = "Interfaz desconectada";
                    labelActualUpload.Text = "Interfaz desconectada";
                    velocidadActualDescarga = 0;
                    velocidadActualSubida = 0;
                    ActualizarTooltipSystemTray();
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
                velocidadActualDescarga = bytesDescarga / (1024.0 * 1024.0);
                velocidadActualSubida = bytesSubida / (1024.0 * 1024.0);

                // Actualizar velocidades máximas
                if (velocidadActualDescarga >= velocidadMaxDescarga)
                {
                    velocidadMaxDescarga = velocidadActualDescarga;
                    labelMaxDownload.Text = $"Máx: {velocidadMaxDescarga:F2} MB/s";
                }
                if (velocidadActualSubida >= velocidadMaxSubida)
                {
                    velocidadMaxSubida = velocidadActualSubida;
                    labelMaxUpload.Text = $"Máx: {velocidadMaxSubida:F2} MB/s";
                }

                // Actualizar labels de la ventana
                labelActualDownload.Text = $"Actual: {velocidadActualDescarga:F2} MB/s";
                labelActualUpload.Text = $"Actual: {velocidadActualSubida:F2} MB/s";

                // Cambiar color según actividad
                labelActualDownload.ForeColor = velocidadActualDescarga > 0.01 ? Color.Green : Color.Black;
                labelActualUpload.ForeColor = velocidadActualSubida > 0.01 ? Color.Blue : Color.Black;

                // Actualizar tooltip del system tray
                ActualizarTooltipSystemTray();

                // Actualizar icono según actividad
                ActualizarIconoSystemTray();

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

        private void ActualizarIconoSystemTray()
        {
            try
            {
                // Cambiar icono según la actividad de red
                const double umbralActividad = 0.01; // MB/s

                if (velocidadActualDescarga > umbralActividad && velocidadActualSubida > umbralActividad)
                {
                    // Ambas direcciones activas - alternar iconos cada segundo para mostrar actividad
                    notifyIcon.Icon = DateTime.Now.Second % 2 == 0 ? iconoDescarga : iconoSubida;
                }
                else if (velocidadActualDescarga > umbralActividad)
                {
                    // Solo descarga activa
                    notifyIcon.Icon = iconoDescarga;
                }
                else if (velocidadActualSubida > umbralActividad)
                {
                    // Solo subida activa
                    notifyIcon.Icon = iconoSubida;
                }
                else
                {
                    // Sin actividad
                    notifyIcon.Icon = iconoInactivo;
                }
            }
            catch (Exception)
            {
                // En caso de error, usar icono por defecto
                notifyIcon.Icon = SystemIcons.Application;
            }
        }

        private void ActualizarTooltipSystemTray()
        {
            try
            {
                string tooltip;
                if (interfazSeleccionada == null)
                {
                    tooltip = "Medidor de Tráfico\nSin interfaz seleccionada";
                }
                else
                {
                    tooltip = $"Medidor de Tráfico\n" +
                             $"Interfaz: {interfazSeleccionada.Name}\n" +
                             $"↓ Descarga: {velocidadActualDescarga:F2} MB/s\n" +
                             $"↑ Subida: {velocidadActualSubida:F2} MB/s";
                }

                // El tooltip no puede exceder 63 caracteres
                if (tooltip.Length > 63)
                {
                    tooltip = $"Tráfico de Red\n↓ {velocidadActualDescarga:F2} MB/s\n↑ {velocidadActualSubida:F2} MB/s";
                }

                notifyIcon.Text = tooltip;
            }
            catch (Exception)
            {
                notifyIcon.Text = "Medidor de Tráfico - Error";
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (checkBoxMinimizeToTray.Checked && this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            MostrarVentana();
        }

        private void mostrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MostrarVentana();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void MostrarVentana()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.BringToFront();
            this.Activate();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Permitir que la ventana se oculte completamente al minimizar
            base.SetVisibleCore(this.ShowInTaskbar || !checkBoxMinimizeToTray.Checked || value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Limpiar el NotifyIcon al cerrar
            notifyIcon.Visible = false;

            // Liberar recursos de iconos personalizados
            iconoDescarga?.Dispose();
            iconoSubida?.Dispose();

            base.OnFormClosing(e);
        }
    }
}