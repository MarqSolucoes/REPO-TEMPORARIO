using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_AlteracaoDefDataRequestDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdDef { get; set; }
        public DateTime DataPagamento { get; set; }
        
    }
}
