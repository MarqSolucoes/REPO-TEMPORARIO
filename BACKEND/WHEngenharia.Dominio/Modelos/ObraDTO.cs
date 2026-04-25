using System;
using System.Collections.Generic;

namespace WHEngenharia.Dominio.Modelos
{
    public class ObraDTO
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

        /// <summary>
        /// Valor restante disponível para novas medições (ValorObra - SomaPrevisoesMedicoes)
        /// </summary>
        public double SaldoMedicao { get; set; }
        public double SaldoMedicaoAjustado { get; set; }
        /// <summary>
        /// Valor restante disponível para novos controles de custo (ValorCustoObra - SomaPrevisoesCusto)
        /// </summary>
        public double SaldoETO { get; set; }
        public double SaldoETOAjustado { get; set; }

        public bool AprovacaoAutomatica { get; set; }
        public bool Bloqueada { get; set; }
        public bool Finalizada { get; set; }
        public bool Cancelada { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public ClienteDTO Cliente { get; set; }
        public CidadeDTO Cidade { get; set; }
        public UsuarioDTO DiretorAprovador { get; set; }
        public List<ObraFaturamentoDTO> Faturamentos { get; set; }
        public List<ObraETODTO> ETOs { get; set; }
        public List<ObraUsuarioAprovacaoDTO> UsuariosAprovadores { get; set; }
    }
}
