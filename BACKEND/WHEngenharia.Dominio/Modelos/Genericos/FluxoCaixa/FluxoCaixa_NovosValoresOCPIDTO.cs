using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa
{
    public class FluxoCaixa_NovosValoresOCPIDTO
    {
        public Int64 IdFluxoCaixa { get; set; }
        public List<FluxoCaixa_NovosValoresOCPI_ValorDTO> Valores { get; set; }
    }
}
