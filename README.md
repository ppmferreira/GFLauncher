# GFLauncher

Um launcher moderno para Grand Fantasia, desenvolvido em C# WinForms, com atualização automática, verificação de integridade e integração com WebView2.

## Recursos

- Atualização automática de arquivos do jogo e do próprio launcher
- Verificação de integridade dos arquivos via GameDataList.txt
- Download incremental de arquivos ausentes ou desatualizados
- Barra de progresso e status detalhado
- Interface moderna com WebView2 para notícias
- Suporte a múltiplas contas (accounts.txt)
- Autoatualização do GFLauncher.exe e GFLauncher.dll (sem travar)

## Como usar

1. **Extraia todos os arquivos do pacote na pasta do jogo**
2. Execute `GFLauncher.exe` como administrador
3. O launcher irá:
   - Verificar e atualizar arquivos do jogo
   - Baixar arquivos ausentes
   - Atualizar a si mesmo se necessário (reinicie após update)
   - Exibir notícias e status
4. Clique em **Escanear** para forçar uma verificação manual
5. Clique em **Iniciar** para abrir o jogo

## Estrutura de arquivos

- `GFLauncher.exe` — O launcher principal
- `GFLauncher.dll` — Biblioteca do launcher
- `GameDataList.txt` — Lista de integridade dos arquivos
- `accounts.txt` — Contas salvas (opcional)
- `version.txt`, `client.ini` — Arquivos importantes do jogo
- `WebView2` — Pasta de dependências do navegador embutido

## Atualização automática

- O launcher baixa novas versões de si mesmo como `.new` (ex: `GFLauncher.exe.new`)
- Ao iniciar, se `.new` existir, faz backup do antigo e troca automaticamente
- Não é necessário baixar manualmente novas versões

## Requisitos

- Windows 7/8/10/11
- .NET Framework 4.7.2 ou superior
- WebView2 Runtime (instalado automaticamente se necessário)

Desenvolvido por Pedro Ferreira.
