CREATE DATABASE WHEngenharia
GO

USE WHEngenharia
GO

CREATE TABLE Usuario(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdCargo BIGINT NULL,
	Nome NVARCHAR(300) NULL,
	HabilitaLogin BIT NOT NULL,
	Login NVARCHAR(3000) NULL,
	Senha NVARCHAR(3000) NULL,
	Ativo BIT NOT NULL,
	GlobalAprovaNotaFiscal BIT NOT NULL,
	GlobalAprovaCotacao BIT NOT NULL,
	GlobalAprovaSolicitacaoCompra BIT NOT NULL,
	GlobalAprovaPedidoInterno BIT NOT NULL,
	GlobalSolicitacaoCompra BIT NOT NULL,
	GlobalPedidoInterno BIT NOT NULL,
	Cargo BIT NOT NULL,
	CargoCadastrar BIT NOT NULL,
	CargoAtivarDesativar BIT NOT NULL,
	Cidade BIT NOT NULL,
	CidadeCadastrar BIT NOT NULL,
	CidadeEditar BIT NOT NULL,
	CidadeAtivarDesativar BIT NOT NULL,
	Cliente BIT NOT NULL,
	ClienteCadastrar BIT NOT NULL,
	ClienteEditar BIT NOT NULL,
	ClienteAtivarDesativar BIT NOT NULL,
	Fornecedor BIT NOT NULL,
	FornecedorCadastrar BIT NOT NULL,
	FornecedorEditar BIT NOT NULL,
	FornecedorAtivarDesativar BIT NOT NULL,
	Material BIT NOT NULL,
	MaterialCadastrar BIT NOT NULL,
	MaterialEditar BIT NOT NULL,
	MaterialAtivarDesativar BIT NOT NULL,
	MaterialCategoria BIT NOT NULL,
	MaterialCategoriaCadastrar BIT NOT NULL,
	MaterialCategoriaAtivarDesativar BIT NOT NULL,
	Usuarios BIT NOT NULL,
	UsuarioCadastrar BIT NOT NULL,
	UsuarioEditar BIT NOT NULL,
	UsuarioTrocarSenha BIT NOT NULL,
	UsuarioAtivarDesativar BIT NOT NULL,
	UsuarioHabilitarDesabilitarLogin BIT NOT NULL,
	Obra BIT NOT NULL,
	ObraCadastrar BIT NOT NULL,
	ObraHabilitarDesabilitarAprovacaoAutomatica BIT NOT NULL,
	ObraEditar BIT NOT NULL,
	ObraEditarMedicoes BIT NOT NULL,
	ObraEditarETO BIT NOT NULL,
	ObraBloquear BIT NOT NULL,
	ObraRelatorioETO BIT NOT NULL,
	ObraRelatorioContaCorrente BIT NOT NULL,
	ObraRelatorioResumoETO BIT NOT NULL,
	Compras BIT NOT NULL,
	ComprasOrdensCompra BIT NOT NULL,
	ComprasOrdensCompraAnexos BIT NOT NULL,
	ComprasOrdensCompraComentarios BIT NOT NULL,
	ComprasOrdensCompraValidacao BIT NOT NULL,
	ComprasOrdensCompraValidacaoGerenciar BIT NOT NULL,
	ComprasOrdensCompraValidacaoValidar BIT NOT NULL,
	ComprasOrdensCompraValidacaoCancelar BIT NOT NULL,
	ComprasOrdensCompraEmCotacao BIT NOT NULL,
	ComprasOrdensCompraEmCotacaoEditar BIT NOT NULL,
	ComprasOrdensCompraEmCotacaoFinalizar BIT NOT NULL,
	ComprasOrdensCompraEmCotacaoCancelar BIT NOT NULL,
	ComprasOrdensCompraEmCotacaoPDF BIT NOT NULL,
	ComprasOrdensCompraParaAprovacao BIT NOT NULL,
	ComprasOrdensCompraParaAprovacaoAprovar BIT NOT NULL,
	ComprasOrdensCompraParaAprovacaoRejeitar BIT NOT NULL,
	ComprasOrdensCompraParaAprovacaoCancelar BIT NOT NULL,
	ComprasOrdensCompraEmCompra BIT NOT NULL,
	ComprasOrdensCompraEmCompraFinalizar BIT NOT NULL,
	ComprasOrdensCompraEmCompraCancelar BIT NOT NULL,
	ComprasOrdensCompraEmCompraPDF BIT NOT NULL,
	ComprasOrdensCompraFinalizadas BIT NOT NULL,
	ComprasOrdensCompraFinalizadasPDF BIT NOT NULL,
	ComprasOrdensCompraFinalizadasClonar BIT NOT NULL,
	ConciliacaoNotaFiscal BIT NOT NULL,
	ConciliacaoNotaFiscalConciliar BIT NOT NULL,
	ConciliacaoNotaFiscalCancelamentoSaldo BIT NOT NULL,
	Financeiro BIT NOT NULL,
	FinanceiroDef BIT NOT NULL,
	FinanceiroDefHabilitarDesabilitarPI BIT NOT NULL,
	FinanceiroFaturamento BIT NOT NULL,
	FinanceiroFaturamentoEntradaFaturamento BIT NOT NULL,
	FinanceiroFaturamentoEditar BIT NOT NULL,
	FinanceiroFaturamentoInformarRecebimento BIT NOT NULL,
	FinanceiroFaturamentoCancelar BIT NOT NULL,
	FinanceiroFaturamentoAnexos BIT NOT NULL,
	FinanceiroNotaFiscal BIT NOT NULL,
	FinanceiroNotaFiscalInformarValores BIT NOT NULL,
	FinanceiroPedidoInterno BIT NOT NULL,
	FinanceiroPedidoInternoInformarComoPago BIT NOT NULL,
	FinanceiroPedidoInternoEditarData BIT NOT NULL,
	FinanceiroPedidoInternoAnexos BIT NOT NULL,
	FinanceiroPedidoInternoRecorrente BIT NOT NULL,
	FinanceiroPedidoInternoRecorrenteCadastrar BIT NOT NULL,
	FinanceiroPedidoInternoRecorrenteEditar BIT NOT NULL,
	FinanceiroPedidoInternoRecorrenteAtivarDesativar BIT NOT NULL,
	Relatorio BIT NOT NULL,
	RelatorioAgenda BIT NOT NULL,
	RelatorioFaturamento BIT NOT NULL,
	RelatorioControleETO BIT NOT NULL,
	RelatorioControleETOAjustar BIT NOT NULL,
	IdUsuarioCadastro BIGINT NULL,
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NULL,
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Cargo(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(300) NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Cargo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Cargo_UsuarioAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Def(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Codigo NVARCHAR(300) NOT NULL,
	Descricao NVARCHAR(300) NOT NULL,
	PodeAbrirPedidoInterno BIT NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Def_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Def_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Cidade(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UF NVARCHAR(300) NOT NULL,
	Nome NVARCHAR(300) NOT NULL,
	AliquotaImpostoISS FLOAT NOT NULL,
	Ativo BIT NOT NULL
)
GO

CREATE TABLE Cliente(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdCidade BIGINT NOT NULL CONSTRAINT FK_Cliente_Cidade REFERENCES Cidade(Id),
	RazaoSocial NVARCHAR(300) NOT NULL,
	NomeFantasia NVARCHAR(300) NOT NULL,
	CNPJ NVARCHAR(300) NOT NULL,
	InscricaoEstadual NVARCHAR(300) NULL,
	EmailFinanceiro NVARCHAR(300) NULL,
	EmailComercial NVARCHAR(300) NULL,
	ResponsavelComercial NVARCHAR(300) NULL,
	TelefoneCelular NVARCHAR(300) NULL,
	TelefoneFixo NVARCHAR(300) NULL,
	CEP NVARCHAR(300) NULL,
	Endereco NVARCHAR(300) NULL,
	Bairro NVARCHAR(300) NULL,
	Observacao NVARCHAR(300) NULL,
	DiasDePagamento INT NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Cliente_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Cliente_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE TipoFornecedor(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(300) NOT NULL,
)
GO

CREATE TABLE CondicaoPagamento(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(300) NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_CondicaoPagamento_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_CondicaoPagamento_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE CondicaoPagamento_Parcelas(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdCondicaoPagamento BIGINT NOT NULL CONSTRAINT FK_CondicaoPagamentoParcelas_CondicaoPagamento REFERENCES CondicaoPagamento(Id),
	DiasCorridos INT NOT NULL,
	PorcentagemValorTotal FLOAT NOT NULL
)
GO

CREATE TABLE Fornecedor(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTipoFornecedor BIGINT NOT NULL CONSTRAINT FK_Fornecedor_TipoFornecedor REFERENCES TipoFornecedor(Id),
	IdCidade BIGINT NOT NULL CONSTRAINT FK_Fornecedor_Cidade REFERENCES Cidade(Id),
	IdCondicaoPagamento BIGINT NOT NULL CONSTRAINT FK_Fornecedor_CondicaoPagamento REFERENCES CondicaoPagamento(Id),
	RazaoSocial NVARCHAR(300) NULL,
	NomeFantasia NVARCHAR(300) NULL,
	CNPJ NVARCHAR(300) NULL,
	InscricaoEstadual NVARCHAR(300) NULL,
	NumeroCadastral NVARCHAR(300) NULL,
	Nome NVARCHAR(300) NULL,
	CPF NVARCHAR(300) NULL,
	Email NVARCHAR(300) NULL,
	NomeVendedor NVARCHAR(300) NULL,
	TelefoneCelular NVARCHAR(300) NULL,
	TelefoneFixo NVARCHAR(300) NULL,
	CEP NVARCHAR(300) NULL,
	Endereco NVARCHAR(300) NULL,
	Bairro NVARCHAR(300) NULL,
	Observacao NVARCHAR(300) NULL,
	Banco NVARCHAR(300) NULL,
	Agencia NVARCHAR(300) NULL,
	Conta NVARCHAR(300) NULL,
	TipoConta NVARCHAR(300) NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Fornecedor_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Fornecedor_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Obra(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdCliente BIGINT NOT NULL CONSTRAINT FK_Obra_Cliente REFERENCES Cliente(Id),
	IdCidade BIGINT NOT NULL CONSTRAINT FK_Obra_Cidade REFERENCES Cidade(Id),
	IdUsuarioDiretorAprovador BIGINT NOT NULL CONSTRAINT FK_Obra_UsuarioDiretorAprovador REFERENCES Usuario(Id),
	Codigo NVARCHAR(300) NOT NULL,
	CodigoSequencia INT NOT NULL,
	CodigoAno INT NOT NULL,
	CodigoProposta NVARCHAR(300) NOT NULL,
	NumeroPedidoCliente NVARCHAR(300) NOT NULL,
	CEP NVARCHAR(300) NOT NULL,
	EnderecoObra NVARCHAR(300) NOT NULL,
	Descricao NVARCHAR(300) NOT NULL,
	DataProposta DATETIME NOT NULL,
	DataInicio DATETIME NOT NULL,
	DataFim DATETIME NOT NULL,
	PrazoDias INT NOT NULL,
	DiasDePagamento INT NOT NULL,
	ValorTotal FLOAT NOT NULL,
	ValorTotalAjustado FLOAT NOT NULL,
	ValorCusto FLOAT NOT NULL,
	ValorCustoAjustado FLOAT NOT NULL,
	ValorMaterial FLOAT NOT NULL,
	ValorNaoComissionado FLOAT NOT NULL,
	ValorCustoGasto FLOAT NULL,
	AliquotaImpostoISS FLOAT NOT NULL,
	AliquotaImpostoINSS FLOAT NULL,
	AliquotaImpostoIR FLOAT NULL,
	AliquotaImpostoArt30 FLOAT NULL,
	ValorSinal FLOAT NULL,
	PercentualEquivalenteSinal FLOAT NULL,
	DataRecebimentoSinal DATETIME NULL,
	AprovacaoAutomatica BIT NOT NULL,
	Bloqueada BIT NOT NULL,
	Finalizada BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Obra_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Obra_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Obra_Ajuste(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdObra BIGINT NULL CONSTRAINT FK_ObraAjuste_Obra REFERENCES Obra(Id),
	Observacao NVARCHAR(300) NOT NULL,
	Visualizado BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_ObraAjuste_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_ObraAjuste_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE ObraControleCusto(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdObra BIGINT NULL CONSTRAINT FK_ObraControleCusto_Obra REFERENCES Obra(Id),
	DataPrevista DATETIME NOT NULL,
	ValorPrevisto FLOAT NULL,
	ValorPrevistoAjustado FLOAT NULL,
	ValorMedido FLOAT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_ObraControleCusto_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_ObraControleCusto_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE ObraMedicao(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdObra BIGINT NULL CONSTRAINT FK_ObraMedicao_Obra REFERENCES Obra(Id),
	IdObraAjuste BIGINT NULL CONSTRAINT FK_ObraMedicao_ObraAjuste REFERENCES Obra_Ajuste(Id),
	DataPrevista DATETIME NOT NULL,
	DataPrevistaRecebimento DATETIME NOT NULL,
	ValorPrevisto FLOAT NULL,
	ValorPrevistoAjustado FLOAT NULL,
	ValorFaturado FLOAT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_ObraMedicao_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_ObraMedicao_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Obra_UsuarioAprovacao(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdObra BIGINT NULL CONSTRAINT FK_ObraUsuarioAprovacao_Obra REFERENCES Obra(Id),
	IdUsuarioAprovacao BIGINT NOT NULL CONSTRAINT FK_ObraUsuarioAprovacao_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
)
GO

CREATE TABLE PedidoInternoRecorrente(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdDef BIGINT NOT NULL CONSTRAINT FK_PedidoInternoRecorrente_DEF REFERENCES Def(Id),
	Descricao NVARCHAR(300) NOT NULL,
	DiaGeracao INT NOT NULL,
	DataLimiteGeracao DATETIME NULL,
	Valor FLOAT NOT NULL,
	NecessitaConfirmacao BIT NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioAprovador BIGINT NULL CONSTRAINT FK_PedidoInternoRecorrente_UsuarioAprovador REFERENCES Usuario(Id),
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoInternoRecorrente_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoInternoRecorrente_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE PedidoInterno(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInternoRecorrente BIGINT NULL CONSTRAINT FK_PedidoInterno_PedidoInternoRecorrente REFERENCES PedidoInternoRecorrente(Id),
	IdUsuarioAprovacao BIGINT NOT NULL CONSTRAINT FK_PedidoInterno_UsuarioAprovacao REFERENCES Usuario(Id),
	IdFornecedorBeneficiario BIGINT NULL CONSTRAINT FK_PedidoInterno_Fornecedor REFERENCES Fornecedor(Id),
	IdUsuarioBeneficiario BIGINT NULL CONSTRAINT FK_PedidoInterno_UsuarioBeneficiario REFERENCES Usuario(Id),
	Codigo BIGINT NOT NULL,
	CodigoFormatado NVARCHAR(300) NOT NULL,
	Descricao NVARCHAR(300) NOT NULL,
	NumeroTotalParcelas INT NOT NULL,
	ValorTotal FLOAT NOT NULL,
	Aprovado BIT NULL,
	DataAprovacao DATETIME NULL,
	ImportadoParaFinanceiro BIT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoInterno_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoInterno_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE PedidoInterno_Arquivos(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInterno BIGINT NOT NULL CONSTRAINT FK_PedidoInternoArquivo_PedidoInterno REFERENCES PedidoInterno(Id),
	Nome NVARCHAR(3000) NOT NULL,
	NomeLogico NVARCHAR(3000) NOT NULL,
	Extensao NVARCHAR(10) NOT NULL,
	TamanhoMB FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoInternoArquivo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE PedidoInterno_Obras(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInterno BIGINT NOT NULL CONSTRAINT FK_PedidoInternoObra_PedidoInterno REFERENCES PedidoInterno(Id),
	IdObra BIGINT NOT NULL CONSTRAINT FK_PedidoInterno_Obra REFERENCES Obra(Id),
	Valor FLOAT NOT NULL,
)
GO

CREATE TABLE PedidoInterno_Parcelas(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInterno BIGINT NOT NULL CONSTRAINT FK_PedidoInternoParcela_PedidoInterno REFERENCES PedidoInterno(Id),
	Parcela INT NOT NULL,
	Valor FLOAT NOT NULL,
	CodigoFormatado NVARCHAR(300) NOT NULL,
	DataPagamento DATETIME NOT NULL,
	PagamentoEfetuado BIT NOT NULL
)
GO

CREATE TABLE PedidoInterno_ParcelaObras(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInternoParcela BIGINT NOT NULL CONSTRAINT FK_PedidoInternoParcelaObra_PedidoInternoParcela REFERENCES PedidoInterno_Parcelas(Id),
	IdObra BIGINT NOT NULL CONSTRAINT FK_PedidoInternoParcelaObra_Obra REFERENCES Obra(Id),
	Valor FLOAT NOT NULL,
)
GO

CREATE TABLE PedidoInterno_ParcelaDEFs(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoInternoParcela BIGINT NOT NULL CONSTRAINT FK_PedidoInternoParcelaDEFs_PedidoInternoParcela REFERENCES PedidoInterno_Parcelas(Id),
	IdDef BIGINT NOT NULL CONSTRAINT FK_PedidoInternoParcelaDEFs_DEF REFERENCES Def(Id),
	Valor FLOAT NOT NULL
)
GO

CREATE TABLE CategoriaMaterial(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_UnidadeMaterial_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_UnidadeMaterial_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE UnidadeMaterial(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL,
	Codigo NVARCHAR(3000) NOT NULL
)
GO

CREATE TABLE Material(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdCategoriaMaterial BIGINT NOT NULL CONSTRAINT FK_Material_Categoria REFERENCES CategoriaMaterial(Id),
	IdUnidadeMaterial BIGINT NULL CONSTRAINT FK_Material_Unidade REFERENCES UnidadeMaterial(Id),
	Descricao NVARCHAR(3000) NOT NULL,
	Ativo BIT NOT NULL,
	Servico BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Material_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Material_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE TipoCentroCusto(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL
)
GO

CREATE TABLE StatusPedidoCompra(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL
)
GO

CREATE TABLE MotivoDevolucaoSaldo(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL,
	Ativo BIT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_MotivoDevolucaoSaldo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_MotivoDevolucaoSaldo_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE StatusSolicitacaoCompra(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL
)
GO

CREATE TABLE SolicitacaoCompra_Rascunho(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Titulo NVARCHAR(3000) NOT NULL,
	ObjetoSerializado NVARCHAR(MAX) NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraRascunho_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE SolicitacaoCompra(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdStatusSolicitacaoCompra BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompra_Status REFERENCES StatusSolicitacaoCompra(Id),
	IdTipoCentroCusto BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompra_TipoCentroCusto REFERENCES TipoCentroCusto(Id),
	IdCentroCustoObra BIGINT NULL CONSTRAINT FK_SolicitacaoCompra_Obra REFERENCES Obra(Id),
	IdCentroCustoDEF BIGINT NULL CONSTRAINT FK_SolicitacaoCompra_Def REFERENCES Def(Id),
	Nome NVARCHAR(3000) NOT NULL,
	Codigo NVARCHAR(3000) NOT NULL,
	CodigoSequencia INT NOT NULL,
	CodigoAno INT NOT NULL,
	EnderecoEntrega NVARCHAR(3000) NOT NULL,
	Observacao NVARCHAR(3000) NOT NULL,
	ObservacaoDeAprovacao NVARCHAR(3000) NULL,
	MotivoCancelamento NVARCHAR(3000) NULL,
	ValorEstimado FLOAT NULL,
	ValorTotalCotado FLOAT NULL,
	ValorMelhorCotacao FLOAT NULL,
	DataEntrega DATETIME NULL,
	Servico BIT NULL,
	IdEngenheiroAprovador BIGINT NULL CONSTRAINT FK_SolicitacaoCompra_EngenheiroAprovador REFERENCES Usuario(Id),
	DataAprovacaoEngenheiro DATETIME NULL,
	IdUsuarioFinalizacaoCotacao BIGINT NULL CONSTRAINT FK_SolicitacaoCompra_UsuarioFinalizacaoCotacao REFERENCES Usuario(Id),
	DataFinalizacaoCotacao DATETIME NULL,
	IdDiretorAprovador BIGINT NULL CONSTRAINT FK_SolicitacaoCompra_DiretorAprovador REFERENCES Usuario(Id),
	DataAprovacaoDiretor DATETIME NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompra_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompra_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE SolicitacaoCompra_Arquivos(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompra BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraArquivo_SolicitacaoCompra REFERENCES SolicitacaoCompra(Id),
	Nome NVARCHAR(3000) NOT NULL,
	NomeLogico NVARCHAR(3000) NOT NULL,
	Extensao NVARCHAR(3000) NOT NULL,
	TamanhoMB FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraArquivo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE SolicitacaoCompra_Comentarios(	
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompra BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraComentario_SolicitacaoCompra REFERENCES SolicitacaoCompra(Id),
	Observacao NVARCHAR(3000) NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraComentario_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE SolicitacaoCompra_Materiais(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompra BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_SolicitacaoCompra REFERENCES SolicitacaoCompra(Id),
	IdMaterial BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_Material REFERENCES Material(Id),
	Quantidade FLOAT NOT NULL,
	ValorUnitarioEstimado FLOAT NULL,
	IdTipoCentroCusto BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_TipoCentroCusto REFERENCES TipoCentroCusto(Id),
	IdCentroCustoObra BIGINT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_Obra REFERENCES Obra(Id),
	IdCentroCustoDEF BIGINT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_Def REFERENCES Def(Id),
)
GO

CREATE TABLE SolicitacaoCompra_MateriaisCotacao(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompraMaterial BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterialCotacao_SolicitacaoCompraMaterial REFERENCES SolicitacaoCompra_Materiais(Id),
	IdFornecedor BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_Fornecedor REFERENCES Fornecedor(Id),
	IdCondicaoPagamento BIGINT NOT NULL CONSTRAINT FK_SolicitacaoCompraMaterial_CondicaoPagamento REFERENCES CondicaoPagamento(Id),
	QuantidadeCotado FLOAT NOT NULL,
	ValorUnitarioCotado FLOAT NULL,
	DataEntrega DATETIME NULL,
	CotacaoFinal BIT NOT NULL,
	CotacaoMaisBarata BIT NOT NULL,
	Frete FLOAT NULL,
	Imposto FLOAT NULL,
)
GO

CREATE TABLE SolicitacaoCompra_MateriaisCotacao_PagamentoManual(	
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompraMaterialCotacao BIGINT NOT NULL CONSTRAINT FK_PagamentoManual_SolicitacaoCompraMaterialCotacao REFERENCES SolicitacaoCompra_MateriaisCotacao(Id),
	Data DATETIME NOT NULL,
	Valor FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PagamentoManual_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PagamentoManual_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE PedidoCompra(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSolicitacaoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_SolicitacaoCompra REFERENCES SolicitacaoCompra(Id),
	IdStatusPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_StatusPedidoCompra REFERENCES StatusPedidoCompra(Id),
	IdFornecedor BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_Fornecedor REFERENCES Fornecedor(Id),
	IdTipoCentroCusto BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_TipoCentroCusto REFERENCES TipoCentroCusto(Id),
	IdCentroCustoObra BIGINT NULL CONSTRAINT FK_PedidoCompra_Obra REFERENCES Obra(Id),
	IdCentroCustoDEF BIGINT NULL CONSTRAINT FK_PedidoCompra_Def REFERENCES Def(Id),
	IdCondicaoPagamento BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_CondicaoPagamento REFERENCES CondicaoPagamento(Id),
	CodigoSequencia INT NOT NULL,
	CodigoAno INT NOT NULL,
	Codigo NVARCHAR(300) NOT NULL,
	ValorTotal FLOAT NOT NULL,
	MotivoCancelamento NVARCHAR(3000) NULL,
	DataEntrega DATETIME NOT NULL,
	ImportadoParaFinanceiro BIT NULL,
	EnderecoEntrega NVARCHAR(3000) NOT NULL,
	Frete FLOAT NOT NULL,
	Imposto FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoCompra_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE PedidoCompra_Arquivos(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompraArquivo_PedidoCompra REFERENCES PedidoCompra(Id),
	Nome NVARCHAR(3000) NOT NULL,
	NomeLogico NVARCHAR(3000) NOT NULL,
	Extensao NVARCHAR(3000) NOT NULL,
	TamanhoMB FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompraArquivo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE PedidoCompra_DevolucaoSaldo(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompraDevolucaoSaldo_PedidoCompra REFERENCES PedidoCompra(Id),
	IdMotivoDevolucaoSaldo BIGINT NOT NULL CONSTRAINT FK_PedidoCompraDevolucaoSaldo_Motivo REFERENCES MotivoDevolucaoSaldo(Id),
	Valor FLOAT NOT NULL,
	Observacao NVARCHAR(3000) NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompraDevolucaoSaldo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoCompraDevolucaoSaldo_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE PedidoCompra_Faturas(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompraFatura_PedidoCompra REFERENCES PedidoCompra(Id),
	CodigoFormatado NVARCHAR(3000) NOT NULL,
	DataFatura DATETIME NOT NULL,
	Valor FLOAT NOT NULL,
	PagamentoEfetuado BIT NOT NULL
)
GO

CREATE TABLE PedidoCompra_Materiais(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompraMaterial_PedidoCompra REFERENCES PedidoCompra(Id),
	IdMaterial BIGINT NOT NULL CONSTRAINT FK_PedidoCompraMaterial_Material REFERENCES Material(Id),
	Quantidade FLOAT NOT NULL,
	QuantidadeConciliada FLOAT NOT NULL,
	ValorUnitario FLOAT NOT NULL,
	ValorTotal FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompraMaterial_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoCompraMaterial_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE PedidoCompra_NotaFiscal(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompra BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_PedidoCompra REFERENCES PedidoCompra(Id),
	IdPedidoCompraArquivo BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_Arquivo REFERENCES PedidoCompra_Arquivos(Id),
	IdFornecedor BIGINT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_Fornecedor REFERENCES Fornecedor(Id),
	Nome NVARCHAR(3000) NOT NULL,
	Descricao NVARCHAR(3000) NOT NULL,
	NumeroNotaFiscal NVARCHAR(3000) NOT NULL,
	Aprovada BIT NULL,
	PagamentoEfetuado BIT NOT NULL,
	Valor FLOAT NOT NULL,
	ValorImposto FLOAT NOT NULL,
	ValorFrete FLOAT NOT NULL,
	DataAprovacao DATETIME NULL,
	DataVencimento DATETIME NOT NULL,
	IdUsuarioAprovacao BIGINT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_UsuarioAprovador REFERENCES Usuario(Id),
	ImportadoParaFinanceiro BIT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscal_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL	
)
GO

CREATE TABLE PedidoCompra_NotaFiscal_Materiais(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompraNotaFiscal BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalMaterial_PedidoCompraNotaFiscal REFERENCES PedidoCompra_NotaFiscal(Id),
	IdPedidoCompraMateriais BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalMaterial_PedidoCompraMaterial REFERENCES PedidoCompra_Materiais(Id),
	Quantidade FLOAT NOT NULL,
	Valor FLOAT NOT NULL
)
GO

CREATE TABLE PedidoCompra_NotaFiscal_Pagamento(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPedidoCompraNotaFiscal BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalPagamento_PedidoCompraNotaFiscal REFERENCES PedidoCompra_NotaFiscal(Id),
	IdPedidoCompraMateriais BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalPagamento_PedidoCompraMaterial REFERENCES PedidoCompra_Materiais(Id),
	ValorBruto FLOAT NULL,
	ValorMaterialAbatido FLOAT NULL,
	ValorBaseCalculo FLOAT NULL,
	AliquotaIR FLOAT NULL,
	AliquotaArt30 FLOAT NULL,
	AliquotaINSS FLOAT NULL,
	AliquotaISS FLOAT NULL,
	ValorIR FLOAT NULL,
	ValorArt30 FLOAT NULL,
	ValorINSS FLOAT NULL,
	ValorISS FLOAT NULL,
	IdTipoCalculoNotaFiscalIR BIGINT NULL,
	IdTipoCalculoNotaFiscalArt30 BIGINT NULL,
	IdTipoCalculoNotaFiscalINSS BIGINT NULL,
	IdTipoCalculoNotaFiscalISS BIGINT NULL,
	DataPagamentoIR DATETIME NULL,
	DataPagamentoArt30 DATETIME NULL,
	DataPagamentoINSS DATETIME NULL,
	DataPagamentoISS DATETIME NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalPagamento_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_PedidoCompraNotaFiscalPagamento_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Agenda (
    Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdObra BIGINT NULL CONSTRAINT FK_Agenda_Obra REFERENCES Obra(Id),
	CodigoObra NVARCHAR(300) NULL,
	IdDef BIGINT NOT NULL CONSTRAINT FK_Agenda_Def REFERENCES Def(Id),
	CodigoDef NVARCHAR(300) NULL,
	IdPedidoCompra BIGINT NULL CONSTRAINT FK_Agenda_PedidoCompra REFERENCES PedidoCompra(Id),
	CodigoPedidoCompra NVARCHAR(300) NULL,
	IdPedidoInterno BIGINT NULL CONSTRAINT FK_Agenda_PedidoInterno REFERENCES PedidoInterno(Id),
	CodigoPedidoInterno NVARCHAR(300) NULL,
	IdFornecedorBeneficiario BIGINT NULL CONSTRAINT FK_Agenda_Fornecedor REFERENCES Fornecedor(Id),
	NomeFantasiaFornecedor NVARCHAR(300) NULL,
	IdUsuarioBeneficiario BIGINT NULL CONSTRAINT FK_Agenda_UsuarioBenefeiciario REFERENCES Usuario(Id),
	NomeUsuarioBeneficiario NVARCHAR(300) NULL,
	IdCliente BIGINT NULL CONSTRAINT FK_Agenda_Cliente REFERENCES Cliente(Id),
	NomeCliente NVARCHAR(300) NULL,
	NumeroNF NVARCHAR(300) NULL,
	DataLancamento DATETIME NOT NULL,
	DataPagamento DATETIME NOT NULL,
	Valor FLOAT NOT NULL,
	PagamentoEfetuado BIT NOT NULL
)
GO

CREATE TABLE StatusFaturamento(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Descricao NVARCHAR(3000) NOT NULL
)
GO

CREATE TABLE Faturamento(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdObra BIGINT NOT NULL CONSTRAINT FK_Faturamento_Obra REFERENCES Obra(Id),
	IdStatusFaturamento BIGINT NOT NULL CONSTRAINT FK_Faturamento_Status REFERENCES StatusFaturamento(Id),
	NumeroNF NVARCHAR(3000) NOT NULL,
	DataFaturamento DATETIME NULL,
	DataRecebimentoPrevisto DATETIME NULL,
	DataRecebimentoRealizado DATETIME NULL,
	Observacao NVARCHAR(3000) NOT NULL,
	ValorBruto FLOAT NOT NULL,
	ValorINSS FLOAT NOT NULL,
	ValorISS FLOAT NOT NULL,
	ValorIR FLOAT NOT NULL,
	ValorArt30 FLOAT NOT NULL,
	ValorDesconto FLOAT NOT NULL,
	ValorLiquido FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_Faturamento_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
	IdUsuarioAlteracao BIGINT NOT NULL CONSTRAINT FK_Faturamento_UsuarioUltimaAlteracao REFERENCES Usuario(Id),
	DataUltimaAlteracao DATETIME NOT NULL
)
GO

CREATE TABLE Faturamento_Arquivos(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdFaturamento BIGINT NOT NULL CONSTRAINT FK_FaturamentoArquivo_Faturamento REFERENCES Faturamento(Id),
	Nome NVARCHAR(3000) NOT NULL,
	NomeLogico NVARCHAR(3000) NOT NULL,
	Extensao NVARCHAR(3000) NOT NULL,
	TamanhoMB FLOAT NOT NULL,
	IdUsuarioCadastro BIGINT NOT NULL CONSTRAINT FK_FaturamentoArquivo_UsuarioCadastro REFERENCES Usuario(Id),
	DataCadastro DATETIME NOT NULL,
)
GO

CREATE TABLE FluxoCaixa(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTipoFluxoCaixa BIGINT NOT NULL,
	IdObra BIGINT NULL CONSTRAINT FK_FluxoCaixa_Obra REFERENCES Obra(Id),
	CodigoObra NVARCHAR(3000) NULL,
	IdDef BIGINT NOT NULL CONSTRAINT FK_FluxoCaixa_DEF REFERENCES Def(Id),
	CodigoDef NVARCHAR(3000) NOT NULL,
	IdPedidoCompra BIGINT NULL CONSTRAINT FK_FluxoCaixa_PedidoCompra REFERENCES PedidoCompra(Id),
	CodigoPedidoCompra NVARCHAR(3000) NULL,
	IdPedidoCompraFatura BIGINT NULL CONSTRAINT FK_FluxoCaixa_PedidoCompraFatura REFERENCES PedidoCompra_Faturas(Id),
	CodigoFatura NVARCHAR(3000) NULL,
	IdPedidoCompraNotaFiscal BIGINT NULL CONSTRAINT FK_FluxoCaixa_PedidoCompraNotaFiscal REFERENCES PedidoCompra_NotaFiscal(Id),
	NumeroNotaFiscalPedidoCompra NVARCHAR(3000) NULL,
	IdPedidoInterno BIGINT NULL CONSTRAINT FK_FluxoCaixa_PedidoInterno REFERENCES PedidoInterno(Id),
	CodigoPedidoInterno NVARCHAR(3000) NULL,
	IdCliente BIGINT NULL CONSTRAINT FK_FluxoCaixa_Cliente REFERENCES Cliente(Id),
	NomeCliente NVARCHAR(3000) NULL,
	IdFaturamento BIGINT NULL CONSTRAINT FK_FluxoCaixa_Faturamento REFERENCES Faturamento(Id),
	NumeroNotaFiscalFaturamento NVARCHAR(3000) NULL,
	IdFornecedorBeneficiario BIGINT NULL CONSTRAINT FK_FluxoCaixa_Fornecedor REFERENCES Fornecedor(Id),
	IdUsuarioBeneficiario BIGINT NULL CONSTRAINT FK_FluxoCaixa_Usuario REFERENCES Usuario(Id),
	DataLancamento DATETIME NOT NULL,
	DataPagamento DATETIME NOT NULL,
	Valor FLOAT NOT NULL,
	PagamentoEfetuado BIT NOT NULL,
	DataPagamentoEfetuado DATETIME NULL,
	IdUsuarioInformouPagamento BIGINT NULL CONSTRAINT FK_FluxoCaixa_UsuarioInformouPagamento REFERENCES Usuario(Id),
)
GO

CREATE TABLE FluxoCaixaEstimativaMensal(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Mes INT NOT NULL,
	Ano INT NOT NULL,
	OrdemCompraPedidoInterno FLOAT NOT NULL,
	NotaFiscal FLOAT NOT NULL,
	ETO FLOAT NOT NULL,
	FolhaPagamento FLOAT NOT NULL,
	Imposto FLOAT NOT NULL,
	DespesasFixas FLOAT NOT NULL,
	Reserva FLOAT NOT NULL,
	Outros FLOAT NOT NULL,
	Transferencias FLOAT NOT NULL,
	TotalDiario FLOAT NOT NULL,
	AReceberFaturado FLOAT NOT NULL,
	AReceberAFaturar FLOAT NOT NULL,
	Estornos FLOAT NOT NULL,
	Saldo FLOAT NOT NULL,
)
GO

CREATE TABLE FluxoCaixaSaldoInicial(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Mes INT NOT NULL,
	Ano INT NOT NULL,
	Saldo FLOAT NOT NULL,
	SaldoCalculado FLOAT NOT NULL
)
GO

CREATE TABLE Historico(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	IdSolicitacaoCompra BIGINT NULL CONSTRAINT FK_Historico_SolicitacaoCompra REFERENCES SolicitacaoCompra(Id),
	IdPedidoCompra BIGINT NULL CONSTRAINT FK_Historico_PedidoCompra REFERENCES PedidoCompra(Id),
	IdPedidoInterno BIGINT NULL CONSTRAINT FK_Historico_PedidoInterno REFERENCES PedidoInterno(Id),
	ValorAntigo NVARCHAR(3000) NULL,
	ValorNovo NVARCHAR(3000) NULL,
	Data DATETIME NOT NULL,
	IdUsuario BIGINT NOT NULL CONSTRAINT FK_Historico_Usuario REFERENCES Usuario(Id),
	Usuario NVARCHAR(3000) NOT NULL
)
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Historico') AND name = 'IX_Historico_SolicitacaoCompra')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Historico_SolicitacaoCompra ON Historico(IdSolicitacaoCompra);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Historico') AND name = 'IX_Historico_PedidoCompra')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Historico_PedidoCompra ON Historico(IdPedidoCompra);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Historico') AND name = 'IX_Historico_PedidoInterno')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Historico_PedidoInterno ON Historico(IdPedidoInterno);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[INSERE_HISTORICO] 
	@IdSolicitacaoCompra BIGINT, 
	@IdPedidoCompra BIGINT, 
	@IdPedidoInterno BIGINT,
	@Campo NVARCHAR(3000),
	@ValorAntigo NVARCHAR(3000),
	@ValorNovo NVARCHAR(3000),
	@IdUsuario BIGINT,
	@Usuario NVARCHAR(3000)
AS
INSERT INTO Historico VALUES (NEWID(), @IdSolicitacaoCompra, @IdPedidoCompra, @IdPedidoInterno, @Campo, @ValorAntigo, @ValorNovo, GETDATE(), @IdUsuario, @Usuario)
GO

CREATE OR ALTER TRIGGER [dbo].[Historico_PedidoCompra] on [dbo].[PedidoCompra] AFTER UPDATE 
AS 
BEGIN
	SET NOCOUNT ON

	DECLARE @IdPedidoCompra BIGINT,
			@IdUsuarioAlteracao BIGINT,
			@IdTemporario BIGINT,
			@ValorAntigo VARCHAR(MAX),
			@ValorNovo VARCHAR(MAX),
			@Usuario NVARCHAR(MAX)
			
	DECLARE CURSOR_REG_ALTERADOS CURSOR FOR
    SELECT Id FROM DELETED

	OPEN CURSOR_REG_ALTERADOS

	FETCH NEXT FROM CURSOR_REG_ALTERADOS INTO @IdPedidoCompra
	WHILE @@fetch_status = 0
	BEGIN
		SELECT @IdUsuarioAlteracao = ins.IdUsuarioAlteracao FROM INSERTED ins WHERE ins.Id = @IdPedidoCompra
		SELECT @Usuario = (SELECT U.Nome FROM Usuario U WHERE U.Id = @IdUsuarioAlteracao)

		IF UPDATE(IdStatusPedidoCompra)
		BEGIN
			SELECT @IdTemporario = ins.IdStatusPedidoCompra FROM DELETED ins WHERE ins.Id = @IdPedidoCompra
			SELECT @ValorAntigo = (SELECT SPC.Descricao FROM StatusPedidoCompra SPC WHERE SPC.Id = @IdTemporario)

			SELECT @IdTemporario = ins.IdStatusPedidoCompra FROM INSERTED ins WHERE ins.Id = @IdPedidoCompra
			SELECT @ValorNovo = (SELECT SPC.Descricao FROM StatusPedidoCompra SPC WHERE SPC.Id = @IdTemporario)

			EXEC INSERE_HISTORICO NULL, @IdPedidoCompra, NULL, 'Status', @ValorAntigo, @ValorNovo, @IdUsuarioAlteracao, @Usuario
		END

		IF UPDATE(IdFornecedor)
		BEGIN
			SELECT @IdTemporario = ins.IdFornecedor FROM DELETED ins WHERE ins.Id = @IdPedidoCompra
			SELECT @ValorAntigo = (SELECT CONCAT(F.NomeFantasia, ' - ', F.CNPJ) FROM Fornecedor F WHERE F.Id = @IdTemporario)

			SELECT @IdTemporario = ins.IdFornecedor FROM INSERTED ins WHERE ins.Id = @IdPedidoCompra
			SELECT @ValorNovo = (SELECT CONCAT(F.NomeFantasia, ' - ', F.CNPJ) FROM Fornecedor F WHERE F.Id = @IdTemporario)

			EXEC INSERE_HISTORICO NULL, @IdPedidoCompra, NULL, 'Fornecedor', @ValorAntigo, @ValorNovo, @IdUsuarioAlteracao, @Usuario
		END

		FETCH NEXT FROM CURSOR_REG_ALTERADOS INTO @IdPedidoCompra
	END

	CLOSE CURSOR_REG_ALTERADOS
    DEALLOCATE CURSOR_REG_ALTERADOS 
END