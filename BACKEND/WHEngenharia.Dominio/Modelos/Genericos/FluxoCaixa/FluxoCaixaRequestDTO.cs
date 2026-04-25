using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa
{
    public class FluxoCaixaRequestDTO
    {
        public string Data { get; set; }
        public string DataFinal { get; set; }
        public bool Credito { get; set; }
    }
}
