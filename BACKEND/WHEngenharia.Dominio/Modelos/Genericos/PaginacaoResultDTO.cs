using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos
{
    public class PaginacaoResultDTO<T>
    {
        public int total { get; set; }
        public List<T> items { get; set; }
    }
}
