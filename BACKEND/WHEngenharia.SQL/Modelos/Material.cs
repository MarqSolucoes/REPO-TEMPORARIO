using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WHEngenharia.SQL.Modelos
{
    public class Material
    {
        public Int64 Id { get; set; }
        public Int64 IdCategoriaMaterial { get; set; }
        public Int64 IdUnidadeMaterial { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Servico { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdCategoriaMaterial")]
        public virtual CategoriaMaterial CategoriaMaterial { get; set; }

        [ForeignKey("IdUnidadeMaterial")]
        public virtual UnidadeMaterial UnidadeMaterial { get; set; }
    }
}
