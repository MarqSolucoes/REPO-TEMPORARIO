using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable(nameof(Usuario), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.IdCargo)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.HabilitaLogin)
                .IsRequired();

            builder.Property(x => x.Login)
                .IsRequired();

            builder.Property(x => x.Senha)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.GlobalAprovaCotacao)
               .IsRequired();

            builder.Property(x => x.GlobalAprovaNotaFiscal)
                .IsRequired();

            builder.Property(x => x.GlobalSolicitacaoCompra)
                .IsRequired();

            builder.Property(x => x.GlobalPedidoInterno)
                .IsRequired();

            builder.Property(x => x.Cidade)
               .IsRequired();

            builder.Property(x => x.CidadeCadastrar)
                .IsRequired();

            builder.Property(x => x.CidadeEditar)
                .IsRequired();

            builder.Property(x => x.CidadeAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.Cargo)
                .IsRequired();

            builder.Property(x => x.CargoCadastrar)
                .IsRequired();

            builder.Property(x => x.CargoAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.Cliente)
                .IsRequired();

            builder.Property(x => x.ClienteCadastrar)
                .IsRequired();

            builder.Property(x => x.ClienteEditar)
                .IsRequired();

            builder.Property(x => x.ClienteAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.Fornecedor)
                .IsRequired();

            builder.Property(x => x.FornecedorCadastrar)
                .IsRequired();

            builder.Property(x => x.FornecedorEditar)
                .IsRequired();

            builder.Property(x => x.FornecedorAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.Material)
                .IsRequired();

            builder.Property(x => x.MaterialCadastrar)
                .IsRequired();

            builder.Property(x => x.MaterialEditar)
                .IsRequired();

            builder.Property(x => x.MaterialAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.MaterialCategoria)
                .IsRequired();

            builder.Property(x => x.MaterialCategoriaCadastrar)
                .IsRequired();

            builder.Property(x => x.MaterialCategoriaAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.Usuarios)
                .IsRequired();

            builder.Property(x => x.UsuarioCadastrar)
                .IsRequired();

            builder.Property(x => x.UsuarioEditar)
                .IsRequired();

            builder.Property(x => x.UsuarioTrocarSenha)
                .IsRequired();

            builder.Property(x => x.UsuarioAtivarDesativar)
                .IsRequired();

            builder.Property(x => x.UsuarioHabilitarDesabilitarLogin)
                .IsRequired();

            builder.Property(x => x.Obra)
                .IsRequired();

            builder.Property(x => x.ObraCadastrar)
                .IsRequired();

            builder.Property(x => x.ObraEditar)
                .IsRequired();


            builder.Property(x => x.ObraBloquear).IsRequired();
            builder.Property(x => x.Compras).IsRequired();
            builder.Property(x => x.ComprasOrdensCompra).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraAnexos).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraComentarios).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraValidacao).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraValidacaoGerenciar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraValidacaoValidar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraValidacaoCancelar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCotacao).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCotacaoEditar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCotacaoFinalizar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCotacaoCancelar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCotacaoPDF).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraParaAprovacao).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraParaAprovacaoAprovar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraParaAprovacaoRejeitar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraParaAprovacaoCancelar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCompra).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCompraFinalizar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCompraCancelar).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraEmCompraPDF).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraFinalizadas).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraFinalizadasPDF).IsRequired();
            builder.Property(x => x.ComprasOrdensCompraFinalizadasClonar).IsRequired();
            builder.Property(x => x.ConciliacaoNotaFiscal).IsRequired();
            builder.Property(x => x.ConciliacaoNotaFiscalConciliar).IsRequired();
            builder.Property(x => x.ConciliacaoNotaFiscalCancelamentoSaldo).IsRequired();
            builder.Property(x => x.Financeiro).IsRequired();
            builder.Property(x => x.FinanceiroDef).IsRequired();
            builder.Property(x => x.FinanceiroDefHabilitarDesabilitarPI).IsRequired();
            builder.Property(x => x.FinanceiroFaturamento).IsRequired();
            builder.Property(x => x.FinanceiroFaturamentoEntradaFaturamento).IsRequired();
            builder.Property(x => x.FinanceiroFaturamentoEditar).IsRequired();
            builder.Property(x => x.FinanceiroFaturamentoInformarRecebimento).IsRequired();
            builder.Property(x => x.FinanceiroFaturamentoCancelar).IsRequired();
            builder.Property(x => x.FinanceiroFaturamentoAnexos).IsRequired();
            builder.Property(x => x.FinanceiroNotaFiscal).IsRequired();
            builder.Property(x => x.FinanceiroNotaFiscalInformarValores).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInterno).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoInformarComoPago).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoEditarData).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoAnexos).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoRecorrente).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoRecorrenteCadastrar).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoRecorrenteEditar).IsRequired();
            builder.Property(x => x.FinanceiroPedidoInternoRecorrenteAtivarDesativar).IsRequired();
            builder.Property(x => x.Relatorio).IsRequired();
            builder.Property(x => x.RelatorioAgenda).IsRequired();
            builder.Property(x => x.RelatorioFaturamento).IsRequired();
            builder.Property(x => x.RelatorioControleETO).IsRequired();
            builder.Property(x => x.RelatorioControleETOAjustar).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro);

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao);

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.CargoUsuario)
                .WithOne()
                .HasForeignKey<Usuario>(x => x.IdCargo)
                .IsRequired();
        }
    }
}
