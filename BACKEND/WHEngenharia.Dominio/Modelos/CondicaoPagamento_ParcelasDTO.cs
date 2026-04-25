using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class CondicaoPagamento_ParcelasDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public int DiasCorridos { get; set; }
        public double PorcentagemValorTotal { get; set; }
    }
}
