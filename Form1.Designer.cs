namespace GFLauncher
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pb_close = new PictureBox();
            pb_scan = new PictureBox();
            pb_options = new PictureBox();
            pb_Start = new PictureBox();
            pb_discord = new PictureBox();
            pn_web = new Panel();
            lbl_status = new Label();
            BackgroundDownloaders = new System.ComponentModel.BackgroundWorker();
            timerAnimation = new System.Windows.Forms.Timer(components);
            pn_porcentagem1 = new Panel();
            pn_pValue1 = new Panel();
            pn_porcentagem2 = new Panel();
            pn_pValue2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pb_close).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_scan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_options).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_Start).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_discord).BeginInit();
            pn_porcentagem1.SuspendLayout();
            pn_porcentagem2.SuspendLayout();
            SuspendLayout();
            // 
            // pb_close
            // 
            pb_close.BackColor = Color.Transparent;
            pb_close.Image = Properties.Resources.mouseexoff;
            pb_close.Location = new Point(872, 306);
            pb_close.Name = "pb_close";
            pb_close.Size = new Size(23, 23);
            pb_close.TabIndex = 0;
            pb_close.TabStop = false;
            pb_close.Click += pb_close_Click;
            pb_close.MouseDown += pb_close_MouseDown;
            pb_close.MouseEnter += pb_close_MouseHover;
            pb_close.MouseLeave += pb_close_MouseLeave;
            pb_close.MouseHover += pb_close_MouseHover;
            // 
            // pb_scan
            // 
            pb_scan.BackColor = Color.Transparent;
            pb_scan.Image = Properties.Resources.Escanear;
            pb_scan.Location = new Point(892, 431);
            pb_scan.Name = "pb_scan";
            pb_scan.Size = new Size(73, 69);
            pb_scan.TabIndex = 1;
            pb_scan.TabStop = false;
            pb_scan.Click += pb_scan_Click;
            pb_scan.MouseDown += pb_scan_MouseDown;
            pb_scan.MouseEnter += pb_scan_MouseEnter;
            pb_scan.MouseLeave += pb_scan_MouseLeave;
            pb_scan.MouseHover += pb_scan_MouseHover;
            // 
            // pb_options
            // 
            pb_options.BackColor = Color.Transparent;
            pb_options.Image = Properties.Resources.OptionsLeave;
            pb_options.Location = new Point(893, 667);
            pb_options.Name = "pb_options";
            pb_options.Size = new Size(72, 71);
            pb_options.TabIndex = 2;
            pb_options.TabStop = false;
            pb_options.Click += pb_options_Click;
            pb_options.MouseDown += pb_options_MouseDown;
            pb_options.MouseEnter += pb_options_MouseEnter;
            pb_options.MouseLeave += pb_options_MouseLeave;
            pb_options.MouseHover += pb_options_MouseHover;
            // 
            // pb_Start
            // 
            pb_Start.BackColor = Color.Transparent;
            pb_Start.Image = Properties.Resources.StartLeave;
            pb_Start.Location = new Point(913, 543);
            pb_Start.Name = "pb_Start";
            pb_Start.Size = new Size(100, 94);
            pb_Start.TabIndex = 3;
            pb_Start.TabStop = false;
            pb_Start.Click += pb_Start_Click;
            pb_Start.MouseDown += pb_Start_MouseDown;
            pb_Start.MouseEnter += pb_Start_MouseEnter;
            pb_Start.MouseLeave += pb_Start_MouseLeave;
            pb_Start.MouseHover += pb_Start_MouseHover;
            // 
            // pb_discord
            // 
            pb_discord.BackColor = Color.Transparent;
            pb_discord.Image = Properties.Resources.DiscordLeave;
            pb_discord.Location = new Point(1008, 625);
            pb_discord.Name = "pb_discord";
            pb_discord.Size = new Size(72, 71);
            pb_discord.TabIndex = 4;
            pb_discord.TabStop = false;
            pb_discord.Click += pb_discord_Click;
            pb_discord.MouseDown += pb_discord_MouseDown;
            pb_discord.MouseEnter += pb_discord_MouseEnter;
            pb_discord.MouseLeave += pb_discord_MouseLeave;
            pb_discord.MouseHover += pb_discord_MouseHover;
            // 
            // pn_web
            // 
            pn_web.BackColor = Color.Transparent;
            pn_web.Location = new Point(403, 356);
            pn_web.Name = "pn_web";
            pn_web.Size = new Size(483, 300);
            pn_web.TabIndex = 5;
            pn_web.Paint += pn_web_Paint;
            // 
            // lbl_status
            // 
            lbl_status.AutoSize = true;
            lbl_status.BackColor = Color.Transparent;
            lbl_status.Location = new Point(403, 667);
            lbl_status.Name = "lbl_status";
            lbl_status.Size = new Size(66, 15);
            lbl_status.TabIndex = 6;
            lbl_status.Text = "Version: 0.0";
            // 
            // BackgroundDownloaders
            // 
            BackgroundDownloaders.DoWork += BackgroundDownloaders_DoWork;
            // 
            // timerAnimation
            // 
            timerAnimation.Interval = 300;
            timerAnimation.Tick += timerAnimation_Tick;
            // 
            // pn_porcentagem1
            // 
            pn_porcentagem1.BackColor = Color.Transparent;
            pn_porcentagem1.Controls.Add(pn_pValue1);
            pn_porcentagem1.Location = new Point(431, 700);
            pn_porcentagem1.Name = "pn_porcentagem1";
            pn_porcentagem1.Size = new Size(429, 11);
            pn_porcentagem1.TabIndex = 7;
            // 
            // pn_pValue1
            // 
            pn_pValue1.BackColor = Color.Crimson;
            pn_pValue1.Location = new Point(-4, 1);
            pn_pValue1.Name = "pn_pValue1";
            pn_pValue1.Size = new Size(10, 10);
            pn_pValue1.TabIndex = 9;
            // 
            // pn_porcentagem2
            // 
            pn_porcentagem2.BackColor = Color.Transparent;
            pn_porcentagem2.Controls.Add(pn_pValue2);
            pn_porcentagem2.Location = new Point(430, 716);
            pn_porcentagem2.Name = "pn_porcentagem2";
            pn_porcentagem2.Size = new Size(428, 10);
            pn_porcentagem2.TabIndex = 8;
            // 
            // pn_pValue2
            // 
            pn_pValue2.BackColor = Color.DarkGoldenrod;
            pn_pValue2.Location = new Point(0, 0);
            pn_pValue2.Name = "pn_pValue2";
            pn_pValue2.Size = new Size(10, 10);
            pn_pValue2.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 255, 0);
            BackgroundImage = Properties.Resources.BackgroundTransparent;
            ClientSize = new Size(1230, 744);
            Controls.Add(pn_porcentagem2);
            Controls.Add(pn_porcentagem1);
            Controls.Add(lbl_status);
            Controls.Add(pn_web);
            Controls.Add(pb_Start);
            Controls.Add(pb_discord);
            Controls.Add(pb_options);
            Controls.Add(pb_scan);
            Controls.Add(pb_close);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Launcher Arkadia";
            TransparencyKey = Color.FromArgb(0, 255, 0);
            Load += Form1_Load;
            MouseDown += Form1_MouseDown;
            MouseEnter += Form1_MouseEnter;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            ((System.ComponentModel.ISupportInitialize)pb_close).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_scan).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_options).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_Start).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_discord).EndInit();
            pn_porcentagem1.ResumeLayout(false);
            pn_porcentagem2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pb_close;
        private PictureBox pb_scan;
        private PictureBox pb_options;
        private PictureBox pb_Start;
        private PictureBox pb_discord;
        private Panel pn_web;
        private Label lbl_status;
        private System.ComponentModel.BackgroundWorker BackgroundDownloaders;
        private System.Windows.Forms.Timer timerAnimation;
        private Panel pn_porcentagem1;
        private Panel pn_pValue1;
        private Panel pn_porcentagem2;
        private Panel pn_pValue2;

        private void pn_web_Paint(object sender, PaintEventArgs e)
        {
            // Melhora visual: borda arredondada, sombra e fundo translúcido
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int radius = 20;
            int shadowOffset = 6;
            Rectangle rect = new Rectangle(0, 0, pn_web.Width - shadowOffset, pn_web.Height - shadowOffset);

            using (var path = RoundedRect(rect, radius))
            using (var shadowPath = RoundedRect(new Rectangle(rect.X + shadowOffset, rect.Y + shadowOffset, rect.Width, rect.Height), radius))
            using (var shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
            using (var borderPen = new Pen(Color.FromArgb(180, 180, 80, 40), 2))
            {
                // Sombra
                g.FillPath(shadowBrush, shadowPath);
                // Fundo
                g.FillPath(bgBrush, path);
                // Borda
                g.DrawPath(borderPen, path);
            }
        }

        // Função auxiliar para criar retângulo arredondado
        private System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}