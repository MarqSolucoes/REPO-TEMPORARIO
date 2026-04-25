using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_CancelarSaldoAbrirPedidoDTO
    {
        public Int64 IdPedidoCompra { get; set; }
        public Int64 IdUsuario { get; set; }
        public double Valor { get; set; }
        public string CNPJNovoFornecedor { get; set; }
    }
}
