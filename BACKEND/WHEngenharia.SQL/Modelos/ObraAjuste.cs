using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class ObraAjuste
    {
        public Int64 Id { get; set; }
        public string NomeLogicoAntes { get; set; }
        public string NomeLogicoDepois { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario Usuario { get; set; }
    }
}
