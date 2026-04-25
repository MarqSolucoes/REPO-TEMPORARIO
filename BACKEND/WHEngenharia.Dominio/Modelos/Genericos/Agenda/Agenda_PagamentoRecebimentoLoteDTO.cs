using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_PagamentoRecebimentoLoteDTO
    {
        public List<Int64> Ids { get; set; }
        public Int64 IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
    }
}
