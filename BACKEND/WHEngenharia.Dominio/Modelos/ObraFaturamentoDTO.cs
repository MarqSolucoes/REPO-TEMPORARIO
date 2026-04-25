using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class ObraFaturamentoDTO
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

        public ObraDTO ObraDaMedicao { get; set; }
        public UsuarioDTO UsuarioCadastro { get; set; }
    }
}
