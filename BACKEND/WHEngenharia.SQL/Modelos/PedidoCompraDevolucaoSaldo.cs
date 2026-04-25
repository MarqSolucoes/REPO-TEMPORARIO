using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraDevolucaoSaldo
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public Int64 IdMotivoDevolucaoSaldo { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdMotivoDevolucaoSaldo")]
        public virtual MotivoDevolucaoSaldo MotivoDevolucao { get; set; }

        [ForeignKey("IdPedidoCompra")]
        public virtual PedidoCompra PedidoCompra { get; set; }
    }
}
