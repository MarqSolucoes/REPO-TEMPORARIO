using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa
{
    public class FluxoCaixa_ResponseDTO
    {
        public string DiaObservacao { get; set; }
        public DateTime? Dia { get; set; }
        public double OrdemCompraPedidoInterno { get; set; }
        public double NotaFiscal { get; set; }
        public double Eto { get; set; }
        public double folhaPagamento { get; set; }
        public double imposto { get; set; }
        public double despesasFixas { get; set; }
        public double reserva { get; set; }
        public double outros { get; set; }
        public double transferencia { get; set; }
        public double totalDiario { get; set; }
        public double aReceberFaturado { get; set; }
        public double aReceberAFaturar { get; set; }
        public double estornos { get; set; }

        public double saldo { get; set; }
    }
}
