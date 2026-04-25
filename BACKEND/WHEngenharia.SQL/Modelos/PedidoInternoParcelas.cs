using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoInternoParcelas
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInterno { get; set; }
        public int Parcela { get; set; }
        public double Valor { get; set; }
        public string CodigoFormatado { get; set; }
        public DateTime DataPagamento { get; set; }
        public bool PagamentoEfetuado { get; set; }

        [ForeignKey("IdPedidoInterno")]
        public virtual PedidoInterno PedidoInterno { get; set; }

        public List<PedidoInternoParcelaObras> ParcelaObras { get; set; }
        public List<PedidoInternoParcelaDEFs> ParcelaDEFs { get; set; } 
    }
}
