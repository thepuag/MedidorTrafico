using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace MedidorTrafico
{
    public partial class MainForm : Form
    {
        // Campos privados
        private NetworkInterface interfazSeleccionada;
        private long bytesRecibidosPrevios = 0;
        private long bytesEnviadosPrevios = 0;
        private double velocidadActualDescarga = 0;
        private double velocidadActualSubida = 0;
        private double velocidadMaxDescarga = 0;
        private double velocidadMaxSubida = 0;
        private bool boxSeleccionado = false;

        // Iconos base
        private Icon iconoDescargaBase;
        private Icon iconoSubidaBase;
        private Icon iconoInactivoBase;

        public MainForm()
        {
            InitializeComponent();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            // Cargar iconos base desde recursos
            try
            {
                // Cargar icono principal (Medidor1.ico)
                using (var iconStream = new MemoryStream(Properties.Resources.Medidor1))
                {
                    iconoInactivoBase = new Icon(iconStream);
                    // Establecer como icono de la aplicación
                    this.Icon = iconoInactivoBase;
                }

                // Cargar icono de descarga (ArrowDown.ico)
                using (var iconStream = new MemoryStream(Properties.Resources.ArrowDown))
                {
                    iconoDescargaBase = new Icon(iconStream);
                }

                // Cargar icono de subida (ArrowUp.ico)
                using (var iconStream = new MemoryStream(Properties.Resources.ArrowUp))
                {
                    iconoSubidaBase = new Icon(iconStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar iconos desde recursos: {ex.Message}\nUsando iconos del sistema.",
                              "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Fallback a iconos del sistema
                iconoDescargaBase = SystemIcons.Information;
                iconoSubidaBase = SystemIcons.Warning;
                iconoInactivoBase = SystemIcons.Application;
                this.Icon = iconoInactivoBase;
            }

            // Inicializar timer
            timer.Tick += timer_Tick;
        }

        private Icon CrearIconoSoloTexto(string velocidadTexto, Color colorTexto)
        {
            try
            {
                // Usar el tamaño completo del icono del system tray (32x32)
                using (var bitmap = new Bitmap(32, 32))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    // Configurar calidad de renderizado
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                    // Fondo transparente
                    graphics.Clear(Color.Transparent);

                    // Configurar fuente más grande para llenar el espacio
                    using (var font = new Font("Arial", 10, FontStyle.Bold))
                    using (var brush = new SolidBrush(colorTexto))
                    using (var brushSombra = new SolidBrush(Color.Black))
                    {
                        // Medir el texto para centrarlo
                        var medidaTexto = graphics.MeasureString(velocidadTexto, font);
                        float x = (32 - medidaTexto.Width) ;
                        float y = (32 - medidaTexto.Height) ;

                        // Dibujar sombra del texto para mejor legibilidad
                        graphics.DrawString(velocidadTexto, font, brushSombra, x + 1, y + 1);

                        // Dibujar el texto principal
                        graphics.DrawString(velocidadTexto, font, brush, x, y);
                    }

                    // Convertir bitmap a icono
                    IntPtr hIcon = bitmap.GetHicon();
                    Icon iconoTexto = Icon.FromHandle(hIcon);
                    return iconoTexto;
                }
            }
            catch (Exception)
            {
                // En caso de error, crear un icono simple
                return SystemIcons.Information;
            }
        }

        private string FormatearVelocidadMBs(double velocidadMBps)
        {
            return $"{velocidadMBps:F1}";
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
                // Inicializar texto
                using (var iconoInicial = CrearIconoSoloTexto("0.0", Color.Gray))
                {
                    notifyIcon.Icon = iconoInicial;
                }

                // Sin tooltip
                notifyIcon.Text = "";
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
                    ActualizarIconoSystemTray();
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

                // Actualizar icono del system tray con texto
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
                const double umbralActividad = 0.01; // MB/s

                bool hayDescarga = velocidadActualDescarga > umbralActividad;
                bool haySubida = velocidadActualSubida > umbralActividad;

                string textoVelocidad;
                Color colorTexto;

                if (hayDescarga && haySubida)
                {
                    // Mostrar la velocidad mayor con su color correspondiente
                    if (velocidadActualDescarga >= velocidadActualSubida)
                    {
                        textoVelocidad = FormatearVelocidadMBs(velocidadActualDescarga);
                        colorTexto = Color.Green; // Verde para descarga
                    }
                    else
                    {
                        textoVelocidad = FormatearVelocidadMBs(velocidadActualSubida);
                        colorTexto = Color.Red; // Rojo para subida
                    }
                }
                else if (hayDescarga)
                {
                    textoVelocidad = FormatearVelocidadMBs(velocidadActualDescarga);
                    colorTexto = Color.Green; // Verde para descarga
                }
                else if (haySubida)
                {
                    textoVelocidad = FormatearVelocidadMBs(velocidadActualSubida);
                    colorTexto = Color.Red; // Rojo para subida
                }
                else
                {
                    textoVelocidad = "0.0";
                    colorTexto = Color.Gray;
                }

                // Crear el icono solo con texto y aplicarlo
                using (var iconoTexto = CrearIconoSoloTexto(textoVelocidad, colorTexto))
                {
                    // Liberar el icono anterior si existe
                    var iconoAnterior = notifyIcon.Icon;
                    notifyIcon.Icon = iconoTexto;

                    // Liberar el icono anterior (excepto los iconos base)
                    if (iconoAnterior != null &&
                        iconoAnterior != iconoDescargaBase &&
                        iconoAnterior != iconoSubidaBase &&
                        iconoAnterior != iconoInactivoBase)
                    {
                        iconoAnterior.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                // En caso de error, usar icono base
                notifyIcon.Icon = iconoInactivoBase ?? SystemIcons.Application;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (checkBoxMinimizarToTray.Checked && this.WindowState == FormWindowState.Minimized)
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
            base.SetVisibleCore(this.ShowInTaskbar || !checkBoxMinimizarToTray.Checked || value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (boxSeleccionado)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.ShowInTaskbar = false;
                return;
            }

            notifyIcon.Visible = false;
            timer?.Stop();
            timer?.Dispose();

            iconoDescargaBase?.Dispose();
            iconoSubidaBase?.Dispose();
            iconoInactivoBase?.Dispose();

            base.OnFormClosing(e);
        }

        private void checkBoxMinimizarToTray_CheckedChanged(object sender, EventArgs e)
        {
            boxSeleccionado = checkBoxMinimizarToTray.Checked;
        }

        private void checkBoxSiempreVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxSiempreVisible.Checked;
        }
    }
}