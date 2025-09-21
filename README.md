## MedidorTrafico - Network Traffic Monitor

**MedidorTrafico** is a real-time Windows Forms application that monitors network interface traffic, displaying current download/upload speeds and tracking maximum throughput values.
[1](#0-0)  The application provides a simple interface for selecting network interfaces and continuously monitoring their data transfer rates. <cite/>

## Architecture Overview

The application follows a classic Windows Forms architecture with three main components:

### Core Classes

#### `MainForm` Class
The main application controller that handles all business logic and user interactions. [2](#0-1)  This partial class is split across two files following the standard Windows Forms pattern.

**Key Fields:**
- `interfazSeleccionada`: Currently selected network interface [3](#0-2) 
- `bytesRecibidosPrevios`, `bytesEnviadosPrevios`: Previous measurement baselines for delta calculations [4](#0-3) 
- `velocidadMaxDescarga`, `velocidadMaxSubida`: Maximum recorded speeds [5](#0-4) 

#### `MainForm.Designer` Class
Auto-generated UI layout definition containing all visual components and their configurations. [6](#0-5) 

**UI Components:**
- `comboInterfaces`: Network interface selector dropdown [7](#0-6) 
- `labelActualDownload`, `labelActualUpload`: Current speed displays [8](#0-7) 
- `pictureDownload`, `pictureUpload`: Direction indicator icons [9](#0-8) 
- `timer`: 1-second interval timer for continuous monitoring [10](#0-9) 

## Method Documentation

### `Form1_Load(object sender, EventArgs e)`
**Purpose:** Initializes the application by discovering and populating available network interfaces. [11](#0-10) 

**Parameters:**
- `sender`: Event source object
- `e`: Event arguments

**Functionality:**
1. Queries all active network interfaces using `NetworkInterface.GetAllNetworkInterfaces()` [12](#0-11) 
2. Filters interfaces to exclude loopback and inactive interfaces
3. Populates the `comboInterfaces` dropdown with available interface names [13](#0-12) 
4. Auto-selects the first available interface [14](#0-13) 

**Error Handling:** Displays user-friendly error messages for interface discovery failures. [15](#0-14) 

### `comboInterfaces_SelectedIndexChanged(object sender, EventArgs e)`
**Purpose:** Handles network interface selection changes and initializes monitoring. [16](#0-15) 

**Parameters:**
- `sender`: ComboBox control that triggered the event
- `e`: Event arguments

**Functionality:**
1. Retrieves the selected interface by name [17](#0-16) 
2. Establishes baseline measurements using `GetIPv4Statistics()` [18](#0-17) 
3. Starts the monitoring timer [19](#0-18) 
4. Resets display labels to initial state [20](#0-19) 

### `timer_Tick(object sender, EventArgs e)`
**Purpose:** Core monitoring method that calculates and displays real-time network speeds. [21](#0-20) 

**Parameters:**
- `sender`: Timer object
- `e`: Event arguments

**Algorithm:**
1. **Interface Validation:** Checks if the selected interface is still operational [22](#0-21) 
2. **Data Collection:** Retrieves current IPv4 statistics [23](#0-22) 
3. **Speed Calculation:** Computes byte deltas from previous measurements [24](#0-23) 
4. **Unit Conversion:** Converts bytes to MB/s using `/ (1024.0 * 1024.0)` [25](#0-24) 
5. **Maximum Tracking:** Updates maximum speed records when exceeded [26](#0-25) 
6. **UI Updates:** Refreshes display labels with formatted speed values [27](#0-26) 
7. **Visual Feedback:** Changes label colors based on activity thresholds (Green for download >0.01 MB/s, Blue for upload >0.01 MB/s) [28](#0-27) 
8. **State Management:** Saves current values as baseline for next iteration [29](#0-28) 

**Error Handling:** Stops monitoring and displays error messages on exceptions. [30](#0-29) 

## Usage Example

```csharp
// Application startup - interfaces are automatically discovered
// User selects an interface from the dropdown
// Timer begins 1-second monitoring cycle
// Real-time speeds are displayed with color-coded activity indicators
```

## Technical Implementation Details

**Timer Configuration:** Uses 1000ms interval for real-time monitoring without excessive system overhead. [31](#0-30) 

**Resource Management:** Utilizes embedded arrow icons for download/upload direction indicators. [32](#0-31) [33](#0-32) 

**Data Source:** Leverages .NET's `System.Net.NetworkInformation` namespace for accessing network interface statistics. [34](#0-33) 

## Notes

The application is designed as a lightweight, single-window monitoring tool with minimal system resource usage. The timer-based polling approach provides real-time feedback while maintaining system stability through comprehensive error handling. The UI is intentionally simple, focusing on essential traffic monitoring functionality without unnecessary complexity.

Wiki pages you might want to explore:
- [Architecture (thepuag/MedidorTrafico)](/wiki/thepuag/MedidorTrafico#3)

### Citations

**File:** MainForm.Designer.cs (L8-8)
```csharp
    partial class MainForm
```

**File:** MainForm.Designer.cs (L11-11)
```csharp
        private ComboBox comboInterfaces;
```

**File:** MainForm.Designer.cs (L12-13)
```csharp
        private PictureBox pictureDownload;
        private PictureBox pictureUpload;
```

**File:** MainForm.Designer.cs (L14-17)
```csharp
        private Label labelActualDownload;
        private Label labelMaxDownload;
        private Label labelMaxUpload;
        private Label labelActualUpload;
```

**File:** MainForm.Designer.cs (L18-18)
```csharp
        private System.Windows.Forms.Timer timer;
```

**File:** MainForm.Designer.cs (L58-58)
```csharp
            pictureDownload.Image = Properties.Resources.ArrowDown2;
```

**File:** MainForm.Designer.cs (L66-66)
```csharp
            pictureUpload.Image = Properties.Resources.ArrowUp2;
```

**File:** MainForm.Designer.cs (L110-111)
```csharp
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
```

**File:** MainForm.Designer.cs (L130-130)
```csharp
            Text = "Medidor de Tráfico de Red";
```

**File:** MainForm.cs (L5-5)
```csharp
using System.Net.NetworkInformation;
```

**File:** MainForm.cs (L10-10)
```csharp
    public partial class MainForm : Form
```

**File:** MainForm.cs (L12-12)
```csharp
        private NetworkInterface interfazSeleccionada;
```

**File:** MainForm.cs (L13-14)
```csharp
        private long bytesRecibidosPrevios;
        private long bytesEnviadosPrevios;
```

**File:** MainForm.cs (L15-16)
```csharp
        private double velocidadMaxDescarga = 0;
        private double velocidadMaxSubida = 0;
```

**File:** MainForm.cs (L23-23)
```csharp
        private void Form1_Load(object sender, EventArgs e)
```

**File:** MainForm.cs (L28-31)
```csharp
                var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                 ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .ToList();
```

**File:** MainForm.cs (L34-38)
```csharp
                comboInterfaces.Items.Clear();
                foreach (var interfaz in interfaces)
                {
                    comboInterfaces.Items.Add(interfaz.Name);
                }
```

**File:** MainForm.cs (L40-43)
```csharp
                if (comboInterfaces.Items.Count > 0)
                {
                    comboInterfaces.SelectedIndex = 0;
                }
```

**File:** MainForm.cs (L52-58)
```csharp
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar interfaces: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
```

**File:** MainForm.cs (L61-61)
```csharp
        private void comboInterfaces_SelectedIndexChanged(object sender, EventArgs e)
```

**File:** MainForm.cs (L67-69)
```csharp
                var nombreSeleccionado = comboInterfaces.SelectedItem.ToString();
                interfazSeleccionada = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(i => i.Name == nombreSeleccionado);
```

**File:** MainForm.cs (L74-76)
```csharp
                    var estadisticas = interfazSeleccionada.GetIPv4Statistics();
                    bytesRecibidosPrevios = estadisticas.BytesReceived;
                    bytesEnviadosPrevios = estadisticas.BytesSent;
```

**File:** MainForm.cs (L78-80)
```csharp
                    // Iniciar el timer
                    timer.Stop();
                    timer.Start();
```

**File:** MainForm.cs (L83-84)
```csharp
                    labelActualDownload.Text = "Actual: 0.00 MB/s";
                    labelActualUpload.Text = "Actual: 0.00 MB/s";
```

**File:** MainForm.cs (L96-96)
```csharp
        private void timer_Tick(object sender, EventArgs e)
```

**File:** MainForm.cs (L103-109)
```csharp
                if (interfazSeleccionada.OperationalStatus != OperationalStatus.Up)
                {
                    timer.Stop();
                    labelActualDownload.Text = "Interfaz desconectada";
                    labelActualUpload.Text = "Interfaz desconectada";
                    return;
                }
```

**File:** MainForm.cs (L112-114)
```csharp
                var estadisticas = interfazSeleccionada.GetIPv4Statistics();
                long bytesRecibidosActuales = estadisticas.BytesReceived;
                long bytesEnviadosActuales = estadisticas.BytesSent;
```

**File:** MainForm.cs (L117-118)
```csharp
                double bytesDescarga = bytesRecibidosActuales - bytesRecibidosPrevios;
                double bytesSubida = bytesEnviadosActuales - bytesEnviadosPrevios;
```

**File:** MainForm.cs (L121-122)
```csharp
                double velocidadDescarga = bytesDescarga / (1024.0 * 1024.0);
                double velocidadSubida = bytesSubida / (1024.0 * 1024.0);
```

**File:** MainForm.cs (L126-135)
```csharp
                if (velocidadDescarga >= velocidadMaxDescarga)
                {
                    velocidadMaxDescarga = velocidadDescarga;
                    labelMaxDownload.Text = $"M�x : {velocidadMaxDescarga:F2} MB/s";
                }
                if (velocidadSubida >= velocidadMaxSubida)
                {
                    velocidadMaxSubida = velocidadSubida;
                    labelMaxUpload.Text = $"M�x : {velocidadMaxSubida:F2} MB/s";
                }
```

**File:** MainForm.cs (L137-138)
```csharp
                labelActualDownload.Text = $"Actual: {velocidadDescarga:F2} MB/s";
                labelActualUpload.Text = $"Actual: {velocidadSubida:F2} MB/s";
```

**File:** MainForm.cs (L145-146)
```csharp
                labelActualDownload.ForeColor = velocidadDescarga > 0.01 ? Color.Green : Color.Black;
                labelActualUpload.ForeColor = velocidadSubida > 0.01 ? Color.Blue : Color.Black;
```

**File:** MainForm.cs (L149-150)
```csharp
                bytesRecibidosPrevios = bytesRecibidosActuales;
                bytesEnviadosPrevios = bytesEnviadosActuales;
```

**File:** MainForm.cs (L152-159)
```csharp
            catch (Exception ex)
            {
                timer.Stop();
                MessageBox.Show($"Error al actualizar estad�sticas: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
```
