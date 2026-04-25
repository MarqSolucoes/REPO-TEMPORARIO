using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_AjusteFaturamentoDTO
    {   
        public Int64 IdObra { get; set; }
        public List<Faturamento_AjusteFaturamentoDataValorDTO> DatasValores { get; set; } = new List<Faturamento_AjusteFaturamentoDataValorDTO>();

    }
}
