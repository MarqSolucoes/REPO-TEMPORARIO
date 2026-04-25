using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_ItemDTO
    {
        public Int64 idObra { get; set; }
        public string codigoObra { get; set; }
        public Int64 idDef { get; set; }
        public string codigoDef { get; set; }
        public Int64 idPedidoCompra { get; set; }
        public string codigoPedidoCompra { get; set; }
        public string notaFiscal { get; set; }
        public DateTime? dataLancamento { get; set; }
        public DateTime? dataPagamento { get; set; }
        public double valor { get; set; }
        public Int64 idFornecedor { get; set; }
        public string descricaoFornecedor { get; set; }
    }
}
