using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_ReprovadaDirecaoDTO
    {
        public Int64 IdNotaFiscal { get; set; }
        public Int64 IdArquivo { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public string NomeArquivo { get; set; }
        public string NotaFiscal { get; set; }
        public string DescricaoNotaFiscal { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string Pedido { get; set; }
        public System.DateTime DataPedido { get; set; }
        public string Obra { get; set; }
        public string EnderecoEntregaObra { get; set; }
        public string Cliente { get; set; }
        public string CNPJCliente { get; set; }
        public string UsuarioAprovador { get; set; }
        public System.DateTime DataAprovacao { get; set; }
    }
}
