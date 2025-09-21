using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MedidorTrafico
{
    partial class MainForm
    {
        private IContainer components = null;
        private ComboBox comboInterfaces;
        private PictureBox pictureDownload;
        private PictureBox pictureUpload;
        private Label labelActualDownload;
        private Label labelMaxDownload;
        private Label labelMaxUpload;
        private Label labelActualUpload;
        private System.Windows.Forms.Timer timer;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem mostrarToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private CheckBox checkBoxMinimizeToTray;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            comboInterfaces = new ComboBox();
            pictureDownload = new PictureBox();
            pictureUpload = new PictureBox();
            labelMaxDownload = new Label();
            labelActualDownload = new Label();
            labelActualUpload = new Label();
            labelMaxUpload = new Label();
            timer = new Timer(components);
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip = new ContextMenuStrip(components);
            mostrarToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            checkBoxMinimizeToTray = new CheckBox();

            ((ISupportInitialize)(pictureDownload)).BeginInit();
            ((ISupportInitialize)(pictureUpload)).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();

            // 
            // comboInterfaces
            // 
            comboInterfaces.DropDownStyle = ComboBoxStyle.DropDownList;
            comboInterfaces.Location = new Point(18, 15);
            comboInterfaces.Margin = new Padding(3, 2, 3, 2);
            comboInterfaces.Name = "comboInterfaces";
            comboInterfaces.Size = new Size(263, 23);
            comboInterfaces.TabIndex = 0;
            comboInterfaces.SelectedIndexChanged += comboInterfaces_SelectedIndexChanged;

            // 
            // pictureDownload
            // 
            pictureDownload.Location = new Point(8, 50);
            pictureDownload.Name = "pictureDownload";
            pictureDownload.Size = new Size(20, 20);
            pictureDownload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureDownload.TabIndex = 3;
            pictureDownload.TabStop = false;
            // Usar el icono de descarga como imagen (convertir de Icon a Bitmap)
            try
            {
                using (var iconStream = new MemoryStream(Properties.Resources.ArrowDown))
                using (var icon = new Icon(iconStream))
                {
                    pictureDownload.Image = icon.ToBitmap();
                }
            }
            catch
            {
                // Si no se puede cargar, usar color de fondo
                pictureDownload.BackColor = Color.Green;
            }

            // 
            // pictureUpload
            // 
            pictureUpload.Location = new Point(8, 80);
            pictureUpload.Name = "pictureUpload";
            pictureUpload.Size = new Size(20, 20);
            pictureUpload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureUpload.TabIndex = 4;
            pictureUpload.TabStop = false;
            // Usar el icono de subida como imagen
            try
            {
                using (var iconStream = new MemoryStream(Properties.Resources.ArrowUp))
                using (var icon = new Icon(iconStream))
                {
                    pictureUpload.Image = icon.ToBitmap();
                }
            }
            catch
            {
                // Si no se puede cargar, usar color de fondo
                pictureUpload.BackColor = Color.Blue;
            }

            // 
            // labelMaxDownload
            // 
            labelMaxDownload.AutoSize = true;
            labelMaxDownload.Font = new Font("Segoe UI", 10F);
            labelMaxDownload.Location = new Point(180, 52);
            labelMaxDownload.Name = "labelMaxDownload";
            labelMaxDownload.Size = new Size(86, 19);
            labelMaxDownload.TabIndex = 1;
            labelMaxDownload.Text = "Max: 0 MB/s";

            // 
            // labelActualDownload
            // 
            labelActualDownload.AutoSize = true;
            labelActualDownload.Font = new Font("Segoe UI", 10F);
            labelActualDownload.Location = new Point(40, 52);
            labelActualDownload.Name = "labelActualDownload";
            labelActualDownload.Size = new Size(98, 19);
            labelActualDownload.TabIndex = 1;
            labelActualDownload.Text = "Actual: 0 MB/s";

            // 
            // labelActualUpload
            // 
            labelActualUpload.AutoSize = true;
            labelActualUpload.Font = new Font("Segoe UI", 10F);
            labelActualUpload.Location = new Point(40, 82);
            labelActualUpload.Name = "labelActualUpload";
            labelActualUpload.Size = new Size(98, 19);
            labelActualUpload.TabIndex = 2;
            labelActualUpload.Text = "Actual: 0 MB/s";

            // 
            // labelMaxUpload
            // 
            labelMaxUpload.AutoSize = true;
            labelMaxUpload.Font = new Font("Segoe UI", 10F);
            labelMaxUpload.Location = new Point(180, 82);
            labelMaxUpload.Name = "labelMaxUpload";
            labelMaxUpload.Size = new Size(86, 19);
            labelMaxUpload.TabIndex = 2;
            labelMaxUpload.Text = "Max: 0 MB/s";

            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;

            // 
            // checkBoxMinimizeToTray
            // 
            checkBoxMinimizeToTray.AutoSize = true;
            checkBoxMinimizeToTray.Location = new Point(18, 110);
            checkBoxMinimizeToTray.Name = "checkBoxMinimizeToTray";
            checkBoxMinimizeToTray.Size = new Size(183, 19);
            checkBoxMinimizeToTray.TabIndex = 5;
            checkBoxMinimizeToTray.Text = "Minimizar a la barra de tareas";
            checkBoxMinimizeToTray.UseVisualStyleBackColor = true;

            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                mostrarToolStripMenuItem,
                salirToolStripMenuItem});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(115, 48);

            // 
            // mostrarToolStripMenuItem
            // 
            mostrarToolStripMenuItem.Name = "mostrarToolStripMenuItem";
            mostrarToolStripMenuItem.Size = new Size(114, 22);
            mostrarToolStripMenuItem.Text = "Mostrar";
            mostrarToolStripMenuItem.Click += mostrarToolStripMenuItem_Click;

            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(114, 22);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;

            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Icon = SystemIcons.Application; // Se configurará dinámicamente en el código
            notifyIcon.Text = "Medidor de Tráfico - Sin datos";
            notifyIcon.Visible = false;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;

            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 145);
            Controls.Add(checkBoxMinimizeToTray);
            Controls.Add(comboInterfaces);
            Controls.Add(pictureDownload);
            Controls.Add(pictureUpload);
            Controls.Add(labelActualDownload);
            Controls.Add(labelMaxDownload);
            Controls.Add(labelActualUpload);
            Controls.Add(labelMaxUpload);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Medidor de Tráfico de Red";
            Load += Form1_Load;
            Resize += MainForm_Resize;

            ((ISupportInitialize)(pictureDownload)).EndInit();
            ((ISupportInitialize)(pictureUpload)).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}