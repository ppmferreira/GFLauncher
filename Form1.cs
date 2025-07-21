using GFLauncher.Properties;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using MessageBox = System.Windows.Forms.MessageBox;
using Application = System.Windows.Forms.Application;

namespace GFLauncher
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [Out] byte[] lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesRead);

        private static bool CheckMemoryProf(IntPtr hProcess, string actual_ip)
        {
            UInt64 MemoryPointer = 0xD77B48;
            UInt64 Offset = 0x104;
            UInt64 pTemp = 0;

            byte[] tmp = new byte[8];
            byte[] Brets = new byte[12];

            IntPtr cout;

            if (ReadProcessMemory(hProcess, (IntPtr)MemoryPointer, tmp, 4, out cout))
            {
                pTemp = BitConverter.ToUInt64(tmp) + Offset;

                if (ReadProcessMemory(hProcess, (IntPtr)pTemp, Brets, 12, out cout))
                {
                    string dx = Encoding.UTF8.GetString(Brets);
                    return actual_ip.CompareTo(dx) == 0;
                }
            }

            return false;
        }

        static System.Timers.Timer? checkTimer;
        Process? GameProc;

        public bool DeteleFiles = false;
        LauncherFunctions WebFunctions = new LauncherFunctions();
        string GameIP = "31.97.23.92";

        public bool Falhas = false;
        public bool Concluido = false;

        bool IsDraggingForm = false;
        System.Drawing.Point MousePos = new System.Drawing.Point();

        public Form1()
        {
            InitializeComponent();

            // Substitui WebBrowser por WebView2 para navegação moderna
            var webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            webView.Dock = DockStyle.Fill;
            webView.Source = new Uri("https://gamearkadia.com.br/news/");
            pn_web.Controls.Add(webView);

            // Cria o Timer
            checkTimer = new System.Timers.Timer(5000);
            checkTimer.Elapsed += (object? sender, ElapsedEventArgs e) => CheckForMultipleGames(sender, e);
            checkTimer.AutoReset = true;
            checkTimer.Start();

            // Moved InstallService and CheckVersion to Form1_Load to prevent disposing form during initialization
        }

        private void InstallService()
        {
            try
            {
                var query = "sc query GrandFantasia";
                string retorno = ExecuteCommandAndGetOutput(query);
                
                if(!retorno.Contains("RUNNING")) 
                {
                    lbl_status.Text = "Checando servicos...";

                    string servicePath = Path.Combine(Application.StartupPath, "GrandFantasia.exe");

                    if (File.Exists(servicePath))
                    {
                        string command = $"sc.exe create GrandFantasia binPath= \"{servicePath}\" start= auto";
                        string output = ExecuteCommandAndGetOutput(command);

                        command = $"sc qc GrandFantasia";
                        output = ExecuteCommandAndGetOutput(command);

                        command = $"sc.exe start GrandFantasia";
                        output = ExecuteCommandAndGetOutput(command);
                    }
                    else
                    {
                        // Instead of showing error and exiting, just log that the service file is not found
                        // This might be normal if the game executable is downloaded later
                        lbl_status.Text = "Servi�o GrandFantasia n�o encontrado - ser� instalado ap�s download do jogo.";
                        return;
                    }
                }
                else
                {
                    lbl_status.Text = "Servi�o GrandFantasia j� est� em execu��o.";
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't exit the application
                lbl_status.Text = "Aviso: Erro ao verificar servi�o - " + ex.Message;
                return;
            }
        }

        private static string ExecuteCommandAndGetOutput(string command)
        {
            ProcessStartInfo pro = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas"
            };

            try
            {
                Process? process = Process.Start(pro);
                if (process is null)
                {
                    throw new Exception("Falha ao iniciar o processo.");
                }
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (output.ToLower().Contains("negado"))
                {
                    throw new Exception("Execute o Launcher como administrador.");
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar o comando.", ex);
            }
        }

        private static void ExecuteCommand(string command)
        {
            ProcessStartInfo pro = new ProcessStartInfo("cmd.exe", "/C " + command);
            pro.WindowStyle = ProcessWindowStyle.Hidden;
            pro.CreateNoWindow = true;
            Process? process = Process.Start(pro);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        static void CheckForMultipleGames(object? sender, ElapsedEventArgs e)
        {
            string gameProcessName = "GrandFantasia";

            Process[] gameProcesses = Process.GetProcessesByName(gameProcessName);

            if (gameProcesses.Length > 6)
            {
                foreach (var process in gameProcesses)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private async void CheckVersion()
        {
            try
            {
                lbl_status.Text = "Checando arquivos...";
                
                // Primeiro compara GameDataList local vs servidor
                await CompararGameDataListAsync();
                
                var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
                if (filesToUpdate.Count > 0)
                {
                    await DownloadFilesFromServerAsync(filesToUpdate);
                }
                else
                {
                    pb_Start.Enabled = true;
                    timerAnimation.Enabled = true;
                    totalFiles = 1;
                    currentFile = 0;
                    currentFileProgress = 100;
                    Bar1Loader(currentFileProgress);
                    totalProgress = 100;
                    Bar2Loader(totalProgress);
                    lbl_status.Text = "Todos os arquivos estão atualizados.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao checar arquivos: {ex.Message}");
            }
        }

        // Funções legadas de atualização removidas. Atualização agora é feita por arquivo individual via GameDataList.txt

        private static bool IsFileReady(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        private static bool IsImportantFile(string filePath)
        {
            return filePath.Contains("version.txt") || filePath.Contains("client.ini");
        }

        private void Bar1Loader(int value)
        {
            int Sz = pn_porcentagem1.Size.Width;
            int SzBar = pn_pValue1.Size.Height;
            double div = (double)value / 100;
            int TotalSize = (int)(Sz * div);
            pn_pValue1.Size = new System.Drawing.Size(TotalSize, SzBar);
        }

        private void Bar2Loader(int value)
        {
            int Sz = pn_porcentagem2.Size.Width;
            int SzBar = pn_pValue2.Size.Height;
            double div = (double)value / 100;
            int TotalSize = (int)(Sz * div);
            pn_pValue2.Size = new System.Drawing.Size(TotalSize, SzBar);
        }

        private async void pb_scan_Click(object sender, EventArgs e)
        {
            pb_scan.Enabled = false;
            pb_Start.Enabled = false;

            // Primeiro compara GameDataList local vs servidor
            await CompararGameDataListAsync();

            var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
            if (filesToUpdate.Count > 0)
            {
                await DownloadFilesFromServerAsync(filesToUpdate);
            }
            else
            {
                pb_Start.Enabled = true;
                pb_scan.Enabled = true; // Habilita novamente o botão de scan
                timerAnimation.Enabled = true;
                totalFiles = 1;
                currentFile = 0;
                currentFileProgress = 100;
                Bar1Loader(currentFileProgress);
                totalProgress = 100;
                Bar2Loader(totalProgress);
                lbl_status.Text = "Todos os arquivos estão atualizados.";
            }
        }

        private int totalFiles = 10;
        private int currentFile = 0;
        private int currentFileProgress = 0;
        private int totalProgress = 0;
        
        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (DeteleFiles)
            {
                string? pasta = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                if (GameProc != null && GameProc.Handle != IntPtr.Zero)
                {
                    if (CheckMemoryProf(GameProc.Handle, GameIP))
                    {
                        Thread.Sleep(500);
                        this.Close();
                    }
                }
                return;
            }

            Bar1Loader(currentFileProgress);
            Bar2Loader(totalProgress);

            if (Falhas)
            {
                pb_scan.Enabled = true;
            }

            if (Concluido)
            {
                Concluido = false;
            }

            if (currentFile < totalFiles)
            {
                currentFileProgress = (int)((float)(currentFile) / totalFiles * 100);
                totalProgress = (int)((float)(currentFile + 1) / totalFiles * 100);
                Thread.Sleep(1000);
                currentFile++;
            }
        }

        private async void BackgroundDownloaders_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var fails = await WebFunctions.excUpdate();

            if (fails)
            {
                Falhas = fails;
            }

            Concluido = !fails;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize services and version check after form is fully loaded
            InstallService();
            CheckVersion();
            
            pb_scan_Click(sender, e);
        }

        private void pb_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pb_close_MouseHover(object sender, EventArgs e)
        {
            pb_close.Image = Resources.mouseexon;
        }

        private void pb_close_MouseLeave(object sender, EventArgs e)
        {
            pb_close.Image = Resources.mouseexoff;
        }

        private void pb_close_MouseDown(object sender, MouseEventArgs e)
        {
            pb_close.Image = Resources.mouseexenter;
        }

        private void pb_scan_MouseLeave(object sender, EventArgs e)
        {
            pb_scan.Image = Resources.EscanearLeave;
        }

        private void pb_scan_MouseDown(object sender, MouseEventArgs e)
        {
            pb_scan.Image = Resources.EscanearDown;
        }

        private void pb_scan_MouseEnter(object sender, EventArgs e)
        {
            pb_scan.Image = Resources.EscanearEnter;
        }

        private void pb_scan_MouseHover(object sender, EventArgs e)
        {
            pb_scan.Image = Resources.EscanearEnter;
        }

        private void pb_Start_MouseDown(object sender, MouseEventArgs e)
        {
            pb_Start.Image = Resources.StartDown;
        }

        private void pb_Start_MouseEnter(object sender, EventArgs e)
        {
            pb_Start.Image = Resources.StartEnter;
        }

        private void pb_Start_MouseHover(object sender, EventArgs e)
        {
            pb_Start.Image = Resources.StartEnter;
        }

        private void pb_Start_MouseLeave(object sender, EventArgs e)
        {
            pb_Start.Image = Resources.StartLeave;
        }

        private void pb_discord_MouseDown(object sender, MouseEventArgs e)
        {
            pb_discord.Image = Resources.DiscordDown;
        }

        private void pb_discord_MouseEnter(object sender, EventArgs e)
        {
            pb_discord.Image = Resources.DiscordEnter;
        }

        private void pb_discord_MouseHover(object sender, EventArgs e)
        {
            pb_discord.Image = Resources.DiscordEnter;
        }

        private void pb_discord_MouseLeave(object sender, EventArgs e)
        {
            pb_discord.Image = Resources.DiscordLeave;
        }

        private void pb_options_MouseDown(object sender, MouseEventArgs e)
        {
            pb_options.Image = Resources.OptionsDown;
        }

        private void pb_options_MouseEnter(object sender, EventArgs e)
        {
            pb_options.Image = Resources.OptionsEnter;
        }

        private void pb_options_MouseHover(object sender, EventArgs e)
        {
            pb_options.Image = Resources.OptionsEnter;
        }

        private void pb_options_MouseLeave(object sender, EventArgs e)
        {
            pb_options.Image = Resources.OptionsLeave;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsDraggingForm = true;
                MousePos = e.Location;
            }
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraggingForm)
            {
                var temp = Location;
                temp.X += e.X - MousePos.X;
                temp.Y += e.Y - MousePos.Y;
                this.Location = temp;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                IsDraggingForm = false;
        }

        static string ExtractDownloadUrl(string html)
        {
            string pattern = @"Download file";
            string x = html.Split(pattern, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Split("href=")[1].Split("id=")[0].Trim();
            return x.Replace("\"", "");
        }
        
        private void pb_discord_Click(object sender, EventArgs e)
        {
            string url = "https://discord.gg/ZQXb2MZder";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void pb_options_Click(object sender, EventArgs e)
        {
            HudConfigs x = new HudConfigs();
            x.ShowDialog();
        }

        private async void pb_Start_Click(object sender, EventArgs e)
        {
            bool ret = await ChecarAnims();

            if (ret)
            {
                var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
                if (filesToUpdate.Count == 0)
                {
                    // Check if accounts.txt exists before trying to read it
                    if (!File.Exists("accounts.txt"))
                    {
                        File.WriteAllText("accounts.txt", "");
                    }
                    string[] accounts = File.ReadAllLines("accounts.txt");
                    string? selectedAccount = null ?? string.Empty;
                    foreach (var account in accounts)
                    {
                        if (account.EndsWith(":S"))
                        {
                            selectedAccount = account;
                            break;
                        }
                    }
                    string login;
                    string password;
                    string parameter = "EasyFun";
                    if (selectedAccount != null && selectedAccount != string.Empty)
                    {
                        string[] accountParts = selectedAccount.Split(':');
                        login = accountParts[0];
                        password = accountParts[1];
                        parameter = $"EasyFun -a {login} -p {password}";
                    }
                    string? pasta = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                    string programName = "GrandFantasia.exe";
                    string fullGamePath = Path.Combine(Application.StartupPath, programName);
                    if (!File.Exists(fullGamePath))
                    {
                        MessageBox.Show("O jogo Grand Fantasia não foi encontrado. Por favor, faça o download primeiro usando o botão 'Escanear'.");
                        return;
                    }
                    ProcessStartInfo startInfo = new ProcessStartInfo(fullGamePath, parameter)
                    {
                        WorkingDirectory = pasta,
                        UseShellExecute = true
                    };
                    GameProc = Process.Start(startInfo);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Existem arquivos desatualizados ou ausentes. Por favor, escaneie e atualize antes de iniciar o jogo.");
                }
            }
        }

        // NOVO: Função para comparar arquivos locais com GameDataList.txt (tamanho, data, hash)
        private static async Task<List<string>> GetFilesToUpdateFromGameDataListAsync()
        {
            var filesToUpdate = new List<string>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string manifestUrl = "https://gamearkadia.com.br/Update/GameDataList.txt";
                    string manifestContent = await client.GetStringAsync(manifestUrl);
                    var manifestLines = manifestContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in manifestLines)
                    {
                        // Exemplo: \readme.txt:20250720012519:6:8466086389
                        var parts = line.Split(':');
                        if (parts.Length < 4) continue;
                        string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
                        string remoteDateStr = parts[1].Trim();
                        string remoteSizeStr = parts[2].Trim();
                        string remoteHash = parts[3].Trim();

                        string localPath = Path.Combine(Application.StartupPath, fileName);
                        if (!File.Exists(localPath))
                        {
                            filesToUpdate.Add(fileName);
                            continue;
                        }
                        var localInfo = new FileInfo(localPath);
                        // Tamanho
                        if (localInfo.Length.ToString() != remoteSizeStr)
                        {
                            filesToUpdate.Add(fileName);
                            continue;
                        }
                        // Data de modificação (formato: yyyyMMddHHmmss)
                        if (remoteDateStr.Length == 14)
                        {
                            DateTime remoteDate;
                            if (DateTime.TryParseExact(remoteDateStr, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out remoteDate))
                            {
                                // Tolerância de 2 segundos para sistemas de arquivos diferentes
                                var diff = Math.Abs((localInfo.LastWriteTime - remoteDate).TotalSeconds);
                                if (diff > 2)
                                {
                                    filesToUpdate.Add(fileName);
                                    continue;
                                }
                            }
                        }
                        // Hash MD5 como número (compatível com script Python)
                        string localHash = GetFileMD5AsNumber(localPath);
                        if (!string.Equals(localHash, remoteHash, StringComparison.OrdinalIgnoreCase))
                        {
                            filesToUpdate.Add(fileName);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao comparar arquivos: {ex.Message}");
            }
            return filesToUpdate;
        }

        // Calcula o hash SHA256 de um arquivo
        private static string GetFileHash(string filePath)
        {
            try
            {
                using (var stream = File.OpenRead(filePath))
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        // NOVO: Função para baixar arquivos do servidor
        private async Task DownloadFilesFromServerAsync(List<string> filesToUpdate)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    totalFiles = filesToUpdate.Count;
                    currentFile = 0;
                    foreach (var fileName in filesToUpdate)
                    {
                        lbl_status.Text = $"Baixando {fileName}...";
                        string fileUrl = $"https://gamearkadia.com.br/Update/{fileName}";
                        string localPath = Path.Combine(Application.StartupPath, fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath) ?? Application.StartupPath);
                        using (var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await stream.CopyToAsync(fs);
                            }
                        }
                        currentFile++;
                        currentFileProgress = (int)((float)(currentFile) / totalFiles * 100);
                        Bar1Loader(currentFileProgress);
                        totalProgress = (int)((float)(currentFile) / totalFiles + 100);
                        Bar2Loader(totalProgress);
                    }
                    lbl_status.Text = "Atualização concluída!";
                    pb_Start.Enabled = true;
                    pb_scan.Enabled = true; // Habilita novamente o botão de scan
                }
            }
            catch (Exception ex)
            {
                lbl_status.Text = $"Erro ao baixar arquivos: {ex.Message}";
                pb_scan.Enabled = true; // Habilita novamente mesmo em caso de erro
            }
        }

        private async Task<bool> ChecarAnims()
        {
            // Verifica se há falhas ou se ainda está processando
            if (Falhas)
            {
                return false;
            }
            
            // Verifica se as animações/progress bars estão completas
            if (currentFile >= totalFiles && currentFileProgress >= 100 && totalProgress >= 100)
            {
                return true;
            }
            
            // Aguarda um pouco para as animações terminarem
            await Task.Delay(100);
            
            // Retorna true se não há downloads em andamento
            return !timerAnimation.Enabled || (currentFile >= totalFiles);
        }

        // NOVO: Função para gerar GameDataList.txt local baseado nos arquivos atuais
        private async Task<bool> GerarGameDataListLocalAsync()
        {
            try
            {
                var fileList = new List<string>();
                string gameDir = Application.StartupPath;
                
                // Percorre todos os arquivos na pasta do jogo
                foreach (string filePath in Directory.GetFiles(gameDir, "*", SearchOption.AllDirectories))
                {
                    var fileInfo = new FileInfo(filePath);
                    
                    // Pula arquivos do launcher e temporários
                    string fileName = Path.GetFileName(filePath);
                    if (fileName.EndsWith(".exe") && fileName.Contains("Launcher") ||
                        fileName == "GameDataList.txt" ||
                        fileName.EndsWith(".tmp") ||
                        fileName.EndsWith(".log"))
                        continue;
                    
                    string relativePath = Path.GetRelativePath(gameDir, filePath);
                    string relativePathWin = "\\" + relativePath.Replace("/", "\\");
                    
                    // Formato: yyyyMMddHHmmss
                    string timestamp = fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss");
                    long size = fileInfo.Length;
                    
                    // Usa MD5 como no script Python
                    string hash = GetFileMD5AsNumber(filePath);
                    
                    string line = $"{relativePathWin}:{timestamp}:{size}:{hash}";
                    fileList.Add(line);
                }
                
                string localPath = Path.Combine(Application.StartupPath, "GameDataList.txt");
                await File.WriteAllLinesAsync(localPath, fileList);
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar GameDataList.txt local: {ex.Message}");
                return false;
            }
        }
        
        // Calcula MD5 e converte para número de 10 dígitos (como no script Python)
        private static string GetFileMD5AsNumber(string filePath)
        {
            try
            {
                using (var stream = File.OpenRead(filePath))
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var hash = md5.ComputeHash(stream);
                    string hexString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    
                    // Converte hex para BigInteger e depois modulo 10^10 para 10 dígitos
                    var bigInt = System.Numerics.BigInteger.Parse("0" + hexString, System.Globalization.NumberStyles.HexNumber);
                    var result = bigInt % (System.Numerics.BigInteger.Pow(10, 10));
                    return result.ToString();
                }
            }
            catch
            {
                return "0";
            }
        }

        // NOVO: Função para comparar o GameDataList.txt local e do servidor
        private async Task CompararGameDataListAsync()
        {
            string localPath = Path.Combine(Application.StartupPath, "GameDataList.txt");
            if (!File.Exists(localPath))
            {
                // Se não existe, gera um novo baseado nos arquivos locais
                lbl_status.Text = "Gerando GameDataList.txt local...";
                bool generated = await GerarGameDataListLocalAsync();
                if (!generated)
                {
                    lbl_status.Text = "Falha ao gerar GameDataList.txt local.";
                    return;
                }
                lbl_status.Text = "GameDataList.txt local gerado.";
            }
            
            try
            {
                string localContent = await File.ReadAllTextAsync(localPath);
                var localLines = localContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var localDict = new Dictionary<string, string>();
                foreach (var line in localLines)
                {
                    var parts = line.Split(':');
                    if (parts.Length < 4) continue;
                    string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
                    localDict[fileName] = line.Trim();
                }

                using (HttpClient client = new HttpClient())
                {
                    string manifestUrl = "https://gamearkadia.com.br/Update/GameDataList.txt";
                    string serverContent = await client.GetStringAsync(manifestUrl);
                    var serverLines = serverContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    var serverDict = new Dictionary<string, string>();
                    foreach (var line in serverLines)
                    {
                        var parts = line.Split(':');
                        if (parts.Length < 4) continue;
                        string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
                        serverDict[fileName] = line.Trim();
                    }

                    // Conta diferenças silenciosamente
                    int diferencas = 0;
                    foreach (var kv in serverDict)
                    {
                        if (!localDict.ContainsKey(kv.Key) || localDict[kv.Key] != kv.Value)
                        {
                            diferencas++;
                        }
                    }
                    foreach (var kv in localDict)
                    {
                        if (!serverDict.ContainsKey(kv.Key))
                        {
                            diferencas++;
                        }
                    }

                    if (diferencas > 0)
                    {
                        lbl_status.Text = $"Encontradas {diferencas} diferenças nos arquivos.";
                    }
                    else
                    {
                        lbl_status.Text = "GameDataList local e servidor estão sincronizados.";
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_status.Text = $"Erro ao comparar manifests: {ex.Message}";
            }
        }
    }
}