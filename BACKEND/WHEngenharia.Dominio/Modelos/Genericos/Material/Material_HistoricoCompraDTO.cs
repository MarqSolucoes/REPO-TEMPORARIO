using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Material
{
    public class Material_HistoricoCompraDTO
    {
        public string Obra { get; set; }
        public string Cliente { get; set; }
        public string Pedido { get; set; }
        public string Fornecedor { get; set; }
        public string Material { get; set; }
        public DateTime Data { get; set; }
        public double QuantidadePedida { get; set; }
        public double QuantidadeConciliada { get; set; }
        public double Valor { get; set; }
    }
}
