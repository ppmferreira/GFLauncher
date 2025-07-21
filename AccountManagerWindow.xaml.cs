using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace GFLauncher
{
    /// <summary>
    /// Lógica interna para AccountManagerWindow.xaml
    /// </summary>
    public partial class AccountManagerWindow : Window
    {
        private List<string> accounts;
        private string filePath = "accounts.txt"; // Caminho do arquivo na raiz do projeto

        public AccountManagerWindow()
        {
            InitializeComponent();
            accounts = new List<string>();

            // Carregar contas do arquivo se ele já existir
            if (File.Exists(filePath))
            {
                accounts.AddRange(File.ReadAllLines(filePath));
            }

            UpdateAccountList();
        }

        // Função MD5 para criptografar a senha
        public string MD5_encode(string str_encode)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, insira o login e a senha.");
                return;
            }

            // Criptografa a senha antes de salvar
            string passwordMD5 = MD5_encode(password);

            // Adiciona a nova conta no formato "Login:senhaMD5:estado"
            string account = $"{login}:{passwordMD5}:N";
            accounts.Add(account);

            // Salva no arquivo .txt
            File.AppendAllText(filePath, account + Environment.NewLine);

            // Limpa os campos de login e senha após adicionar
            txtLogin.Clear();
            txtPassword.Clear();

            UpdateAccountList();
        }

        private void btnRemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lstAccounts.SelectedItem != null)
            {
                string selectedAccount = lstAccounts.SelectedItem.ToString();
                accounts.Remove(selectedAccount);

                // Atualiza o arquivo .txt removendo a conta selecionada
                File.WriteAllLines(filePath, accounts);

                UpdateAccountList();
            }
            else
            {
                MessageBox.Show("Selecione uma conta para remover.");
            }
        }

        private void btnSelectAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lstAccounts.SelectedItem != null)
            {
                string selectedAccount = lstAccounts.SelectedItem.ToString();
                string[] accountParts = selectedAccount.Split(':');
                string login = accountParts[0];
                string passwordMD5 = accountParts[1];

                // Atualiza o estado de todas as contas para 'N'
                for (int i = 0; i < accounts.Count; i++)
                {
                    if (accounts[i].Contains($"{login}:{passwordMD5}:N"))
                    {
                        accounts[i] = $"{login}:{passwordMD5}:S"; // Atualiza o estado para 'S' (selecionada)
                    }
                    else
                    {
                        string[] accountDetail = accounts[i].Split(':');
                        accounts[i] = $"{accountDetail[0]}:{accountDetail[1]}:N"; // Define as outras como 'N'
                    }
                }

                // Atualiza o arquivo .txt com as mudanças
                File.WriteAllLines(filePath, accounts);

                txtSelectedAccount.Text = $"Conta selecionada: {login}"; // Atualiza o texto da conta selecionada
            }
            else
            {
                MessageBox.Show("Selecione uma conta.");
                txtSelectedAccount.Text = "Nenhuma conta selecionada."; // Atualiza quando não há conta selecionada
            }

            UpdateAccountList();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Fecha a janela
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove(); // Mover a janela
            }
        }

        private void UpdateAccountList()
        {
            lstAccounts.ItemsSource = null;
            lstAccounts.ItemsSource = accounts;
        }
    }
}
