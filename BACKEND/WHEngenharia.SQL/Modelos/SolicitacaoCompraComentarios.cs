using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraComentarios
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public string Observacao { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        [ForeignKey("IdSolicitacaoCompra")]
        public virtual SolicitacaoCompra SolicitacaoCompra { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }
    }
}
