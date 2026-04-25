using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_PostFornecedor
    {
        public List<Int64> idFornecedores { get; set; }
        public Int64 idSolicitacaoCompra { get; set; }
    }
}
