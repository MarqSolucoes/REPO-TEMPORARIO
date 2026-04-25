using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class SolicitacaoCompraRascunhoDTO
    {
        public Int64 Id { get; set; }
        public string Titulo { get; set; }
        public string ObjetoSerializado { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
