using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraMaterialCotacaoPagamentoManual
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompraMaterialCotacao { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdSolicitacaoCompraMaterialCotacao")]
        public virtual SolicitacaoCompraMateriaisCotacao SolicitacaoCompraMateriaisCotacao { get; set; }
    }
}
