using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class Faturamento
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public Int64 IdStatusFaturamento { get; set; }
        public string NumeroNF { get; set; }
        public DateTime DataFaturamento { get; set; }
        public DateTime DataRecebimentoPrevisto { get; set; }
        public DateTime? DataRecebimentoRealizado { get; set; }
        public string Observacao { get; set; }
        public double ValorBruto { get; set; }
        public double ValorINSS { get; set; }
        public double ValorISS { get; set; }
        public double ValorIR { get; set; }
        public double ValorArt30 { get; set; }
        public double ValorDesconto { get; set; }
        public double ValorMaterial { get; set; }
        public double ValorSinal { get; set; }
        public double ValorLiquido { get; set; }
        public double ValorRecebido { get; set; }
        public double ValorNaoComissionado { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdObra")]
        public virtual Obra Obra { get; set; }

        [ForeignKey("IdStatusFaturamento")]
        public virtual StatusFaturamento Status { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }

        public List<FaturamentoArquivos> Arquivos { get; set; }
    }
}
