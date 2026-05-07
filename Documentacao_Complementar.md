# Documentação Complementar — WHEngenharia ERP

> **Classificação:** Interno — restrito à equipe de desenvolvimento  
> **Versão:** 1.0 — Maio de 2026  
> **Repositório de referência:** `MarqSolucoes/REPO-TEMPORARIO`

---

## Índice

1. [Visão Geral do Sistema](#1-visão-geral-do-sistema)
2. [Estrutura dos Repositórios](#2-estrutura-dos-repositórios)
3. [Estrutura Detalhada do Backend (WH_Engenharia_API)](#3-estrutura-detalhada-do-backend-wh_engenharia_api)
4. [Estrutura Detalhada do Frontend (WH_Engenharia_DashBoard)](#4-estrutura-detalhada-do-frontend-wh_engenharia_dashboard)
5. [Fluxo de Autenticação](#5-fluxo-de-autenticação)
6. [Guia para Novos Desenvolvedores](#6-guia-para-novos-desenvolvedores)
7. [Passo a Passo — Configuração do Ambiente](#7-passo-a-passo--configuração-do-ambiente)
8. [Variáveis de Configuração](#8-variáveis-de-configuração)
9. [Convenções e Padrões do Projeto](#9-convenções-e-padrões-do-projeto)
10. [Módulos Funcionais](#10-módulos-funcionais)
11. [Problemas Conhecidos e Débitos Técnicos](#11-problemas-conhecidos-e-débitos-técnicos)

---

## 1. Visão Geral do Sistema

O **WHEngenharia ERP** é um sistema de gestão empresarial desenvolvido para a WH Engenharia. Ele cobre os seguintes domínios de negócio:

| Módulo | Descrição |
|--------|-----------|
| **Compras** | Solicitações de compra, pedidos, notas fiscais, fornecedores |
| **Obras** | Gestão de obras, medições, controle de custo, ETO |
| **Financeiro** | Fluxo de caixa, faturamento, agenda financeira, DEFs |
| **RH** | Funcionários, cargos |
| **Configurações** | Usuários, permissões, materiais, clientes, etc. |
| **Relatórios** | Relatório financeiro, controle ETO, agenda |

**Arquitetura geral:**

```
┌─────────────────────────────────┐      HTTP (porta 5500)       ┌─────────────────────────────────┐
│  WH_Engenharia_DashBoard        │ ───────────────────────────► │  WH_Engenharia_API              │
│  (Vue.js 2 — porta 3000)        │                              │  (ASP.NET Core 5 — porta 5500)  │
└─────────────────────────────────┘                              └──────────────┬──────────────────┘
                                                                                │
                                                                                ▼
                                                                 ┌──────────────────────────────────┐
                                                                 │  SQL Server                      │
                                                                 │  (Azure VM — porta 1469)         │
                                                                 └──────────────────────────────────┘
```

---

## 2. Estrutura dos Repositórios

O sistema está dividido em **dois repositórios Git separados**:

| Repositório | Pasta no REPO-TEMPORARIO | Tecnologia | Porta padrão |
|-------------|--------------------------|------------|--------------|
| `WH_Engenharia_API` | `BACKEND/` | ASP.NET Core 5 (.NET 5) | **5500** |
| `WH_Engenharia_DashBoard` | `FRONT/` | Vue.js 2 (Node.js) | **3000** |

> **Atenção:** No repositório temporário (`REPO-TEMPORARIO`), ambos coexistem em uma única pasta. No ambiente real, cada um é clonado separadamente.

---

## 3. Estrutura Detalhada do Backend (WH_Engenharia_API)

### 3.1 Solution e Projetos

```
WHEngenharia.API.sln
├── WHEngenharia.API/            ← Ponto de entrada (ASP.NET Core Web API)
├── WHEngenharia.Dominio/        ← DTOs e modelos de transferência de dados
├── WHEngenharia.SQL/            ← Acesso a dados (Entity Framework Core + repositórios)
├── WHEngenharia.Servicos/       ← Regras de negócio
├── WHEngenharia.Services.PedidoInternoRecorrente/   ← Windows Service legado
├── WHEngenharia.Services.PrevisaoFluxoCaixa/        ← Windows Service legado
├── WHEngenharia.Services.AjusteDataFaturamento/     ← Windows Service legado
└── WHEngenharia.CustosFixos/    ← Projeto stub (sem implementação ativa)
```

### 3.2 Fluxo de chamada dentro do Backend

```
Controller (recebe HTTP)
    └── Serviço (regras de negócio)
            └── Repositório (Entity Framework Core)
                    └── SQL Server
```

> **Nota importante:** Os serviços são instanciados manualmente com `new` dentro dos controllers (ex.: `new ServicoPedidoCompra(_context, _mapper)`), em vez de serem injetados pelo container de DI. Isso é um débito técnico conhecido.

### 3.3 Estrutura de pastas — WHEngenharia.API

```
WHEngenharia.API/
├── Controllers/         ← 31 controllers (um por domínio)
├── Properties/
│   ├── launchSettings.json
│   └── PublishProfiles/
├── appsettings.json             ← Configuração base (JWT, CORS, diretório de upload)
├── appsettings.Development.json ← String de conexão do BD (dev/local)
├── appsettings.Test.json        ← String de conexão do BD (teste)
├── Program.cs                   ← Ponto de entrada; escuta na porta 5500
├── Startup.cs                   ← Configuração de middlewares, JWT, CORS, Swagger
└── WHEngenharia.API.csproj
```

### 3.4 Estrutura de pastas — WHEngenharia.SQL

```
WHEngenharia.SQL/
├── AutoMapper/          ← Perfis de mapeamento DTO ↔ Entidade
├── Modelos/             ← Entidades do banco (mapeadas via EF Core)
├── Persistencia/        ← IEntityTypeConfiguration por entidade (Fluent API)
├── Repositorios/        ← ~29 repositórios (um por entidade)
├── WHEngenhariaContext.cs  ← DbContext principal
└── WHEngenharia.SQL.csproj
```

### 3.5 Controllers disponíveis

| Controller | Rota base | Domínio |
|------------|-----------|---------|
| `AuthController` | `/auth` | Autenticação JWT |
| `UsuarioController` | `/api/Usuario` | Usuários e senhas |
| `ObraController` | `/api/Obra` | Gestão de obras |
| `PedidoCompraController` | `/api/PedidoCompra` | Pedidos de compra |
| `SolicitacaoCompraController` | `/api/SolicitacaoCompra` | Solicitações de compra |
| `FaturamentoController` | `/api/Faturamento` | Faturamentos |
| `FluxoCaixaController` | `/api/FluxoCaixa` | Fluxo de caixa |
| `FinanceiroController` | `/api/Financeiro` | Dados financeiros |
| `AgendaController` | `/api/Agenda` | Agenda financeira |
| `MaterialController` | `/api/Material` | Materiais |
| `FornecedorController` | `/api/Fornecedor` | Fornecedores |
| `ClienteController` | `/api/Cliente` | Clientes |
| `FuncionarioController` | `/api/Funcionario` | Funcionários |
| `DefController` | `/api/Def` | Tabela de DEFs |
| `NotaFiscalController` | `/api/NotaFiscal` | Notas fiscais |
| `RelatorioFinanceiroController` | `/api/RelatorioFinanceiro` | Relatórios financeiros |
| `RelatorioControleETOController` | `/api/RelatorioControleETO` | Relatório ETO |
| `ServicesController` | `/api/Services` | Acionamento de Windows Services |
| *(+ outros)* | | |

### 3.6 Dependências NuGet principais

| Pacote | Versão | Finalidade |
|--------|--------|-----------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 5.0.17 | ORM / acesso ao SQL Server |
| `AutoMapper` | 11.0.1 | Mapeamento DTO ↔ Entidade |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 5.0.17 | Autenticação JWT |
| `Swashbuckle.AspNetCore` | 6.3.1 | Swagger / documentação de API |
| `Aspose.Cells` | 24.8.0 | Geração de planilhas Excel |
| `Newtonsoft.Json` | 13.0.3 | Serialização JSON |

---

## 4. Estrutura Detalhada do Frontend (WH_Engenharia_DashBoard)

### 4.1 Estrutura de pastas

```
FRONT/
├── public/
├── src/
│   ├── assets/          ← Imagens, ícones, logo
│   ├── components/      ← Componentes reutilizáveis (Sidebar, Widget, etc.)
│   ├── pages/           ← Páginas por módulo:
│   │   ├── Compras/
│   │   ├── Configuracoes/
│   │   ├── Dashboard/
│   │   ├── Financeiro/
│   │   ├── Login/
│   │   ├── Obras/
│   │   ├── RH/
│   │   └── Relatorios/
│   ├── store/           ← Vuex (auth.js, layout.js, register.js)
│   ├── services/
│   │   └── api.service.js  ← Cliente HTTP global (Axios) — 102 KB
│   ├── Routes/          ← Vue Router
│   ├── config.js        ← URL da API por ambiente
│   └── main.js
├── package.json
└── vue.config.js
```

### 4.2 Configuração de URL da API

Arquivo: `src/config.js`

```javascript
const hostApi = process.env.NODE_ENV === "development"
  ? "http://localhost"
  : "http://whengenharia.solinski.com.br";

const portApi = 5500;  // mesma porta em dev e produção
const baseURLApi = `${hostApi}:${portApi}/api`;
```

- **Desenvolvimento local:** `http://localhost:5500/api`
- **Produção:** `http://whengenharia.solinski.com.br:5500/api`

### 4.3 Dependências NPM principais

| Pacote | Finalidade |
|--------|-----------|
| `vue` ^2.7.16 | Framework principal |
| `vuex` | Gerenciamento de estado |
| `vue-router` | Roteamento |
| `axios` | Requisições HTTP |
| `bootstrap-vue` | Componentes UI |
| `js-sha256` | Hash SHA-256 de senha (client-side) |
| `apexcharts` / `chart.js` | Gráficos |
| `@fullcalendar/*` | Calendário da agenda |
| `@amcharts/amcharts4` | Gráficos avançados |

### 4.4 Scripts NPM

| Comando | Descrição |
|---------|-----------|
| `npm run start` | Inicia o servidor de desenvolvimento na porta 3000 (sem backend mock) |
| `npm run start:backend` | Inicia com variável `VUE_APP_BACKEND=true` |
| `npm run build` | Gera build de produção |
| `npm run lint` | Executa o linter ESLint |

---

## 5. Fluxo de Autenticação

```
1. Usuário digita login e senha no navegador
        ↓
2. Frontend aplica SHA-256 na senha (js-sha256, Login.vue:122)
   const senha = sha256(String(this.$refs.senha.value));
        ↓
3. POST /auth/token?user=LOGIN&password=HASH_SHA256
        ↓
4. Backend compara: Usuario.Login == user && Usuario.Senha == hash && HabilitaLogin
   (RepositorioAutenticacao.cs)
        ↓
5. Backend retorna JWT token (válido por tempo definido no Jwt:Key do appsettings)
        ↓
6. Frontend armazena token no localStorage e configura
   axios.defaults.headers.common["Authorization"] = "Bearer " + token;
        ↓
7. Todas as chamadas subsequentes incluem o header Authorization Bearer
```

> **Nota sobre senhas:** As senhas são armazenadas como SHA-256 (gerado no frontend). Não há salt e não há processamento adicional no backend. Para novos desenvolvedores: ao cadastrar um usuário via banco diretamente ou seed, a senha deve ser armazenada como o SHA-256 hexadecimal da senha desejada (64 caracteres hex).

---

## 6. Guia para Novos Desenvolvedores

### 6.1 Pré-requisitos de conhecimento

Antes de começar, é recomendável ter familiaridade com:

- **Backend:** C#, ASP.NET Core, Entity Framework Core, SQL Server, JWT
- **Frontend:** JavaScript (ES6+), Vue.js 2, Vuex, Axios, HTML/CSS

### 6.2 Entendendo o fluxo de uma feature típica

Para implementar uma nova funcionalidade (ex.: um novo módulo de "Contratos"):

**No Backend (WH_Engenharia_API):**

1. Criar o modelo de banco em `WHEngenharia.SQL/Modelos/Contrato.cs`
2. Criar a configuração EF em `WHEngenharia.SQL/Persistencia/ContratoConfiguration.cs`
3. Registrar no `WHEngenhariaContext.cs` (adicionar `DbSet<Contrato>`)
4. Criar o DTO em `WHEngenharia.Dominio/Modelos/ContratoDTO.cs`
5. Criar o repositório em `WHEngenharia.SQL/Repositorios/RepositorioContrato.cs`
6. Criar o serviço em `WHEngenharia.Servicos/ServicoContrato.cs`
7. Criar o controller em `WHEngenharia.API/Controllers/ContratoController.cs`
8. Adicionar o mapeamento em `WHEngenharia.SQL/AutoMapper/AutoMapperConfig.cs`

**No Frontend (WH_Engenharia_DashBoard):**

1. Criar a página em `src/pages/Contratos/Contrato.vue`
2. Adicionar as chamadas de API em `src/services/api.service.js`
3. Registrar a rota em `src/Routes/`
4. Adicionar o link no menu lateral (Sidebar)

### 6.3 Adicionando um novo endpoint

```csharp
// WHEngenharia.API/Controllers/ContratoController.cs
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContratoController : ControllerBase
{
    private IMapper _mapper;
    private WHEngenhariaContext _context;
    private ServicoContrato _servicoContrato;

    public ContratoController(WHEngenhariaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _servicoContrato = new ServicoContrato(_context, _mapper); // padrão atual do projeto
    }

    [HttpGet]
    public async Task<ActionResult<List<ContratoDTO>>> Get()
    {
        return Ok(await _servicoContrato.Get());
    }
}
```

### 6.4 Adicionando uma chamada de API no Frontend

```javascript
// src/services/api.service.js — padrão existente
getContratos: (result) => {
  axios
    .get("/Contrato")
    .then((apiReturn) => {
      result({ status: apiReturn.status, data: apiReturn.data });
    })
    .catch((error) => {
      result({ status: error.response.status, message: error.response.data });
    });
},
```

### 6.5 Consultando o Swagger

Com a API rodando, acesse:

```
http://localhost:5500/swagger
```

Todos os endpoints são listados automaticamente. É possível executar chamadas diretamente pela interface.

### 6.6 Verificando as permissões de usuário

As permissões ficam na tabela `Usuario` como colunas boolean (ex.: `UsuarioVisualizarCompras`, `UsuarioAprovadorCompras`). No frontend, o objeto completo do usuário logado é armazenado em `localStorage` com a chave `usuarioDTO`. Para verificar uma permissão:

```javascript
const usuario = JSON.parse(localStorage.getItem('usuarioDTO'));
if (usuario.usuarioVisualizarCompras) {
  // exibir menu de compras
}
```

---

## 7. Passo a Passo — Configuração do Ambiente

### 7.1 Requisitos de software

| Software | Versão mínima | Download |
|----------|---------------|----------|
| .NET SDK | **5.0** | https://dotnet.microsoft.com/download/dotnet/5.0 |
| Node.js | **14.x ou 16.x** | https://nodejs.org |
| npm | **6.x+** (vem com o Node) | — |
| SQL Server | 2016+ ou Azure SQL | https://www.microsoft.com/sql-server |
| Git | qualquer | https://git-scm.com |
| Visual Studio 2019/2022 ou VS Code | — | — |

> **Verificação rápida:**
> ```bash
> dotnet --version   # deve retornar 5.0.x
> node --version     # deve retornar v14.x ou v16.x
> npm --version
> ```

---

### 7.2 Configuração do Backend (WH_Engenharia_API)

#### Passo 1 — Clonar o repositório

```bash
git clone https://github.com/MarqSolucoes/WH_Engenharia_API.git
cd WH_Engenharia_API
```

#### Passo 2 — Configurar a string de conexão

Crie ou edite o arquivo `WHEngenharia.API/appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SEU_SERVIDOR;Initial Catalog=WHEngenharia;User Id=SEU_USUARIO;Password=SUA_SENHA;MultipleActiveResultSets=True;"
  }
}
```

> Substitua `SEU_SERVIDOR`, `SEU_USUARIO` e `SUA_SENHA` pelas credenciais do SQL Server local ou do ambiente compartilhado.

#### Passo 3 — Restaurar o banco de dados

O script de criação do banco está em `Documentos/WHEngenharia.sql`.  
Execute-o no SQL Server Management Studio (SSMS) ou via linha de comando:

```bash
sqlcmd -S SEU_SERVIDOR -U SEU_USUARIO -P SUA_SENHA -i "Documentos/WHEngenharia.sql"
```

Se houver dados de carga inicial, execute também `Documentos/WHEngenharia - Dados.sql`.

#### Passo 4 — Restaurar pacotes NuGet e compilar

```bash
cd WHEngenharia.API       # pasta que contém o .sln fica um nível acima
cd ..
dotnet restore WHEngenharia.API.sln
dotnet build WHEngenharia.API.sln
```

#### Passo 5 — Iniciar a API

```bash
cd WHEngenharia.API
dotnet run
```

A API estará disponível em:
- **API:** `http://localhost:5500/api`
- **Swagger:** `http://localhost:5500/swagger`

**Ou via Visual Studio:** abrir `WHEngenharia.API.sln` e pressionar **F5** (perfil `IIS Express` sobe na porta 5500).

#### Verificação

Acesse `http://localhost:5500/swagger` no navegador. Você deve ver a lista de todos os endpoints.

---

### 7.3 Configuração do Frontend (WH_Engenharia_DashBoard)

#### Passo 1 — Clonar o repositório

```bash
git clone https://github.com/MarqSolucoes/WH_Engenharia_DashBoard.git
cd WH_Engenharia_DashBoard
```

#### Passo 2 — Instalar dependências

```bash
npm install
```

> O comando pode demorar alguns minutos na primeira execução (mais de 60 pacotes).

#### Passo 3 — Verificar a URL da API

Abra `src/config.js` e confirme que a URL aponta para onde sua API está rodando:

```javascript
const hostApi = process.env.NODE_ENV === "development"
  ? "http://localhost"   // ← deve apontar para onde a API está
  : "http://whengenharia.solinski.com.br";

const portApi = 5500;    // ← porta da API
```

Nenhuma alteração é necessária se você seguiu o passo 7.2 e a API está em `localhost:5500`.

#### Passo 4 — Iniciar o servidor de desenvolvimento

```bash
npm run start
```

O frontend estará disponível em: **`http://localhost:3000`**

#### Passo 5 — Fazer login

Credenciais padrão para o ambiente de desenvolvimento (verifique com o responsável pelo banco):

- O usuário deve existir na tabela `Usuario` do banco
- A senha no banco é armazenada como **SHA-256 hexadecimal** da senha em texto puro
  - Exemplo: senha `admin123` → SHA-256 → `240be518fabd2724ddb6f04eeb1da5967448d7e831186422d11efaf82a6b0e2b` (a confirmar com seu seed)

#### Verificação

Você deve ver a tela de login em `http://localhost:3000`. Após autenticar, o dashboard principal é exibido.

---

### 7.4 Rodando os dois juntos (desenvolvimento local completo)

Abra **dois terminais**:

**Terminal 1 — API:**
```bash
cd WH_Engenharia_API/WHEngenharia.API
dotnet run
# API disponível em http://localhost:5500
```

**Terminal 2 — Frontend:**
```bash
cd WH_Engenharia_DashBoard
npm run start
# Frontend disponível em http://localhost:3000
```

Acesse `http://localhost:3000` no navegador.

---

### 7.5 Build de produção (Frontend)

```bash
npm run build
```

Os arquivos estáticos são gerados em `dist/`. Eles devem ser servidos por um servidor web (ex.: IIS, Nginx, Apache).

---

## 8. Variáveis de Configuração

### 8.1 Backend — appsettings.json

| Chave | Descrição | Exemplo |
|-------|-----------|---------|
| `ConnectionStrings:DefaultConnection` | String de conexão SQL Server | `Data Source=...` |
| `Jwt:Key` | Chave secreta para assinar/verificar tokens JWT | string aleatória longa |
| `Jwt:Issuer` | Identificador do emissor do token | `InventarioNeTAuthenticationServer` |
| `Jwt:Audience` | Audiência esperada do token | `InventarioNeTPostmanClient` |
| `CorsOrigins` | Origens permitidas pelo CORS (separadas por vírgula) | `http://localhost:3000,...` |
| `DiretorioUpload` | Caminho no servidor para salvar arquivos enviados | `C:\WHEngenharia\Upload` |
| `ExtensoesPermitidasUpload` | Extensões de arquivo permitidas no upload | `.PDF,.XLSX,...` |

> **Boas práticas:** Nunca commite credenciais reais no repositório. Use variáveis de ambiente ou `appsettings.{Ambiente}.json` adicionado ao `.gitignore`.

### 8.2 Frontend — src/config.js

| Variável | Descrição |
|----------|-----------|
| `hostApi` | Host da API (varia por `NODE_ENV`) |
| `portApi` | Porta da API (`5500`) |
| `baseURLApi` | URL base completa (`host:porta/api`) |

---

## 9. Convenções e Padrões do Projeto

### 9.1 Nomenclatura

| Elemento | Padrão | Exemplo |
|----------|--------|---------|
| Controllers | `{Entidade}Controller` | `PedidoCompraController` |
| Serviços | `Servico{Entidade}` | `ServicoPedidoCompra` |
| Repositórios | `Repositorio{Entidade}` | `RepositorioPedidoCompra` |
| DTOs | `{Entidade}DTO` | `PedidoCompraDTO` |
| Entidades SQL | `{Entidade}` (sem sufixo) | `PedidoCompra` |
| Configurações EF | `{Entidade}Configuration` | `PedidoCompraConfiguration` |
| Páginas Vue | `{NomePagina}.vue` (PascalCase) | `SolicitacaoCompra.vue` |

### 9.2 Idioma do código

- **Código-fonte (C# e JavaScript):** Português (variáveis, métodos, classes)
- **Configurações e infraestrutura:** Inglês

### 9.3 Autenticação nas rotas

- Todas as rotas do backend exigem `[Authorize]` por padrão
- Exceções explícitas usam `[AllowAnonymous]` (ex.: `/auth/token`)
- No frontend, o token JWT é enviado em todas as requisições via header `Authorization: Bearer {token}`

### 9.4 Tratamento de erros

O padrão atual dos controllers é:

```csharp
try { ... }
catch (Exception ex) { return BadRequest(ex.Message); }
```

Exceções não são logadas. Ao adicionar código novo, prefira incluir logging explícito.

---

## 10. Módulos Funcionais

### 10.1 Compras

- **Solicitação de Compra:** criação, aprovação em múltiplos estágios, rascunho
- **Pedido de Compra:** vínculo com fornecedor, itens, notas fiscais
- **Nota Fiscal:** registro e vínculo com pedidos

### 10.2 Obras

- **Cadastro de Obra:** dados do cliente, datas, engenheiro responsável
- **Medição:** registro de medições por obra
- **Controle de Custo:** lançamentos de custo por obra
- **ETO (Extrato de Tudo Obra):** relatório consolidado

### 10.3 Financeiro

- **Fluxo de Caixa:** lançamentos por DEF (código de classificação financeira)
- **Agenda Financeira:** visualização de pagamentos e recebimentos por data
- **Faturamento:** emissão e controle de faturas
- **DEF:** tabela de códigos de classificação financeira

### 10.4 Relatórios

- Relatório Financeiro (gerado em Excel via Aspose.Cells)
- Controle ETO
- Relatório de Agenda

### 10.5 Configurações

- Usuários e permissões
- Materiais e categorias
- Clientes, fornecedores, funcionários
- Condições de pagamento

---

## 11. Problemas Conhecidos e Débitos Técnicos

Esta seção documenta limitações conhecidas para que novos desenvolvedores não sejam surpreendidos.

| # | Problema | Impacto | Local |
|---|----------|---------|-------|
| 1 | Credenciais de BD nos arquivos `appsettings.*.json` commitados | Segurança crítica | Repositório |
| 2 | Senha SMTP hardcoded no código | Segurança crítica | `ServicoEmail.cs` |
| 3 | JWT signing key commitada | Segurança crítica | `appsettings.json` |
| 4 | Senhas armazenadas como SHA-256 sem salt (client-side) | Segurança moderada | `Login.vue`, BD |
| 5 | Hash SHA-256 enviado como query string em `AlterarSenha` | Segurança moderada | `UsuarioController.cs` |
| 6 | Serviços instanciados com `new` em vez de injeção de dependência | Acoplamento, testabilidade | Todos os controllers |
| 7 | Nenhum logging de exceções | Diagnóstico difícil | Todos os controllers |
| 8 | Vue.js 2 em EOL (fim de vida desde dez/2023) | Segurança, manutenibilidade | Frontend |
| 9 | Sem testes automatizados | Qualidade | Projeto inteiro |
| 10 | Sem EF Migrations — schema gerenciado manualmente | Risco de inconsistência | BD |
| 11 | Endpoints de Windows Services sem autenticação (`AllowAnonymous`) | Segurança | `ServicesController.cs` |
| 12 | Bug de cálculo duplicado no Fluxo de Caixa (DEF 03.33 somado 3x) | Dados incorretos | `ServicoFluxoCaixa.cs` |
| 13 | Infraestrutura hospedada em conta Azure pessoal do desenvolvedor | Risco operacional | Azure |
| 14 | CORS completamente aberto em produção | Segurança | `Startup.cs` |

---

*Documento criado em maio de 2026 com base na inspeção direta do código-fonte do repositório `MarqSolucoes/REPO-TEMPORARIO`.*  
*Mantenha este documento atualizado à medida que o sistema evolui.*
