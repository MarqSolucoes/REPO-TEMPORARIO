using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class FluxoCaixaSaldoInicialDTO
    {
        public Int64 Id { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Saldo { get; set; }
        public double SaldoCalculado { get; set; }
    }
}
