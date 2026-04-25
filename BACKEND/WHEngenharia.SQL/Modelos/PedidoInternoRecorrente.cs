using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoInternoRecorrente
    {
        public Int64 Id { get; set; }
        public Int64 IdDEF { get; set; }
        public string Descricao { get; set; }
        public int DiaGeracao { get; set; }
        public DateTime? DataLimiteGeracao { get; set; }
        public double Valor { get; set; }
        public bool NecessitaConfirmacao { get; set; }
        public bool Ativo { get; set; }
        public Int64? IdUsuarioAprovador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdDEF")]
        public virtual DEF DEF { get; set; }
        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }
    }
}
