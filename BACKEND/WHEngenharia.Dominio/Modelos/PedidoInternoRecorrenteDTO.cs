using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoRecorrenteDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdDEF { get; set; }
        public string Descricao { get; set; }
        public int DiaGeracao { get; set; }
        public DateTime? DataLimiteGeracao { get; set; }
        public double Valor { get; set; }
        public bool NecessitaConfirmacao { get; set; }
        public bool Ativo { get; set; }
        public Int64? IdUsuarioAprovador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public DEFDTO DEF { get; set; }
        public UsuarioDTO UsuarioCadastro { get; set; }
    }
}
