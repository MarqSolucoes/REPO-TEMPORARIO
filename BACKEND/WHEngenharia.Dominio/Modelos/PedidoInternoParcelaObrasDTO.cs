using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoParcelaObrasDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInternoParcela { get; set; }
        public Int64 IdObra { get; set; }
        public double Valor { get; set; }
    }
}
