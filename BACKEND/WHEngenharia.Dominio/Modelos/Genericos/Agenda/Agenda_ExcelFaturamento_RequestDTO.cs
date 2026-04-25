using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_ExcelFaturamento_RequestDTO
    {
        public List<Int64> idsObras { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
    }
}
