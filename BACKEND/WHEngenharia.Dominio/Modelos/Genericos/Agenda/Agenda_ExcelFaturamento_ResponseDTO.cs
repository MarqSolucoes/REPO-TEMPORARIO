using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos.Faturamento;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_ExcelFaturamento_ResponseDTO
    {
        public List<Faturamento_ValoresAFaturarDTO> AFaturar { get; set; }
        public List<FaturamentoDTO> FaturadoAReceber { get; set; }
        public List<FaturamentoDTO> FaturadoRecebido { get; set; }

    }
}
