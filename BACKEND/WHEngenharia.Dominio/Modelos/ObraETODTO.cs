using System;

namespace WHEngenharia.Dominio.Modelos
{
    public class ObraETODTO
    {
        public Int64 Id { get; set; }
        public Int64 IdObra { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public double ValorConsumido { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
    }
}
