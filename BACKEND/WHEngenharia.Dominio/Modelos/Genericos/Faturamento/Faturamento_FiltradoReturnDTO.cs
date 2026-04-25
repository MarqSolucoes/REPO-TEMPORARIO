using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_FiltradoReturnDTO
    {
        public List<FaturamentoDTO> FaturamentosDTO { get; set; }
        public List<ObraFaturamentoDTO> MedicoesDTO { get; set; }
        public double SomaAFaturar { get; set; }
        public double SomaFaturadoRecebido { get; set; }
        public double SomaFaturadoAReceber { get; set; }
        public double SomaFaturadoAReceberLiquido { get; set; }
        public double SinalRecebido { get; set; }
        public double SinalDescontado { get; set; }
        public double SaldoSinal { get; set; }
        public double ValorNaoComissionado { get; set; }
        public double ValorNaoComissionadoUtilizado { get; set; }
        public double SaldoValorNaoComissionado { get; set; }
    }
}
