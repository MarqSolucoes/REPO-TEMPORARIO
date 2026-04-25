using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class CondicaoPagamento
    {
        public Int64 Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public List<CondicaoPagamento_Parcelas> Parcelas { get; set; }
    }
}
