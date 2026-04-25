using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Obra
{
    public class Obra_ItemContaCorrenteDTO
    {
        public string OrdemCompra { get; set; }
        public string NotaFiscal { get; set; }
        public DateTime? Data { get; set; }
        public DateTime DataRecebimentoPrevisto { get; set; }
        public double ValorPrevisto { get; set; }
        public double Valor { get; set; }
        public string Fornecedor { get; set; }
        public bool Credito { get; set; }
    }
}
