using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class DEFDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdTipoPedidoInterno { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public char Tipo { get; set; }
        public bool PodeAbrirPedidoInternoRecorrente { get; set; }
        public bool FinanceiroPodeSerAlterado { get; set; }
        public bool FinanceiroPodeTerValorSubdividido { get; set; }

        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
    }
}
