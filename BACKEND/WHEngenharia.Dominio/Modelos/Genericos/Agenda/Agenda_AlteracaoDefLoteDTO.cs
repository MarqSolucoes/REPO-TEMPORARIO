using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_AlteracaoDefLoteDTO
    {
         public Int64 IdDef { get; set; }
        public Int64 IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public List<Int64> Ids { get; set; }
    }
}
