using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoInterno
    {
        public Int64 Id { get; set; }
        public Int64? IdPedidoInternoRecorrente { get; set; }
        public Int64 IdUsuarioAprovacao { get; set; }
        public Int64? IdFornecedorBeneficiario { get; set; }
        public Int64? IdUsuarioBeneficiario { get; set; }
        public Int64 IdDef { get; set; }
        public Int64? IdObra { get; set; }
        public Int64 Codigo { get; set; }
        public string CodigoFormatado { get; set; }
        public string Descricao { get; set; }
        public int NumeroTotalParcelas { get; set; }
        public double ValorTotal { get; set; }
        public bool? Aprovado { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public bool Cancelado { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }


        [ForeignKey("IdUsuarioAprovacao")]
        public virtual Usuario UsuarioAprovacao { get; set; }

        [ForeignKey("IdFornecedorBeneficiario")]
        public virtual Fornecedor FornecedorBeneficiario { get; set; }

        [ForeignKey("IdUsuarioBeneficiario")]
        public virtual Usuario UsuarioBeneficiario { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }


        [ForeignKey("IdDef")]
        public virtual DEF DEF { get; set; }


        [ForeignKey("IdObra")]
        public virtual Obra Obra { get; set; }


        public List<PedidoInternoArquivos> Arquivos { get; set; }
        public List<PedidoInternoParcelas> Parcelas { get; set; }
        public List<PedidoInternoObras> Obras { get; set; }


    }
}
