using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_ResponseDTO
    {
        public Int64 count { get; set; }
        public List<SolicitacaoCompraDTO> solicitacoes { get; set; }
    }
}
