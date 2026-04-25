using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class Fornecedor
    {
        public Int64 Id { get; set; }
        public Int64 IdTipoFornecedor { get; set; }
        public Int64 IdCidade { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string NumeroCadastral { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string NomeVendedor { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Observacao { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string TipoConta { get; set; }
        public bool Ativo { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdTipoFornecedor")]
        public virtual TipoFornecedor TipoFornecedor { get; set; }
        
        [ForeignKey("IdCidade")]
        public virtual Cidade Cidade { get; set; }

        [ForeignKey("IdCondicaoPagamento")]
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }
    }
}
