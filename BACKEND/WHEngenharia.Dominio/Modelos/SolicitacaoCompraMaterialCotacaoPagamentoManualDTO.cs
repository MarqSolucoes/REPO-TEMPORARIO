using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class SolicitacaoCompraMaterialCotacaoPagamentoManualDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompraMaterialCotacao { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
    }
}
