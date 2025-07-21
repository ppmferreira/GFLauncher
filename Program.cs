namespace GFLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar a aplica��o: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}