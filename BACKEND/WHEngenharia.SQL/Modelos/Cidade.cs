using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class Cidade
    {
        public Int64 Id { get; set; }
        public string UF { get; set; }
        public string Nome { get; set; }
        public double AliquotaImpostoISS { get; set; }
        public bool Ativo { get; set; }
    }
}
