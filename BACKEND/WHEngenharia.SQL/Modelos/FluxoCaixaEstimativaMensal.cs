using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class FluxoCaixaEstimativaMensal
    {
        public Int64 Id { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double OrdemCompraPedidoInterno { get; set; }
        public double NotaFiscal { get; set; }
        public double ETO { get; set; }
        public double FolhaPagamento { get; set; }
        public double Imposto { get; set; }
        public double DespesasFixas { get; set; }
        public double Reserva { get; set; }
        public double Outros { get; set; }
        public double Transferencias { get; set; }
        public double TotalDiario { get; set; }
        public double AReceberFaturado { get; set; }
        public double AReceberAFaturar { get; set; }
        public double Estornos { get; set; }
        public double Saldo { get; set; }
    }
}
