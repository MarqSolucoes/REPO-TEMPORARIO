using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WHEngenharia.SQL.Modelos
{
    public class Obra
    {
        public Int64 Id { get; set; }
        public Int64 IdCliente { get; set; }
        public Int64 IdCidade { get; set; }
        public Int64 IdUsuarioDiretorAprovador { get; set; }
        public string Codigo { get; set; }
        public int CodigoSequencia { get; set; }
        public int CodigoAno { get; set; }
        public string CodigoProposta { get; set; }
        public string NumeroPedidoCliente { get; set; }
        public string CEP { get; set; }
        public string EnderecoObra { get; set; }
        public string EnderecoEntrega { get; set; }
        public string Descricao { get; set; }
        public DateTime DataProposta { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int PrazoDias { get; set; }
        public int DiasDePagamento { get; set; }
        public double ValorTotal { get; set; }
        public double ValorMaterial { get; set; }
        public double ValorNaoComissionado { get; set; }
        public double AliquotaImpostoISS { get; set; }
        public double? AliquotaImpostoINSS { get; set; }
        public double? AliquotaImpostoIR { get; set; }
        public double? AliquotaImpostoArt30 { get; set; }
        public double? ValorSinal { get; set; }
        public double? PercentualEquivalenteSinal { get; set; }
        public double ETO { get; set; }
        public double Gasto { get; set; }
        public double Saldo { get; set; }
        public DateTime? DataRecebimentoSinal { get; set; }
        public bool AprovacaoAutomatica { get; set; }
        public bool Bloqueada { get; set; }
        public bool Finalizada { get; set; }
        public bool Cancelada { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdCidade")]
        public virtual Cidade Cidade { get; set; }

        [ForeignKey("IdUsuarioDiretorAprovador")]
        public virtual Usuario DiretorAprovador { get; set; }

        public List<ObraFaturamento> Faturamentos { get; set; }
        public List<ObraETO> ETOs { get; set; }
        public List<ObraUsuarioAprovacao> UsuariosAprovadores { get; set; }
    }
}
