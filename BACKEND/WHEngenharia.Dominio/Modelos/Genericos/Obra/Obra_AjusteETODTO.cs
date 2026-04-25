using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Obra
{
    public  class Obra_AjusteETODTO
    {
        public string CodigoObra { get; set; }
        public List<Tuple<DateTime, double>> Valores { get; set; } = new();
    }
}
