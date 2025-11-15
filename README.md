#  Mini Loja PHP + Unity

Este repositório contém os arquivos necessários para rodar uma mini loja utilizando PHP e JSON, que pode ser acessada por um jogo na Unity.

## Alunos integrantes da equipe

* João Pedro Torres 
* Mateus Henrique Medeiros Diniz

## Professores responsáveis

* Roque Anderson Saldanha Teixeira

## Pré-requisitos

Antes de rodar o projeto, você precisará ter o XAMPP instalado na sua máquina.

### Como baixar e instalar o XAMPP

1. **Baixar o XAMPP:**
   - Acesse o site oficial do [XAMPP](https://www.apachefriends.org/pt_br/download.html).
   - Escolha a versão do XAMPP de acordo com o seu sistema operacional (Windows, Linux, ou macOS).
   - Clique em "Download" para baixar o instalador.

2. **Instalar o XAMPP:**
   - Após o download, execute o instalador e siga as instruções na tela.
   - Durante a instalação, escolha os componentes que deseja instalar. Para esse projeto, apenas o Apache é necessário.

3. **Iniciar o XAMPP:**
   - Após a instalação, abra o painel de controle do XAMPP.
   - Inicie os serviços **Apache** clicando em "Start" ao lado de cada um.

## Estrutura de Diretórios

Depois de instalar e iniciar o XAMPP, você precisa colocar os arquivos do projeto dentro do diretório correto para que o servidor Apache consiga rodar os arquivos PHP.

### Passo 1: Localize o diretório "htdocs"
- No seu computador, vá até o diretório onde o XAMPP foi instalado. Por padrão, ele está localizado em:
  - **Windows:** `C:\xampp\htdocs`
  - **macOS/Linux:** `/Applications/XAMPP/htdocs`

### Passo 2: Coloque os arquivos do projeto

- Copie todos os arquivos que estão na pasta **`XAMPP`** (o código PHP, arquivos de banco de dados, etc.) para dentro da pasta **`htdocs`**.
- A estrutura final dentro de **`htdocs`** deve ser algo como:

```text
htdocs/
├─ getAll.php      # Retorna todos os itens disponíveis para compra
├─ item.php        # Realiza a compra de um item
├─ items.json      # Lista de todos os itens da loja
└─ player.json     # Armazena a quantidade de moedas do jogador
