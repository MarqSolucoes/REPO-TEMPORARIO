using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoParcelaDEFsDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInternoParcela { get; set; }
        public Int64 IdDef { get; set; }
        public double Valor { get; set; }

        
        public PedidoInternoParcelasDTO PedidoInternoParcela { get; set; }
        public DEFDTO DEF { get; set; }
    }
}
