using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class SolicitacaoCompraComentariosDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public string Observacao { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        public UsuarioDTO UsuarioCadastro { get; set; }
    }
}
