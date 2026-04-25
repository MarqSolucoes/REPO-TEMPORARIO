using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_ValoresAFaturarDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public double Valor { get; set; }
        public double INSS { get; set; }
        public double ISS { get; set; }
        public double IR { get; set; }
        public double ART30 { get; set; }
        public double ValorLiquido { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
