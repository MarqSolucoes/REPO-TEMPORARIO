using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Historico
{
    public class Historico_RequestDTO
    {
        public bool SolicitacaoCompra { get; set; }
        public bool PedidoCompra { get; set; }
        public bool PedidoInterno { get; set; }
        public string Codigo { get; set; }
    }
}
