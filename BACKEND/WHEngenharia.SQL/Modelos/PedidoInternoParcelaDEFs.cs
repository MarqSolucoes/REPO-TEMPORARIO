using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoInternoParcelaDEFs
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInternoParcela { get; set; }
        public Int64 IdDef { get; set; }
        public double Valor { get; set; }

        [ForeignKey("IdPedidoInternoParcela")]
        public virtual PedidoInternoParcelas PedidoInternoParcela { get; set; }

        [ForeignKey("IdDef")]
        public virtual DEF DEF { get; set; } 
    }
}
