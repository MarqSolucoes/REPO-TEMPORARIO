using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class CidadeDTO
    {
        public Int64 Id { get; set; }
        public string UF { get; set; }
        public string Nome { get; set; }
        public double AliquotaImpostoISS { get; set; }
        public bool Ativo { get; set; }
    }
}
