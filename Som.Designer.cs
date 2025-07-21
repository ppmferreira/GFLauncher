using System;

namespace GFLauncher
{
    partial class Som
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Som));
            groupBox1 = new GroupBox();
            button2 = new Button();
            button1 = new Button();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            trackBar2 = new TrackBar();
            trackBar1 = new TrackBar();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(trackBar2);
            groupBox1.Controls.Add(trackBar1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(7, 9);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(257, 269);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Configuração de Audio";
            // 
            // button2
            // 
            button2.Location = new Point(133, 240);
            button2.Name = "button2";
            button2.Size = new Size(118, 23);
            button2.TabIndex = 9;
            button2.Text = "Cancelar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(6, 240);
            button1.Name = "button1";
            button1.Size = new Size(121, 23);
            button1.TabIndex = 8;
            button1.Text = "Confirmar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox2
            // 
            comboBox2.FlatStyle = FlatStyle.Popup;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Ligado", "Desligado" });
            comboBox2.Location = new Point(86, 197);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(165, 23);
            comboBox2.TabIndex = 7;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox1
            // 
            comboBox1.FlatStyle = FlatStyle.Popup;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Padrão", "Repetir Música", "Com Intervalo" });
            comboBox1.Location = new Point(86, 37);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(165, 23);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // trackBar2
            // 
            trackBar2.Location = new Point(86, 132);
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(165, 45);
            trackBar2.TabIndex = 5;
            trackBar2.TickStyle = TickStyle.Both;
            trackBar2.Value = 1;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(86, 81);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(165, 45);
            trackBar1.TabIndex = 5;
            trackBar1.TickStyle = TickStyle.Both;
            trackBar1.Value = 1;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 201);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 3;
            label4.Text = "Silencioso";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 148);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 2;
            label3.Text = "Áudio";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 95);
            label2.Name = "label2";
            label2.Size = new Size(41, 15);
            label2.TabIndex = 1;
            label2.Text = "Fundo";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 45);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Modo";
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(275, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(433, 269);
            textBox1.TabIndex = 1;
            textBox1.Text = resources.GetString("textBox1.Text");
            textBox1.Visible = false;
            // 
            // Som
            // 
            ClientSize = new Size(273, 284);
            Controls.Add(textBox1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Som";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Som";
            TopMost = true;
            Load += Som_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private GroupBox groupBox1;
        private TrackBar trackBar2;
        private TrackBar trackBar1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
    }
}