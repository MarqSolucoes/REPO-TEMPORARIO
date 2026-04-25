using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_CarimboResultDTO
    {
        public string Obra { get; set; }
        public string PedidoCompra { get; set; }
        public string NotaFiscal { get; set; }
        public string fornecedor { get; set; }
        public string cnpj { get; set; }
        public double ValorBruto { get; set; }
        public double ValorMaterialAbatido { get; set; }
        public double ValorArt30 { get; set; }
        public double ValorINSS { get; set; }
        public double ValorISS { get; set; }
        public double ValorIR { get; set; }
        public double ValorLiquido { get; set; }
        public DateTime DataPagamentoArt30 { get; set; }
        public DateTime DataPagamentoINSS { get; set; }
        public DateTime DataPagamentoISS { get; set; }
        public DateTime DataPagamentoIR { get; set; }
    }
}
