# GFLauncher

> Um launcher moderno, rápido e seguro para Grand Fantasia, feito em C# WinForms.

[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20me%20a%20coffee-FFDD00?style=for-the-badge&logo=buy-me-a-coffee&logoColor=black)](https://coff.ee/pedroferreiradev)

---

## ✨ Principais Recursos

- Atualização automática dos arquivos do jogo e do próprio launcher
- Verificação de integridade via `GameDataList.txt`
- Download incremental apenas do que mudou
- Barra de progresso e status detalhado
- Interface moderna com WebView2 para notícias
- Suporte a múltiplas contas
- Autoatualização do launcher sem travar

## 🚀 Como usar

1. Extraia todos os arquivos do pacote na pasta do jogo
2. Execute `GFLauncher.exe` como administrador
3. O launcher irá:
   - Verificar e atualizar arquivos do jogo
   - Baixar arquivos ausentes
   - Atualizar a si mesmo se necessário (reinicie após update)
   - Exibir notícias e status
4. Clique em **Escanear** para forçar uma verificação manual
5. Clique em **Iniciar** para abrir o jogo

## 📁 Estrutura de arquivos

- `GFLauncher.exe` — O launcher principal
- `GFLauncher.dll` — Biblioteca do launcher
- `GameDataList.txt` — Lista de integridade dos arquivos
- `accounts.txt` — Contas salvas (opcional)
- `version.txt`, `client.ini` — Arquivos importantes do jogo
- `GFLauncher.exe.WebView2` — Pasta de dependências do navegador embutido (IGNORADA pelo launcher)

## 🔒 Exclusão automática da pasta WebView2

O launcher ignora automaticamente todos os arquivos e subpastas dentro de `GFLauncher.exe.WebView2` ao gerar o `GameDataList.txt`.

**Vantagens:**

- Nenhum arquivo da pasta `GFLauncher.exe.WebView2` será listado ou baixado pelo launcher.
- Reduz erros de acesso e conflitos de arquivos em uso.
- Atualizações mais rápidas e sem downloads desnecessários.

## 🔄 Atualização automática (Ainda vou fazer)

- O launcher baixa novas versões de si mesmo como `.new` (ex: `GFLauncher.exe.new`)
- Ao iniciar, se `.new` existir, faz backup do antigo e troca automaticamente
- Não é necessário baixar manualmente novas versões

## 💻 Requisitos

- Windows 7/8/10/11
- .NET Framework 4.7.2 ou superior
- WebView2 Runtime (instalado automaticamente se necessário)

---

Desenvolvido por Pedro Ferreira — [Buy me a coffee!](https://coff.ee/pedroferreiradev)
