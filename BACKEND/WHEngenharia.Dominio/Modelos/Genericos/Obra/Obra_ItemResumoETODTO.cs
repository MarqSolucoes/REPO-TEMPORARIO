using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Obra
{
    public class Obra_ItemResumoETODTO
    {
        public string CodigoObra { get; set; }
        public string Cliente { get; set; }
        public string Escopo { get; set; }
        public double ETO { get; set; }
        public double Gasto { get; set; }
        public double Saldo { get; set; }
    }
}
