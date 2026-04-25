using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Relatorio
{
    public class Relatorio_ControleEtoAjusteDTO
    {
        public List<Relatorio_ControleEtoProximasDatasDTO> Datas { get; set; }
        public string Observacao { get; set; }
        public Int64 IdObra { get; set; }
        public Int64 IdUsuario { get; set; }
    }
}
