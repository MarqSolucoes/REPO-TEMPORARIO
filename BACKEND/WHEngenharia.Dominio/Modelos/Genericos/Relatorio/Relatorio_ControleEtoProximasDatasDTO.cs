using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Relatorio
{
    public class Relatorio_ControleEtoProximasDatasDTO
    {
        public int Id { get; set; }
        public Int64 IdObraMedicao { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }
}
