using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class ObraUsuarioAprovacao
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public Int64 IdUsuarioAprovacao { get; set; }

        [ForeignKey("IdObra")]
        public Obra Obra { get; set; }

        [ForeignKey("IdUsuarioAprovacao")]
        public virtual Usuario Usuario { get; set; }
    }
}
