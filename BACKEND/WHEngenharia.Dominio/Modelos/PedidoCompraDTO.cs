using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoCompraDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdStatusPedidoCompra { get; set; }
        public Int64 IdFornecedor { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public Int32 CodigoSequencia { get; set; }
        public Int32 CodigoAno { get; set; }
        public string Codigo { get; set; }
        public float ValorTotal { get; set; }
        public double ValorDesconto { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime DataEntrega { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public string EnderecoEntrega { get; set; }
        public double Frete { get; set; }
        public double Imposto { get; set; }
        public double Saldo { get; set; }
        public string ObservacaoParaFornecedor { get; set; }
        public Int64 IdUsuarioComprador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public StatusPedidoCompraDTO StatusPedidoCompra { get; set; }
        public SolicitacaoCompraDTO SolicitacaoCompra { get; set; }
        public FornecedorDTO Fornecedor { get; set; }
        public TipoCentroCustoDTO TipoCentroCusto { get; set; }
        public ObraDTO CentroCustoObra { get; set; }
        public DEFDTO CentroCustoDEF { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public UsuarioDTO UsuarioComprador { get; set; }
        public List<PedidoCompraFaturaDTO> Faturas { get; set; }
        public List<PedidoCompraMateriaisDTO> Materiais { get; set; }
        public List<PedidoCompraArquivosDTO> Arquivos { get; set; }
        public List<PedidoCompraNotaFiscalDTO> NotasFiscais { get; set; }
        public List<PedidoCompraDevolucaoSaldoDTO> CancelamentosSaldo { get; set; }
    }
}
