using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GFLauncher
{
    /// <summary>
    /// Lógica interna para language.xaml
    /// </summary>
    public partial class language : Window
    {
        public bool DeteleFiles = false;
        LauncherFunctions WebFunctions = new LauncherFunctions();
        string url = "http://https://www.gamearkadia.com.br";
        int[] versionInDisc = new int[] { 0, 0 };
        string VerFile = "Version.txt";
        public language()
        {
            InitializeComponent();
        }


        private void win_lang_Initialized(object sender, EventArgs e)
        {
            cb_langs.Items.Add("PT-BR");
            cb_langs.Items.Add("ENG-US");
            cb_langs.Items.Add("ES-ES");
            cb_langs.Items.Add("FR-FR");

        }

        private void cb_langs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch(cb_langs.SelectedItem)
            {
                case "PT-BR":
                    WebFunctions.DownloadFile("https://www.gamearkadia.com.br/updatebr.html");
                    break;
                case "ENG-US":
                    WebFunctions.DownloadFile("https://www.www.gamearkadia.com.br/updateen.html");
                    break;
                case "ES-ES":
                    WebFunctions.DownloadFile("https://www.www.gamearkadia.com.br/updatees.html");
                    break;
                case "FR-FR":
                    WebFunctions.DownloadFile("https://www.www.gamearkadia.com.br/updatefr.html");
                    break;
            };

            string file_ = System.Windows.Forms.Application.StartupPath + "\\lang.ini";

            using (StreamWriter sw = new StreamWriter(file_))
            {
                sw.WriteLine("Lang=" + cb_langs.SelectedItem.ToString());
            }



        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
