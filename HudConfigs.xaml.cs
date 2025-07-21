using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GFLauncher
{
    /// <summary>
    /// Lógica interna para HudConfigs.xaml
    /// </summary>
    public partial class HudConfigs : Window
    {

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HT_CAPTION = 0x0002;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public HudConfigs()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Graphic graphic = new Graphic();
            graphic.ShowDialog();
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Grand Fantasia Arkadia © \nSince 2025\nLauncher v0.1", "Grand Fantasia Arkadia - 2025", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

        }

        private void btn_Accounts_Click(object sender, RoutedEventArgs e)
        {
            AccountManagerWindow accountManagerWindow = new AccountManagerWindow();
            accountManagerWindow.ShowDialog(); // Abre a janela de gerenciamento de contas
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_sound_Click(object sender, RoutedEventArgs e)
        {
            Som x = new Som();
            x.ShowDialog();
        }

        private void btn_Tools_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    ReleaseCapture();
                    SendMessage(new WindowInteropHelper(this).Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }

            
        }
    }
}
