using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class ObraAjusteDTO
    {
        public Int64 Id { get; set; }
        public string NomeLogicoAntes { get; set; }
        public string NomeLogicoDepois { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        public UsuarioDTO Usuario { get; set; }
    }
}
