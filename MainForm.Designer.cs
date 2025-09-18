using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MedidorTrafico
{
    partial class MainForm
    {
        private IContainer components = null;
        private ComboBox comboInterfaces;
        private Label labelDownload;
        private Label labelUpload;
        private Label labelMaxDownload;
        private Label labelMaxUpload;
        private System.Windows.Forms.Timer timer;

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
            labelDownload = new Label();
            labelMaxDownload = new Label();
            labelUpload = new Label();
            labelMaxUpload = new Label();
            timer = new Timer(components);
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
            // labelDownload
            // 
            labelDownload.AutoSize = true;
            labelDownload.Font = new Font("Segoe UI", 10F);
            labelDownload.Location = new Point(18, 52);
            labelDownload.Name = "labelDownload";
            labelDownload.Size = new Size(116, 19);
            labelDownload.TabIndex = 1;
            labelDownload.Text = "Descarga: 0 MB/s";
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
            // labelUpload
            // 
            labelUpload.AutoSize = true;
            labelUpload.Font = new Font("Segoe UI", 10F);
            labelUpload.Location = new Point(18, 82);
            labelUpload.Name = "labelUpload";
            labelUpload.Size = new Size(101, 19);
            labelUpload.TabIndex = 2;
            labelUpload.Text = "Subida: 0 MB/s";
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 135);
            Controls.Add(comboInterfaces);
            Controls.Add(labelDownload);
            Controls.Add(labelMaxDownload);
            Controls.Add(labelUpload);
            Controls.Add(labelMaxUpload);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Medidor de Tráfico de Red";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        private Label label1;
    }
}