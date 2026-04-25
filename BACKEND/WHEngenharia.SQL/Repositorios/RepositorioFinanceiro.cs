using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioFinanceiro
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioAgenda _repositorioAgenda;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;
        private RepositorioObra _repositorioObra;

        public RepositorioFinanceiro(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAgenda = new RepositorioAgenda(_context, _mapper);
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
            _repositorioObra = new RepositorioObra(_context, _mapper);
        }

        public async Task<List<Financeiro_EntradaDTO>> ObtemEntradas()
        {
            var result = new List<Financeiro_EntradaDTO>();

            _context.PedidoInterno.AsNoTracking()
                .Include(x => x.FornecedorBeneficiario)
                .Include(x=>x.DEF)
                .Include(x => x.Obra)
                .Include(x => x.Parcelas)
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.UsuarioCadastro)
                .Where(x => x.ImportadoParaFinanceiro == null && x.Aprovado == true).ToList().ForEach(pedidoInterno =>
                {
                    var datasVencimento = "";
                    pedidoInterno.Parcelas.ForEach(parcela =>
                    {
                        datasVencimento += $"[{parcela.DataPagamento.ToString("dd/MM/yyyy")} - {parcela.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}] \n\n ";
                    });

                    result.Add(new Financeiro_EntradaDTO()
                    {
                        Cliente = pedidoInterno.UsuarioBeneficiario?.Nome ?? "",
                        Descricao = $"PI - {pedidoInterno.Descricao}",
                        DataVencimento=datasVencimento,
                        DataSolicitacao = pedidoInterno.DataCadastro,
                        DEF = pedidoInterno.DEF.Codigo,//string.Join(',', pedidoInterno.DEFs.Select(x => x.DEF.Codigo).ToList()),
                        Fornecedor = pedidoInterno.FornecedorBeneficiario?.NomeFantasia ?? "",
                        IdPedidoCompra = null,
                        IdPedidoInterno = pedidoInterno.Id,
                        Obra = pedidoInterno.Obra?.Codigo??"",
                        Valor = pedidoInterno.ValorTotal,
                        PedidoCompra = "",
                        PedidoInterno = pedidoInterno.CodigoFormatado
                    });
                });

            _context.PedidoCompra.AsNoTracking()
                    .Include(x => x.CentroCustoDEF)
                    .Include(x => x.CentroCustoObra)
                    .ThenInclude(x => x.Cliente)
                    .Include(x => x.Faturas)
                    .Include(x => x.Fornecedor)
                    .Include(x => x.NotasFiscais)
                    .Include(x => x.SolicitacaoCompra)
                    .Where(x => x.ImportadoParaFinanceiro == null && x.IdStatusPedidoCompra == 2).ToList().ForEach(pedidoCompra =>
                {
                    var datasVencimento = "";
                    pedidoCompra.Faturas.ForEach(fatura =>
                    {
                        datasVencimento += $"[{fatura.DataFatura.ToString("dd/MM/yyyy")} - {fatura.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}] \n\n ";
                       
                    });

                    result.Add(new Financeiro_EntradaDTO()
                    {
                        Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                        DataVencimento = datasVencimento,
                        DataSolicitacao = pedidoCompra.DataCadastro,
                        DEF = "03.02",
                        Descricao = $"OC - {pedidoCompra.Codigo}",
                        Fornecedor = pedidoCompra.Fornecedor?.NomeFantasia ?? "",
                        IdPedidoCompra = pedidoCompra.Id,
                        IdPedidoInterno = null,
                        Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                        Valor = pedidoCompra.ValorTotal,
                        PedidoCompra = pedidoCompra.Codigo,
                        PedidoInterno = ""
                    });
                });

            _context.PedidoCompra.AsNoTracking()
                .Include(x => x.NotasFiscais)
                .ThenInclude(x => x.Fornecedor)
                .Include(x => x.NotasFiscais)
                .ThenInclude(x => x.Arquivo)
                .Include(x=>x.CentroCustoObra)
                .ThenInclude(x=>x.Cliente)
                .Include(x=>x.Fornecedor)
                .Where(x=> x.IdStatusPedidoCompra != 3 && x.NotasFiscais.Any(y => y.ImportadoParaFinanceiro == null)).ToList().ForEach(pedidoCompra =>
              {
                  var notasFiscais = pedidoCompra.NotasFiscais.Where(x => x.ImportadoParaFinanceiro == null && x.Aprovada == true).ToList();
                  var notasFiscaisAgrupadas = notasFiscais.GroupBy(x => x.IdPedidoCompraArquivo);

                  foreach (var item in notasFiscaisAgrupadas)
                  {
                      var nota = notasFiscais.FirstOrDefault(x => x.IdPedidoCompraArquivo == item.Key);

                      string datasVencimento = "";
                      double valor = 0.0;
                      notasFiscais.Where(x => x.IdPedidoCompraArquivo == item.Key).ToList().ForEach(nf =>
                      {
                          datasVencimento += $"[{nf.DataVencimento.ToString("dd/MM/yyyy")} - {nf.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}] \n\n ";
                          valor += nf.Valor;
                      });
                      result.Add(new Financeiro_EntradaDTO()
                      {
                          IdPedidoCompraNotaFiscal = nota.Id,
                          Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                          DataVencimento = datasVencimento,
                          DataSolicitacao = nota.DataCadastro,
                          DEF = "03.01",
                          Descricao = $"NF - {nota.Arquivo.Nome}",
                          Fornecedor = nota.Fornecedor?.NomeFantasia ?? "",
                          IdPedidoCompra = pedidoCompra.Id,
                          IdPedidoInterno = null,
                          Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                          Valor = valor,
                          PedidoCompra = pedidoCompra.Codigo,
                          PedidoInterno = "",
                          NomeNF = nota.Nome
                      });
                  }

                  //pedidoCompra.NotasFiscais.Where(x => x.ImportadoParaFinanceiro == null && x.Aprovada==true).ToList().ForEach(pedidoCompraNotaFiscal =>
                  //{
                  //    result.Add(new Financeiro_EntradaDTO()
                  //    {
                  //        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                  //        Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                  //        DataVencimento = pedidoCompraNotaFiscal.DataVencimento.ToString("dd/MM/yyyy"),
                  //        DataSolicitacao = pedidoCompraNotaFiscal.DataCadastro,
                  //        DEF = "03.01",
                  //        Descricao = $"NF - {pedidoCompraNotaFiscal.Nome}",
                  //        Fornecedor = pedidoCompraNotaFiscal.Fornecedor?.NomeFantasia ?? "",
                  //        IdPedidoCompra = pedidoCompra.Id,
                  //        IdPedidoInterno = null,
                  //        Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                  //        Valor = pedidoCompraNotaFiscal.Valor,
                  //        PedidoCompra = pedidoCompra.Codigo,
                  //        PedidoInterno = "",
                  //        NomeNF=pedidoCompraNotaFiscal.Nome
                  //    });
                  //});

              });

            return result.OrderBy(x => x.DataSolicitacao).ToList();
        }

        public async Task<List<Financeiro_EntradaDTO>> ObtemEntradasRecusadas()
        {
            var result = new List<Financeiro_EntradaDTO>();

            _context.PedidoInterno.AsNoTracking()
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.Obras)
                .ThenInclude(x => x.Obra)
                .Include(x => x.Parcelas)
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.UsuarioCadastro)
                .Where(x => x.ImportadoParaFinanceiro == false && x.Aprovado == true).ToList().ForEach(pedidoInterno =>
                {
                    var datasVencimento = "";
                    pedidoInterno.Parcelas.ForEach(parcela =>
                    {
                        datasVencimento += $"{parcela.DataPagamento.ToString("dd/MM/yyyy")} e ";
                    });

                    datasVencimento = datasVencimento.Substring(0, datasVencimento.Length - 2);

                    result.Add(new Financeiro_EntradaDTO()
                    {
                        Cliente = pedidoInterno.UsuarioBeneficiario?.Nome ?? "",
                        Descricao = $"PI - {pedidoInterno.Descricao}",
                        DataVencimento = datasVencimento,
                        DataSolicitacao = pedidoInterno.DataCadastro,
                        DEF = "",//string.Join(',', pedidoInterno.DEFs.Select(x => x.DEF.Codigo).ToList()),
                        Fornecedor = pedidoInterno.FornecedorBeneficiario?.NomeFantasia ?? "",
                        IdPedidoCompra = null,
                        IdPedidoInterno = pedidoInterno.Id,
                        Obra = string.Join(',', pedidoInterno.Obras.Select(x => x.Obra.Codigo).ToList()),
                        Valor = pedidoInterno.ValorTotal,
                        PedidoCompra = "",
                        PedidoInterno = pedidoInterno.CodigoFormatado
                    });
                });

            _context.PedidoCompra.AsNoTracking()
                    .Include(x => x.CentroCustoDEF)
                    .Include(x => x.CentroCustoObra)
                    .ThenInclude(x => x.Cliente)
                    .Include(x => x.Faturas)
                    .Include(x => x.Fornecedor)
                    .Include(x => x.NotasFiscais)
                    .Include(x => x.SolicitacaoCompra)
                    .Where(x => x.ImportadoParaFinanceiro == false).ToList().ForEach(pedidoCompra =>
                    {
                        var datasVencimento = "";
                        pedidoCompra.Faturas.ForEach(fatura =>
                        {
                            datasVencimento += $"{fatura.DataFatura.ToString("dd/MM/yyyy")} {Environment.NewLine}";
                        });

                        result.Add(new Financeiro_EntradaDTO()
                        {
                            Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                            DataVencimento = datasVencimento,
                            DataSolicitacao = pedidoCompra.DataCadastro,
                            DEF = "",
                            Descricao = $"OC - {pedidoCompra.Codigo}",
                            Fornecedor = pedidoCompra.Fornecedor?.NomeFantasia ?? "",
                            IdPedidoCompra = pedidoCompra.Id,
                            IdPedidoInterno = null,
                            Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                            Valor = pedidoCompra.ValorTotal,
                            PedidoCompra = pedidoCompra.Codigo,
                            PedidoInterno = ""
                        });
                    });

            _context.PedidoCompra.AsNoTracking()
                .Include(x => x.NotasFiscais)
                .ThenInclude(x => x.Arquivo)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Fornecedor)
                .Where(x => x.NotasFiscais.Any(y => y.ImportadoParaFinanceiro == false)).ToList().ForEach(pedidoCompra =>
                {
                    var notasFiscais = pedidoCompra.NotasFiscais.Where(x => x.ImportadoParaFinanceiro == false && x.Aprovada == true).ToList();
                    var notasFiscaisAgrupadas = notasFiscais.GroupBy(x => x.IdPedidoCompraArquivo);

                    foreach (var item in notasFiscaisAgrupadas)
                    {
                        var nota = notasFiscais.FirstOrDefault(x => x.IdPedidoCompraArquivo == item.Key);

                        string datasVencimento = "";
                        double valor = 0.0;
                        notasFiscais.Where(x => x.IdPedidoCompraArquivo == item.Key).ToList().ForEach(nf =>
                        {
                            datasVencimento += $"[{nf.DataVencimento.ToString("dd/MM/yyyy")} - {nf.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}] \n\n ";
                            valor += nf.Valor;
                        });
                        result.Add(new Financeiro_EntradaDTO()
                        {
                            IdPedidoCompraNotaFiscal = nota.Id,
                            Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                            DataVencimento = datasVencimento,
                            DataSolicitacao = nota.DataCadastro,
                            DEF = "03.01",
                            Descricao = $"NF - {nota.Arquivo.Nome}",
                            Fornecedor = nota.Fornecedor?.NomeFantasia ?? "",
                            IdPedidoCompra = pedidoCompra.Id,
                            IdPedidoInterno = null,
                            Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                            Valor = valor,
                            PedidoCompra = pedidoCompra.Codigo,
                            PedidoInterno = "",
                            NomeNF = nota.Nome
                        });
                    }

                    //pedidoCompra.NotasFiscais.Where(x => x.ImportadoParaFinanceiro == false && x.Aprovada == true).ToList().ForEach(pedidoCompraNotaFiscal =>
                    //{
                    //    result.Add(new Financeiro_EntradaDTO()
                    //    {
                    //        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                    //        Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                    //        DataVencimento = pedidoCompraNotaFiscal.DataVencimento.ToString("dd/MM/yyyy"),
                    //        DataSolicitacao = pedidoCompraNotaFiscal.DataCadastro,
                    //        DEF = "",
                    //        Descricao = $"OC {pedidoCompra.Codigo} NF - {pedidoCompraNotaFiscal.NumeroNotaFiscal}",
                    //        Fornecedor = pedidoCompra.Fornecedor?.NomeFantasia ?? "",
                    //        IdPedidoCompra = pedidoCompra.Id,
                    //        IdPedidoInterno = null,
                    //        Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                    //        Valor = pedidoCompraNotaFiscal.Valor,
                    //        PedidoCompra = pedidoCompra.Codigo,
                    //        PedidoInterno = "",
                    //        NomeNF = pedidoCompraNotaFiscal.Nome
                    //    });
                    //});

                });

            return result.OrderBy(x => x.DataSolicitacao).ToList();
        }

        public async Task AutorizaEntrada(Financeiro_EntradaDTO entradaDTO)
        {
            if (entradaDTO.IdPedidoCompraNotaFiscal.HasValue)
            {
                //Nota fiscal de um pedido de compra

                var def0301 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.01");
                var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal.FirstOrDefaultAsync(y => y.Id == entradaDTO.IdPedidoCompraNotaFiscal);
                var idPedidoCompraNotaFiscalArquivo = pedidoCompraNotaFiscal?.IdPedidoCompraArquivo ?? 0;

                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra_NotaFiscal SET ImportadoParaFinanceiro = 1 WHERE IdPedidoCompraArquivo = {idPedidoCompraNotaFiscalArquivo}");

                var notasFiscais = await _context.PedidoCompra_NotaFiscal.Where(y => y.IdPedidoCompraArquivo == idPedidoCompraNotaFiscalArquivo).ToListAsync();
                var pedidoCompra = await _context.PedidoCompra
                    .Include(x => x.SolicitacaoCompra)
                    .ThenInclude(x => x.CentroCustoObra)
                    .ThenInclude(x => x.Cliente)
                    .FirstOrDefaultAsync(x => x.Id == pedidoCompraNotaFiscal.IdPedidoCompra);

                notasFiscais.ForEach(notaFiscal =>
                {
                    //Adiciona o fluxo de caixa 03.01
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        CodigoDef = def0301.Codigo,
                        CodigoObra = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                        CodigoPedidoCompra = pedidoCompra.Codigo,
                        CodigoPedidoInterno = null,
                        DataLancamento = DateTime.Now,
                        DataPagamento = notaFiscal.DataVencimento,
                        Id = 0,
                        IdCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                        IdDef = def0301.Id,
                        IdFaturamento = null,
                        IdObra = pedidoCompra.IdCentroCustoObra,
                        IdPedidoCompra = pedidoCompra.Id,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = notaFiscal.Id,
                        IdPedidoInterno = null,
                        IdTipoFluxoCaixa = 2,
                        NomeCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = notaFiscal.NumeroNotaFiscal,
                        PagamentoEfetuado = false,
                        Valor = notaFiscal.Valor,
                        IdFornecedorBeneficiario = notaFiscal.IdFornecedor,
                    }).Wait();
                });

                //Abate o valor do fluxo de caixa com codigo 03.02
                await _repositorioObra.RetiraValorDef0302(pedidoCompra.Id, notasFiscais.Sum(x => x.Valor));
            }
            else if (entradaDTO.IdPedidoCompra.HasValue)
            {
                //Apenas o pedido de compra. Cadastrar as previsões de pagamento das faturas

                var pedidoDeCompraParaEntrada = _context.PedidoCompra.AsNoTracking()
                   .Include(x => x.Faturas)
                   .Include(x => x.Fornecedor)
                   .Include(x => x.SolicitacaoCompra)
                   .ThenInclude(x => x.CentroCustoObra)
                   .ThenInclude(x => x.Cliente)
                   .FirstOrDefault(x => x.Id == entradaDTO.IdPedidoCompra);


                _context.Entry(pedidoDeCompraParaEntrada).State = EntityState.Detached;

                var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

                pedidoDeCompraParaEntrada.Faturas.ForEach(fatura =>
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        CodigoDef = def0302.Codigo,
                        CodigoObra = pedidoDeCompraParaEntrada.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                        CodigoPedidoCompra = fatura.CodigoFormatado,
                        CodigoPedidoInterno = null,
                        CodigoFatura = fatura.CodigoFormatado,
                        DataLancamento = DateTime.Now,
                        DataPagamento = fatura.DataFatura,
                        Id = 0,
                        IdCliente = pedidoDeCompraParaEntrada.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                        IdDef = def0302.Id,
                        IdFaturamento = null,
                        IdObra = pedidoDeCompraParaEntrada.IdCentroCustoObra,
                        IdPedidoCompra = pedidoDeCompraParaEntrada.Id,
                        IdPedidoCompraFatura = fatura.Id,
                        IdPedidoCompraNotaFiscal = null,
                        IdPedidoInterno = null,
                        IdTipoFluxoCaixa = 2,
                        NomeCliente = pedidoDeCompraParaEntrada.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = null,
                        PagamentoEfetuado = false,
                        Valor = fatura.Valor,
                        IdFornecedorBeneficiario=pedidoDeCompraParaEntrada.IdFornecedor
                    }).Wait();
                });

                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra SET ImportadoParaFinanceiro = 1 WHERE Id = {pedidoDeCompraParaEntrada.Id}");

                await _repositorioObra.RetiraValorDef0303(pedidoDeCompraParaEntrada.IdCentroCustoObra ?? 0, pedidoDeCompraParaEntrada.ValorTotal);
            }
            else if (entradaDTO.IdPedidoInterno.HasValue)
            {
                //Pedido interno

                var pedidoInterno = await _context.PedidoInterno.AsNoTracking()
                    .Include(x => x.Parcelas)
                    .ThenInclude(x => x.ParcelaDEFs)
                    .ThenInclude(x => x.DEF)
                    .Include(x => x.Parcelas)
                    .ThenInclude(x => x.ParcelaObras)
                    .ThenInclude(x => x.Obra)
                    .Include(x => x.FornecedorBeneficiario)
                    .Include(x => x.UsuarioBeneficiario)
                    .Include(x=>x.DEF)
                    .Include(x => x.Obra)   
                    .ThenInclude(x=>x.Cliente)
                    .FirstOrDefaultAsync(x => x.Id == entradaDTO.IdPedidoInterno);


                pedidoInterno.Parcelas.ForEach(parcela =>
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        CodigoDef = pedidoInterno.DEF.Codigo,
                        CodigoFatura = parcela.CodigoFormatado,
                        CodigoObra =pedidoInterno.Obra?.Codigo??null,
                        CodigoPedidoCompra = null,
                        CodigoPedidoInterno = pedidoInterno.CodigoFormatado,
                        DataLancamento = pedidoInterno.DataCadastro,
                        DataPagamento = parcela.DataPagamento,
                        Id = 0,
                        IdCliente = pedidoInterno?.Obra?.IdCliente??null,
                        IdDef = pedidoInterno.IdDef,
                        IdFaturamento = null,
                        IdFornecedorBeneficiario = pedidoInterno.IdFornecedorBeneficiario,
                        IdObra = pedidoInterno?.IdObra ?? null,
                        IdPedidoCompra = null,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = null,
                        IdPedidoInterno = pedidoInterno.Id,
                        IdTipoFluxoCaixa = pedidoInterno.DEF.Tipo=='P'?2:1,
                        IdUsuarioBeneficiario = pedidoInterno.IdUsuarioBeneficiario,
                        NomeCliente = pedidoInterno?.Obra?.Cliente?.NomeFantasia??null,
                        NumeroNotaFiscalFaturamento = null,
                        NumeroNotaFiscalPedidoCompra = null,
                        PagamentoEfetuado = false,
                        Valor = parcela.Valor
                    }).Wait();

                });

                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoInterno SET ImportadoParaFinanceiro = 1 WHERE Id = {pedidoInterno.Id}");
            }

        }

        public async Task CancelaEntrada(Financeiro_EntradaDTO entradaDTO, Int64 idUsuario)
        {
            if (entradaDTO.IdPedidoCompraNotaFiscal.HasValue)
            {
                var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal.FirstOrDefaultAsync(y => y.Id == entradaDTO.IdPedidoCompraNotaFiscal);
                var idPedidoCompraNotaFiscalArquivo = pedidoCompraNotaFiscal?.IdPedidoCompraArquivo ?? 0;

                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra_NotaFiscal SET ImportadoParaFinanceiro = 0 WHERE IdPedidoCompraArquivo = {idPedidoCompraNotaFiscalArquivo}");

                var nomeUsuario = _context.Usuario.FirstOrDefault(x => x.Id == idUsuario)?.Nome ?? "";

                var novoHistorico = new Historico();
                novoHistorico.Id = Guid.NewGuid();
                novoHistorico.Campo = $"Nota fiscal cancelada: {entradaDTO.MotivoRecusa}";
                novoHistorico.Data = DateTime.Now;
                novoHistorico.IdPedidoCompra = entradaDTO.IdPedidoCompra;
                novoHistorico.IdUsuario = idUsuario;
                novoHistorico.Usuario = nomeUsuario;

                await _context.AddAsync(novoHistorico);
                await _context.SaveChangesAsync();
            }
            else if (entradaDTO.IdPedidoCompra.HasValue)
                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra SET ImportadoParaFinanceiro = 0 WHERE Id = {entradaDTO.IdPedidoCompra}");
            else if (entradaDTO.IdPedidoInterno.HasValue)
                await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoInterno SET ImportadoParaFinanceiro = 0 WHERE Id = {entradaDTO.IdPedidoInterno}");
        }
    }
}
