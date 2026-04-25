using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WHEngenharia.SQL.Modelos
{
    public class Usuario
    {
        public Int64 Id { get; set; }
        public Int64 IdCargo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool HabilitaLogin { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }

        public bool GlobalAprovaNotaFiscal { get; set; }
        public bool GlobalAprovaCotacao { get; set; }
        public bool GlobalAprovaSolicitacaoCompra { get; set; }
        public bool GlobalAprovaPedidoInterno { get; set; }


        public bool GlobalSolicitacaoCompra { get; set; }
        public bool GlobalPedidoInterno { get; set; }


        public bool Cargo { get; set; }
        public bool CargoCadastrar { get; set; }
        public bool CargoAtivarDesativar { get; set; }

        public bool Cidade { get; set; }
        public bool CidadeCadastrar { get; set; }
        public bool CidadeEditar { get; set; }
        public bool CidadeAtivarDesativar { get; set; }

        public bool Cliente { get; set; }
        public bool ClienteCadastrar { get; set; }
        public bool ClienteEditar { get; set; }
        public bool ClienteAtivarDesativar { get; set; }

        public bool Fornecedor { get; set; }
        public bool FornecedorCadastrar { get; set; }
        public bool FornecedorEditar { get; set; }
        public bool FornecedorAtivarDesativar { get; set; }

        public bool Material { get; set; }
        public bool MaterialCadastrar { get; set; }
        public bool MaterialEditar { get; set; }
        public bool MaterialAtivarDesativar { get; set; }

        public bool MaterialCategoria { get; set; }
        public bool MaterialCategoriaCadastrar { get; set; }
        public bool MaterialCategoriaAtivarDesativar { get; set; }

        public bool Usuarios { get; set; }
        public bool UsuarioCadastrar { get; set; }
        public bool UsuarioEditar { get; set; }
        public bool UsuarioTrocarSenha { get; set; }
        public bool UsuarioAtivarDesativar { get; set; }
        public bool UsuarioHabilitarDesabilitarLogin { get; set; }

        public bool Obra { get; set; }
        public bool ObraCadastrar { get; set; }
        public bool ObraHabilitarDesabilitarAprovacaoAutomatica { get; set; }
        public bool ObraEditar { get; set; }
        public bool ObraEditarMedicoes { get; set; }
        public bool ObraEditarETO { get; set; }
        public bool ObraBloquear { get; set; }
        public bool ObraRelatorioETO { get; set; }
        public bool ObraRelatorioContaCorrente { get; set; }
        public bool ObraRelatorioResumoETO { get; set; }

        public bool Compras { get; set; }
        public bool ComprasOrdensCompra { get; set; }
        public bool ComprasOrdensCompraAnexos { get; set; }
        public bool ComprasOrdensCompraComentarios { get; set; }
        public bool ComprasOrdensCompraValidacao { get; set; }
        public bool ComprasOrdensCompraValidacaoGerenciar { get; set; }
        public bool ComprasOrdensCompraValidacaoValidar { get; set; }
        public bool ComprasOrdensCompraValidacaoCancelar { get; set; }
        public bool ComprasOrdensCompraEmCotacao { get; set; }
        public bool ComprasOrdensCompraEmCotacaoEditar { get; set; }
        public bool ComprasOrdensCompraEmCotacaoFinalizar { get; set; }
        public bool ComprasOrdensCompraEmCotacaoCancelar { get; set; }
        public bool ComprasOrdensCompraEmCotacaoPDF { get; set; }
        public bool ComprasOrdensCompraParaAprovacao { get; set; }
        public bool ComprasOrdensCompraParaAprovacaoAprovar { get; set; }
        public bool ComprasOrdensCompraParaAprovacaoRejeitar { get; set; }
        public bool ComprasOrdensCompraParaAprovacaoCancelar { get; set; }
        public bool ComprasOrdensCompraEmCompra { get; set; }
        public bool ComprasOrdensCompraEmCompraFinalizar { get; set; }
        public bool ComprasOrdensCompraEmCompraCancelar { get; set; }
        public bool ComprasOrdensCompraEmCompraPDF { get; set; }
        public bool ComprasOrdensCompraFinalizadas { get; set; }
        public bool ComprasOrdensCompraFinalizadasPDF { get; set; }
        public bool ComprasOrdensCompraFinalizadasClonar { get; set; }
        public bool ConciliacaoNotaFiscal { get; set; }
        public bool ConciliacaoNotaFiscalConciliar { get; set; }
        public bool ConciliacaoNotaFiscalCancelamentoSaldo { get; set; }
        public bool Financeiro { get; set; }
        public bool FinanceiroDef { get; set; }
        public bool FinanceiroDefHabilitarDesabilitarPI { get; set; }
        public bool FinanceiroFaturamento { get; set; }
        public bool FinanceiroFaturamentoEntradaFaturamento { get; set; }
        public bool FinanceiroFaturamentoEditar { get; set; }
        public bool FinanceiroFaturamentoInformarRecebimento { get; set; }
        public bool FinanceiroFaturamentoCancelar { get; set; }
        public bool FinanceiroFaturamentoAnexos { get; set; }
        public bool FinanceiroNotaFiscal { get; set; }
        public bool FinanceiroNotaFiscalInformarValores { get; set; }
        public bool FinanceiroPedidoInterno { get; set; }
        public bool FinanceiroPedidoInternoInformarComoPago { get; set; }
        public bool FinanceiroPedidoInternoEditarData { get; set; }
        public bool FinanceiroPedidoInternoAnexos { get; set; }
        public bool FinanceiroPedidoInternoRecorrente { get; set; }
        public bool FinanceiroPedidoInternoRecorrenteCadastrar { get; set; }
        public bool FinanceiroPedidoInternoRecorrenteEditar { get; set; }
        public bool FinanceiroPedidoInternoRecorrenteAtivarDesativar { get; set; }
        public bool Relatorio { get; set; }
        public bool RelatorioAgenda { get; set; }
        public bool RelatorioFaturamento { get; set; }
        public bool RelatorioControleETO { get; set; }
        public bool RelatorioControleETOAjustar { get; set; }
        public Int64? IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64? IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdCargo")]
        public virtual Cargo CargoUsuario { get; set; }
    }
}
