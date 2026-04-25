using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Persistencia;

namespace WHEngenharia.SQL
{
    public class WHEngenhariaContext : DbContext
    {
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CategoriaMaterial> CategoriaMaterial { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<CondicaoPagamento> CondicaoPagamento { get; set; }
        public DbSet<CondicaoPagamento_Parcelas> CondicaoPagamento_Parcelas { get; set; }
        public DbSet<DEF> DEF { get; set; }
        public DbSet<Faturamento> Faturamento { get; set; }
        public DbSet<FaturamentoRecebimento> Faturamento_Recebimento { get; set; }
        public DbSet<FaturamentoArquivos> Faturamento_Arquivos { get; set; }
        public DbSet<FluxoCaixa> FluxoCaixa { get; set; }
        public DbSet<FluxoCaixaEstimativaMensal> FluxoCaixaEstimativaMensal { get; set; }
        public DbSet<FluxoCaixaSaldoInicial> FluxoCaixaSaldoInicial { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Historico> Historico { get; set; }
        public DbSet<MotivoDevolucaoSaldo> MotivoDevolucaoSaldo { get; set; }
        public DbSet<Obra> Obra { get; set; }
        public DbSet<ObraAjuste> ObraAjuste { get; set; }
        public DbSet<ObraFaturamento> ObraMedicao { get; set; }
        public DbSet<ObraETO> ObraControleCusto { get; set; }
        public DbSet<ObraUsuarioAprovacao> ObraUsuarioAprovacao { get; set; }
        public DbSet<PedidoCompra> PedidoCompra { get; set; }
        public DbSet<PedidoCompraDevolucaoSaldo> PedidoCompra_DevolucaoSaldo { get; set; }
        public DbSet<PedidoCompraFatura> PedidoCompra_Faturas { get; set; }
        public DbSet<PedidoCompraNotaFiscal> PedidoCompra_NotaFiscal { get; set; }
        public DbSet<PedidoCompraNotaFiscalMateriais> PedidoCompra_NotaFiscal_Materiais { get; set; }
        public DbSet<PedidoCompraNotaFiscalPagamento> PedidoCompra_NotaFiscal_Pagamentos { get; set; }
        public DbSet<PedidoCompraMateriais> PedidoCompra_Materiais { get; set; }
        public DbSet<PedidoCompraArquivos> PedidoCompra_Arquivos { get; set; }
        public DbSet<PedidoInterno> PedidoInterno { get; set; }
        public DbSet<PedidoInternoArquivos> PedidoInterno_Arquivos { get; set; }
        public DbSet<PedidoInternoObras> PedidoInternoObras { get; set; }
        public DbSet<PedidoInternoParcelas> PedidoInternoParcelas { get; set; }
        public DbSet<PedidoInternoParcelaObras> PedidoInternoParcelaObras { get; set; }
        public DbSet<PedidoInternoParcelaDEFs> PedidoInternoParcelaDEFs { get; set; }
        public DbSet<PedidoInternoRecorrente> PedidoInternoRecorrente { get; set; }
        public DbSet<PeriodoAjusteETO> PeriodoAjusteETO { get; set; }
        public DbSet<TipoCentroCusto> TipoCentroCusto { get; set; }
        public DbSet<TipoFornecedor> TipoFornecedor { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<SolicitacaoCompra> SolicitacaoCompra { get; set; }
        public DbSet<SolicitacaoCompraArquivos> SolicitacaoCompra_Arquivos { get; set; }
        public DbSet<SolicitacaoCompraComentarios> SolicitacaoCompra_Comentarios { get; set; }
        public DbSet<SolicitacaoCompraMateriais> SolicitacaoCompra_Materiais { get; set; }
        public DbSet<SolicitacaoCompraMateriaisCotacao> SolicitacaoCompra_MateriaisCotacao { get; set; }
        public DbSet<SolicitacaoCompraMaterialCotacaoPagamentoManual> SolicitacaoCompra_MateriaisCotacao_PagamentoManual { get; set; }
        public DbSet<SolicitacaoCompraRascunho> SolicitacaoCompraRascunho { get; set; }
        public DbSet<StatusSolicitacaoCompra> StatusSolicitacaoCompra { get; set; }
        public DbSet<UnidadeMaterial> UnidadeMaterial { get; set; }


        public WHEngenhariaContext(DbContextOptions<WHEngenhariaContext> options) :base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WHEngenhariaContext).Assembly);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
    }
}
