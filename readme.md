# 🛒 GPS Store - Sistema de E-commerce

Este projeto é uma solução completa de E-commerce com gestão de pedidos, carteira digital (cashback) e painel administrativo, desenvolvido como teste técnico para Analista de Sistemas Pleno.

---

## 🚀 Tecnologias Utilizadas

- **Backend:** .NET 6 (C#), Entity Framework Core, SQLite.
- https://builds.dotnet.microsoft.com/dotnet/Sdk/6.0.428/dotnet-sdk-6.0.428-win-x64.exe
- **Frontend:** Angular 18, Standalone Components, Signals.
- **Testes:** xUnit.
- **Arquitetura:** Clean Architecture, Repository Pattern, SOLID.

---

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado em sua máquina:

1.  **[.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)** (ou superior).
2.  **[Node.js](https://nodejs.org/)** (Versão LTS recomendada).
3.  **Angular CLI** (Opcional, mas recomendado: `npm install -g @angular/cli`).

---

## 🔧 Como Rodar o Projeto (Passo a Passo)

Siga os passos abaixo para iniciar o Backend e o Frontend separadamente.

### Passo 1: Configurando o Backend (API)

O Backend é responsável pela regra de negócio e banco de dados SQLite.

1.  Abra o terminal e entre na pasta do servidor:
    ```bash
    cd BackEnd
    ```

2.  Restaure as dependências do projeto:
    ```bash
    dotnet restore
    ```

3.  Crie/Atualize o Banco de Dados (SQLite):
    ```bash
    # Instala a ferramenta do EF (caso não tenha)
    dotnet tool install --global dotnet-ef

    # Executa as migrations para criar o arquivo .db
    dotnet ef database update
    ```

4.  Inicie a API:
    ```bash
    dotnet run
    ```
    > A API estará rodando em: `https://localhost:50932` (Swagger disponível).

---

### Passo 2: Configurando o Frontend (Web)

O Frontend é a interface visual feita em Angular. Mantenha o terminal do Backend aberto e **abra um novo terminal**.

1.  Entre na pasta da interface:
    ```bash
    cd InterfaceGraphics
    ```

2.  Instale as dependências (pode demorar alguns minutos na primeira vez):
    ```bash
    npm install
    ```

3.  Inicie o servidor de desenvolvimento:
    ```bash
    npm start
    ```
    > O comando `-o` abrirá automaticamente o navegador em `http://localhost:4200`.

---

## 🧪 Como Rodar os Testes Unitários

O projeto possui testes automatizados para validar as regras de negócio (ex: cálculo de totais, status de pedidos).

1.  No terminal, vá para a pasta de testes (se houver uma pasta separada `Ecommerce.Tests` na raiz):
    ```bash
    cd Ecommerce.Tests
    dotnet test
    ```
    *(Caso os testes estejam dentro do BackEnd, rode o comando dentro da pasta BackEnd).*

---

## 🔑 Acesso ao Sistema

O sistema possui uma rotina de "Seed" (população de dados) que cria um usuário administrador e produtos iniciais automaticamente na primeira execução.

- **Usuário Admin:** `admin@gps.com`
- **Senha:** `admin123`
- **Cliente Exemplo:** `joao@teste.com` (Senha criada no cadastro ou via Seed).

Você também pode criar uma conta nova clicando em **"Novo Cliente"** na tela de login ou no menu lateral.

---

## ✨ Funcionalidades Principais

1.  **Vitrine Virtual:** Listagem de produtos com ordenação (mais novos primeiro) e busca.
2.  **Carteira de Cashback:** Pagamento de pedidos usando saldo e extrato de movimentações.
3.  **Gestão de Pedidos:** Fluxo completo (Criado -> Pago -> Entregue/Cancelado).
4.  **Admin:** Cadastro de Produtos e Clientes com tabelas de listagem.
5.  **Instalador Visual:** Wizard estilo Windows para setup inicial do banco.

---

### 📄 Licença

Este projeto foi desenvolvido para fins de avaliação técnica.
