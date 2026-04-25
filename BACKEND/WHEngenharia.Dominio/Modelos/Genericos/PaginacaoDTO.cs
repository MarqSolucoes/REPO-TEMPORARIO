using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos
{
    public class PaginacaoDTO
    {
        public int skip { get; set; }
        public int take { get; set; }
        public string descricao { get; set; }
    }
}
