using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFLauncher
{
    public partial class Som : Form
    {
        public Som()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "client.ini";
            string filePath = Path.Combine(Application.StartupPath, fileName);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(textBox1.Text);
            }
            MessageBox.Show("Arquivo de configuração salvo!", "Grand Fantasia Arkadia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Padrão")
            {
                string text = textBox1.Text;
                string pattern1 = @"BGMType=.*";
                string replacement1 = "BGMType=0" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
            if (comboBox1.SelectedItem.ToString() == "Repetir Música")
            {
                string text = textBox1.Text;
                string pattern1 = @"BGMType=.*";
                string replacement1 = "BGMType=1" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
            if (comboBox1.SelectedItem.ToString() == "Com Intervalo")
            {
                string text = textBox1.Text;
                string pattern1 = @"BGMType=.*";
                string replacement1 = "BGMType=2" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("SoundValoume=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf(Environment.NewLine, startIndex);
                if (endIndex >= 0)
                {
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    line = "SoundValoume=" + (trackBar2.Value / 10f).ToString("F1", CultureInfo.InvariantCulture);
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("BGMValoume=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf(Environment.NewLine, startIndex);
                if (endIndex >= 0)
                {
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    line = "BGMValoume=" + (trackBar1.Value / 10f).ToString("F1", CultureInfo.InvariantCulture);
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }

        }

        private void Som_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                if (comboBox2.SelectedItem.ToString() == "Ligado")
                {
                    string text = textBox1.Text;
                    string pattern1 = @"SoundMute=.*";
                    string replacement1 = "SoundMute=1" + Environment.NewLine;
                    string modifiedText = Regex.Replace(text, pattern1, replacement1);
                    textBox1.Text = modifiedText;
                }
                if (comboBox2.SelectedItem.ToString() == "Desligado")
                {
                    string text = textBox1.Text;
                    string pattern1 = @"SoundMute=.*";
                    string replacement1 = "SoundMute=0" + Environment.NewLine;
                    string modifiedText = Regex.Replace(text, pattern1, replacement1);
                    textBox1.Text = modifiedText;
                }

            }
        }
    }
}
