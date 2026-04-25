using System;
using System.Collections.Generic;
using System.Text;

namespace WHEngenharia.SQL.Modelos
{
    public class Cargo
    {
        public Int64 Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
    }
}
