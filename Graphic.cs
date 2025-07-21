using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GFLauncher
{
    public partial class Graphic : Form
    {
        public Graphic()
        {
            InitializeComponent();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString()
                                       == "60hz")
            {
                string text = textBox1.Text;
                string pattern1 = "ScreenFrequency=.*";
                string replacement1 = "ScreenFrequency=60" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
            if (comboBox3.SelectedItem.ToString()
                                         == "120hz")
            {
                string text = textBox1.Text;
                string pattern1 = "ScreenFrequency=.*";
                string replacement1 = "ScreenFrequency=120" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
            if (comboBox3.SelectedItem.ToString()
                                         == "144hz")
            {
                string text = textBox1.Text;
                string pattern1 = "ScreenFrequency=.*";
                string replacement1 = "ScreenFrequency=144" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
            if (comboBox3.SelectedItem.ToString()
                                         == "240hz")
            {
                string text = textBox1.Text;
                string pattern1 = "ScreenFrequency=.*";
                string replacement1 = "ScreenFrequency=240" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1);
                textBox1.Text = modifiedText;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string pattern1 = "ScreenWidth=.*";
            string pattern2 = "ScreenHeight=.*";
            string modifiedText = text;
            string resolution = comboBox1.SelectedItem.ToString();

            switch (resolution)
            {
                case "800x600":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=800" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=600" + Environment.NewLine);
                    break;
                case "1024x768":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1024" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=768" + Environment.NewLine);
                    break;
                case "1152x864":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1152" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=864" + Environment.NewLine);
                    break;
                case "1280x720":
                case "1280x768":
                case "1280x800":
                case "1280x960":
                case "1280x1024":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1280" + Environment.NewLine);
                    switch (resolution)
                    {
                        case "1280x720":
                            modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=720" + Environment.NewLine);
                            break;
                        case "1280x768":
                            modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=768" + Environment.NewLine);
                            break;
                        case "1280x800":
                            modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=800" + Environment.NewLine);
                            break;
                        case "1280x960":
                            modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=960" + Environment.NewLine);
                            break;
                        case "1280x1024":
                            modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=1024" + Environment.NewLine);
                            break;
                    }
                    break;
                case "1360x768":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1360" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=768" + Environment.NewLine);
                    break;
                case "1366x768":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1366" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=768" + Environment.NewLine);
                    break;
                case "1600x900":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1600" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=900" + Environment.NewLine);
                    break;
                case "1600x1024":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1600" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=1024" + Environment.NewLine);
                    break;
                case "1600x1050":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1600" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=1050" + Environment.NewLine);
                    break;
                case "1920x1080":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1920" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=1080" + Environment.NewLine);
                    break;
                case "1440x900":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=1440" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=900" + Environment.NewLine);
                    break;
                case "2560x1440":
                    modifiedText = Regex.Replace(text, pattern1, "ScreenWidth=2560" + Environment.NewLine);
                    modifiedText = Regex.Replace(modifiedText, pattern2, "ScreenHeight=1440" + Environment.NewLine);
                    break;

            }

            textBox1.Text = modifiedText;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string pattern1 = "ShadowLevel=.*";
            string comboText = comboBox2.SelectedItem.ToString();
            if (comboText == "Nenhuma")
            {
                string replacement1 = "ShadowLevel=1" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }
            if (comboText == "Sombra do Personagem")
            {
                string replacement1 = "ShadowLevel=2" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }
            if (comboText == "Todas as Sombras")
            {
                string replacement1 = "ShadowLevel=3" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string pattern = "CharacterTexture=.*";
            string replacement = "";
            if (comboBox4.SelectedItem.ToString() == "Normal")
            {
                replacement = "CharacterTexture=0" + Environment.NewLine;
            }
            else if (comboBox4.SelectedItem.ToString() == "Boa")
            {
                replacement = "CharacterTexture=1" + Environment.NewLine;
            }

            string modifiedText = Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
            textBox1.Text = modifiedText;

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedItem.ToString() == "Normal")
            {
                string text = textBox1.Text;
                string pattern1 = "SceneTexture=.*";
                string replacement1 = "SceneTexture=0" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }
            else if (comboBox5.SelectedItem.ToString() == "Boa")
            {
                string text = textBox1.Text;
                string pattern1 = "SceneTexture=.*";
                string replacement1 = "SceneTexture=1" + Environment.NewLine;
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comboText = comboBox6.SelectedItem.ToString();
            string text = textBox1.Text;
            string pattern = "BloomShader=.*";
            if (comboText == "Nenhum")
            {
                string replacement = "BloomShader=0\r\n";
                string modifiedText = Regex.Replace(text, pattern, replacement);
                textBox1.Text = modifiedText;
            }
            else if (comboText == "Normal")
            {
                string replacement = "BloomShader=1\r\n";
                string modifiedText = Regex.Replace(text, pattern, replacement);
                textBox1.Text = modifiedText;
            }
            else if (comboText == "Alto")
            {
                string replacement = "BloomShader=3\r\n";
                string modifiedText = Regex.Replace(text, pattern, replacement);
                textBox1.Text = modifiedText;
            }

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedItem != null)
            {
                string selectedItem = comboBox7.SelectedItem.ToString();
                if (selectedItem == "30" || selectedItem == "60" || selectedItem == "120" ||
                    selectedItem == "144" || selectedItem == "240" || selectedItem == "Ilimitado")

                {
                    string text = textBox1.Text;
                    string pattern = "FpsLockValue=.*";
                    if (selectedItem == "Ilimitado") selectedItem = "9999";
                    string replacement = $"FpsLockValue={selectedItem}" + Environment.NewLine;
                    string modifiedText = Regex.Replace(text, pattern, replacement);
                    textBox1.Text = modifiedText;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("ViewCharacterRange=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf("\r\n", startIndex);
                if (endIndex >= 0)
                {
                    string line = "ViewCharacterRange=" + trackBar1.Value.ToString();
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("ViewRange=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf("\r\n", startIndex);
                if (endIndex >= 0)
                {
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    line = "ViewRange=" + trackBar2.Value.ToString();
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }

        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("CharacterEffectNum=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf(Environment.NewLine, startIndex);
                if (endIndex >= 0)
                {
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    line = "CharacterEffectNum=" + trackBar3.Value.ToString();
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int startIndex = text.IndexOf("ShadowType=");
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf(Environment.NewLine, startIndex);
                if (endIndex >= 0)
                {
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    line = "ShadowType=" + trackBar4.Value.ToString();
                    text = text.Remove(startIndex, endIndex - startIndex);
                    text = text.Insert(startIndex, line);
                    textBox1.Text = text;
                }
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                string text = textBox1.Text;
                string pattern1 = "PPMonochrome=.*";
                string replacement1 = "PPMonochrome=1\r\n";
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }
            else
            {
                string text = textBox1.Text;
                string pattern1 = "PPMonochrome=.*";
                string replacement1 = "PPMonochrome=0\r\n";
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                string text = textBox1.Text;
                string pattern1 = "PPSepia=.*";
                string replacement1 = "PPSepia=1\r\n";
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }
            else
            {
                string text = textBox1.Text;
                string pattern1 = "PPSepia=.*";
                string replacement1 = "PPSepia=0\r\n";
                string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                textBox1.Text = modifiedText;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 1;
            trackBar2.Value = 1;
            trackBar3.Value = 1;
            trackBar4.Value = 1;
            comboBox1.SelectedItem = "800x600";
            comboBox2.SelectedItem = "Nenhuma";
            comboBox3.SelectedItem = "60hz";
            comboBox4.SelectedItem = "Normal";
            comboBox5.SelectedItem = "Normal";
            comboBox6.SelectedItem = "Nenhum";
            comboBox7.SelectedItem = "30";
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            checkBox3.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 20;
            trackBar2.Value = 3;
            trackBar3.Value = 12;
            trackBar4.Value = 3;
            comboBox1.SelectedItem = "1280x720";
            comboBox2.SelectedItem = "Sombra do personagem";
            comboBox3.SelectedItem = "60hz";
            comboBox4.SelectedItem = "Normal";
            comboBox5.SelectedItem = "Boa";
            comboBox6.SelectedItem = "Normal";
            comboBox7.SelectedItem = "60";
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 20;
            trackBar2.Value = 3;
            trackBar3.Value = 12;
            trackBar4.Value = 3;
            comboBox1.SelectedItem = "1920x1080";
            comboBox2.SelectedItem = "Todas as Sombras";
            comboBox3.SelectedItem = "240hz";
            comboBox4.SelectedItem = "Boa";
            comboBox5.SelectedItem = "Boa";
            comboBox6.SelectedItem = "Alto";
            comboBox7.SelectedItem = "Ilimitado";
            checkBox1.Checked = false;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string fileName = "client.ini";
            string filePath = Path.Combine(Application.StartupPath, fileName);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(textBox1.Text);
            }
            MessageBox.Show("Arquivo de configuração salvo!", "Grand Fantasia Arkadia", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string programName = "GrandFantasia.exe";
            string parameter = "EasyFun";
            Process.Start(programName, parameter);

        }

        private void Graphic_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (checkBox1.Checked)
                {
                    string text = textBox1.Text;
                    string pattern1 = "FullScreenMode=.*";
                    string replacement1 = "FullScreenMode=0\r\n";
                    string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                    textBox1.Text = modifiedText;
                }
                else
                {
                    string text = textBox1.Text;
                    string pattern1 = "FullScreenMode=.*";
                    string replacement1 = "FullScreenMode=1\r\n";
                    string modifiedText = Regex.Replace(text, pattern1, replacement1, RegexOptions.IgnoreCase);
                    textBox1.Text = modifiedText;
                }

            }
        }
    }
}
