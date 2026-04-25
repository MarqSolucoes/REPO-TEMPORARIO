using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class MaterialDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdCategoriaMaterial { get; set; }
        public Int64 IdUnidadeMaterial { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public bool Servico { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public CategoriaMaterialDTO CategoriaMaterial { get; set; }
        public UnidadeMaterialDTO UnidadeMaterial { get; set; }
    }
}
