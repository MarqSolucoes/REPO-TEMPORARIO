using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_PedidoCompraMateriaisDTO
    {
        public Int64 Id { get; set; }
        public double Quantidade { get; set; }
        public double QuantidadeConciliada { get; set; }
        public double QuantidadeParaConciliar { get; set; }
        public double ValorConciliado { get; set; }
        public double ValorComprado { get; set; }
        public string Material { get; set; }
    }
}
