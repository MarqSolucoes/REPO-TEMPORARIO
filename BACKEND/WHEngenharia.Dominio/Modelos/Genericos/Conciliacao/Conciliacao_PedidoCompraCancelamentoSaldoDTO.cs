using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_PedidoCompraCancelamentoSaldoDTO
    {
        public Int64 Id { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }

        public MotivoDevolucaoSaldoDTO MotivoDevolucao { get; set; }
    }
}
