using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoCompraMateriaisDTO
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

        public virtual MaterialDTO Material { get; set; }
    }
}
