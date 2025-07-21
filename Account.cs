namespace GFLauncher
{
    public class Account
    {
        public string DisplayName { get; set; }  // Nome para exibir na lista
        public string Login { get; set; }        // Login usado no jogo
        public string PasswordMD5 { get; set; }  // Senha salva em MD5 (para armazenar)
        public string Status { get; set; }       // "S" para selecionado, "N" para n�o selecionado
        public string RawPassword { get; set; }  // Senha em texto puro (usada s� em execu��o, N�O salvar no arquivo)

        public override string ToString()
        {
            return DisplayName; // Para mostrar o nome na ListBox
        }
    }
}
