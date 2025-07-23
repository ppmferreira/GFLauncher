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

            // Inicialização normal do WebView2
            try
            {
                var webView = new Microsoft.Web.WebView2.WinForms.WebView2();
                webView.Dock = DockStyle.Fill;
                webView.Source = new Uri("https://gamearkadia.com.br/news/");
                pn_web.Controls.Add(webView);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao inicializar WebView2: {ex.Message}");
            }

            // Cria o Timer normalmente
            checkTimer = new System.Timers.Timer(5000);
            checkTimer.Elapsed += (object? sender, ElapsedEventArgs e) => CheckForMultipleGames(sender, e);
            checkTimer.AutoReset = true;
            checkTimer.Start();
        }

        private void InstallService()
        {
            try
            {
                var query = "sc query GrandFantasia";
                string retorno = ExecuteCommandAndGetOutput(query);
                
                if(!retorno.Contains("RUNNING")) 
                {
                    // Atualiza status usando Invoke para thread safety
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() => lbl_status.Text = "Checando servicos..."));
                    }

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
                        if (this.IsHandleCreated && !this.IsDisposed)
                        {
                            this.BeginInvoke(new Action(() => lbl_status.Text = "Serviço GrandFantasia não encontrado - será instalado após download do jogo."));
                        }
                        return;
                    }
                }
                else
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() => lbl_status.Text = "Serviço GrandFantasia já está em execução."));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't exit the application
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.BeginInvoke(new Action(() => lbl_status.Text = "Aviso: Erro ao verificar serviço - " + ex.Message));
                }
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
            try
            {
                string gameProcessName = "GrandFantasia";
                Process[] gameProcesses = Process.GetProcessesByName(gameProcessName);

                if (gameProcesses.Length > 6)
                {
                    // Para o timer para evitar reentrância
                    if (checkTimer != null)
                        checkTimer.Stop();

                    foreach (var process in gameProcesses)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (Exception)
                        {
                            // Ignora erros ao matar processos
                        }
                    }

                    // Reinicia o timer após a operação
                    if (checkTimer != null)
                        checkTimer.Start();
                }
            }
            catch (Exception ex)
            {
                // Loga o erro, mas não deixa o timer travar o app
                Debug.WriteLine($"Erro no CheckForMultipleGames: {ex.Message}");
            }
        }

        private async void CheckVersion()
        {
            // Executa todo o fluxo pesado em thread separada para nunca travar a UI
            _ = Task.Run(async () =>
            {
                try
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() => lbl_status.Text = "Checando arquivos..."));
                    }
                    // Primeiro compara GameDataList local vs servidor
                    await CompararGameDataListAsync();
                    var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
                    if (filesToUpdate.Count > 0)
                    {
                        await DownloadFilesFromServerAsync(filesToUpdate);
                    }
                    else
                    {
                        if (this.IsHandleCreated && !this.IsDisposed)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                pb_Start.Enabled = true;
                                pb_scan.Enabled = true;
                                // Não ativa timerAnimation, apenas atualiza as barras e status
                                totalFiles = 1;
                                currentFile = 1;
                                currentFileProgress = 100;
                                Bar1Loader(currentFileProgress);
                                totalProgress = 100;
                                Bar2Loader(totalProgress);
                                lbl_status.Text = "Todos os arquivos estão atualizados.";
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() => MessageBox.Show($"Erro ao checar arquivos: {ex.Message}")));
                    }
                }
            });
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

            // Executa todo o fluxo pesado em thread separada para nunca travar a UI
            _ = Task.Run(async () =>
            {
                this.BeginInvoke(new Action(() => lbl_status.Text = "Reavaliando arquivos locais..."));
                bool result = await GerarGameDataListLocalAsync((fileAtual, idx, total) => {
                    if (this.IsHandleCreated)
                    {
                        bool update = false;
                        if (idx % 500 == 0 || idx == total)
                            update = true;
                        if (update)
                        {
                            this.BeginInvoke(new Action(() => {
                                lbl_status.Text = $"Reavaliando arquivos locais... ({idx}/{total})";
                                currentFile = idx;
                                totalFiles = total;
                                currentFileProgress = (int)((float)idx / total * 100);
                                Bar1Loader(currentFileProgress);
                            }));
                        }
                    }
                });

                if (!result)
                {
                    this.BeginInvoke(new Action(() => {
                        lbl_status.Text = "Falha ao reavaliar arquivos locais.";
                        pb_scan.Enabled = true;
                        pb_Start.Enabled = true;
                    }));
                    return;
                }

                // Agora compara GameDataList local vs servidor
                await CompararGameDataListAsync();

                var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
                if (filesToUpdate.Count > 0)
                {
                    // Dispara o download em background, não espera terminar
                    _ = DownloadFilesFromServerAsync(filesToUpdate);
                }
                else
                {
                    this.BeginInvoke(new Action(() => {
                        pb_Start.Enabled = true;
                        pb_scan.Enabled = true; // Habilita novamente o botão de scan
                        totalFiles = 1;
                        currentFile = 1;
                        currentFileProgress = 100;
                        totalProgress = 100;
                        Bar1Loader(100);
                        Bar2Loader(100);
                        lbl_status.Text = "Todos os arquivos estão atualizados.";
                    }));
                }
            });
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
                        // Remove Thread.Sleep que trava a UI - usar timer para delay
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

            // Remove o Thread.Sleep(1000) que causa travamento da UI
            // O timer já controla o intervalo de atualização
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


private async void Form1_Load(object sender, EventArgs e)
{
    // Mostra a janela imediatamente
    this.Show();
    lbl_status.Text = "Escaneando arquivos...";
    lbl_status.Refresh();
    pb_scan.Enabled = false;
    pb_Start.Enabled = false;
    totalFiles = 1;
    currentFile = 0;
    currentFileProgress = 0;
    Bar1Loader(0);

    // Executa todo o fluxo pesado em thread separada para nunca travar a UI
    _ = Task.Run(async () =>
    {
        DateTime lastUiUpdate = DateTime.Now;
        int lastScanIdx = 0;
        bool result = await GerarGameDataListLocalAsync((fileAtual, idx, total) =>
        {
            if (this.IsHandleCreated)
            {
                bool update = false;
                if (idx % 10 == 0 || (DateTime.Now - lastUiUpdate).TotalMilliseconds > 100 || idx == total)
                {
                    lastUiUpdate = DateTime.Now;
                    update = true;
                }
                if (update)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        lbl_status.Text = $"Escaneando arquivos... ({idx}/{total})";
                        currentFile = idx;
                        totalFiles = total;
                        currentFileProgress = (int)((float)idx / total * 100);
                        Bar1Loader(currentFileProgress);
                        Bar2Loader(100);
                        lastScanIdx = idx;
                    }));
                }
            }
        });

        if (!result)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    lbl_status.Text = "Falha ao gerar GameDataList.txt local.";
                    currentFileProgress = 0;
                    Bar1Loader(0);
                    pb_scan.Enabled = true;
                    pb_Start.Enabled = true;
                }));
            }
            return;
        }

        // 2. Compara com o servidor e baixa arquivos se necessário
        if (this.IsHandleCreated)
        {
            this.BeginInvoke(new Action(() =>
            {
                lbl_status.Text = "Comparando arquivos com o servidor...";
            }));
        }
        await CompararGameDataListAsync();

        // Busca arquivos desatualizados
        var filesToUpdate = await GetFilesToUpdateFromGameDataListAsync();
        if (filesToUpdate.Count > 0)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    lbl_status.Text = $"Baixando {filesToUpdate.Count} arquivos do servidor...";
                    pb_scan.Enabled = false;
                    pb_Start.Enabled = false;
                }));
            }
            await DownloadFilesFromServerAsync(filesToUpdate);
        }
        else
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    currentFileProgress = 100;
                    totalProgress = 100;
                    Bar1Loader(100);
                    Bar2Loader(100);
                    pb_scan.Enabled = true;
                    pb_Start.Enabled = true;
                    lbl_status.Text = "Pronto para jogar!";
                }));
            }
        }
    });
}

        // Wrapper para rodar CheckVersion como async Task
        private async Task CheckVersionAsyncWrapper()
        {
            await Task.Yield(); // Garante que não bloqueia a UI
            CheckVersion();
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
            // Desabilita o botão para evitar múltiplos cliques
            pb_Start.Enabled = false;
            lbl_status.Text = "Iniciando jogo...";

            try
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
                    pb_Start.Enabled = true;
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
            catch (Exception ex)
            {
                lbl_status.Text = $"Erro ao iniciar o jogo: {ex.Message}";
                pb_Start.Enabled = true;
            }
        }

        // NOVO: Função otimizada: compara apenas os manifests (linhas), sem acessar o disco para cada arquivo
        private static async Task<List<string>> GetFilesToUpdateFromGameDataListAsync()
        {
            var filesToUpdate = new List<string>();
            try
            {
                // Lê o manifest local
                string localPath = Path.Combine(Application.StartupPath, "GameDataList.txt");
                if (!File.Exists(localPath))
                {
                    // Se não existe, gera um novo baseado nos arquivos locais
                    bool generated = await GerarGameDataListLocalAsync();
                    if (!generated)
                        return filesToUpdate;
                }
                var localLines = await File.ReadAllLinesAsync(localPath);
                var localDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var line in localLines)
                {
                    var parts = line.Split(':');
                    if (parts.Length < 4) continue;
                    string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
                    localDict[fileName] = line.Trim();
                }

                // Lê o manifest do servidor
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    string manifestUrl = "https://gamearkadia.com.br/Update/GameDataList.txt";
                    string manifestContent = await client.GetStringAsync(manifestUrl);
                    var manifestLines = manifestContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in manifestLines)
                    {
                        var parts = line.Split(':');
                        if (parts.Length < 4) continue;
                        string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
                        string serverLine = line.Trim();
                        if (!localDict.ContainsKey(fileName) || localDict[fileName] != serverLine)
                        {
                            filesToUpdate.Add(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao comparar manifests: {ex.Message}");
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
        int maxParallel = 3; // Limite de downloads simultâneos
        using (HttpClient client = new HttpClient())
        using (SemaphoreSlim semaphore = new SemaphoreSlim(maxParallel))
        {
            totalFiles = filesToUpdate.Count;
            currentFile = 0;
            object progressLock = new object();
            var tasks = new List<Task>();
            string currentFileName = "";
            int currentFilePercent = 0;
            bool[] fileDone = new bool[filesToUpdate.Count];

            for (int i = 0; i < filesToUpdate.Count; i++)
            {
                int idx = i;
                string fileName = filesToUpdate[idx];
                var task = Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        string fileUrl = $"https://gamearkadia.com.br/Update/{fileName}";
                        string localPath = Path.Combine(Application.StartupPath, fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath) ?? Application.StartupPath);

                        string downloadPath = localPath;

                        using (var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var fs = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] buffer = new byte[81920];
                                long totalRead = 0;
                                long totalLength = 0;
                                if (response.Content.Headers.ContentLength.HasValue)
                                    totalLength = response.Content.Headers.ContentLength.Value;

                                int read;
                                int lastPercent = 0;
                                while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
                                {
                                    await fs.WriteAsync(buffer.AsMemory(0, read));
                                    totalRead += read;
                                    if (totalLength > 0)
                                    {
                                        int percent = (int)((double)totalRead / totalLength * 100);
                                        if (percent != lastPercent)
                                        {
                                            lastPercent = percent;
                                            // Atualiza progresso local em memória, não na UI
                                            lock (progressLock)
                                            {
                                                currentFileName = fileName;
                                                currentFilePercent = percent;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (progressLock)
                        {
                            currentFileName = fileName;
                        }
                        this.Invoke(new Action(() => lbl_status.Text = $"Erro ao baixar {fileName}: {ex.Message}"));
                    }
                    finally
                    {
                        lock (progressLock)
                        {
                            currentFile++;
                            currentFileProgress = (int)((float)(currentFile) / totalFiles * 100);
                            fileDone[idx] = true;
                        }
                        semaphore.Release();
                    }
                });
                tasks.Add(task);
            }

            // Atualiza UI centralizadamente enquanto houver downloads em andamento
            await Task.Run(async () =>
            {
                while (true)
                {
                    int doneCount;
                    string fileNow;
                    int percentNow;
                    lock (progressLock)
                    {
                        doneCount = 0;
                        foreach (var done in fileDone) if (done) doneCount++;
                        fileNow = currentFileName;
                        percentNow = currentFilePercent;
                    }
                    this.Invoke(new Action(() =>
                    {
                        if (!string.IsNullOrEmpty(fileNow))
                            lbl_status.Text = $"Baixando: {fileNow}";
                        Bar1Loader(currentFileProgress);
                        Bar2Loader(percentNow);
                        totalProgress = currentFileProgress;
                    }));
                    if (doneCount >= filesToUpdate.Count)
                        break;
                    await Task.Delay(100);
                }
            });

            await Task.WhenAll(tasks);

            currentFileProgress = 100;
            totalProgress = 100;
            this.Invoke(new Action(() =>
            {
                Bar1Loader(100);
                Bar2Loader(100);
                lbl_status.Text = "Atualização concluída!";
                pb_Start.Enabled = true;
                pb_scan.Enabled = true;
            }));
        }
    }
    catch (Exception ex)
    {
        this.Invoke(new Action(() =>
        {
            lbl_status.Text = $"Erro ao baixar arquivos: {ex.Message}";
            pb_scan.Enabled = true;
        }));
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

        // NOVO: Função para gerar GameDataList.txt local baseado nos arquivos atuais, com callback de progresso (agora com paralelismo)
        private static async Task<bool> GerarGameDataListLocalAsync(Action<string, int, int>? onProgress = null)
        {
            try
            {
                string gameDir = Application.StartupPath;
                var arquivos = Directory.GetFiles(gameDir, "*", SearchOption.AllDirectories);
                int total = arquivos.Length;
                int idx = 0;
                var fileList = new System.Collections.Concurrent.ConcurrentBag<string>();
                object progressLock = new object();

                // Processa arquivos em paralelo, limitando o grau de paralelismo para evitar sobrecarga de disco
                int maxDegree = Math.Min(Environment.ProcessorCount, 8); // Limite máximo de threads
                await Task.Run(() =>
                {
                    Parallel.ForEach(arquivos, new ParallelOptions { MaxDegreeOfParallelism = maxDegree }, filePath =>
                    {
                        FileInfo fileInfo;
                        try
                        {
                            fileInfo = new FileInfo(filePath);
                        }
                        catch
                        {
                            return;
                        }

                        string fileName = Path.GetFileName(filePath);
                        // Exclui GameDataList.txt, accounts.txt, arquivos temporários, logs, o próprio launcher e sua DLL
                        if (fileName == "GameDataList.txt" ||
                            fileName.Equals("accounts.txt", StringComparison.OrdinalIgnoreCase) ||
                            fileName.EndsWith(".tmp") ||
                            fileName.EndsWith(".log") ||
                            fileName.Equals("GFLauncher.exe", StringComparison.OrdinalIgnoreCase) ||
                            fileName.Equals("GFLauncher.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }

                        string relativePath = Path.GetRelativePath(gameDir, filePath);
                        string relativePathWin = "\\" + relativePath.Replace("/", "\\");
                        string timestamp;
                        long size;

                        try
                        {
                            timestamp = fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss");
                            size = fileInfo.Length;
                        }
                        catch
                        {
                            return;
                        }

                        string hash;
                        try
                        {
                            hash = GetFileMD5AsNumber(filePath);
                        }
                        catch
                        {
                            hash = "0";
                        }

                        string line = $"{relativePathWin}:{timestamp}:{size}:{hash}";
                        fileList.Add(line);

                        // Atualiza UI a cada 50 arquivos para não sobrecarregar
                        lock (progressLock)
                        {
                            idx++;
                            if (onProgress != null && (idx % 50 == 0 || idx == total))
                            {
                                onProgress(relativePathWin, idx, total);
                            }
                        }
                    });
                });

                // Ordena por caminho relativo para manter ordem estável
                var orderedList = fileList.ToList();
                orderedList.Sort(StringComparer.OrdinalIgnoreCase);
                string localPath = Path.Combine(Application.StartupPath, "GameDataList.txt");
                await File.WriteAllLinesAsync(localPath, orderedList);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao gerar GameDataList.txt local: {ex.Message}");
                return false;
            }
        }
        
        // Calcula MD5 e converte para número de 10 dígitos (como no script Python) - otimizado
        private static string GetFileMD5AsNumber(string filePath)
        {
            try
            {
                // Para arquivos muito grandes (>100MB), pula o hash para não travar
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > 100 * 1024 * 1024) // 100MB
                {
                    return fileInfo.Length.ToString();
                }
                
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
                    // Não atualiza lbl_status aqui, apenas retorna (pode logar se quiser)
                    // Se quiser, pode logar: Debug.WriteLine($"Diferenças encontradas: {diferencas}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao comparar manifests: {ex.Message}");
            }
        }


private static async Task ProcessGameDataFileListAsync()
{
    try
    {
        // 1. Obter o IP do connect.ini ou locate.ini
        string ip = GetServerIP();
        if (string.IsNullOrEmpty(ip))
        {
            throw new Exception("IP do servidor não encontrado.");
        }

        // 2. Baixar o GameDataFileList.txt
        string gameDataFileListUrl = "https://gamearkadia.com.br/Update/GameDataList.txt";
        string gameDataFileListContent;
        using (HttpClient client = new HttpClient())
        {
            gameDataFileListContent = await client.GetStringAsync(gameDataFileListUrl);
        }

        // 3. Processar cada linha do arquivo (formato: \filename:timestamp:size:checksum)
        var lines = gameDataFileListContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            throw new Exception("GameDataFileList.txt está vazio.");
        }

        var filesToDownload = new List<string>();
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length < 4) continue;

            string fileName = parts[0].TrimStart(new char[] {'\\', '/'}).Trim();
            string remoteDateStr = parts[1].Trim();
            string remoteSizeStr = parts[2].Trim();
            string remoteHash = parts[3].Trim();

            string localFilePath = Path.Combine(Application.StartupPath, fileName);

            if (!File.Exists(localFilePath))
            {
                filesToDownload.Add(fileName);
                continue;
            }

            var fileInfo = new FileInfo(localFilePath);

            // Verifica tamanho primeiro
            if (fileInfo.Length.ToString() != remoteSizeStr)
            {
                filesToDownload.Add(fileName);
                continue;
            }

            // Para arquivos grandes, só verifica tamanho
            if (fileInfo.Length > 50 * 1024 * 1024) // 50MB
            {
                continue;
            }

            // Verifica data de modificação (tolerância de 2 segundos)
            if (remoteDateStr.Length == 14)
            {
                DateTime remoteDate;
                if (DateTime.TryParseExact(remoteDateStr, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out remoteDate))
                {
                    var diff = Math.Abs((fileInfo.LastWriteTime - remoteDate).TotalSeconds);
                    if (diff > 2)
                    {
                        filesToDownload.Add(fileName);
                        continue;
                    }
                }
            }

            // Verifica hash (compatível com método do launcher)
            string localHash = GetFileMD5AsNumber(localFilePath);
            if (!string.Equals(localHash, remoteHash, StringComparison.OrdinalIgnoreCase))
            {
                filesToDownload.Add(fileName);
                continue;
            }
        }

        // 4. Fazer download dos arquivos necessários
        await DownloadFilesAsync(filesToDownload, ip);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Erro ao processar GameDataFileList: {ex.Message}");
    }
}

private static string GetServerIP()
{
    string connectIniPath = Path.Combine(Application.StartupPath, "connect.ini");
    string locateIniPath = Path.Combine(Application.StartupPath, "locate.ini");

    if (File.Exists(connectIniPath))
    {
        return File.ReadAllText(connectIniPath).Trim();
    }
    else if (File.Exists(locateIniPath))
    {
        return File.ReadAllText(locateIniPath).Trim();
    }

    return string.Empty;
}

private static async Task DownloadFilesAsync(List<string> filesToDownload, string ip)
{
    using (HttpClient client = new HttpClient())
    {
        int maxParallelDownloads = Environment.ProcessorCount > 2 ? 3 : 1;
        var semaphore = new SemaphoreSlim(maxParallelDownloads);
        var tasks = new List<Task>();

        foreach (var file in filesToDownload)
        {
            await semaphore.WaitAsync();

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    string fileUrl = $"http://{ip}/{file}";
                    string localFilePath = Path.Combine(Application.StartupPath, file);

                    // Criar diretório se não existir
                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath) ?? string.Empty);

                    // Baixar arquivo
                    var fileBytes = await client.GetByteArrayAsync(fileUrl);
                    await File.WriteAllBytesAsync(localFilePath, fileBytes);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao baixar arquivo {file}: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
}
    }
}