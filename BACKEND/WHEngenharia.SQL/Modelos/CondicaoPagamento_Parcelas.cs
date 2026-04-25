using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class CondicaoPagamento_Parcelas
    {
        public Int64 Id { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public int DiasCorridos { get; set; }
        public double PorcentagemValorTotal { get; set; }

        [ForeignKey("IdCondicaoPagamento")]
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }
    }
}
