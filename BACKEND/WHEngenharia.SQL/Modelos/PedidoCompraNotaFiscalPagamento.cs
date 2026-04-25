using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraNotaFiscalPagamento
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompraNotaFiscal { get; set; }
        public double? ValorBruto { get; set; }
        public double? ValorMaterialAbatido { get; set; }
        public double? ValorBaseCalculo { get; set; }
        public double? AliquotaIR { get; set; }
        public double? AliquotaArt30 { get; set; }
        public double? AliquotaINSS { get; set; }
        public double? AliquotaISS { get; set; }
        public double? ValorIR { get; set; }
        public double? ValorArt30 { get; set; }
        public double? ValorINSS { get; set; }
        public double? ValorISS { get; set; }
        public Int64? IdTipoCalculoNotaFiscalIR { get; set; }
        public Int64? IdTipoCalculoNotaFiscalArt30 { get; set; }
        public Int64? IdTipoCalculoNotaFiscalINSS { get; set; }
        public Int64? IdTipoCalculoNotaFiscalISS { get; set; }
        public DateTime? DataPagamentoIR { get; set; }
        public DateTime? DataPagamentoArt30 { get; set; }
        public DateTime? DataPagamentoINSS { get; set; }
        public DateTime? DataPagamentoISS { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }


        [ForeignKey("IdPedidoCompraNotaFiscal")]
        public virtual PedidoCompraNotaFiscal PedidoCompraNotaFiscal { get; set; }
    }
}
