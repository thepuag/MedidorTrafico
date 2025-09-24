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
        private Timer timer;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem mostrarToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private CheckBox checkBoxMinimizarToTray;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
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
            checkBoxMinimizarToTray = new CheckBox();
            checkBoxSiempreVisible = new CheckBox();
            ((ISupportInitialize)pictureDownload).BeginInit();
            ((ISupportInitialize)pictureUpload).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // comboInterfaces
            // 
            comboInterfaces.DropDownStyle = ComboBoxStyle.DropDownList;
            comboInterfaces.Location = new Point(8, 4);
            comboInterfaces.Margin = new Padding(3, 2, 3, 2);
            comboInterfaces.Name = "comboInterfaces";
            comboInterfaces.Size = new Size(286, 23);
            comboInterfaces.TabIndex = 0;
            comboInterfaces.SelectedIndexChanged += comboInterfaces_SelectedIndexChanged;
            // 
            // pictureDownload
            // 
            pictureDownload.Location = new Point(8, 35);
            pictureDownload.Name = "pictureDownload";
            pictureDownload.Size = new Size(20, 20);
            pictureDownload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureDownload.TabIndex = 3;
            pictureDownload.TabStop = false;
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
                // Dejar vacío si no se puede cargar el icono
            }
            // 
            // pictureUpload
            // 
            pictureUpload.Location = new Point(8, 65);
            pictureUpload.Name = "pictureUpload";
            pictureUpload.Size = new Size(20, 20);
            pictureUpload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureUpload.TabIndex = 4;
            pictureUpload.TabStop = false;
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
                // Dejar vacío si no se puede cargar el icono
            }
            // 
            // labelMaxDownload
            // 
            labelMaxDownload.AutoSize = true;
            labelMaxDownload.Font = new Font("Segoe UI", 10F);
            labelMaxDownload.Location = new Point(180, 35);
            labelMaxDownload.Name = "labelMaxDownload";
            labelMaxDownload.Size = new Size(86, 19);
            labelMaxDownload.TabIndex = 1;
            labelMaxDownload.Text = "Max: 0 MB/s";
            // 
            // labelActualDownload
            // 
            labelActualDownload.AutoSize = true;
            labelActualDownload.Font = new Font("Segoe UI", 10F);
            labelActualDownload.Location = new Point(40, 35);
            labelActualDownload.Name = "labelActualDownload";
            labelActualDownload.Size = new Size(98, 19);
            labelActualDownload.TabIndex = 1;
            labelActualDownload.Text = "Actual: 0 MB/s";
            // 
            // labelActualUpload
            // 
            labelActualUpload.AutoSize = true;
            labelActualUpload.Font = new Font("Segoe UI", 10F);
            labelActualUpload.Location = new Point(40, 66);
            labelActualUpload.Name = "labelActualUpload";
            labelActualUpload.Size = new Size(98, 19);
            labelActualUpload.TabIndex = 2;
            labelActualUpload.Text = "Actual: 0 MB/s";
            // 
            // labelMaxUpload
            // 
            labelMaxUpload.AutoSize = true;
            labelMaxUpload.Font = new Font("Segoe UI", 10F);
            labelMaxUpload.Location = new Point(180, 66);
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
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            // El icono se asignará desde el código principal
            notifyIcon.Text = "Medidor de Tráfico - Sin datos";
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { mostrarToolStripMenuItem, salirToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(116, 48);
            // 
            // mostrarToolStripMenuItem
            // 
            mostrarToolStripMenuItem.Name = "mostrarToolStripMenuItem";
            mostrarToolStripMenuItem.Size = new Size(115, 22);
            mostrarToolStripMenuItem.Text = "Mostrar";
            mostrarToolStripMenuItem.Click += mostrarToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(115, 22);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // checkBoxMinimizarToTray
            // 
            checkBoxMinimizarToTray.AutoSize = true;
            checkBoxMinimizarToTray.Location = new Point(10, 120);
            checkBoxMinimizarToTray.Name = "checkBoxMinimizarToTray";
            checkBoxMinimizarToTray.Size = new Size(230, 19);
            checkBoxMinimizarToTray.TabIndex = 5;
            checkBoxMinimizarToTray.Text = "Al cerrar, minimizar a la barra de tareas";
            checkBoxMinimizarToTray.UseVisualStyleBackColor = true;
            checkBoxMinimizarToTray.CheckedChanged += checkBoxMinimizarToTray_CheckedChanged;
            // 
            // checkBoxSiempreVisible
            // 
            checkBoxSiempreVisible.AutoSize = true;
            checkBoxSiempreVisible.Location = new Point(10, 98);
            checkBoxSiempreVisible.Name = "checkBoxSiempreVisible";
            checkBoxSiempreVisible.Size = new Size(158, 19);
            checkBoxSiempreVisible.TabIndex = 6;
            checkBoxSiempreVisible.Text = "Mantener siempre visible";
            checkBoxSiempreVisible.UseVisualStyleBackColor = true;
            checkBoxSiempreVisible.CheckedChanged += checkBoxSiempreVisible_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 145);
            Controls.Add(checkBoxSiempreVisible);
            Controls.Add(checkBoxMinimizarToTray);
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
            ((ISupportInitialize)pictureDownload).EndInit();
            ((ISupportInitialize)pictureUpload).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        private CheckBox checkBoxSiempreVisible;
    }
}