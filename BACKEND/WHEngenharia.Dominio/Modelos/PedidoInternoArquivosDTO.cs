using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoArquivosDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInterno { get; set; }
        public string Nome { get; set; }
        public string NomeLogico { get; set; }
        public string Extensao { get; set; }
        public double TamanhoMB { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        public UsuarioDTO UsuarioCadastro { get; set; }
    }
}
