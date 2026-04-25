using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class Agenda
    {
        public Int64 Id { get; set; }
        public Int64? IdObra { get; set; }
        public string CodigoObra { get; set; }
        public Int64 IdDef { get; set; }
        public string CodigoDef { get; set; }
        public Int64? IdPedidoCompra { get; set; }
        public string CodigoPedidoCompra { get; set; }
        public Int64? IdPedidoInterno { get; set; }
        public string CodigoPedidoInterno { get; set; }
        public Int64? IdFornecedorBeneficiario { get; set; }
        public string NomeFantasiaFornecedor { get; set; }
        public Int64? IdUsuarioBeneficiario { get; set; }
        public string NomeUsuarioBeneficiario { get; set; }
        public Int64? IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string NumeroNF { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public double Valor { get; set; }
        public bool PagamentoEfetuado { get; set; }
    }
}
