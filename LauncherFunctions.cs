using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace GFLauncher
{

    internal class LauncherFunctions
    {
        private bool DownloadSuccess = false;
        private bool DownloadFailed = false;
        private bool DownloadCancelled = false;

        private List<string> allLines = new List<string>();
        private List<int[]> Versions = new List<int[]>();

        private HttpClient Client = new HttpClient();

        private int CountVersionsUpdate = 0;
        private int CountUpdateDone     = 0;



        // Publicação para progress bar futuras
        public float    ProgressOne = 0;
        public float    ProgressTwo = 0;


        public string FailStatusStr = "";
        // Verificações de nova versão e que versão
        public int[]    CheckVersion = { 0, 0 };
        public bool     NewVersion = false;


        static string ExtractDownloadUrl(string html)
        {
            string pattern = @"Download file";
            string x = html.Split(pattern, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Split("href=")[1].Split("id=")[0].Trim();
            return x.Replace("\"", "");
        }


        public void ExtractorFile(string fileName)
        {

            byte[] signature = { 80, 75, 3, 4 }; // Assinatura do arquivo ZIP
            byte[] fileBytes = File.ReadAllBytes(fileName);
            if (fileBytes.Length >= 4 && fileBytes[0] == signature[0] && fileBytes[1] == signature[1] && fileBytes[2] == signature[2] && fileBytes[3] == signature[3])
            {
                // extrair o arquivo zip baixado
                if (File.Exists(fileName) && Path.GetExtension(fileName).ToLower() == ".zip")
                {
                    //ZipFile.ExtractToDirectory(download_path, Application.StartupPath);
                    using (ZipArchive archive = ZipFile.Open(fileName, ZipArchiveMode.Update))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string entryPath = Path.Combine(Application.StartupPath, entry.FullName);
                            // se o arquivo já existe, sobrescreve
                            entry.ExtractToFile(entryPath, true);
                        }
                    }
                    File.Delete(fileName);

                }
            }
            else
            {
                File.Delete(fileName);
                MessageBox.Show("Ocorreu um erro durante o download a pagina não devolveu o arquivo");
            }

        }

        public async Task<bool> DownloadFile(string url)
        {
            url = "https://www.gamearkadia.com.br/Update/update.zip";


            HttpResponseMessage response = await Client.GetAsync(url);

            //|| response.Content.Headers.ContentType.MediaType != "application/zip"
            if (response.Content.Headers.ContentType == null )
            {
                DownloadFailed = true;

                FailStatusStr = ("O arquivo não é um arquivo zip." + url);
                return DownloadFailed;

            }

            var StrFileName = Path.GetFileName(url);

            if (response.Content.Headers.ContentType.MediaType != "text/html")
            {
                if(response.Content.Headers.ContentLength == null)
                {
                    DownloadFailed = true;
                    FailStatusStr =  ("A pagina não devolveu um Zip");
                    return DownloadFailed;
                }


                long totalBytes = response.Content.Headers.ContentLength.Value;
                byte[] buffer = new byte[4096];
                Stream stream = new FileStream(StrFileName, FileMode.Create);

                using (var streamReader = await response.Content.ReadAsStreamAsync())
                {
                    int bytesRead = 0;
                    long bytesDownloaded = 0;
                    do
                    {
                        bytesRead = await streamReader.ReadAsync(buffer, 0, buffer.Length);
                        bytesDownloaded += bytesRead;
                        float percentage = (bytesDownloaded / totalBytes) * 100;
                        ProgressOne = percentage;
                        Console.WriteLine(ProgressOne);
                        stream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                }

                stream.Close();
                response.Dispose();

                ExtractorFile(StrFileName);
            }
            else
            {
                DownloadFailed = true;
                FailStatusStr = ("O arquivo não é um arquivo zip.");
            }

            return DownloadFailed;

        }



        private async Task DownloadStrings(string url, int[] version)
        {
            HttpResponseMessage response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resposta = await response.Content.ReadAsStringAsync();
                string[] lines = resposta.Split(new string[] { "<br>" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (line == string.Empty)
                        continue;


                    var web_version = line.Split('|').FirstOrDefault();
                    var new_url = line.Split('|').Last().Trim();

                    if (web_version == null || web_version == string.Empty)
                        continue;

                    var v = web_version.Split(".");
                    CheckVersion[0] = Convert.ToInt32(v[0].Trim());
                    CheckVersion[1] = Convert.ToInt32(v[1].Trim());

                    if ((CheckVersion[0] > version[0]) || (CheckVersion[0] >= version[0] && CheckVersion[1] > version[1]))
                    {
                        string downloadUrl = "";
                        using (HttpClient client = new HttpClient())
                        {
                            downloadUrl = new_url; // client.GetStringAsync(new_url).Result.Trim();
                            //string html = client.GetStringAsync(new_url).Result;
                            //downloadUrl = ExtractDownloadUrl(html);
                        }

                        allLines.Add( downloadUrl ); 
                        Versions.Add( CheckVersion );

                        NewVersion = true;
                        CountVersionsUpdate++;
                    }
                }
            }
            else
            {
                DownloadCancelled = true;
            }

            // Adiciona o return da tarefa
            return;
        }



        public async Task<bool> GetInternetVersion(string url, int[] version)
        {
            DownloadCancelled = false;
            NewVersion = false;
            CountVersionsUpdate = 0;
            CountUpdateDone = 0;
            await DownloadStrings(url, version);

            return NewVersion;

        }

        public int[] ActualVersionWritter()
        {
            //if(CountUpdateDone == CountVersionsUpdate)
            //    return Versions[CountUpdateDone-1];

            return Versions[CountUpdateDone];
        }


        public async Task<bool> excUpdate()
        {
            
            if (allLines.Count > 0)
            {
                foreach (var item in allLines)
                {
                   var x =  await DownloadFile(item);
                   if(x)
                   { break; }



                    CountUpdateDone++;
                   ProgressTwo = (CountVersionsUpdate / CountUpdateDone) * 100;
                }
            }

            return DownloadFailed;
        }

    }
}
