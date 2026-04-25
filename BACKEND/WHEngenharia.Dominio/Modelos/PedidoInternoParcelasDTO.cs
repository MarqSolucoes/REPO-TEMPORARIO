using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoParcelasDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInterno { get; set; }
        public int Parcela { get; set; }
        public double Valor { get; set; }
        public string CodigoFormatado { get; set; }
        public DateTime DataPagamento { get; set; }
        public bool PagamentoEfetuado { get; set; }

        public List<PedidoInternoParcelaObrasDTO> ParcelaObras { get; set; }
        public List<PedidoInternoParcelaDEFsDTO> ParcelaDEFs { get; set; }
    }
}
