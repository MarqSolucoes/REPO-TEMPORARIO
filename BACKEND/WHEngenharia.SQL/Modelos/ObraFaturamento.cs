using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class ObraFaturamento
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataPrevistaRecebimento { get; set; }
        public double Valor { get; set; }
        public double ValorFaturado { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdObra")]
        public Obra Obra { get; set; }

        [ForeignKey("IdUsuarioCadastro")]
        public virtual Usuario UsuarioCadastro { get; set; }
    }
}
