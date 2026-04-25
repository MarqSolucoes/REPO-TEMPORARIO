using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class Historico
    {
        public Guid Id { get; set; }
        public Int64? IdSolicitacaoCompra { get; set; }
        public Int64? IdPedidoCompra { get; set; }
        public Int64? IdPedidoInterno { get; set; }
        public string Campo { get; set; }
        public string ValorAntigo { get; set; }
        public string ValorNovo { get; set; }
        public DateTime Data { get; set; }
        public Int64 IdUsuario { get; set; }
        public string Usuario { get; set; }
    }
}
