using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompra
    {
        public Int64 Id { get; set; }
        public Int64 IdStatusSolicitacaoCompra { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public int CodigoSequencia { get; set; }
        public int CodigoAno { get; set; }
        public string EnderecoEntrega { get; set; }
        public string Observacao { get; set; }
        public string ObservacaoDeAprovacao { get; set; }
        public string ObservacaoParaFornecedor { get; set; }
        public string MotivoCancelamento { get; set; }
        public double? ValorEstimado { get; set; }
        public double? ValorTotalCotado { get; set; }
        public double? ValorMelhorCotacao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public bool Servico { get; set; }
        public Int64? IdEngenheiroAprovador { get; set; }
        public DateTime? DataAprovacaoEngenheiro { get; set; }
        public Int64? IdUsuarioFinalizacaoCotacao { get; set; }
        public DateTime? DataFinalizacaoCotacao { get; set; }
        public Int64? IdDiretorAprovador { get; set; }
        public DateTime? DataAprovacaoDiretor { get; set; }
        public Int64? IdUsuarioComprador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }


        [ForeignKey("IdStatusSolicitacaoCompra")]
        public virtual StatusSolicitacaoCompra StatusSolicitacaoCompra { get; set; }
        
        [ForeignKey("IdTipoCentroCusto")]
        public virtual TipoCentroCusto TipoCentroCusto { get; set; }

        [ForeignKey("IdEngenheiroAprovador")]
        public virtual Usuario UsuarioEngenheiroAprovador { get; set; }

        [ForeignKey("IdUsuarioFinalizacaoCotacao")]
        public virtual Usuario UsuarioFinalizacaoCotacao { get; set; }

        [ForeignKey("IdDiretorAprovador")]
        public virtual Usuario UsuarioDiretorAprovador { get; set; }

        [ForeignKey("IdUsuarioComprador")]
        public virtual Usuario UsuarioComprador { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }

        [ForeignKey("IdCentroCustoObra")]
        public virtual Obra CentroCustoObra { get; set; }

        [ForeignKey("IdCentroCustoDEF")]
        public virtual DEF CentroCustoDEF { get; set; }

        public List<SolicitacaoCompraMateriais> Materiais { get; set; }
        public List<SolicitacaoCompraArquivos> Arquivos { get; set; }
        public List<SolicitacaoCompraComentarios> Comentarios { get; set; }
    }
}
