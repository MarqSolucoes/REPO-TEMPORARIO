using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class ObraUsuarioAprovacaoDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public Int64 IdUsuarioAprovacao { get; set; }

        public UsuarioDTO Usuario { get; set; }
    }
}
