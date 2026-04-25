using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Relatorio
{
    public class Relatorio_ControleEtoDTO
    {
        public Int64 IdObra { get; set; }
        public bool Bloqueada { get; set; }
        public string Codigo { get; set; }
        public string Cliente { get; set; }
        public string Descricao { get; set; }
        public double Custo { get; set; }
        public double Gasto { get; set; }
        public double Saldo { get; set; }
        public bool ComentariosNaoVisualizados { get; set; }
    }
}
