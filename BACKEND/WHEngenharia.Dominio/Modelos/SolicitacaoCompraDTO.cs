using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraDTO
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

        public StatusSolicitacaoCompraDTO StatusSolicitacaoCompra { get; set; }
        public TipoCentroCustoDTO TipoCentroCusto { get; set; }
        public UsuarioDTO UsuarioEngenheiroAprovador { get; set; }
        public UsuarioDTO UsuarioFinalizacaoCotacao { get; set; }
        public UsuarioDTO UsuarioDiretorAprovador { get; set; }
        public UsuarioDTO UsuarioComprador { get; set; }
        public UsuarioDTO UsuarioCadastro { get; set; }
        public ObraDTO CentroCustoObra { get; set; }
        public DEFDTO CentroCustoDEF { get; set; }

        public List<SolicitacaoCompraMateriaisDTO> Materiais { get; set; }
        public List<SolicitacaoCompraArquivosDTO> Arquivos { get; set; }
        public List<SolicitacaoCompraComentariosDTO> Comentarios { get; set; }
    }
}
