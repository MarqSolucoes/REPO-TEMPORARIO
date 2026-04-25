using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Conciliacao;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoCompra;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioPedidoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioAgenda _repositorioAgenda;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;
        private RepositorioObra _repositorioObra;

        public RepositorioPedidoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAgenda = new RepositorioAgenda(_context, _mapper);
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
            _repositorioObra = new RepositorioObra(_context, _mapper);
        }

        public async Task<PedidoCompraDTO> Get(Int64 id)
        {
            return _mapper.Map<PedidoCompraDTO>(await _context.PedidoCompra.AsNoTracking()
                .Include(x => x.StatusPedidoCompra)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioDiretorAprovador)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioEngenheiroAprovador)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.CentroCustoObra)
                .Include(x => x.Fornecedor)
                .ThenInclude(x => x.Cidade)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .ThenInclude(x => x.UnidadeMaterial)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                //.Include(x => x.NotasFiscais)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x => x.CondicaoPagamento)
                .Include(x => x.Faturas)
                .Include(x=>x.UsuarioComprador)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<List<PedidoCompraDTO>> GetByStatus(Int64 idStatus)
        {
            return _mapper.Map<List<PedidoCompraDTO>>(await _context.PedidoCompra.AsNoTracking()
                .Include(x=>x.Faturas)
                .Include(x => x.StatusPedidoCompra)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioDiretorAprovador)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioEngenheiroAprovador)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.Fornecedor)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x => x.CondicaoPagamento)
                .Include(x=>x.UsuarioComprador)
                .Where(x => x.IdStatusPedidoCompra == idStatus).ToListAsync());
        }

        public async Task<List<PedidoCompraDTO>> ObtemFinalizadosFiltrados(PedidoCompra_FiltroPedidosFinalizadosDTO parametrosDTO)
        {
            var query = _context.PedidoCompra.AsNoTracking()
                .Include(x => x.StatusPedidoCompra)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x=>x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Fornecedor)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x => x.CondicaoPagamento)
                .Include(x=>x.UsuarioComprador)
                .AsQueryable();

            query = query.Where(x => x.IdStatusPedidoCompra > 1);

            if (parametrosDTO.dataSolicitacaoInicial.HasValue)
                query = query.Where(x => x.DataCadastro >= parametrosDTO.dataSolicitacaoInicial.Value);

            if (parametrosDTO.dataSolicitacaoFinal.HasValue)
                query = query.Where(x => x.DataCadastro.Date <= parametrosDTO.dataSolicitacaoFinal.Value.Date);

            if (parametrosDTO.idsCliente != null && parametrosDTO.idsCliente.Count > 0)
                query = query.Where(x => parametrosDTO.idsCliente.Contains(x.CentroCustoObra.IdCliente));

            if (parametrosDTO.idsFornecedor != null && parametrosDTO.idsFornecedor.Count > 0)
                query = query.Where(x => parametrosDTO.idsFornecedor.Contains(x.IdFornecedor));

            if (parametrosDTO.idsMateriais != null && parametrosDTO.idsMateriais.Count > 0)
                query = query.Where(x => x.Materiais.Any(y => parametrosDTO.idsMateriais.Contains(y.IdMaterial)));

            if (parametrosDTO.idsObra != null && parametrosDTO.idsObra.Count > 0)
                query = query.Where(x => parametrosDTO.idsObra.Contains(x.IdCentroCustoObra.Value));

            if (parametrosDTO.idsPedidoCompra != null && parametrosDTO.idsPedidoCompra.Count > 0)
                query = query.Where(x => parametrosDTO.idsPedidoCompra.Contains(x.Id));

            var result = await query.ToListAsync();

            result.ForEach(pedidoCompra =>
            {
                pedidoCompra.NotasFiscais = _context.PedidoCompra_NotaFiscal.AsNoTracking().Include(x=>x.Arquivo).Include(x=>x.Fornecedor).Where(x => x.IdPedidoCompra == pedidoCompra.Id).ToList();
            });

            return _mapper.Map<List<PedidoCompraDTO>>(result);
        }

        public async Task<List<PedidoCompraDTO>> GetParaConciliacaoPorUsuario(Int64 idUsuario)
        {
            var pedidosMateriais = _mapper.Map<List<PedidoCompraDTO>>(await _context.PedidoCompra.AsNoTracking()
                .Include(x => x.StatusPedidoCompra)
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x=>x.CentroCustoObra)
                .Include(x => x.Fornecedor)
                .ThenInclude(x=>x.Cidade)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x => x.CondicaoPagamento)
                .Include(x=>x.Faturas)
                .Where(x =>
                    x.IdStatusPedidoCompra == 2 &&
                    x.ImportadoParaFinanceiro == true &&
                    !x.CentroCustoObra.Cancelada &&
                    (!x.SolicitacaoCompra.Servico && x.Materiais.Any(y => y.QuantidadeConciliada < y.Quantidade)) &&
                    (x.NotasFiscais.Where(nf => nf.Aprovada == true).Sum(y => y.Valor) + x.CancelamentosSaldo.Sum(y => y.Valor)) < x.ValorTotal).ToListAsync());

            var pedidosServicos = _mapper.Map<List<PedidoCompraDTO>>(await _context.PedidoCompra.AsNoTracking()
                .Include(x => x.StatusPedidoCompra)
                .Include(x => x.SolicitacaoCompra)
                .Include(x => x.Fornecedor)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x => x.CondicaoPagamento)
                .Include(x => x.Faturas)
                .Where(x =>
                    x.IdStatusPedidoCompra == 2 &&
                    x.ImportadoParaFinanceiro == true &&
                    !x.CentroCustoObra.Cancelada &&
                    (x.SolicitacaoCompra.Servico && (x.NotasFiscais.Where(nf=>nf.Aprovada ==true).Sum(y => y.Valor) + x.CancelamentosSaldo.Sum(y => y.Valor)) < x.ValorTotal)).ToListAsync());

            pedidosMateriais.AddRange(pedidosServicos);

            pedidosMateriais.ForEach(x =>
            {
                x.NotasFiscais = _mapper.Map<List<PedidoCompraNotaFiscalDTO>>(_context.PedidoCompra_NotaFiscal.Where(y => y.IdPedidoCompra == x.Id).ToList());
            });

            return pedidosMateriais;
        }


        public async Task<List<Conciliacao_ReprovadaDirecaoDTO>> ConciliacaoReprovadasDirecao()
        {
            var sql = @"
        SELECT
	                        PCNF.Id AS IdNotaFiscal,
            PCNF.Nome AS NotaFiscal,
            PCNF.Descricao AS DescricaoNotaFiscal,
            PCNF.NumeroNotaFiscal AS NumeroNotaFiscal,
            PC.Codigo AS Pedido,
            PC.DataCadastro AS DataPedido,
            PC.Id AS IdPedidoCompra,
            O.Codigo AS Obra,
            O.EnderecoEntrega AS EnderecoEntregaObra,
            C.NomeFantasia AS Cliente,
            C.CNPJ AS CNPJCliente,
            UsuarioAprovador.Nome AS UsuarioAprovador,
            PCNF.DataAprovacao AS DataAprovacao,
            PCA.Id AS IdArquivo,
            PCA.Nome AS NomeArquivo
        FROM PedidoCompra_NotaFiscal PCNF
        INNER JOIN PedidoCompra PC ON PC.Id = PCNF.IdPedidoCompra
        INNER JOIN Obra O ON O.Id = PC.IdCentroCustoObra
        INNER JOIN Cliente C ON C.Id = O.IdCliente
        INNER JOIN Fornecedor F ON F.Id = PCNF.IdFornecedor
        INNER JOIN PedidoCompra_Arquivos PCA ON PCA.Id = PCNF.IdPedidoCompraArquivo
        INNER JOIN Usuario UsuarioAprovador ON UsuarioAprovador.Id = PCNF.IdUsuarioAprovacao
        WHERE PCNF.Aprovada = 0 AND DevolucaoVisualizada = 0";

            var lista = new List<Conciliacao_ReprovadaDirecaoDTO>();

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new Conciliacao_ReprovadaDirecaoDTO
                            {
                                IdNotaFiscal = Convert.ToInt64(reader["IdNotaFiscal"].ToString()),
                                NotaFiscal = reader["NotaFiscal"].ToString(),
                                DescricaoNotaFiscal = reader["DescricaoNotaFiscal"].ToString(),
                                NumeroNotaFiscal = reader["NumeroNotaFiscal"].ToString(),
                                Pedido = reader["Pedido"].ToString(),
                                DataPedido = Convert.ToDateTime(reader["DataPedido"]),
                                Obra = reader["Obra"].ToString(),
                                EnderecoEntregaObra = reader["EnderecoEntregaObra"].ToString(),
                                Cliente = reader["Cliente"].ToString(),
                                CNPJCliente = reader["CNPJCliente"].ToString(),
                                UsuarioAprovador = reader["UsuarioAprovador"].ToString(),
                                DataAprovacao = Convert.ToDateTime(reader["DataAprovacao"]),
                                IdArquivo = Convert.ToInt64(reader["IdArquivo"].ToString()),
                                NomeArquivo = reader["NomeArquivo"].ToString(),
                                IdPedidoCompra = Convert.ToInt64(reader["IdPedidoCompra"].ToString())
                            };

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public async Task ReenviarNotaFiscalParaDiretoria(Int64 idPedidoCompraNotaFiscal)
        {
            var idPedidoComrpaArquivo = _context.PedidoCompra_NotaFiscal.FirstOrDefault(x => x.Id == idPedidoCompraNotaFiscal).IdPedidoCompraArquivo;

            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = NULL, DevolucaoVisualizada = 1, DataAprovacao = NULL, IdUsuarioAprovacao = NULL WHERE IdPedidoCompraArquivo = {idPedidoComrpaArquivo}");
        }

        public async Task<List<Conciliacao_ReprovadaDirecaoDTO>> ConciliacaoReprovadasFinanceiro()
        {
            var lista = new List<Conciliacao_ReprovadaDirecaoDTO>();

            _context.PedidoCompra.AsNoTracking()
                .Include(x => x.NotasFiscais)
                .ThenInclude(x => x.Arquivo)
                .Include(x => x.NotasFiscais)
                .ThenInclude(x => x.UsuarioDiretorAprovador)
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

                        lista.Add(new Conciliacao_ReprovadaDirecaoDTO()
                        {
                            Cliente = pedidoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? "",
                            CNPJCliente = pedidoCompra.CentroCustoObra?.Cliente?.CNPJ,
                            DataPedido = pedidoCompra.DataCadastro,
                            DataAprovacao = nota.DataAprovacao ?? DateTime.MinValue,
                            DescricaoNotaFiscal = nota.Descricao,
                            EnderecoEntregaObra = pedidoCompra.CentroCustoObra?.EnderecoEntrega ?? "",
                            IdArquivo = nota.IdPedidoCompraArquivo,
                            IdNotaFiscal = nota.Id,
                            IdPedidoCompra = pedidoCompra.Id,
                            NomeArquivo = nota.Arquivo.Nome,
                            NotaFiscal = nota.Nome,
                            NumeroNotaFiscal = nota.NumeroNotaFiscal,
                            Obra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                            Pedido = pedidoCompra.Codigo,
                            UsuarioAprovador = nota.UsuarioDiretorAprovador?.Nome ?? ""
                        });
                    }
                });
            return lista;
        }

        public async Task ReenviarNotaFiscalParaFinanceiro(Int64 idPedidoCompraNotaFiscal)
        {
            var idPedidoComrpaArquivo = _context.PedidoCompra_NotaFiscal.FirstOrDefault(x => x.Id == idPedidoCompraNotaFiscal).IdPedidoCompraArquivo;

            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET ImportadoParaFinanceiro = NULL, DevolucaoVisualizada = 1 WHERE idPedidoCompraArquivo = {idPedidoComrpaArquivo}");
        }

        public async Task MarcarVisualizacaoNotaFiscalReprovada(Int64 idPedidoCompraNotaFiscal)
        {
            var idPedidoComrpaArquivo = _context.PedidoCompra_NotaFiscal.FirstOrDefault(x => x.Id == idPedidoCompraNotaFiscal).IdPedidoCompraArquivo;

            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET DevolucaoVisualizada = 1 WHERE idPedidoCompraArquivo = {idPedidoComrpaArquivo}");
        }

        public async Task<Conciliacao_PedidoCompraDTO> GetParaConciliacao(Int64 id)
        {
            var pedidoCompra = _mapper.Map<PedidoCompraDTO>(await _context.PedidoCompra.AsNoTracking()
                .Include(x=>x.SolicitacaoCompra)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .Include(x => x.NotasFiscais)
                .ThenInclude(x=>x.Fornecedor)
                .Include(x => x.CancelamentosSaldo)
                .ThenInclude(x => x.MotivoDevolucao)
                .Include(x=>x.Fornecedor)
                .Include(x=>x.CondicaoPagamento)
                .ThenInclude(x=>x.Parcelas)
                .Include(x => x.Faturas)
                .FirstOrDefaultAsync(x => x.Id == id));

            var conciliacao_PedidoCompraDTO = new Conciliacao_PedidoCompraDTO();
            conciliacao_PedidoCompraDTO.Descricao = "";
            conciliacao_PedidoCompraDTO.Fornecedor = pedidoCompra.Fornecedor;
            conciliacao_PedidoCompraDTO.ValorFrete = 0;
            conciliacao_PedidoCompraDTO.ValorImposto = 0;
            conciliacao_PedidoCompraDTO.Codigo = pedidoCompra.Codigo;
            conciliacao_PedidoCompraDTO.Id = pedidoCompra.Id;
            conciliacao_PedidoCompraDTO.ValorTotalPedido = pedidoCompra.ValorTotal;
            conciliacao_PedidoCompraDTO.ValorGasto = pedidoCompra.NotasFiscais.Where(x => x.Aprovada == true).Sum(x => x.Valor);
            conciliacao_PedidoCompraDTO.ValorPendenteAprovacao = pedidoCompra.NotasFiscais.Where(x => !x.Aprovada.HasValue).Sum(x => x.Valor);
            conciliacao_PedidoCompraDTO.ValorPrevistoGasto = pedidoCompra.NotasFiscais.Where(x => x.Aprovada == true && !x.PagamentoEfetuado).Sum(x => x.Valor);
            conciliacao_PedidoCompraDTO.SaldoCancelado = pedidoCompra.CancelamentosSaldo.Sum(x => x.Valor);
            conciliacao_PedidoCompraDTO.SaldoDisponivel = conciliacao_PedidoCompraDTO.ValorTotalPedido - conciliacao_PedidoCompraDTO.ValorGasto- conciliacao_PedidoCompraDTO.SaldoCancelado- conciliacao_PedidoCompraDTO.ValorPendenteAprovacao;
            conciliacao_PedidoCompraDTO.Faturas = pedidoCompra.Faturas;

            if (pedidoCompra.CondicaoPagamento.Descricao != "Manual")
                conciliacao_PedidoCompraDTO.DataVencimento = pedidoCompra.DataCadastro.AddDays(pedidoCompra.CondicaoPagamento.Parcelas.FirstOrDefault().DiasCorridos);
            
            conciliacao_PedidoCompraDTO.CondicaoPagamento = pedidoCompra.CondicaoPagamento;


            conciliacao_PedidoCompraDTO.CancelamentosSaldo = new List<Conciliacao_PedidoCompraCancelamentoSaldoDTO>();
            conciliacao_PedidoCompraDTO.Materiais = new List<Conciliacao_PedidoCompraMateriaisDTO>();
            conciliacao_PedidoCompraDTO.NotasFiscais = new List<Conciliacao_PedidoCompraNotaFiscalDTO>();

            if (pedidoCompra.SolicitacaoCompra.Servico)
            {
                pedidoCompra.Materiais.ToList().ForEach(x =>
                    {
                        conciliacao_PedidoCompraDTO.Materiais.Add(new Conciliacao_PedidoCompraMateriaisDTO()
                        {
                            Id = x.Id,
                            Material = x.Material.Descricao,
                            Quantidade = x.Quantidade,
                            QuantidadeConciliada = x.QuantidadeConciliada,
                            ValorComprado = x.ValorUnitario
                        });
                    });
            }
            else
            {
                pedidoCompra.Materiais.Where(x => x.QuantidadeConciliada < x.Quantidade).ToList().ForEach(x =>
                {
                    conciliacao_PedidoCompraDTO.Materiais.Add(new Conciliacao_PedidoCompraMateriaisDTO()
                    {
                        Id = x.Id,
                        Material = x.Material.Descricao,
                        Quantidade = x.Quantidade,
                        QuantidadeConciliada = x.QuantidadeConciliada,
                        ValorComprado = x.ValorUnitario
                    });
                });
            }

            pedidoCompra.NotasFiscais.ForEach(x =>
            {
                var arquivo = pedidoCompra.Arquivos.FirstOrDefault(y => y.Id == x.IdPedidoCompraArquivo);

                conciliacao_PedidoCompraDTO.NotasFiscais.Add(new Conciliacao_PedidoCompraNotaFiscalDTO()
                {
                    Id = x.Id,
                    IdArquivo = arquivo.Id,
                    Aprovada = x.Aprovada,
                    DataAprovacao = x.DataAprovacao,
                    DataCadastro = x.DataCadastro,
                    Descricao = x.Descricao,
                    PagamentoEfetuado = x.PagamentoEfetuado,
                    Valor = x.Valor,
                    Nome = arquivo.Nome,
                    NumeroNotaFiscal = x.NumeroNotaFiscal,
                    NomeLogico = arquivo.NomeLogico,
                    DataVencimento = x.DataVencimento,
                    Fornecedor=x.Fornecedor
                });
            });

            pedidoCompra.CancelamentosSaldo.ForEach(x =>
            {
                conciliacao_PedidoCompraDTO.CancelamentosSaldo.Add(new Conciliacao_PedidoCompraCancelamentoSaldoDTO()
                {
                    Id = x.Id,
                    Observacao = x.Observacao,
                    Valor = x.Valor,
                    DataCadastro = x.DataCadastro,
                    MotivoDevolucao = x.MotivoDevolucao
                });
            });

            return conciliacao_PedidoCompraDTO;
        }

        public async Task DevolverParaCotacao(Int64 id, Int64 idUsuario)
        {
            var idObra = (await _context.PedidoCompra.FirstOrDefaultAsync(x => x.Id == id))?.IdCentroCustoObra ?? 0;
            var obra = await _context.Obra.AsNoTracking().Include(x => x.Cliente).FirstOrDefaultAsync(x => x.Id == idObra);
            var valor = _context.FluxoCaixa.Where(x => x.IdPedidoCompra == id && x.CodigoDef == "03.02")?.Sum(x => x.Valor)??0;

            //Apaga todas as previsões de pagamento do pedido
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompra = {id} AND CodigoDef = '03.02'");

            //Cancela o pedido
            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra SET IdStatusPedidoCompra = 3, DataUltimaAlteracao = GETDATE(), IdUsuarioAlteracao = {idUsuario} WHERE Id = {id}");

            //Volta a solicitação de compra para o status "aprovado pela diretoria"
            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 2, DataUltimaAlteracao = GETDATE(), IdUsuarioAlteracao = {idUsuario} WHERE Id = (SELECT PC.IdSolicitacaoCompra FROM PedidoCompra PC WHERE PC.Id = {id})");
            
            var previsaoFluxoCaixa = _context.FluxoCaixa.FirstOrDefault(x => x.IdObra == idObra && x.CodigoDef == "03.03");

            if (previsaoFluxoCaixa != null)
            {
                previsaoFluxoCaixa.Valor += valor;

                _context.Entry(previsaoFluxoCaixa).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                var def0303 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.03");

                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    Id = 0,
                    IdDef = def0303.Id,
                    CodigoDef = def0303.Codigo,
                    IdObra = idObra,
                    CodigoObra = obra.Codigo ?? "",
                    IdPedidoCompra = null,
                    IdPedidoCompraFatura = null,
                    CodigoPedidoCompra = null,
                    IdPedidoInterno = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = DateTime.Now,
                    DataPagamento = DateTime.Now.AddDays(10),
                    IdCliente = obra.IdCliente,
                    NomeCliente = obra.Cliente.NomeFantasia,
                    NumeroNotaFiscalPedidoCompra = null,
                    Valor = valor,
                    IdFornecedorBeneficiario = null
                }).Wait();
            }

        }

        public async Task<List<PedidoCompraNotaFiscalDTO>> ObtemNotasConciliadas(Conciliacao_NotasConciliadasParametrosDTO parametrosDTO)
        {
            var query = _context.PedidoCompra_NotaFiscal
                .AsNoTracking()
                .Include(x=>x.Arquivo)
                .AsQueryable();

            if (parametrosDTO.DataInicial.HasValue)
                query = query.Where(x => x.DataVencimento.Date >= parametrosDTO.DataInicial.Value.Date);

            if (parametrosDTO.DataFinal.HasValue)
                query = query.Where(x => x.DataVencimento.Date <= parametrosDTO.DataFinal.Value.Date);

            if (parametrosDTO.IdsFornecedores != null && parametrosDTO.IdsFornecedores.Count > 0)
                query = query.Where(x => parametrosDTO.IdsFornecedores.Contains(x.PedidoCompra.IdFornecedor));

            if (!string.IsNullOrEmpty(parametrosDTO.NumeroNotaFiscal))
                query = query.Where(x => x.NumeroNotaFiscal == parametrosDTO.NumeroNotaFiscal);

            if (!string.IsNullOrEmpty(parametrosDTO.NumeroPedido))
                query = query.Where(x => x.PedidoCompra.Codigo.EndsWith(parametrosDTO.NumeroPedido));

            if (parametrosDTO.Valor.HasValue && parametrosDTO.Valor.Value>0)
                query = query.Where(x => x.Valor == parametrosDTO.Valor);

            var result = await query.ToListAsync();

            var idsPedidoCompra = result.Select(x => x.IdPedidoCompra).Distinct().ToList();

            var pedidosCompra = _context.PedidoCompra.Include(x=>x.Fornecedor).AsNoTracking().Where(x => idsPedidoCompra.Contains(x.Id));


            result.ForEach(x =>
            {
                x.PedidoCompra = pedidosCompra.FirstOrDefault(y => y.Id == x.IdPedidoCompra);
                x.PedidoCompra.Codigo = x.PedidoCompra.Codigo;
            });

            return _mapper.Map<List<PedidoCompraNotaFiscalDTO>>(result);
        }

        public async Task<PedidoCompraNotaFiscalDTO> Post(PedidoCompraNotaFiscalDTO pedidoCompraNotaFiscalDTO)
        {
            try
            {
                var pedidoCompraNotaFiscal = new PedidoCompraNotaFiscal();
                pedidoCompraNotaFiscal.Aprovada = pedidoCompraNotaFiscalDTO.Aprovada;
                pedidoCompraNotaFiscal.DevolucaoVisualizada = false;
                pedidoCompraNotaFiscal.DataVencimento = pedidoCompraNotaFiscalDTO.DataVencimento;
                pedidoCompraNotaFiscal.DataCadastro = pedidoCompraNotaFiscalDTO.DataCadastro;
                pedidoCompraNotaFiscal.DataUltimaAlteracao = pedidoCompraNotaFiscalDTO.DataUltimaAlteracao;
                pedidoCompraNotaFiscal.Descricao = pedidoCompraNotaFiscalDTO.Descricao;
                pedidoCompraNotaFiscal.Id = pedidoCompraNotaFiscalDTO.Id;
                pedidoCompraNotaFiscal.IdPedidoCompra = pedidoCompraNotaFiscalDTO.IdPedidoCompra;
                pedidoCompraNotaFiscal.IdPedidoCompraArquivo = pedidoCompraNotaFiscalDTO.IdPedidoCompraArquivo;
                pedidoCompraNotaFiscal.IdUsuarioAlteracao = pedidoCompraNotaFiscalDTO.IdUsuarioAlteracao;
                pedidoCompraNotaFiscal.IdUsuarioCadastro = pedidoCompraNotaFiscalDTO.IdUsuarioCadastro;
                pedidoCompraNotaFiscal.Nome = pedidoCompraNotaFiscalDTO.Nome;
                pedidoCompraNotaFiscal.NumeroNotaFiscal = pedidoCompraNotaFiscalDTO.NumeroNotaFiscal;
                pedidoCompraNotaFiscal.ImportadoParaFinanceiro = pedidoCompraNotaFiscalDTO.ImportadoParaFinanceiro;
                pedidoCompraNotaFiscal.Cancelada = false;

                await _context.PedidoCompra_NotaFiscal.AddAsync(pedidoCompraNotaFiscal);
                await _context.SaveChangesAsync();

                return _mapper.Map<PedidoCompraNotaFiscalDTO>(pedidoCompraNotaFiscal);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<PedidoCompraDTO> Put(PedidoCompraDTO pedidoCompraDTO)
        {
            var nomeUsuarioAlteracao = _context.Usuario.FirstOrDefault(x => x.Id == pedidoCompraDTO.IdUsuarioAlteracao)?.Nome ?? "";

            var pedidoCompra = await _context.PedidoCompra.FirstOrDefaultAsync(x => x.Id == pedidoCompraDTO.Id);

            if (pedidoCompra == null)
                throw new Exception("Pedido de compra não identificado");


            var possuiAlteracaoCondicaoPagamento = pedidoCompra.IdCondicaoPagamento == pedidoCompraDTO.IdCondicaoPagamento ? false : true;

            if(possuiAlteracaoCondicaoPagamento)
            {
                if (pedidoCompraDTO.IdCondicaoPagamento == 7 && pedidoCompraDTO.Faturas.Sum(x => x.Valor) != pedidoCompraDTO.ValorTotal)
                    throw new Exception("Valor das faturas é diferente do valor do pedido");
            }

            pedidoCompra.DataUltimaAlteracao = pedidoCompraDTO.DataUltimaAlteracao;
            pedidoCompra.IdStatusPedidoCompra = pedidoCompraDTO.IdStatusPedidoCompra;
            pedidoCompra.IdUsuarioAlteracao = pedidoCompraDTO.IdUsuarioAlteracao;
            pedidoCompra.MotivoCancelamento = pedidoCompraDTO.MotivoCancelamento;
            pedidoCompra.IdFornecedor = pedidoCompraDTO.IdFornecedor;
            pedidoCompra.IdCondicaoPagamento = pedidoCompraDTO.IdCondicaoPagamento;
            pedidoCompra.ImportadoParaFinanceiro = pedidoCompraDTO.ImportadoParaFinanceiro;
            pedidoCompra.EnderecoEntrega = pedidoCompraDTO.EnderecoEntrega;
            pedidoCompra.DataEntrega = pedidoCompraDTO.DataEntrega;
            pedidoCompra.ObservacaoParaFornecedor = pedidoCompraDTO.ObservacaoParaFornecedor;
            pedidoCompra.ValorDesconto = pedidoCompraDTO.ValorDesconto;
            pedidoCompra.Frete = pedidoCompraDTO.Frete;
            pedidoCompra.Imposto = pedidoCompraDTO.Imposto;

            pedidoCompra.ValorTotal = pedidoCompraDTO.Materiais.Sum(x => x.ValorUnitario * x.Quantidade) + pedidoCompraDTO.Frete + pedidoCompraDTO.Imposto;

            _context.Entry(pedidoCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            pedidoCompra = await _context.PedidoCompra
                .Include(x => x.SolicitacaoCompra)
                .ThenInclude(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == pedidoCompraDTO.Id);

            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET EnderecoEntrega = '{pedidoCompraDTO.EnderecoEntrega}' WHERE Id = {pedidoCompra.IdSolicitacaoCompra}");

            //if (pedidoCompra.IdStatusPedidoCompra == 3)
            //{
            //    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompra =  {pedidoCompra.Id}");
            //    await _repositorioObra.AtualizaMedicoesFluxoCaixa(pedidoCompra.IdCentroCustoObra??0, pedidoCompra.ValorTotal);
            //}

            pedidoCompraDTO.Materiais.ForEach(pedidoCompraMaterialDTO =>
            {
                var pedidoCompra_Material = _context.PedidoCompra_Materiais.FirstOrDefault(x => x.Id == pedidoCompraMaterialDTO.Id);
                pedidoCompra_Material.Quantidade = pedidoCompraMaterialDTO.Quantidade;
                pedidoCompra_Material.ValorUnitario = pedidoCompraMaterialDTO.ValorUnitario;
                pedidoCompra_Material.ValorTotal = pedidoCompraMaterialDTO.Quantidade * pedidoCompraMaterialDTO.ValorUnitario;

                _context.Entry(pedidoCompra_Material).State = EntityState.Modified;
                
            });

            _context.SaveChanges();

            //Atualizar valor do pedido de compra
            //await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE PedidoCompra SET ValorTotal = (SELECT SUM((PCM.Quantidade * PCM.ValorUnitario) * ((100 - (SCMC.Desconto))/100)) FROM PedidoCompra_Materiais PCM INNER JOIN PedidoCompra PC ON PC.Id = PCM.IdPedidoCompra INNER JOIN SolicitacaoCompra_Materiais SCM ON SCM.IdMaterial = PCM.IdMaterial AND SCM.IdSolicitacaoCompra = PC.IdSolicitacaoCompra INNER JOIN SolicitacaoCompra_MateriaisCotacao SCMC ON SCMC.IdSolicitacaoCompraMaterial = SCM.Id AND SCMC.IdFornecedor = PC.IdFornecedor WHERE PCM.IdPedidoCompra = {pedidoCompraDTO.Id}) + Frete + Imposto WHERE Id = {pedidoCompra.Id}");

            //Atualizar faturas do pedido de compra
            var condicaoPagamento = _context.CondicaoPagamento.Include(x => x.Parcelas).FirstOrDefault(x => x.Id == pedidoCompra.IdCondicaoPagamento);
            var pedidoCompraFaturas = await _context.PedidoCompra_Faturas.Where(x => x.IdPedidoCompra == pedidoCompra.Id).ToListAsync();
            
            var intChar = 65;
            
            var maisDeUmaParcela = condicaoPagamento.Parcelas.Count > 1 ? true : false;
       
                await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraFatura IN (SELECT PCF.Id FROM PedidoCompra_Faturas PCF WHERE PCF.IdPedidoCompra = {pedidoCompraDTO.Id})");
                await _context.Database.ExecuteSqlRawAsync($"DELETE FROM PedidoCompra_Faturas WHERE IdPedidoCompra = {pedidoCompraDTO.Id}");

                var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

                if (condicaoPagamento.Id == 7)
                {
                    //A condição de pagamento é manual, portanto precisa buscar os valores corretamente

                    pedidoCompraDTO.Faturas.ForEach(faturaDTO =>
                    {
                        var fatura = new PedidoCompraFatura();
                        fatura.Id = 0;
                        fatura.IdPedidoCompra = pedidoCompraDTO.Id;
                        fatura.DataFatura = faturaDTO.DataFatura;
                        fatura.PagamentoEfetuado = false;
                        fatura.Valor = faturaDTO.Valor;
                        fatura.CodigoFormatado = $"{pedidoCompraDTO.Codigo}{(maisDeUmaParcela ? $".{Convert.ToChar(intChar)}" : "")}";

                        var novoHistorico = new Historico();
                        novoHistorico.Id = Guid.NewGuid();
                        novoHistorico.Campo = $"Pagamento na data '{fatura.DataFatura.ToString("dd/MM/yyyy")}' com valor {fatura.Valor.ToString().Replace(',', '.')} inserido";
                        novoHistorico.Data = DateTime.Now;
                        novoHistorico.IdPedidoCompra = pedidoCompraDTO.Id;
                        novoHistorico.IdUsuario = pedidoCompraDTO.IdUsuarioAlteracao;
                        novoHistorico.Usuario = nomeUsuarioAlteracao;
                        _context.Historico.Add(novoHistorico);

                        _context.PedidoCompra_Faturas.Add(fatura);
                        _context.SaveChanges();

                        intChar++;

                        //_repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        //{
                        //    Id = 0,
                        //    IdDef = def0302.Id,
                        //    CodigoDef = def0302.Codigo,
                        //    IdObra = pedidoCompra.IdCentroCustoObra,
                        //    CodigoObra = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                        //    IdPedidoCompra = pedidoCompra.Id,
                        //    IdPedidoCompraFatura = fatura.Id,
                        //    CodigoPedidoCompra = pedidoCompra.Codigo,
                        //    IdPedidoInterno = null,
                        //    CodigoPedidoInterno = null,
                        //    DataLancamento = DateTime.Now,
                        //    DataPagamento = fatura.DataFatura,
                        //    IdCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                        //    NomeCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        //    NumeroNotaFiscalPedidoCompra = null,
                        //    Valor = fatura.Valor,
                        //    IdFornecedorBeneficiario = pedidoCompra.IdFornecedor
                        //}).Wait();
                    });
                }
                else
                {
                    
                    condicaoPagamento.Parcelas.ForEach(parcelaCondicaoCompra =>
                    {
                        var fatura = new PedidoCompraFatura();
                        fatura.Id = 0;
                        fatura.IdPedidoCompra = pedidoCompraDTO.Id;
                        fatura.DataFatura = pedidoCompraDTO.DataCadastro.AddDays(parcelaCondicaoCompra.DiasCorridos);
                        fatura.PagamentoEfetuado = false;
                        fatura.Valor = pedidoCompra.ValorTotal / condicaoPagamento.Parcelas.Count;
                        fatura.CodigoFormatado = $"{pedidoCompraDTO.Codigo}{(maisDeUmaParcela ? $".{Convert.ToChar(intChar)}" : "")}";

                        var novoHistorico = new Historico();
                        novoHistorico.Id = Guid.NewGuid();
                        novoHistorico.Campo = $"Pagamento na data '{fatura.DataFatura.ToString("dd/MM/yyyy")}' com valor {fatura.Valor.ToString().Replace(',', '.')} inserido";
                        novoHistorico.Data = DateTime.Now;
                        novoHistorico.IdPedidoCompra = pedidoCompraDTO.Id;
                        novoHistorico.IdUsuario = pedidoCompraDTO.IdUsuarioAlteracao;
                        novoHistorico.Usuario = nomeUsuarioAlteracao;
                        _context.Historico.Add(novoHistorico);

                        _context.PedidoCompra_Faturas.Add(fatura);
                        _context.SaveChanges();

                        intChar++;
                    });
                }




            

                //Atualizar informações do pedido no fluxo de caixa
                //Atualizar ETO
                return _mapper.Map<PedidoCompraDTO>(pedidoCompra);
        }

        public async Task<PedidoCompraArquivosDTO> Upload(PedidoCompraArquivosDTO pedidoCompraArquivosDTO)
        {
            if (_context.PedidoCompra_Arquivos.Any(x => x.IdPedidoCompra == pedidoCompraArquivosDTO.IdPedidoCompra && x.Nome == pedidoCompraArquivosDTO.Nome))
                throw new Exception("Já existe um arquivo com este nome");

            var pedidoCompraArquivos = new PedidoCompraArquivos();
            pedidoCompraArquivos.DataCadastro = pedidoCompraArquivosDTO.DataCadastro;
            pedidoCompraArquivos.Extensao = pedidoCompraArquivosDTO.Extensao;
            pedidoCompraArquivos.Id = pedidoCompraArquivosDTO.Id;
            pedidoCompraArquivos.IdPedidoCompra = pedidoCompraArquivosDTO.IdPedidoCompra;
            pedidoCompraArquivos.IdUsuarioCadastro = pedidoCompraArquivosDTO.IdUsuarioCadastro;
            pedidoCompraArquivos.Nome = pedidoCompraArquivosDTO.Nome;
            pedidoCompraArquivos.NomeLogico = pedidoCompraArquivosDTO.NomeLogico;
            pedidoCompraArquivos.TamanhoMB = pedidoCompraArquivosDTO.TamanhoMB;

            await _context.PedidoCompra_Arquivos.AddAsync(pedidoCompraArquivos);
            await _context.SaveChangesAsync();

            return _mapper.Map<PedidoCompraArquivosDTO>(pedidoCompraArquivos);
        }

        public async Task<PedidoCompraArquivosDTO> GetFile(Int64 id)
        {
            return _mapper.Map<PedidoCompraArquivosDTO>(await _context.PedidoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<PedidoCompraArquivosDTO> ObtemArquivo(Int64 idPedidoCompraArquivo)
        {
            return _mapper.Map<PedidoCompraArquivosDTO>(await _context.PedidoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == idPedidoCompraArquivo));
        }

        public async Task<List<PedidoCompraArquivosDTO>> ObtemArquivos(Int64 idPedidoCompra)
        {
            return _mapper.Map<List<PedidoCompraArquivosDTO>>(await _context.PedidoCompra_Arquivos.Include(x => x.UsuarioCadastro).Where(x => x.IdPedidoCompra == idPedidoCompra).ToListAsync());
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            var arquivo = await _context.PedidoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == idArquivo);

            if (arquivo == null)
                throw new Exception("Arquivo não encontrado");

            _context.PedidoCompra_Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluiNotaFiscal(Int64 idPedidoCompraNotaFiscal)
        {
            var pedidoCompraNotaFiscal = _context.PedidoCompra_NotaFiscal.FirstOrDefault(x => x.Id == idPedidoCompraNotaFiscal);

            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM PedidoCompra_NotaFiscal_Materiais WHERE IdPedidoCompraNotaFiscal = {idPedidoCompraNotaFiscal}");
            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM PedidoCompra_NotaFiscal WHERE Id = {idPedidoCompraNotaFiscal}");
            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM PedidoCompra_Arquivos WHERE Id = {pedidoCompraNotaFiscal.IdPedidoCompraArquivo}");
            
        }

        public async Task ConciliarNotaFiscal(Conciliacao_PedidoCompraDTO conciliacao_PedidoCompraDTO, Int64 idPedidoCompraNotaFiscal, Int64 idUsuario)
        {
            var intChar = 65;

            var primeiraData = conciliacao_PedidoCompraDTO.DataValorPagamento.OrderBy(x => x.data).FirstOrDefault();

            var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal.FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);
            var pedidoCompra = await _context.PedidoCompra.AsNoTracking()
                .Include(x => x.SolicitacaoCompra)
                .FirstOrDefaultAsync(x => x.Id == pedidoCompraNotaFiscal.IdPedidoCompra);

            var pedidoCompraNotaFiscalNome = pedidoCompraNotaFiscal.Nome.Split('.')[0];

            if (!conciliacao_PedidoCompraDTO.Materiais.Any(x => x.QuantidadeParaConciliar > 0) && !pedidoCompra.SolicitacaoCompra.Servico)
                throw new Exception("Não é possível conciliar uma quantidade de materiais igual a zero");

            var somatoriaNotasFiscais = _context.PedidoCompra_NotaFiscal.Where(x => x.IdPedidoCompra == pedidoCompraNotaFiscal.IdPedidoCompra && x.Aprovada != false && x.Id != pedidoCompraNotaFiscal.Id).Sum(x => x.Valor);
            var valorPedidoCompra = pedidoCompra?.ValorTotal ?? 0;
            var valorConciliado = conciliacao_PedidoCompraDTO.Materiais.Sum(x => x.QuantidadeParaConciliar * x.ValorConciliado);
            var valorConciliadoComImpostoEFrete = valorConciliado + conciliacao_PedidoCompraDTO.ValorFrete + conciliacao_PedidoCompraDTO.ValorImposto;
            var valorCancelamentoSaldo = _context.PedidoCompra_DevolucaoSaldo.Where(x => x.IdPedidoCompra == pedidoCompraNotaFiscal.IdPedidoCompra).Sum(x => x.Valor);

            if (somatoriaNotasFiscais + valorConciliadoComImpostoEFrete  > valorPedidoCompra)
                throw new Exception("Valor das notas fiscais excede o valor do pedido");

                if (!pedidoCompra.SolicitacaoCompra.Servico && conciliacao_PedidoCompraDTO.Materiais.Any(x => x.QuantidadeParaConciliar > (x.Quantidade - x.QuantidadeConciliada)))
                throw new Exception("Não é possível conciliar uma quantidade de material maior que a quantidade disponível para conciliação.");
            

            if(conciliacao_PedidoCompraDTO.DataValorPagamento.Count>1)
            {
                pedidoCompraNotaFiscal.Nome = $"{pedidoCompraNotaFiscalNome} - {Convert.ToChar(intChar)}";
                intChar++;
            }

            pedidoCompraNotaFiscal.Descricao = conciliacao_PedidoCompraDTO.Descricao;
            pedidoCompraNotaFiscal.ValorImposto = conciliacao_PedidoCompraDTO.ValorImposto;
            pedidoCompraNotaFiscal.ValorFrete = conciliacao_PedidoCompraDTO.ValorFrete;
            pedidoCompraNotaFiscal.NumeroNotaFiscal = conciliacao_PedidoCompraDTO.NumeroNotaFiscal;
            pedidoCompraNotaFiscal.DataVencimento = primeiraData.data;
            pedidoCompraNotaFiscal.IdFornecedor = conciliacao_PedidoCompraDTO.IdFornecedorEscolhido;

            if (pedidoCompra.SolicitacaoCompra.Servico)
            {
                if ((somatoriaNotasFiscais + valorConciliadoComImpostoEFrete + valorCancelamentoSaldo) >= valorPedidoCompra)
                {
                    var materiais = conciliacao_PedidoCompraDTO.Materiais.ToList();

                    materiais.ForEach(material =>
                    {
                        var pedidoCompraMaterial = _context.PedidoCompra_Materiais.FirstOrDefault(x => x.Id == material.Id);
                        pedidoCompraMaterial.QuantidadeConciliada = pedidoCompraMaterial.Quantidade;

                        if (pedidoCompra.SolicitacaoCompra.Servico)
                            pedidoCompraNotaFiscal.Valor += (material.QuantidadeParaConciliar > 0 ? material.QuantidadeParaConciliar : 1) * material.ValorConciliado;
                        else
                            pedidoCompraNotaFiscal.Valor += material.QuantidadeParaConciliar * material.ValorConciliado;

                        _context.Entry(pedidoCompraMaterial).State = EntityState.Modified;

                        var pedidoCompraNotaFiscalMateriais = new PedidoCompraNotaFiscalMateriais();
                        pedidoCompraNotaFiscalMateriais.Id = 0;
                        pedidoCompraNotaFiscalMateriais.IdPedidoCompraMateriais = material.Id;
                        pedidoCompraNotaFiscalMateriais.IdPedidoCompraNotaFiscal = idPedidoCompraNotaFiscal;
                        pedidoCompraNotaFiscalMateriais.Quantidade = material.QuantidadeParaConciliar;
                        pedidoCompraNotaFiscalMateriais.Valor = material.ValorConciliado;

                        _context.PedidoCompra_NotaFiscal_Materiais.Add(pedidoCompraNotaFiscalMateriais);
                    });
                }
            }
            else
            {
                var materiais = conciliacao_PedidoCompraDTO.Materiais.Where(x => x.QuantidadeParaConciliar > 0).ToList();

                materiais.ForEach(material =>
                {
                    var pedidoCompraMaterial = _context.PedidoCompra_Materiais.FirstOrDefault(x => x.Id == material.Id);
                    pedidoCompraMaterial.QuantidadeConciliada += material.QuantidadeParaConciliar;

                    _context.Entry(pedidoCompraMaterial).State = EntityState.Modified;

                    var pedidoCompraNotaFiscalMateriais = new PedidoCompraNotaFiscalMateriais();
                    pedidoCompraNotaFiscalMateriais.Id = 0;
                    pedidoCompraNotaFiscalMateriais.IdPedidoCompraMateriais = material.Id;
                    pedidoCompraNotaFiscalMateriais.IdPedidoCompraNotaFiscal = idPedidoCompraNotaFiscal;
                    pedidoCompraNotaFiscalMateriais.Quantidade = material.QuantidadeParaConciliar;
                    pedidoCompraNotaFiscalMateriais.Valor = material.ValorConciliado;

                    _context.PedidoCompra_NotaFiscal_Materiais.Add(pedidoCompraNotaFiscalMateriais);
                });
            }

            pedidoCompraNotaFiscal.Valor = primeiraData.valor;

            _context.Entry(pedidoCompraNotaFiscal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal.AsNoTracking().Include(x => x.Materiais).Include(x=>x.Arquivo).FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

            //Apaga a primeira data que foi utilizada para o primeiro registro de nota fiscal
            conciliacao_PedidoCompraDTO.DataValorPagamento.Remove(primeiraData);

            //Cadastra mais N entradas de notas fiscais de acordo com a quantidade de datas de pagamento que restaram
            
            conciliacao_PedidoCompraDTO.DataValorPagamento.ForEach(dataPagamento =>
            {
                var novoPedidoCompraNotaFiscal = new PedidoCompraNotaFiscal();
                novoPedidoCompraNotaFiscal.Aprovada = null;
                novoPedidoCompraNotaFiscal.DataAprovacao = null;
                novoPedidoCompraNotaFiscal.DataCadastro = DateTime.Now;
                novoPedidoCompraNotaFiscal.DataUltimaAlteracao = DateTime.Now;
                novoPedidoCompraNotaFiscal.DataVencimento = dataPagamento.data;
                novoPedidoCompraNotaFiscal.Descricao = pedidoCompraNotaFiscal.Descricao;
                novoPedidoCompraNotaFiscal.Id = 0;
                novoPedidoCompraNotaFiscal.IdFornecedor = conciliacao_PedidoCompraDTO.IdFornecedorEscolhido;
                novoPedidoCompraNotaFiscal.IdPedidoCompra = pedidoCompraNotaFiscal.IdPedidoCompra;
                novoPedidoCompraNotaFiscal.IdPedidoCompraArquivo = pedidoCompraNotaFiscal.IdPedidoCompraArquivo;
                novoPedidoCompraNotaFiscal.IdUsuarioAlteracao = idUsuario;
                novoPedidoCompraNotaFiscal.IdUsuarioAprovacao = null;
                novoPedidoCompraNotaFiscal.IdUsuarioCadastro = idUsuario;
                novoPedidoCompraNotaFiscal.ImportadoParaFinanceiro = null;
                novoPedidoCompraNotaFiscal.Nome = $"{pedidoCompraNotaFiscalNome} - {Convert.ToChar(intChar)}";
                novoPedidoCompraNotaFiscal.NumeroNotaFiscal = pedidoCompraNotaFiscal.NumeroNotaFiscal;
                novoPedidoCompraNotaFiscal.PagamentoEfetuado = false;
                novoPedidoCompraNotaFiscal.Valor = dataPagamento.valor;
                novoPedidoCompraNotaFiscal.ValorFrete = pedidoCompraNotaFiscal.ValorFrete;
                novoPedidoCompraNotaFiscal.ValorImposto = pedidoCompraNotaFiscal.ValorImposto;

                _context.PedidoCompra_NotaFiscal.Add(novoPedidoCompraNotaFiscal);
                _context.SaveChanges();

                intChar++;

                pedidoCompraNotaFiscal.Materiais.ForEach(material =>
                {
                    var novoPedidoCompraNotaFiscalMaterial = new PedidoCompraNotaFiscalMateriais();
                    novoPedidoCompraNotaFiscalMaterial.Id = 0;
                    novoPedidoCompraNotaFiscalMaterial.IdPedidoCompraMateriais = material.IdPedidoCompraMateriais;
                    novoPedidoCompraNotaFiscalMaterial.IdPedidoCompraNotaFiscal = novoPedidoCompraNotaFiscal.Id;
                    novoPedidoCompraNotaFiscalMaterial.Quantidade = material.Quantidade;
                    novoPedidoCompraNotaFiscalMaterial.Valor = material.Valor;

                    _context.PedidoCompra_NotaFiscal_Materiais.Add(novoPedidoCompraNotaFiscalMaterial);
                    _context.SaveChanges();
                });
            });
        }

        public async Task<PedidoCompraDTO> CancelarSaldoAbrirPedido(Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            var fornecedor = await _context.Fornecedor.FirstOrDefaultAsync(x => x.CNPJ == conciliacao_CancelarSaldoAbrirPedidoDTO.CNPJNovoFornecedor);

            if(fornecedor==null)
                throw new Exception($"Novo fornecedor não encontrado para o CNPJ '{conciliacao_CancelarSaldoAbrirPedidoDTO.CNPJNovoFornecedor}'. Favor cadastrar");

            var pedidoCompra = await _context.PedidoCompra
                .Include(x => x.Faturas)
                .Include(x => x.Materiais)
                .Include(x=>x.SolicitacaoCompra)
                .ThenInclude(x=>x.CentroCustoObra)
                .ThenInclude(x=>x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == conciliacao_CancelarSaldoAbrirPedidoDTO.IdPedidoCompra);

            var novoPedidoCompra = new PedidoCompra();
            novoPedidoCompra.CodigoAno = DateTime.Now.Year;
            novoPedidoCompra.CodigoSequencia = (_context.PedidoCompra.Where(x => x.CodigoAno == DateTime.Now.Year).Max(x => (int?)x.CodigoSequencia) ?? 97000) + 1;
            novoPedidoCompra.Codigo = $"{pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo}.P{novoPedidoCompra.CodigoSequencia}";
            novoPedidoCompra.DataCadastro = DateTime.Now;
            novoPedidoCompra.DataEntrega = pedidoCompra.DataEntrega;
            novoPedidoCompra.DataUltimaAlteracao = DateTime.Now;
            novoPedidoCompra.Id = 0;
            novoPedidoCompra.IdCentroCustoDEF = pedidoCompra.IdCentroCustoDEF;
            novoPedidoCompra.IdCentroCustoObra = pedidoCompra.IdCentroCustoObra;
            novoPedidoCompra.IdCondicaoPagamento = pedidoCompra.IdCondicaoPagamento;
            novoPedidoCompra.IdFornecedor = fornecedor.Id;
            novoPedidoCompra.IdSolicitacaoCompra = pedidoCompra.IdSolicitacaoCompra;
            novoPedidoCompra.IdStatusPedidoCompra = pedidoCompra.IdStatusPedidoCompra;
            novoPedidoCompra.IdTipoCentroCusto = pedidoCompra.IdTipoCentroCusto;
            novoPedidoCompra.IdUsuarioAlteracao = conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario;
            novoPedidoCompra.IdUsuarioCadastro = conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario;
            novoPedidoCompra.ImportadoParaFinanceiro = true;
            novoPedidoCompra.ValorTotal = pedidoCompra.ValorTotal;

            await _context.PedidoCompra.AddAsync(novoPedidoCompra);
            await _context.SaveChangesAsync();

            var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

            pedidoCompra.Faturas.ForEach(fatura =>
            {
                var novaFatura = new PedidoCompraFatura();
                novaFatura.DataFatura = fatura.DataFatura;
                novaFatura.Id = 0;
                novaFatura.IdPedidoCompra = novoPedidoCompra.Id;
                novaFatura.PagamentoEfetuado = false;
                novaFatura.Valor = fatura.Valor;

                _context.PedidoCompra_Faturas.Add(novaFatura);
                _context.SaveChanges();

                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    Id = 0,
                    IdDef = def0302.Id,
                    CodigoDef = def0302.Codigo,
                    IdObra = novoPedidoCompra.IdCentroCustoObra,
                    CodigoObra = novoPedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                    IdPedidoCompra = novoPedidoCompra.Id,
                    IdPedidoCompraFatura = novaFatura.Id,
                    CodigoPedidoCompra = novoPedidoCompra.Codigo,
                    IdPedidoInterno = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = DateTime.Now,
                    DataPagamento = novaFatura.DataFatura,
                    IdCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                    NomeCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                    NumeroNotaFiscalPedidoCompra = null,
                    Valor = novaFatura.Valor,
                    IdFornecedorBeneficiario=novoPedidoCompra.IdFornecedor
                }).Wait();
            });

            pedidoCompra.Materiais.ForEach(material =>
            {
                var novoMaterial = new PedidoCompraMateriais();
                novoMaterial.DataCadastro = DateTime.Now;
                novoMaterial.DataUltimaAlteracao = DateTime.Now;
                novoMaterial.Id = 0;
                novoMaterial.IdMaterial = material.IdMaterial;
                novoMaterial.IdPedidoCompra = novoPedidoCompra.Id;
                novoMaterial.IdUsuarioAlteracao = conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario;
                novoMaterial.IdUsuarioCadastro = conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario;
                novoMaterial.Quantidade = material.Quantidade;
                novoMaterial.QuantidadeConciliada = 0;
                novoMaterial.ValorTotal = material.ValorTotal;
                novoMaterial.ValorUnitario = material.ValorUnitario;

                _context.PedidoCompra_Materiais.Add(novoMaterial);
                _context.SaveChanges();
            });

            return await Get(novoPedidoCompra.Id);
        }

        public async Task<FornecedorDTO> TrocarFornecedor(Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            var fornecedor = await _context.Fornecedor.FirstOrDefaultAsync(x => x.CNPJ == conciliacao_CancelarSaldoAbrirPedidoDTO.CNPJNovoFornecedor);

            if (fornecedor == null)
                throw new Exception($"Novo fornecedor não encontrado para o CNPJ '{conciliacao_CancelarSaldoAbrirPedidoDTO.CNPJNovoFornecedor}'. Favor cadastrar");

            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra SET IdFornecedor = {fornecedor.Id}, IdUsuarioAlteracao = {conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario}, DataUltimaAlteracao = GETDATE() WHERE Id = {conciliacao_CancelarSaldoAbrirPedidoDTO.IdPedidoCompra}");
            
            return _mapper.Map<FornecedorDTO>(fornecedor);
        }

        public async Task<PedidoCompraDevolucaoSaldoDTO> CancelarSaldo(PedidoCompraDevolucaoSaldoDTO pedidoCompraDevolucaoSaldoDTO)
        {
            if (!_context.MotivoDevolucaoSaldo.Any(x => x.Id == pedidoCompraDevolucaoSaldoDTO.IdMotivoDevolucaoSaldo))
                throw new Exception("Motivo de devolução inválido");

            if (!_context.PedidoCompra.Any(x => x.Id == pedidoCompraDevolucaoSaldoDTO.IdPedidoCompra))
                throw new Exception("Pedido de compra inválido");

            var pedidoCompraDevolucaoSaldo = new PedidoCompraDevolucaoSaldo();
            pedidoCompraDevolucaoSaldo.Id = 0;
            pedidoCompraDevolucaoSaldo.DataCadastro = pedidoCompraDevolucaoSaldoDTO.DataCadastro;
            pedidoCompraDevolucaoSaldo.DataUltimaAlteracao = pedidoCompraDevolucaoSaldoDTO.DataCadastro;
            pedidoCompraDevolucaoSaldo.IdMotivoDevolucaoSaldo = pedidoCompraDevolucaoSaldoDTO.IdMotivoDevolucaoSaldo;
            pedidoCompraDevolucaoSaldo.IdPedidoCompra = pedidoCompraDevolucaoSaldoDTO.IdPedidoCompra;
            pedidoCompraDevolucaoSaldo.IdUsuarioAlteracao = pedidoCompraDevolucaoSaldoDTO.IdUsuarioAlteracao;
            pedidoCompraDevolucaoSaldo.IdUsuarioCadastro = pedidoCompraDevolucaoSaldoDTO.IdUsuarioCadastro;
            pedidoCompraDevolucaoSaldo.Observacao = pedidoCompraDevolucaoSaldoDTO.Observacao;
            pedidoCompraDevolucaoSaldo.Valor = pedidoCompraDevolucaoSaldoDTO.Valor;

            await _context.PedidoCompra_DevolucaoSaldo.AddAsync(pedidoCompraDevolucaoSaldo);
            await _context.SaveChangesAsync();


            #region Devolve para 03.03

            var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");
            var def0303 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.03");

            var obra = _context.PedidoCompra.Include(x => x.CentroCustoObra).FirstOrDefault(x => x.Id == pedidoCompraDevolucaoSaldo.IdPedidoCompra).CentroCustoObra;

            var fluxoCaixa = _context.FluxoCaixa.Where(x => x.IdObra == obra.Id && x.IdDef == def0303.Id).OrderBy(x => x.DataPagamento).FirstOrDefault();

            if (fluxoCaixa != null)
            {
                fluxoCaixa.Valor += pedidoCompraDevolucaoSaldo.Valor;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                //Criar um novo 03.03
                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    Id = 0,
                    IdDef = def0303.Id,
                    CodigoDef = def0303.Codigo,
                    IdObra = obra.Id,
                    CodigoObra = obra.Codigo ?? "",
                    IdPedidoCompra = null,
                    IdPedidoCompraFatura = null,
                    CodigoPedidoCompra = null,
                    IdPedidoInterno = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = DateTime.Now,
                    DataPagamento = DateTime.Now.AddDays(10),
                    IdCliente = obra.IdCliente,
                    NomeCliente = obra.Cliente.NomeFantasia,
                    NumeroNotaFiscalPedidoCompra = null,
                    Valor = pedidoCompraDevolucaoSaldoDTO.Valor,
                    IdFornecedorBeneficiario = null
                }).Wait();
            }

            #endregion

            #region Retira do 03.02

            var saldoTotal = pedidoCompraDevolucaoSaldo.Valor;

            _context.FluxoCaixa.Where(x => x.IdPedidoCompra == pedidoCompraDevolucaoSaldo.IdPedidoCompra && x.IdDef == def0302.Id).OrderBy(x => x.DataPagamento).ToList().ForEach(fluxoCaixa =>
            {
                if (saldoTotal > 0)
                {
                    if (fluxoCaixa.Valor > 0)
                    {
                        //Ainda tem saldo a ser descontado

                        if (fluxoCaixa.Valor > saldoTotal)
                        {
                            //Saldo do eto é maior que o saldo da NF
                            fluxoCaixa.Valor = fluxoCaixa.Valor - saldoTotal;
                            saldoTotal = 0;
                        }
                        else
                        {
                            //Saldo da OCPI  é menor que o saldo da NF
                            saldoTotal = saldoTotal - fluxoCaixa.Valor;
                            fluxoCaixa.Valor = 0;
                        }


                        _context.Entry(fluxoCaixa).State = EntityState.Modified;
                        
                    }
                }
            });


            _context.SaveChanges();

            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompra = {pedidoCompraDevolucaoSaldo.Id} AND Valor = 0");

            #endregion

            return _mapper.Map<PedidoCompraDevolucaoSaldoDTO>(pedidoCompraDevolucaoSaldo);
        }

        public async Task<List<PedidoCompra_NotaFiscalParaAprovacaoDTO>> ObtemNotasFiscaisParaAprovacao(Int64 IdUsuario)
        {
            var retorno = new List<PedidoCompra_NotaFiscalParaAprovacaoDTO>();


            var idsObrasDoUsuario = _context.Obra.Where(x => x.IdUsuarioDiretorAprovador == IdUsuario).Select(x => x.Id).ToList();
            _context.PedidoCompra.AsNoTracking()
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Fornecedor)
                .Include(x=>x.NotasFiscais)
                .Where(x => x.NotasFiscais.Any(y => !y.Aprovada.HasValue) && idsObrasDoUsuario.Contains(x.IdCentroCustoObra.Value)).ToList().ForEach(pedidoCompra =>
            {
                var notasAgrupadas = pedidoCompra.NotasFiscais.GroupBy(x => x.IdPedidoCompraArquivo).ToList();

                foreach (var nota in notasAgrupadas)
                {
                    if (nota.Any(x => !x.Aprovada.HasValue))
                    {
                        var pedidoCompraNotaFiscal = _mapper.Map<PedidoCompraNotaFiscalDTO>(nota.FirstOrDefault());
                        pedidoCompraNotaFiscal.PedidoCompra.NotasFiscais = null;
                        pedidoCompraNotaFiscal.Valor = nota.Sum(x => x.Valor);
                        var saldoPedido = float.Parse((pedidoCompra.ValorTotal - (pedidoCompra.NotasFiscais.Where(y => y.IdPedidoCompra == pedidoCompra.Id && y.Aprovada != false).Sum(y => y.Valor)) + (_context.PedidoCompra_DevolucaoSaldo.Where(y => y.IdPedidoCompra == pedidoCompra.Id).Sum(y => y.Valor))).ToString());
                        var valorPedido = pedidoCompra.ValorTotal;

                        retorno.Add(new PedidoCompra_NotaFiscalParaAprovacaoDTO()
                        {
                            valorPedido=valorPedido,
                            pedidoCompraNotaFiscal = pedidoCompraNotaFiscal,
                            saldoPedido = saldoPedido, 
                            datasPagamento = $"{string.Join(" ", nota.Select(x => $"[{x.DataVencimento.ToString("dd/MM/yyyy") } - {x.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}]" ).ToList())}",
                            vencimentoProximo = nota.Any(x => x.DataVencimento <= DateTime.Now.AddDays(7))
                        });
                    }
                }
            });




            //var pedidosCompraNotasFiscais = _mapper.Map<List<PedidoCompraNotaFiscalDTO>>(await _context.PedidoCompra_NotaFiscal.AsNoTracking().Include(x=>x.Fornecedor).Where(x => idsObrasDoUsuario.Contains(x.PedidoCompra.CentroCustoObra.Id) && !x.Aprovada.HasValue).ToListAsync());
            //var pedidosCompra = _mapper.Map<List<PedidoCompraDTO>>( await _context.PedidoCompra
            //        .Include(x => x.CentroCustoObra)
            //        .ThenInclude(x => x.Cliente)
            //        .Include(x=>x.Fornecedor)
            //        .Where(x => pedidosCompraNotasFiscais.Select(x => x.IdPedidoCompra).Distinct().ToList().Contains(x.Id)).ToListAsync());

            //pedidosCompraNotasFiscais.ForEach(x =>
            //{
            //    var pedidoCompra = pedidosCompra.FirstOrDefault(y => y.Id == x.IdPedidoCompra);
            //    float saldoPedido = float.Parse((pedidoCompra.ValorTotal - (_context.PedidoCompra_NotaFiscal.Where(y => y.IdPedidoCompra == x.IdPedidoCompra && y.Aprovada != false).Sum(y => y.Valor)) + (_context.PedidoCompra_DevolucaoSaldo.Where(y => y.IdPedidoCompra == x.IdPedidoCompra).Sum(y => y.Valor))).ToString());
            //    x.PedidoCompra = pedidoCompra;
            //    retorno.Add(new PedidoCompra_NotaFiscalParaAprovacaoDTO()
            //    {
            //        pedidoCompraNotaFiscal = x,
            //        saldoPedido = saldoPedido
            //    });
            //});

            retorno = retorno.OrderBy(x => x.pedidoCompraNotaFiscal.DataVencimento).ToList();
            return retorno;
        }

        public async Task<List<SolicitacaoCompraDTO>> ObtemCotacoesParaAprovacao(Int64 IdUsuario)
        {
            var idsObrasDoUsuario = _context.Obra.Where(x => x.UsuariosAprovadores.Any(y => y.IdUsuarioAprovacao == IdUsuario)).Select(x => x.Id).ToList();

            return _mapper.Map<List<SolicitacaoCompraDTO>>((await _context.SolicitacaoCompra.Where(x => idsObrasDoUsuario.Contains(x.IdCentroCustoObra.Value) && x.IdStatusSolicitacaoCompra == 3).ToListAsync()));
        }

        public async Task CancelarNotaFiscal(Int64 idPedidoCompraNotaFiscal, Int64 idUsuario)
        {
            var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal
                .Include(x=>x.PedidoCompra)
                .ThenInclude(x=>x.SolicitacaoCompra)
                .FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

            if(pedidoCompraNotaFiscal!=null)
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = 0 WHERE Id = {idPedidoCompraNotaFiscal}");
                await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {idPedidoCompraNotaFiscal}");

                var saldoTotal = pedidoCompraNotaFiscal.Valor;

                var previsaoFluxoCaixa = _context.FluxoCaixa.FirstOrDefault(x => x.IdPedidoCompra == pedidoCompraNotaFiscal.IdPedidoCompra && x.CodigoDef== "03.02" && x.Valor > 0);

                if (previsaoFluxoCaixa != null)
                {
                    previsaoFluxoCaixa.Valor += saldoTotal;

                    _context.Entry(previsaoFluxoCaixa).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

                    var novaFatura = new PedidoCompraFatura();
                    novaFatura.DataFatura = DateTime.Now.AddDays(7);
                    novaFatura.Id = 0;
                    novaFatura.CodigoFormatado = pedidoCompraNotaFiscal.PedidoCompra.Codigo;
                    novaFatura.IdPedidoCompra = pedidoCompraNotaFiscal.IdPedidoCompra;
                    novaFatura.PagamentoEfetuado = false;
                    novaFatura.Valor = pedidoCompraNotaFiscal.Valor;

                    _context.PedidoCompra_Faturas.Add(novaFatura);
                    _context.SaveChanges();

                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        Id = 0,
                        IdDef = def0302.Id,
                        CodigoDef = def0302.Codigo,
                        IdObra = pedidoCompraNotaFiscal.PedidoCompra.IdCentroCustoObra,
                        CodigoObra = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                        IdPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Id,
                        IdPedidoCompraFatura = novaFatura.Id,
                        CodigoPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Codigo,
                        IdPedidoInterno = null,
                        CodigoPedidoInterno = null,
                        DataLancamento = DateTime.Now,
                        DataPagamento = novaFatura.DataFatura,
                        IdCliente = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                        NomeCliente = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = null,
                        Valor = pedidoCompraNotaFiscal.Valor,
                        IdFornecedorBeneficiario = pedidoCompraNotaFiscal.PedidoCompra.IdFornecedor
                    }).Wait();
                }
            }
        }

        public async Task AlterarObraNotaFiscal(Int64 idPedidoCompraNotaFiscal, Int64 idNovaObra, Int64 idUsuario)
        {
            var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal
                .Include(x => x.PedidoCompra)
                .ThenInclude(x => x.SolicitacaoCompra)
                .FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

            var novaObra = await _context.Obra.Include(x=>x.Cliente).FirstOrDefaultAsync(x => x.Id == idNovaObra);

            if (pedidoCompraNotaFiscal.PedidoCompra.IdCentroCustoObra != idNovaObra)
            {
                //Atualiza a obra do pedido da nota fiscal em questão
                await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra SET IdCentroCustoObra = {idNovaObra} WHERE Id = {pedidoCompraNotaFiscal.IdPedidoCompra}");

                //Atualiza as previsões de pagamento do pedido de compra com a obra alterada
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET IdObra = {idNovaObra}, CodigoObra = '{novaObra.Codigo}', IdCliente = {novaObra.IdCliente}, NomeCliente = '{novaObra.Cliente.NomeFantasia}' WHERE IdPedidoCompra = {pedidoCompraNotaFiscal.IdPedidoCompra} AND IdDef IN (74, 75)");


                if (pedidoCompraNotaFiscal != null && pedidoCompraNotaFiscal.PedidoCompra.IdCentroCustoObra != idNovaObra)
                {
                    await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = 0 WHERE Id = {idPedidoCompraNotaFiscal}");
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {idPedidoCompraNotaFiscal}");

                    var saldoTotal = pedidoCompraNotaFiscal.Valor;

                    var previsaoFluxoCaixa = _context.FluxoCaixa.FirstOrDefault(x => x.IdPedidoCompra == pedidoCompraNotaFiscal.IdPedidoCompra && x.CodigoDef== "03.02" && x.Valor > 0);

                    if (previsaoFluxoCaixa != null)
                    {
                        previsaoFluxoCaixa.Valor += saldoTotal;

                        _context.Entry(previsaoFluxoCaixa).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

                        var novaFatura = new PedidoCompraFatura();
                        novaFatura.DataFatura = DateTime.Now.AddDays(7);
                        novaFatura.Id = 0;
                        novaFatura.CodigoFormatado = pedidoCompraNotaFiscal.PedidoCompra.Codigo;
                        novaFatura.IdPedidoCompra = pedidoCompraNotaFiscal.IdPedidoCompra;
                        novaFatura.PagamentoEfetuado = false;
                        novaFatura.Valor = pedidoCompraNotaFiscal.Valor;

                        _context.PedidoCompra_Faturas.Add(novaFatura);
                        _context.SaveChanges();

                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            Id = 0,
                            IdDef = def0302.Id,
                            CodigoDef = def0302.Codigo,
                            IdObra = pedidoCompraNotaFiscal.PedidoCompra.IdCentroCustoObra,
                            CodigoObra = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.Codigo ?? "",
                            IdPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Id,
                            IdPedidoCompraFatura = novaFatura.Id,
                            CodigoPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Codigo,
                            IdPedidoInterno = null,
                            CodigoPedidoInterno = null,
                            DataLancamento = DateTime.Now,
                            DataPagamento = novaFatura.DataFatura,
                            IdCliente = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                            NomeCliente = pedidoCompraNotaFiscal.PedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                            NumeroNotaFiscalPedidoCompra = null,
                            Valor = pedidoCompraNotaFiscal.Valor,
                            IdFornecedorBeneficiario = pedidoCompraNotaFiscal.PedidoCompra.IdFornecedor
                        }).Wait();
                    }


                }
            }
        }

        public async Task AprovarReprovarNotaFiscal(Int64 idPedidoCompraNotaFiscal, bool aprovada, Int64 idUsuario)
        {
            //await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = {(aprovada ? 1 : 0)}, DataAprovacao = GETDATE(), IdUsuarioAprovacao = {idUsuario} WHERE Id = {idPedidoCompraNotaFiscal}");

            if (!aprovada)
            {
                var valorTotalETO = 0.0;

                var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal.FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

                _context.PedidoCompra_NotaFiscal.Where(x => x.IdPedidoCompraArquivo == pedidoCompraNotaFiscal.IdPedidoCompraArquivo).ToList().ForEach(pcnf =>
                {
                    if (pcnf.ImportadoParaFinanceiro == true)
                        valorTotalETO += pcnf.Valor;

                    _context.Database.ExecuteSqlRaw($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = 0, DevolucaoVisualizada = 0, DataAprovacao = GETDATE(), IdUsuarioAprovacao = {idUsuario} WHERE Id = {pcnf.Id}");

                    _context.Database.ExecuteSqlRaw($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {pcnf.Id}");
                });


                _context.Database.ExecuteSqlRaw($@"UPDATE
                                                                    PCM
                                                                SET
                                                                    PCM.QuantidadeConciliada = PCM.QuantidadeConciliada - PCNFM.Quantidade
                                                                FROM
                                                                    PedidoCompra_Materiais AS PCM
                                                                    INNER JOIN PedidoCompra_NotaFiscal_Materiais AS PCNFM
                                                                        ON PCM.Id = PCNFM.IdPedidoCompraMateriais
                                                                WHERE
                                                                    PCNFM.IdPedidoCompraNotaFiscal = {idPedidoCompraNotaFiscal}");

                await VoltaValorParaPrevisaoPagamentoPedido(valorTotalETO, pedidoCompraNotaFiscal.IdPedidoCompra);
                
            }
            else
            {

                var pcnf = await _context.PedidoCompra_NotaFiscal.FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

                _context.PedidoCompra_NotaFiscal.Where(x => x.IdPedidoCompraArquivo == pcnf.IdPedidoCompraArquivo).ToList().ForEach(x =>
                {
                    _context.Database.ExecuteSqlRaw($"UPDATE PedidoCompra_NotaFiscal SET Aprovada = 1, DevolucaoVisualizada = 1, DataAprovacao = GETDATE(), IdUsuarioAprovacao = {idUsuario} WHERE Id = {x.Id}");
                });

                ////Realiza a inserção do valor no fluxo de caixa, porém é necessário posteriormente atualizar com os valores de impostos
                ////informados pelo financeiro

                //var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal
                //    .Include(x=>x.PedidoCompra)
                //    .ThenInclude(x=>x.CentroCustoObra)
                //    .ThenInclude(x=>x.Cliente)
                //    .FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaFiscal);

                //var def0301 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.01");

                //_repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                //{
                //    Id = 0,
                //    IdDef = def0301.Id,
                //    CodigoDef = def0301.Codigo,
                //    IdObra = pedidoCompraNotaFiscal.PedidoCompra?.IdCentroCustoObra,
                //    CodigoObra = pedidoCompraNotaFiscal.PedidoCompra?.CentroCustoObra?.Codigo ?? "",
                //    IdPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Id,
                //    IdPedidoCompraFatura = null,
                //    CodigoPedidoCompra = pedidoCompraNotaFiscal.PedidoCompra.Codigo,
                //    IdPedidoInterno = null,
                //    CodigoPedidoInterno = null,
                //    DataLancamento = pedidoCompraNotaFiscal.DataCadastro,
                //    DataPagamento = pedidoCompraNotaFiscal.DataVencimento,
                //    IdCliente = pedidoCompraNotaFiscal.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                //    NomeCliente = pedidoCompraNotaFiscal.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                //    NumeroNF = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                //    Valor = pedidoCompraNotaFiscal.Valor
                //}).Wait();
            }
        }

        public async Task<PedidoCompraNotaFiscalPagamentoDTO> PostNotaFiscalPagamento(PedidoCompraNotaFiscalPagamentoDTO pagamentoDTO)
        {
            var novaEntidade = false;

            var pedidoCompraNotaFiscalPagamento = await _context.PedidoCompra_NotaFiscal_Pagamentos.FirstOrDefaultAsync(x => x.Id == pagamentoDTO.Id);
            if (pedidoCompraNotaFiscalPagamento == null)
            {
                pedidoCompraNotaFiscalPagamento = new PedidoCompraNotaFiscalPagamento();
                pedidoCompraNotaFiscalPagamento.Id = 0;
                pedidoCompraNotaFiscalPagamento.DataCadastro = pagamentoDTO.DataCadastro;
                pedidoCompraNotaFiscalPagamento.IdUsuarioCadastro = pagamentoDTO.IdUsuarioCadastro;
            }

            pedidoCompraNotaFiscalPagamento.AliquotaArt30 = pagamentoDTO.AliquotaArt30;
            pedidoCompraNotaFiscalPagamento.AliquotaINSS = pagamentoDTO.AliquotaINSS;
            pedidoCompraNotaFiscalPagamento.AliquotaIR = pagamentoDTO.AliquotaIR;
            pedidoCompraNotaFiscalPagamento.AliquotaISS = pagamentoDTO.AliquotaISS;
            pedidoCompraNotaFiscalPagamento.DataPagamentoArt30 = pagamentoDTO.DataPagamentoArt30;
            pedidoCompraNotaFiscalPagamento.DataPagamentoINSS = pagamentoDTO.DataPagamentoINSS;
            pedidoCompraNotaFiscalPagamento.DataPagamentoIR = pagamentoDTO.DataPagamentoIR;
            pedidoCompraNotaFiscalPagamento.DataPagamentoISS = pagamentoDTO.DataPagamentoISS;
            pedidoCompraNotaFiscalPagamento.DataUltimaAlteracao = pagamentoDTO.DataUltimaAlteracao;
            pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal = pagamentoDTO.IdPedidoCompraNotaFiscal;
            pedidoCompraNotaFiscalPagamento.IdTipoCalculoNotaFiscalArt30 = pagamentoDTO.IdTipoCalculoNotaFiscalArt30;
            pedidoCompraNotaFiscalPagamento.IdTipoCalculoNotaFiscalINSS = pagamentoDTO.IdTipoCalculoNotaFiscalINSS;
            pedidoCompraNotaFiscalPagamento.IdTipoCalculoNotaFiscalIR = pagamentoDTO.IdTipoCalculoNotaFiscalIR;
            pedidoCompraNotaFiscalPagamento.IdTipoCalculoNotaFiscalISS = pagamentoDTO.IdTipoCalculoNotaFiscalISS;
            pedidoCompraNotaFiscalPagamento.IdUsuarioAlteracao = pagamentoDTO.IdUsuarioAlteracao;
            pedidoCompraNotaFiscalPagamento.ValorArt30 = pagamentoDTO.ValorArt30;
            pedidoCompraNotaFiscalPagamento.ValorBaseCalculo = pagamentoDTO.ValorBaseCalculo;
            pedidoCompraNotaFiscalPagamento.ValorBruto = pagamentoDTO.ValorBruto;
            pedidoCompraNotaFiscalPagamento.ValorINSS = pagamentoDTO.ValorINSS;
            pedidoCompraNotaFiscalPagamento.ValorIR = pagamentoDTO.ValorIR;
            pedidoCompraNotaFiscalPagamento.ValorISS = pagamentoDTO.ValorISS;
            pedidoCompraNotaFiscalPagamento.ValorMaterialAbatido = pagamentoDTO.ValorMaterialAbatido;

            if (pedidoCompraNotaFiscalPagamento.Id == 0)
            {
                await _context.PedidoCompra_NotaFiscal_Pagamentos.AddAsync(pedidoCompraNotaFiscalPagamento);
                novaEntidade = true;
            }
            else
                _context.Entry(pedidoCompraNotaFiscalPagamento).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            
            var idDef0301 = (await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01")).Id;

            //Realiza um update do valor da nota fiscal no fluxo de caixa com o valor liquido
            await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {(pedidoCompraNotaFiscalPagamento.ValorBruto - (pedidoCompraNotaFiscalPagamento.ValorIR ?? 0) - (pedidoCompraNotaFiscalPagamento.ValorArt30 ?? 0) - (pedidoCompraNotaFiscalPagamento.ValorINSS ?? 0) - (pedidoCompraNotaFiscalPagamento.ValorISS ?? 0)).ToString().Replace(',','.')} WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {idDef0301}");

            var def030101_INSS = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.01");
            var def030102_IR = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.02");
            var def030103_ART30 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.03");
            var def030104_ISS = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.04");

            var pedidoCompraNotaFiscal = await _context.PedidoCompra_NotaFiscal
                    .Include(x => x.PedidoCompra)
                    .ThenInclude(x => x.CentroCustoObra)
                    .ThenInclude(x => x.Cliente)
                    .Include(x => x.PedidoCompra)
                    .ThenInclude(x => x.Fornecedor)
                    .FirstOrDefaultAsync(x => x.Id == pagamentoDTO.IdPedidoCompraNotaFiscal);

            if (novaEntidade)
            {
                if (pedidoCompraNotaFiscalPagamento.ValorINSS.HasValue && pedidoCompraNotaFiscalPagamento.ValorINSS > 0)
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        Id = 0,
                        IdDef = def030101_INSS.Id,
                        CodigoDef = def030101_INSS.Codigo,
                        IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                        CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                        IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                        CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                        IdPedidoInterno = null,
                        CodigoPedidoInterno = null,
                        DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                        DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoINSS.Value,
                        IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                        NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                        Valor = pedidoCompraNotaFiscalPagamento.ValorINSS.Value,
                        IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null,
                        IdTipoFluxoCaixa=2
                    }).Wait();
                }

                if (pedidoCompraNotaFiscalPagamento.ValorIR.HasValue && pedidoCompraNotaFiscalPagamento.ValorIR > 0)
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        Id = 0,
                        IdDef = def030102_IR.Id,
                        CodigoDef = def030102_IR.Codigo,
                        IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                        CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                        IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                        CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                        IdPedidoInterno = null,
                        CodigoPedidoInterno = null,
                        DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                        DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoIR.Value,
                        IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                        NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                        Valor = pedidoCompraNotaFiscalPagamento.ValorIR.Value,
                        IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null,
                        IdTipoFluxoCaixa = 2
                    }).Wait();
                }

                if (pedidoCompraNotaFiscalPagamento.ValorArt30.HasValue && pedidoCompraNotaFiscalPagamento.ValorArt30 > 0)
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        Id = 0,
                        IdDef = def030103_ART30.Id,
                        CodigoDef = def030103_ART30.Codigo,
                        IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                        CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                        IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                        CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                        IdPedidoInterno = null,
                        CodigoPedidoInterno = null,
                        DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                        DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoArt30.Value,
                        IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                        NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                        Valor = pedidoCompraNotaFiscalPagamento.ValorArt30.Value,
                        IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null,
                        IdTipoFluxoCaixa = 2
                    }).Wait();
                }

                if (pedidoCompraNotaFiscalPagamento.ValorISS.HasValue && pedidoCompraNotaFiscalPagamento.ValorISS > 0)
                {
                    _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                    {
                        Id = 0,
                        IdDef = def030104_ISS.Id,
                        CodigoDef = def030104_ISS.Codigo,
                        IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                        CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                        IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                        IdPedidoCompraFatura = null,
                        IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                        CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                        IdPedidoInterno = null,
                        CodigoPedidoInterno = null,
                        DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                        DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoArt30.Value,
                        IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                        NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                        NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                        Valor = pedidoCompraNotaFiscalPagamento.ValorISS.Value,
                        IdFornecedorBeneficiario = pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor ?? null,
                        IdTipoFluxoCaixa = 2
                    }).Wait();
                }
            }
            else
            {
                var valorFinalNota = 0.0;

                if (pagamentoDTO.ValorINSS.HasValue && pagamentoDTO.ValorINSS > 0)
                {
                    valorFinalNota = valorFinalNota - pedidoCompraNotaFiscalPagamento.ValorINSS.Value;

                    if ((await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {pedidoCompraNotaFiscalPagamento.ValorINSS.Value.ToString().Replace(',', '.')} WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030101_INSS.Id}")) == 0)
                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            Id = 0,
                            IdDef = def030101_INSS.Id,
                            CodigoDef = def030101_INSS.Codigo,
                            IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                            CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                            IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                            IdPedidoCompraFatura = null,
                            IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                            CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                            IdPedidoInterno = null,
                            CodigoPedidoInterno = null,
                            DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                            DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoINSS.Value,
                            IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                            NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                            NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                            Valor = pedidoCompraNotaFiscalPagamento.ValorINSS.Value,
                            IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null
                        }).Wait();
                }
                else
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030101_INSS.Id}");

                if (pagamentoDTO.ValorIR.HasValue && pagamentoDTO.ValorIR > 0)
                {
                    valorFinalNota = valorFinalNota - pedidoCompraNotaFiscalPagamento.ValorIR.Value;

                    if ((await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {pedidoCompraNotaFiscalPagamento.ValorIR.Value.ToString().Replace(',', '.')} WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030102_IR.Id}")) == 0)
                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            Id = 0,
                            IdDef = def030102_IR.Id,
                            CodigoDef = def030102_IR.Codigo,
                            IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                            CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                            IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                            IdPedidoCompraFatura = null,
                            IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                            CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                            IdPedidoInterno = null,
                            CodigoPedidoInterno = null,
                            DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                            DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoIR.Value,
                            IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                            NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                            NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                            Valor = pedidoCompraNotaFiscalPagamento.ValorIR.Value,
                            IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null
                        }).Wait();
                }
                else
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030102_IR.Id}");

                if (pagamentoDTO.ValorArt30.HasValue && pagamentoDTO.ValorArt30 > 0)
                {
                    valorFinalNota = valorFinalNota - pedidoCompraNotaFiscalPagamento.ValorArt30.Value;

                    if ((await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {pedidoCompraNotaFiscalPagamento.ValorArt30.Value.ToString().Replace(',', '.')} WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030103_ART30.Id}")) == 0)
                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            Id = 0,
                            IdDef = def030103_ART30.Id,
                            CodigoDef = def030103_ART30.Codigo,
                            IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                            CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                            IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                            IdPedidoCompraFatura = null,
                            IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                            CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                            IdPedidoInterno = null,
                            CodigoPedidoInterno = null,
                            DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                            DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoArt30.Value,
                            IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                            NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                            NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                            Valor = pedidoCompraNotaFiscalPagamento.ValorArt30.Value,
                            IdFornecedorBeneficiario=pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor??null
                        }).Wait();
                }
                else
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030103_ART30.Id}");

                if (pagamentoDTO.ValorISS.HasValue && pagamentoDTO.ValorISS > 0)
                {
                    valorFinalNota = valorFinalNota - pedidoCompraNotaFiscalPagamento.ValorISS.Value;

                    if ((await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {pedidoCompraNotaFiscalPagamento.ValorISS.Value.ToString().Replace(',', '.')} WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030104_ISS.Id}")) == 0)
                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            Id = 0,
                            IdDef = def030104_ISS.Id,
                            CodigoDef = def030104_ISS.Codigo,
                            IdObra = pedidoCompraNotaFiscal?.PedidoCompra?.IdCentroCustoObra ?? null,
                            CodigoObra = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Codigo ?? null,
                            IdPedidoCompra = pedidoCompraNotaFiscal?.IdPedidoCompra ?? null,
                            IdPedidoCompraFatura = null,
                            IdPedidoCompraNotaFiscal = pedidoCompraNotaFiscal.Id,
                            CodigoPedidoCompra = pedidoCompraNotaFiscal?.PedidoCompra?.Codigo ?? null,
                            IdPedidoInterno = null,
                            CodigoPedidoInterno = null,
                            DataLancamento = pedidoCompraNotaFiscalPagamento.DataCadastro,
                            DataPagamento = pedidoCompraNotaFiscalPagamento.DataPagamentoArt30.Value,
                            IdCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.IdCliente ?? null,
                            NomeCliente = pedidoCompraNotaFiscal?.PedidoCompra?.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                            NumeroNotaFiscalPedidoCompra = pedidoCompraNotaFiscal.NumeroNotaFiscal,
                            Valor = pedidoCompraNotaFiscalPagamento.ValorISS.Value,
                            IdFornecedorBeneficiario = pedidoCompraNotaFiscal?.PedidoCompra?.IdFornecedor ?? null
                        }).Wait();
                }
                else
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompraNotaFiscal = {pedidoCompraNotaFiscalPagamento.IdPedidoCompraNotaFiscal} AND IdDef = {def030104_ISS.Id}");
            }

            return _mapper.Map<PedidoCompraNotaFiscalPagamentoDTO>(pedidoCompraNotaFiscalPagamento);
        }

        public async Task<PedidoCompraNotaFiscalPagamentoDTO> ObtemNotaFiscalPagamento(Int64 idPedidoCompraNotaFiscal)
        {
            return _mapper.Map<PedidoCompraNotaFiscalPagamentoDTO>(await _context.PedidoCompra_NotaFiscal_Pagamentos.FirstOrDefaultAsync(x => x.IdPedidoCompraNotaFiscal == idPedidoCompraNotaFiscal));
        }

        public async Task VoltaValorParaPrevisaoPagamentoPedido(double valor, Int64 idPedidoCompra)
        {
            var pedidoCompra = await _context.PedidoCompra
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x=>x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == idPedidoCompra);

            var previsaoObra = await _context.FluxoCaixa.Where(x => x.IdPedidoCompra==idPedidoCompra && x.CodigoDef == "03.02").FirstOrDefaultAsync();

            if (previsaoObra == null)
            {
                var def0302 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.02");

                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    Id = 0,
                    IdDef = def0302.Id,
                    CodigoDef = def0302.Codigo,
                    IdObra = pedidoCompra.IdCentroCustoObra,
                    CodigoObra = pedidoCompra.CentroCustoObra?.Codigo ?? "",
                    IdPedidoCompra = pedidoCompra.Id,
                    IdPedidoCompraFatura = null,
                    CodigoPedidoCompra = pedidoCompra.Codigo,
                    IdPedidoInterno = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = DateTime.Now,
                    DataPagamento = DateTime.Now.AddDays(10),
                    IdCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.IdCliente ?? null,
                    NomeCliente = pedidoCompra.SolicitacaoCompra.CentroCustoObra?.Cliente?.NomeFantasia ?? null,
                    NumeroNotaFiscalPedidoCompra = null,
                    Valor = valor,
                    IdFornecedorBeneficiario = pedidoCompra.IdFornecedor
                }).Wait();
            }
            else
            {
                previsaoObra.Valor += valor;

                _context.Entry(previsaoObra).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public async Task<List<PedidoCompraNotaFiscalDTO>> ObtemNotasFiscaisAlteracaoData(Int64 idPedidoCompraNotaFiscal)
        {
            var idArquivo = _context.PedidoCompra_NotaFiscal.FirstOrDefault(x => x.Id == idPedidoCompraNotaFiscal)?.IdPedidoCompraArquivo ?? 0;

            return _mapper.Map<List<PedidoCompraNotaFiscalDTO>>(await _context.PedidoCompra_NotaFiscal.Where(x => x.IdPedidoCompraArquivo == idArquivo).ToListAsync());
        }

        public async Task CancelarPedido(Int64 id, string motivoCancelamento)
        {
            var pedidoCompra = await _context.PedidoCompra.Include(x => x.CentroCustoObra).ThenInclude(x => x.Cliente).FirstOrDefaultAsync(x => x.Id == id);
            pedidoCompra.IdStatusPedidoCompra = 3;
            pedidoCompra.MotivoCancelamento = motivoCancelamento;
            pedidoCompra.DataUltimaAlteracao = DateTime.Now;

            _context.Entry(pedidoCompra).State = EntityState.Modified;

            var def0302 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.02");
            var def0303 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.03");

            var fluxos0302 = await _context.FluxoCaixa.Where(x => x.IdPedidoCompra == id && x.IdDef == def0302.Id && !x.Cancelado).ToListAsync();

            var soma0302 = fluxos0302.Sum(x => x.Valor);

            var fluxo0303 = await _context.FluxoCaixa.Where(x => x.IdObra == pedidoCompra.IdCentroCustoObra && x.IdDef == def0303.Id).OrderBy(x => x.DataPagamento).FirstOrDefaultAsync();

            if(fluxo0303!=null)
            {
                fluxo0303.Valor += soma0302;

                _context.Entry(fluxo0303).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                
                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    Id = 0,
                    IdDef = def0303.Id,
                    CodigoDef = def0303.Codigo,
                    IdObra = pedidoCompra.IdCentroCustoObra,
                    CodigoObra = pedidoCompra.CentroCustoObra.Codigo ?? "",
                    IdPedidoCompra = null,
                    IdPedidoCompraFatura = null,
                    CodigoPedidoCompra = null,
                    IdPedidoInterno = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = DateTime.Now,
                    DataPagamento = DateTime.Now.AddDays(10),
                    IdCliente = pedidoCompra.CentroCustoObra.IdCliente,
                    NomeCliente = pedidoCompra.CentroCustoObra.Cliente.NomeFantasia,
                    NumeroNotaFiscalPedidoCompra = null,
                    Valor = soma0302,
                    IdFornecedorBeneficiario = null
                }).Wait();
            }

            fluxos0302.ForEach(x =>
            {
                x.Cancelado = true;
                _context.Entry(x).State = EntityState.Modified;
            });

            await _context.SaveChangesAsync();
        }
    }
}
