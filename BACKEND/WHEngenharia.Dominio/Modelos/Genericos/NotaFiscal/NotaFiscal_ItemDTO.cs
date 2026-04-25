using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.NotaFiscal
{
    public class NotaFiscal_ItemDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdArquivo { get; set; }
        public string Nome { get; set; }
        public string Obra { get; set; }
        public string Pedido { get; set; }
        public string Fornecedor { get; set; }
        public string NumeroNota { get; set; }
        public string Status { get; set; }
        public string Pagamento { get; set; }
        public double Valor { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
