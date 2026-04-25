using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.AutoMapper
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Agenda, AgendaDTO>();
            CreateMap<Cargo, CargoDTO>();
            CreateMap<CategoriaMaterial, CategoriaMaterialDTO>();
            CreateMap<Cidade, CidadeDTO>();
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<CondicaoPagamento, CondicaoPagamentoDTO>();
            CreateMap<CondicaoPagamento_Parcelas, CondicaoPagamento_ParcelasDTO>();
            CreateMap<DEF, DEFDTO>();
            CreateMap<Faturamento, FaturamentoDTO>();
            CreateMap<FaturamentoArquivos, FaturamentoArquivosDTO>();
            CreateMap<FluxoCaixa, FluxoCaixaDTO>();
            CreateMap<FluxoCaixaEstimativaMensal, FluxoCaixaEstimativaMensalDTO>();
            CreateMap<FluxoCaixaSaldoInicial, FluxoCaixaSaldoInicialDTO>();
            CreateMap<Fornecedor, FornecedorDTO>();
            CreateMap<Funcionario, FuncionarioDTO>();
            CreateMap<Historico, HistoricoDTO>();
            CreateMap<Material, MaterialDTO>();
            CreateMap<MotivoDevolucaoSaldo, MotivoDevolucaoSaldoDTO>();
            CreateMap<Obra, ObraDTO>();
            CreateMap<ObraAjuste, ObraAjusteDTO>();
            CreateMap<ObraFaturamento, ObraFaturamentoDTO>();
            CreateMap<ObraETO, ObraETODTO>();
            CreateMap<ObraUsuarioAprovacao, ObraUsuarioAprovacaoDTO>();
            CreateMap<PedidoCompra, PedidoCompraDTO>();
            CreateMap<PedidoCompraFatura, PedidoCompraFaturaDTO>();
            CreateMap<PedidoCompraDevolucaoSaldo, PedidoCompraDevolucaoSaldoDTO>();
            CreateMap<PedidoCompraNotaFiscal, PedidoCompraNotaFiscalDTO>();
            CreateMap<PedidoCompraNotaFiscalMateriais, PedidoCompraNotaFiscalMateriaisDTO>();
            CreateMap<PedidoCompraNotaFiscalPagamento, PedidoCompraNotaFiscalPagamentoDTO>();
            CreateMap<PedidoCompraArquivos, PedidoCompraArquivosDTO>();
            CreateMap<PedidoCompraMateriais, PedidoCompraMateriaisDTO>();
            CreateMap<PedidoInterno, PedidoInternoDTO>();
            CreateMap<PedidoInternoArquivos, PedidoInternoArquivosDTO>();
            CreateMap<PedidoInternoObras, PedidoInternoObrasDTO>();
            CreateMap<PedidoInternoParcelas, PedidoInternoParcelasDTO>();
            CreateMap<PedidoInternoParcelaDEFs, PedidoInternoParcelaDEFsDTO>();
            CreateMap<PedidoInternoParcelaObras, PedidoInternoParcelaObrasDTO>();
            CreateMap<PedidoInternoRecorrente, PedidoInternoRecorrenteDTO>();
            CreateMap<RH, RHDTO>();
            CreateMap<SolicitacaoCompra, SolicitacaoCompraDTO>();
            CreateMap<SolicitacaoCompraDTO, SolicitacaoCompra_Post>();
            CreateMap<SolicitacaoCompraArquivos, SolicitacaoCompraArquivosDTO>();
            CreateMap<SolicitacaoCompraComentarios, SolicitacaoCompraComentariosDTO>();
            CreateMap<SolicitacaoCompraMateriais, SolicitacaoCompraMateriaisDTO>();
            CreateMap<SolicitacaoCompraMateriaisCotacao, SolicitacaoCompraMateriaisCotacaoDTO>();
            CreateMap<SolicitacaoCompraMaterialCotacaoPagamentoManual, SolicitacaoCompraMaterialCotacaoPagamentoManualDTO>();
            CreateMap<SolicitacaoCompraRascunho, SolicitacaoCompraRascunhoDTO>();
            CreateMap<StatusFaturamento, StatusFaturamentoDTO>();
            CreateMap<StatusMedicao, StatusMedicaoDTO>();
            CreateMap<StatusPedidoCompra, StatusPedidoCompraDTO>();
            CreateMap<StatusSolicitacaoCompra, StatusSolicitacaoCompraDTO>();
            CreateMap<TipoCentroCusto, TipoCentroCustoDTO>();
            CreateMap<TipoFornecedor, TipoFornecedorDTO>();
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<UnidadeMaterial, UnidadeMaterialDTO>();
        }
    }
}
