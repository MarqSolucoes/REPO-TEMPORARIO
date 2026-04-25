using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_AlteracaoValorDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public double Valor { get; set; }
    }
}
