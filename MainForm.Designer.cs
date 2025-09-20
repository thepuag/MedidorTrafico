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
        private PictureBox pictureDownload;
        private PictureBox pictureUpload;
        private Label labelActualDownload;
        private Label labelMaxDownload;
        private Label labelMaxUpload;
        private Label labelActualUpload;
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
            pictureDownload = new PictureBox();
            pictureUpload = new PictureBox();
            labelMaxDownload = new Label();
            labelActualDownload = new Label();
            labelActualUpload = new Label();
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
            // pictureDownload
            // 
            pictureDownload.Location = new Point(8, 50);
            pictureDownload.Name = "pictureDownload";
            pictureDownload.Size = new Size(20, 20);
            pictureDownload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureDownload.Image = Properties.Resources.ArrowDown2;
            // 
            // pictureUpload
            // 
            pictureUpload.Location = new Point(8, 80);
            pictureUpload.Name = "pictureUpload";
            pictureUpload.Size = new Size(20, 20);
            pictureUpload.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureUpload.Image = Properties.Resources.ArrowUp2;
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 135);
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
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
