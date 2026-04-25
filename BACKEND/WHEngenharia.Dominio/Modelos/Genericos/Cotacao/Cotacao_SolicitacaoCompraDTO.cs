using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Cotacao
{
    public class Cotacao_SolicitacaoCompraDTO
    {
        public Int64 Id { get; set; }
        public string Codigo { get; set; }
        public string Obra { get; set; }
        public string Cliente { get; set; }
        public string Endereco { get; set; }
        public string Observacao { get; set; }
        public string ObservacaoDeAprovacao { get; set; }
        public string ObservacaoParaFornecedor { get; set; }
        public bool Servico { get; set; }
        public DateTime? DataEntrega { get; set; }
        public double? ValorEstimado { get; set; }
        public bool? AprovacaoAutomatica { get; set; }

        public List<Cotacao_SolicitacaoCompra_FornecedorDTO> Fornecedores { get; set; }
        public List<Cotacao_SolicitacaoCompra_MaterialDTO> Materiais { get; set; }
    }
}
