using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraMateriais
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public Int64 IdMaterial { get; set; }
        public double Quantidade { get; set; }
        public double QuantidadeConciliada { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotal { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdPedidoCompra")]
        public virtual PedidoCompra PedidoCompra { get; set; }

        [ForeignKey("IdMaterial")]
        public virtual Material Material { get; set; }
    }
}
