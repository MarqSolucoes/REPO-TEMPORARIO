using System;

namespace WHEngenharia.Dominio.Modelos
{
    public class ClienteDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdCidade { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string EmailFinanceiro { get; set; }
        public string EmailComercial { get; set; }
        public string ResponsavelComercial { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Observacao { get; set; }
        public int DiasDePagamento { get; set; }
        public bool Ativo { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public CidadeDTO Cidade { get; set; }
    }
}
