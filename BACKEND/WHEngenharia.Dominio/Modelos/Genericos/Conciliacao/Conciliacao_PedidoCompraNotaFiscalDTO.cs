using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_PedidoCompraNotaFiscalDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdArquivo { get; set; }
        public string Descricao { get; set; }
        public string NumeroNotaFiscal { get; set; } = "";
        public string Nome { get; set; }
        public string NomeLogico { get; set; }
        public bool? Aprovada { get; set; }
        public bool DevolucaoVisualizada { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public double Valor { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataVencimento { get; set; }

        public FornecedorDTO Fornecedor { get; set; }
    }
}
