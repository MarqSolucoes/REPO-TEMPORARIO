using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Obra
{
    public  class Obra_AjusteETOResultDTO
    {
        public Int64 IdObra { get; set; }
        public string CodigoObra { get; set; }
        public string Cliente { get; set; }
        public string Status { get; set; }

        public double Custo { get; set; }
        public double Gasto { get; set; }
        public double Saldo { get; set; }

        public List<Tuple<DateTime, double>> ETO { get; set; }
    }
}
