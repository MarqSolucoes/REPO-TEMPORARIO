using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioPedidoInternoRecorrente
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioAgenda _repositorioAgenda;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;

        public RepositorioPedidoInternoRecorrente(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAgenda = new RepositorioAgenda(_context, _mapper);
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
        }

        public async Task<List<PedidoInternoRecorrenteDTO>> Get()
        {
            return _mapper.Map<List<PedidoInternoRecorrenteDTO>>(
                await _context.PedidoInternoRecorrente.AsNoTracking()
                .Include(x => x.DEF)
                .Include(x => x.UsuarioCadastro)
                .ToListAsync());
        }

        public async Task<PedidoInternoRecorrenteDTO> Get(Int64 Id)
        {
            return _mapper.Map<PedidoInternoRecorrenteDTO>(
                await _context.PedidoInternoRecorrente.AsNoTracking()
                .Include(x => x.DEF)
                .Include(x => x.UsuarioCadastro)
                .FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<PedidoInternoRecorrenteDTO> Post(PedidoInternoRecorrenteDTO pedidoInternoRecorrenteDTO)
        {
            var pedidoInternoRecorrente = new PedidoInternoRecorrente();
            pedidoInternoRecorrente.Ativo = pedidoInternoRecorrenteDTO.Ativo;
            pedidoInternoRecorrente.DataCadastro = pedidoInternoRecorrenteDTO.DataCadastro;
            pedidoInternoRecorrente.DataLimiteGeracao = pedidoInternoRecorrenteDTO.DataLimiteGeracao;
            pedidoInternoRecorrente.DataUltimaAlteracao = pedidoInternoRecorrenteDTO.DataUltimaAlteracao;
            pedidoInternoRecorrente.Descricao = pedidoInternoRecorrenteDTO.Descricao;
            pedidoInternoRecorrente.DiaGeracao = pedidoInternoRecorrenteDTO.DiaGeracao;
            pedidoInternoRecorrente.Id = 0;
            pedidoInternoRecorrente.IdDEF = pedidoInternoRecorrenteDTO.IdDEF;
            pedidoInternoRecorrente.IdUsuarioAlteracao = pedidoInternoRecorrenteDTO.IdUsuarioAlteracao;
            pedidoInternoRecorrente.IdUsuarioCadastro = pedidoInternoRecorrenteDTO.IdUsuarioCadastro;
            pedidoInternoRecorrente.NecessitaConfirmacao = pedidoInternoRecorrenteDTO.NecessitaConfirmacao;
            pedidoInternoRecorrente.Valor = pedidoInternoRecorrenteDTO.Valor;
            pedidoInternoRecorrente.IdUsuarioAprovador = pedidoInternoRecorrenteDTO.IdUsuarioAprovador;

            await _context.PedidoInternoRecorrente.AddAsync(pedidoInternoRecorrente);
            await _context.SaveChangesAsync();

            await GeraPedidosInternos();

            return _mapper.Map<PedidoInternoRecorrenteDTO>(pedidoInternoRecorrente);
        }

        public async Task<PedidoInternoRecorrenteDTO> Put(PedidoInternoRecorrenteDTO pedidoInternoRecorrenteDTO)
        {
            var pedidoInternoRecorrente = await _context.PedidoInternoRecorrente.FirstOrDefaultAsync(x => x.Id == pedidoInternoRecorrenteDTO.Id);

            if (pedidoInternoRecorrente == null)
                throw new Exception("Pedido interno recorrente não encontrada");

            pedidoInternoRecorrente.Ativo = pedidoInternoRecorrenteDTO.Ativo;
            pedidoInternoRecorrente.DataLimiteGeracao = pedidoInternoRecorrenteDTO.DataLimiteGeracao;
            pedidoInternoRecorrente.Descricao = pedidoInternoRecorrenteDTO.Descricao;
            pedidoInternoRecorrente.DiaGeracao = pedidoInternoRecorrenteDTO.DiaGeracao;
            pedidoInternoRecorrente.IdUsuarioAlteracao = pedidoInternoRecorrenteDTO.IdUsuarioAlteracao;
            pedidoInternoRecorrente.NecessitaConfirmacao = pedidoInternoRecorrenteDTO.NecessitaConfirmacao;
            pedidoInternoRecorrente.Valor = pedidoInternoRecorrenteDTO.Valor;
            pedidoInternoRecorrente.IdUsuarioAprovador = pedidoInternoRecorrenteDTO.IdUsuarioAprovador;

            _context.Entry(pedidoInternoRecorrente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<PedidoInternoRecorrenteDTO>(pedidoInternoRecorrente);
        }

        public async Task GeraPedidosInternos()
        {
            var usuarioSistema = await _context.Usuario.FirstOrDefaultAsync(x => x.Nome == "Sistema");
            var pedidosInternosRecorrentes = await _context.PedidoInternoRecorrente.AsNoTracking().Include(x => x.DEF).Where(x => x.Ativo && (!x.DataLimiteGeracao.HasValue || x.DataLimiteGeracao.Value.Date > DateTime.Now.Date)).ToListAsync();

            pedidosInternosRecorrentes.ForEach(pedidoInternoRecorrente =>
            {
                var def = _context.DEF.FirstOrDefault(x => x.Id == pedidoInternoRecorrente.IdDEF);

                for (int mes = 0; mes < 6; mes++)
                {
                    var dataComparacao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(mes);

                    if (!_context.PedidoInterno.AsNoTracking().Any(y => y.IdPedidoInternoRecorrente == pedidoInternoRecorrente.Id && y.Parcelas.Any(z => z.DataPagamento.Year == dataComparacao.Year && z.DataPagamento.Month == dataComparacao.Month)))
                    {
                        var pedidoInterno = new PedidoInterno();
                        pedidoInterno.Id = 0;
                        pedidoInterno.IdPedidoInternoRecorrente = pedidoInternoRecorrente.Id;
                        pedidoInterno.Aprovado = !pedidoInternoRecorrente.NecessitaConfirmacao;

                        Int64? codigoExistente = (_context.PedidoInterno.AsNoTracking().FirstOrDefault(y => y.IdPedidoInternoRecorrente == pedidoInternoRecorrente.Id))?.Codigo ?? null;

                        pedidoInterno.Codigo = !codigoExistente.HasValue ? (_context.PedidoInterno.AsNoTracking().Max(x => (int?)x.Codigo) ?? 0) + 1 : codigoExistente.Value;
                        pedidoInterno.CodigoFormatado = $"P{pedidoInterno.Codigo.ToString().PadLeft(6, '0')}";
                        pedidoInterno.DataCadastro = DateTime.Now;
                        pedidoInterno.DataUltimaAlteracao = DateTime.Now;
                        pedidoInterno.DataAprovacao = DateTime.Now;
                        pedidoInterno.Descricao = pedidoInternoRecorrente.Descricao;
                        pedidoInterno.IdUsuarioAlteracao = usuarioSistema.Id;
                        pedidoInterno.IdUsuarioAprovacao = usuarioSistema.Id;
                        pedidoInterno.IdUsuarioBeneficiario = usuarioSistema.Id;
                        pedidoInterno.IdUsuarioCadastro = usuarioSistema.Id;
                        pedidoInterno.NumeroTotalParcelas = 1;
                        pedidoInterno.ValorTotal = pedidoInternoRecorrente.Valor;
                        pedidoInterno.ImportadoParaFinanceiro = true;
                        pedidoInterno.IdDef = pedidoInternoRecorrente.IdDEF;
                        pedidoInterno.Cancelado = false;

                        _context.PedidoInterno.Add(pedidoInterno);
                        _context.SaveChanges();

                        _context.Entry(pedidoInterno).State = EntityState.Detached;

                        var pedidoInternoParcela = new PedidoInternoParcelas();
                        pedidoInternoParcela.CodigoFormatado = $"P{pedidoInterno.Codigo.ToString().PadLeft(6, '0')}";
                        pedidoInternoParcela.DataPagamento = new DateTime(DateTime.Now.Year, dataComparacao.Month, pedidoInternoRecorrente.DiaGeracao);
                        pedidoInternoParcela.Id = 0;
                        pedidoInternoParcela.IdPedidoInterno = pedidoInterno.Id;
                        pedidoInternoParcela.PagamentoEfetuado = false;
                        pedidoInternoParcela.Parcela = 1;
                        pedidoInternoParcela.Valor = pedidoInternoRecorrente.Valor;

                        _context.PedidoInternoParcelas.Add(pedidoInternoParcela);
                        _context.SaveChanges();

                        var pedidoInternoParcelaDEF = new PedidoInternoParcelaDEFs();
                        pedidoInternoParcelaDEF.Id = 0;
                        pedidoInternoParcelaDEF.IdDef = pedidoInternoRecorrente.IdDEF;
                        pedidoInternoParcelaDEF.IdPedidoInternoParcela = pedidoInternoParcela.Id;
                        pedidoInternoParcelaDEF.Valor = pedidoInternoRecorrente.Valor;

                        _context.PedidoInternoParcelaDEFs.Add(pedidoInternoParcelaDEF);
                        _context.SaveChanges();

                        _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                        {
                            CodigoDef = def.Codigo,
                            CodigoFatura = null,
                            CodigoObra = null,
                            CodigoPedidoCompra = null,
                            CodigoPedidoInterno = pedidoInterno.CodigoFormatado,
                            DataLancamento = pedidoInterno.DataCadastro,
                            DataPagamento = pedidoInternoParcela.DataPagamento,
                            DataPagamentoEfetuado = null,
                            Fornecedor = null,
                            Id = 0,
                            IdCliente = null,
                            IdDef = def.Id,
                            IdFaturamento = null,
                            IdFornecedorBeneficiario = null,
                            IdObra = null,
                            IdPedidoCompra = null,
                            IdPedidoCompraFatura = null,
                            IdPedidoCompraNotaFiscal = null,
                            IdPedidoInterno = pedidoInterno.Id,
                            IdTipoFluxoCaixa = 2,
                            IdUsuarioBeneficiario = null,
                            IdUsuarioInformouPagamento = null,
                            NomeCliente = pedidoInterno.Descricao,
                            NumeroNotaFiscalFaturamento = null,
                            NumeroNotaFiscalPedidoCompra = null,
                            PagamentoEfetuado = false,
                            UsuarioBeneficiario = null,
                            UsuarioPagamento = null,
                            Valor = pedidoInternoParcela.Valor
                        }).Wait();
                    }
                }
            });



            for (int i = 0; i < pedidosInternosRecorrentes.Count; i++)
            {
                
            }
        }
    }
}