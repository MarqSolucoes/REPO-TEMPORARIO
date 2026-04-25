using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class RHDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdFuncionario { get; set; }
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public double Salario { get; set; }
        public double SalarioHoraExtra { get; set; }
        public double SalarioDesconto { get; set; }
        public double SalarioComissao { get; set; }
        public double ValeRefeicaoWH { get; set; }
        public double ValeRefeicaoFuncionario { get; set; }
        public double ValeTransporteWH { get; set; }
        public double AssistenciaMedicaWH { get; set; }
        public double AssistenciaMedicaFuncionario { get; set; }
        public double AssistenciaOdontologicaWH { get; set; }
        public double AssistenciaOdontologicaFuncionario { get; set; }
        public double SeguroWH { get; set; }
        public double SeguroFuncionario { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public FuncionarioDTO Funcionario { get; set; }
    }
}
