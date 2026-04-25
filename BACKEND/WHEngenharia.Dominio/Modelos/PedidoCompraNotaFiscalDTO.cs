using System;
using System.Collections.Generic;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoCompraNotaFiscalDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public Int64 IdPedidoCompraArquivo { get; set; }
        public Int64? IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public bool? Aprovada { get; set; }
        public bool DevolucaoVisualizada { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public double Valor { get; set; }
        public double ValorImposto { get; set; }
        public double ValorFrete { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Cancelada { get; set; }
        public Int64? IdUsuarioAprovacao { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }


        public PedidoCompraDTO PedidoCompra { get; set; }
        public PedidoCompraArquivosDTO Arquivo { get; set; }
        public FornecedorDTO Fornecedor { get; set; }
        public UsuarioDTO UsuarioDiretorAprovador { get; set; }
        public List<PedidoCompraNotaFiscalMateriaisDTO> Materiais { get; set; }
        public List<PedidoCompraNotaFiscalPagamentoDTO> Pagamentos { get; set; }
    }
}
