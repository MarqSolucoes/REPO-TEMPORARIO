# WH_Engenharia_API

## Arquitetura

A API foi fragmentada por domínio de negócio em 6 serviços independentes:

| Serviço | Porta | Responsabilidade | Controllers |
|---------|-------|------------------|-------------|
| `WHEngenharia.API.Auth` | 5501 | Autenticação e Usuários | AuthController, UsuarioController, CargoController, HistoricoController |
| `WHEngenharia.API.Cadastro` | 5502 | Cadastros Básicos | CategoriaMaterialController, CidadeController, ClienteController, CondicaoPagamentoController, FornecedorController, FuncionarioController, MaterialController |
| `WHEngenharia.API.Obra` | 5503 | Gestão de Obras | ObraController, ObraControleCustoController, ObraMedicaoController, AgendaController, RelatorioControleETOController |
| `WHEngenharia.API.Financeiro` | 5504 | Financeiro | FinanceiroController, FluxoCaixaController, FluxoCaixaSaldoInicialController, FaturamentoController, NotaFiscalController, RelatorioFinanceiroController |
| `WHEngenharia.API.Compras` | 5505 | Gestão de Compras | PedidoCompraController, PedidoCompraNotaFiscalController, SolicitacaoCompraController, SolicitacaoCompraRascunhoController, StatusSolicitacaoCompraController, MotivoDevolucaoController |
| `WHEngenharia.API.PedidoInterno` | 5506 | Pedidos Internos | PedidoInternoController, PedidoInternoRecorrenteController, DefController, ServicesController |

## Projetos Compartilhados

Todos os serviços acima referenciam os mesmos projetos de infraestrutura:

- `WHEngenharia.SQL` — contexto EF Core e repositórios
- `WHEngenharia.Dominio` — modelos e DTOs
- `WHEngenharia.Servicos` — lógica de negócio

## Jobs (Background Services)

- `WHEngenharia.Services.PedidoInternoRecorrente`
- `WHEngenharia.Services.PrevisaoFluxoCaixa`
- `WHEngenharia.Services.AjusteDataFaturamento`

## API Legada

O projeto `WHEngenharia.API` (porta 5500) é mantido para compatibilidade retroativa enquanto a migração dos clientes para os novos serviços por domínio não for concluída.
