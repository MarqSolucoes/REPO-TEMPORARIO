using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.NotaFiscal
{
    public class NotaFiscal_ItemFiltroRequestDTO
    {
        public Int64? idObra { get; set; }
        public Int64? idPedido { get; set; }
        public Int64? idFornecedor { get; set; }
        public Int64? idStatus { get; set; }
        public Int64? idPagamento { get; set; }
        public DateTime? dataPagamentoInicial { get; set; }
        public DateTime? dataPagamentoFinal { get; set; }
        public string numeroNotaFiscal { get; set; }
    }
}
