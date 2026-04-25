using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraArquivos
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public string Nome { get; set; }
        public string NomeLogico { get; set; }
        public string Extensao { get; set; }
        public double TamanhoMB { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        [ForeignKey("IdPedidoCompra")]
        public virtual PedidoCompra PedidoCompra { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }
    }
}
