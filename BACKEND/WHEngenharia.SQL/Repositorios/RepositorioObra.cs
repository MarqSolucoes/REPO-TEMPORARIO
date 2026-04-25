using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Obra;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioObra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;

        public RepositorioObra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
        }

        public async Task<List<ObraDTO>> Get(Int64 idUsuario, bool apenasAtivos = false)
        {
            bool listarApenasObrasDoUsuario = false;

            if ((await _context.Usuario.Include(x => x.CargoUsuario).FirstOrDefaultAsync(x => x.Id == idUsuario)).CargoUsuario.Descricao == "Engenheiro")
                listarApenasObrasDoUsuario = true;

            if (apenasAtivos)
                return _mapper.Map<List<ObraDTO>>(await _context.Obra.AsNoTracking()
                    .Include(x => x.Cidade)
                    .Include(x => x.Cliente)
                    .Include(x => x.DiretorAprovador)
                    .Include(x => x.UsuariosAprovadores)
                    .Include(x => x.Faturamentos)
                    .Include(x => x.ETOs)
                    .Where(x => !x.Bloqueada && !x.Finalizada && (listarApenasObrasDoUsuario ? x.UsuariosAprovadores.Any(y => y.IdUsuarioAprovacao == idUsuario) : true))
                    .ToListAsync());
            else
                return _mapper.Map<List<ObraDTO>>(await _context.Obra.AsNoTracking()
                    .Include(x => x.Cidade)
                    .Include(x => x.Cliente)
                    .Include(x => x.DiretorAprovador)
                    .Include(x => x.UsuariosAprovadores)
                    .ThenInclude(x => x.Usuario)
                    .Include(x => x.Faturamentos)
                    .Include(x => x.ETOs)
                    .Where(x => (listarApenasObrasDoUsuario ? x.UsuariosAprovadores.Any(y => y.IdUsuarioAprovacao == idUsuario) : true))
                    .ToListAsync());
        }

        public async Task<ObraDTO> Get(Int64 Id)
        {
            return _mapper.Map<ObraDTO>(await _context.Obra.AsNoTracking()
                .Include(x => x.Cidade)
                .Include(x => x.Cliente)
                .Include(x => x.DiretorAprovador)
                .Include(x => x.UsuariosAprovadores)
                .ThenInclude(x=>x.Usuario)
                .Include(x => x.Faturamentos)
                .Include(x => x.ETOs)
                .FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<ObraDTO> Post(ObraDTO obraDTO)
        {
            var obra = new Obra();
            obra.AprovacaoAutomatica = obraDTO.AprovacaoAutomatica;
            obra.Bloqueada = obraDTO.Bloqueada;
            obra.Finalizada = obraDTO.Finalizada;
            obra.Cidade = null;
            obra.Cliente = null;
            obra.CodigoAno = DateTime.Now.Year;
            obra.CodigoSequencia = (_context.Obra.Max(x => (int?)x.CodigoSequencia) ?? 42422) + 1;
            obra.Codigo = $"O{obra.CodigoSequencia.ToString()}";
            obra.CodigoProposta = obraDTO.CodigoProposta;
            obra.CEP = obraDTO.CEP;
            obra.ETOs = null;
            obra.DataCadastro = obraDTO.DataCadastro;
            obra.DataFim = obraDTO.DataFim;
            obra.DataInicio = obraDTO.DataInicio;
            obra.DataProposta = obraDTO.DataProposta;
            obra.DataUltimaAlteracao = obraDTO.DataUltimaAlteracao;
            obra.Descricao = obraDTO.Descricao;
            obra.DiasDePagamento = obraDTO.DiasDePagamento;
            obra.EnderecoObra = obraDTO.EnderecoObra;
            obra.EnderecoEntrega = obraDTO.EnderecoEntrega;
            obra.Id = 0;
            obra.IdCidade = obraDTO.IdCidade;
            obra.IdCliente = obraDTO.IdCliente;
            obra.IdUsuarioAlteracao = obraDTO.IdUsuarioAlteracao;
            obra.IdUsuarioCadastro = obraDTO.IdUsuarioCadastro;
            obra.IdUsuarioDiretorAprovador = obraDTO.IdUsuarioDiretorAprovador;
            obra.NumeroPedidoCliente = obraDTO.NumeroPedidoCliente;
            obra.Faturamentos = null;
            obra.PrazoDias = obraDTO.PrazoDias;
            obra.ETO = obraDTO.ETOs.Sum(x => x.Valor);
            obra.Gasto = 0;
            obra.Saldo = obraDTO.ETO;
            obra.ValorNaoComissionado = obraDTO.ValorNaoComissionado;
            obra.AliquotaImpostoISS = obraDTO.AliquotaImpostoISS;
            obra.AliquotaImpostoINSS = obraDTO.AliquotaImpostoINSS;
            obra.AliquotaImpostoIR = obraDTO.AliquotaImpostoIR;
            obra.AliquotaImpostoArt30 = obraDTO.AliquotaImpostoArt30;
            obra.ValorSinal = obraDTO.ValorSinal;
            obra.PercentualEquivalenteSinal = obraDTO.PercentualEquivalenteSinal;
            obra.DataRecebimentoSinal = obraDTO.DataRecebimentoSinal;
            obra.ValorTotal = obraDTO.ValorTotal;
            obra.Cancelada = false;

            await _context.Obra.AddAsync(obra);
            await _context.SaveChangesAsync(); // obra.Id gerado aqui

            _context.Entry(obra).State = EntityState.Detached;

            foreach (var usuarioAprovacao in obraDTO.UsuariosAprovadores)
            {
                var obraUsuarioAprovacao = new ObraUsuarioAprovacao();
                obraUsuarioAprovacao.Id = 0;
                obraUsuarioAprovacao.IdObra = obra.Id;
                obraUsuarioAprovacao.IdUsuarioAprovacao = usuarioAprovacao.IdUsuarioAprovacao;

                _context.ObraUsuarioAprovacao.Add(obraUsuarioAprovacao);
                await _context.SaveChangesAsync();
            }

            var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.Id == obra.IdCliente);

            var def0303 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.03");

            foreach (var x in obraDTO.ETOs.OrderBy(x => x.Data))
            {
                var controleCusto = new Modelos.ObraETO();
                controleCusto.DataCadastro = DateTime.Now;
                controleCusto.Data = x.Data;
                controleCusto.DataUltimaAlteracao = DateTime.Now;
                controleCusto.Id = 0;
                controleCusto.IdObra = obra.Id;
                controleCusto.IdUsuarioAlteracao = obraDTO.IdUsuarioAlteracao;
                controleCusto.IdUsuarioCadastro = obraDTO.IdUsuarioCadastro;
                controleCusto.ValorConsumido = 0;
                controleCusto.Valor = x.Valor;

                _context.ObraControleCusto.Add(controleCusto);
                await _context.SaveChangesAsync(); // controleCusto.Id gerado aqui

                var fluxo0303 = new FluxoCaixa();
                fluxo0303.CodigoDef = def0303.Codigo;
                fluxo0303.CodigoFatura = null;
                fluxo0303.CodigoObra = obra.Codigo;
                fluxo0303.CodigoPedidoCompra = null;
                fluxo0303.CodigoPedidoInterno = null;
                fluxo0303.DataLancamento = DateTime.Now;
                fluxo0303.DataPagamento = controleCusto.Data;
                fluxo0303.DataPagamentoEfetuado = null;
                fluxo0303.Id = 0;
                fluxo0303.IdCliente = obra.IdCliente;
                fluxo0303.IdDef = def0303.Id;
                fluxo0303.IdFaturamento = null;
                fluxo0303.IdFornecedorBeneficiario = null;
                fluxo0303.IdObra = obra.Id;
                fluxo0303.IdPedidoCompra = null;
                fluxo0303.IdPedidoCompraFatura = null;
                fluxo0303.IdPedidoCompraNotaFiscal = null;
                fluxo0303.IdPedidoInterno = null;
                fluxo0303.IdTipoFluxoCaixa = 2;
                fluxo0303.IdUsuarioBeneficiario = null;
                fluxo0303.IdUsuarioInformouPagamento = null;
                fluxo0303.NomeCliente = cliente.NomeFantasia;
                fluxo0303.NumeroNotaFiscalFaturamento = null;
                fluxo0303.NumeroNotaFiscalPedidoCompra = null;
                fluxo0303.PagamentoEfetuado = false;
                fluxo0303.Valor = controleCusto.Valor;

                _context.FluxoCaixa.Add(fluxo0303);
                await _context.SaveChangesAsync();
            }

            var def0210 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10"); //A FATURAR
            var def0211 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.11"); //INSS
            var def0212 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.12"); //IR
            var def0213 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.13"); //ISS
            var def0214 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.14"); //ART30

            foreach (var x in obraDTO.Faturamentos.OrderBy(x => x.Data))
            {
                var obraMedicao = new ObraFaturamento();
                obraMedicao.DataCadastro = DateTime.Now;
                obraMedicao.Data = x.Data;
                obraMedicao.DataPrevistaRecebimento = x.DataPrevistaRecebimento;
                obraMedicao.DataUltimaAlteracao = DateTime.Now;
                obraMedicao.Id = 0;
                obraMedicao.IdObra = obra.Id;
                obraMedicao.IdUsuarioAlteracao = obraDTO.IdUsuarioAlteracao;
                obraMedicao.IdUsuarioCadastro = obraDTO.IdUsuarioCadastro;
                obraMedicao.ValorFaturado = 0;
                obraMedicao.Valor = x.Valor;

                _context.ObraMedicao.Add(obraMedicao);
                await _context.SaveChangesAsync(); // obraMedicao.Id gerado aqui

                var impostoArt30 = (x.Valor * obraDTO.AliquotaImpostoArt30 ?? 0) / 100;
                var impostoINSS = (x.Valor * obraDTO.AliquotaImpostoINSS ?? 0) / 100;
                var impostoIR = (x.Valor * obraDTO.AliquotaImpostoIR ?? 0) / 100;
                var impostoISS = (x.Valor * obraDTO.AliquotaImpostoISS) / 100;

                #region 02.10 - A faturar

                var fluxo0210 = new FluxoCaixa();
                fluxo0210.CodigoDef = def0210.Codigo;
                fluxo0210.CodigoFatura = null;
                fluxo0210.CodigoObra = obra.Codigo;
                fluxo0210.CodigoPedidoCompra = null;
                fluxo0210.CodigoPedidoInterno = null;
                fluxo0210.DataLancamento = DateTime.Now;
                fluxo0210.DataPagamento = obraMedicao.Data;
                fluxo0210.DataPagamentoEfetuado = null;
                fluxo0210.Id = 0;
                fluxo0210.IdCliente = obra.IdCliente;
                fluxo0210.IdDef = def0210.Id;
                fluxo0210.IdFaturamento = null;
                fluxo0210.IdFornecedorBeneficiario = null;
                fluxo0210.IdObra = obra.Id;
                fluxo0210.IdPedidoCompra = null;
                fluxo0210.IdPedidoCompraFatura = null;
                fluxo0210.IdPedidoCompraNotaFiscal = null;
                fluxo0210.IdPedidoInterno = null;
                fluxo0210.IdTipoFluxoCaixa = 1;
                fluxo0210.IdUsuarioBeneficiario = null;
                fluxo0210.IdUsuarioInformouPagamento = null;
                fluxo0210.NomeCliente = cliente?.NomeFantasia ?? "";
                fluxo0210.NumeroNotaFiscalFaturamento = null;
                fluxo0210.NumeroNotaFiscalPedidoCompra = null;
                fluxo0210.PagamentoEfetuado = false;
                fluxo0210.Valor = obraMedicao.Valor;

                _context.FluxoCaixa.Add(fluxo0210);
                await _context.SaveChangesAsync(); // fluxo0210.Id gerado aqui — necessário para IdFluxoCaixaPai abaixo

                #endregion

                #region 02.11 - INSS

                var fluxo0211 = new FluxoCaixa();
                fluxo0211.CodigoDef = def0211.Codigo;
                fluxo0211.CodigoFatura = null;
                fluxo0211.CodigoObra = obra.Codigo;
                fluxo0211.CodigoPedidoCompra = null;
                fluxo0211.CodigoPedidoInterno = null;
                fluxo0211.DataLancamento = DateTime.Now;
                fluxo0211.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0211.DataPagamentoEfetuado = null;
                fluxo0211.Id = 0;
                fluxo0211.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0211.IdCliente = obra.IdCliente;
                fluxo0211.IdDef = def0211.Id;
                fluxo0211.IdFaturamento = null;
                fluxo0211.IdFornecedorBeneficiario = null;
                fluxo0211.IdObra = obra.Id;
                fluxo0211.IdPedidoCompra = null;
                fluxo0211.IdPedidoCompraFatura = null;
                fluxo0211.IdPedidoCompraNotaFiscal = null;
                fluxo0211.IdPedidoInterno = null;
                fluxo0211.IdTipoFluxoCaixa = 1;
                fluxo0211.IdUsuarioBeneficiario = null;
                fluxo0211.IdUsuarioInformouPagamento = null;
                fluxo0211.NomeCliente = cliente?.NomeFantasia ?? "";
                fluxo0211.NumeroNotaFiscalFaturamento = null;
                fluxo0211.NumeroNotaFiscalPedidoCompra = null;
                fluxo0211.PagamentoEfetuado = false;
                fluxo0211.Valor = impostoINSS;

                _context.FluxoCaixa.Add(fluxo0211);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.12 - IR

                var fluxo0212 = new FluxoCaixa();
                fluxo0212.CodigoDef = def0212.Codigo;
                fluxo0212.CodigoFatura = null;
                fluxo0212.CodigoObra = obra.Codigo;
                fluxo0212.CodigoPedidoCompra = null;
                fluxo0212.CodigoPedidoInterno = null;
                fluxo0212.DataLancamento = DateTime.Now;
                fluxo0212.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0212.DataPagamentoEfetuado = null;
                fluxo0212.Id = 0;
                fluxo0212.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0212.IdCliente = obra.IdCliente;
                fluxo0212.IdDef = def0212.Id;
                fluxo0212.IdFaturamento = null;
                fluxo0212.IdFornecedorBeneficiario = null;
                fluxo0212.IdObra = obra.Id;
                fluxo0212.IdPedidoCompra = null;
                fluxo0212.IdPedidoCompraFatura = null;
                fluxo0212.IdPedidoCompraNotaFiscal = null;
                fluxo0212.IdPedidoInterno = null;
                fluxo0212.IdTipoFluxoCaixa = 1;
                fluxo0212.IdUsuarioBeneficiario = null;
                fluxo0212.IdUsuarioInformouPagamento = null;
                fluxo0212.NomeCliente = cliente?.NomeFantasia ?? "";
                fluxo0212.NumeroNotaFiscalFaturamento = null;
                fluxo0212.NumeroNotaFiscalPedidoCompra = null;
                fluxo0212.PagamentoEfetuado = false;
                fluxo0212.Valor = impostoIR;

                _context.FluxoCaixa.Add(fluxo0212);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.13 - ISS

                var fluxo0213 = new FluxoCaixa();
                fluxo0213.CodigoDef = def0213.Codigo;
                fluxo0213.CodigoFatura = null;
                fluxo0213.CodigoObra = obra.Codigo;
                fluxo0213.CodigoPedidoCompra = null;
                fluxo0213.CodigoPedidoInterno = null;
                fluxo0213.DataLancamento = DateTime.Now;
                fluxo0213.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0213.DataPagamentoEfetuado = null;
                fluxo0213.Id = 0;
                fluxo0213.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0213.IdCliente = obra.IdCliente;
                fluxo0213.IdDef = def0213.Id;
                fluxo0213.IdFaturamento = null;
                fluxo0213.IdFornecedorBeneficiario = null;
                fluxo0213.IdObra = obra.Id;
                fluxo0213.IdPedidoCompra = null;
                fluxo0213.IdPedidoCompraFatura = null;
                fluxo0213.IdPedidoCompraNotaFiscal = null;
                fluxo0213.IdPedidoInterno = null;
                fluxo0213.IdTipoFluxoCaixa = 1;
                fluxo0213.IdUsuarioBeneficiario = null;
                fluxo0213.IdUsuarioInformouPagamento = null;
                fluxo0213.NomeCliente = cliente?.NomeFantasia ?? "";
                fluxo0213.NumeroNotaFiscalFaturamento = null;
                fluxo0213.NumeroNotaFiscalPedidoCompra = null;
                fluxo0213.PagamentoEfetuado = false;
                fluxo0213.Valor = impostoISS;

                _context.FluxoCaixa.Add(fluxo0213);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.14 - ART30

                var fluxo0214 = new FluxoCaixa();
                fluxo0214.CodigoDef = def0214.Codigo;
                fluxo0214.CodigoFatura = null;
                fluxo0214.CodigoObra = obra.Codigo;
                fluxo0214.CodigoPedidoCompra = null;
                fluxo0214.CodigoPedidoInterno = null;
                fluxo0214.DataLancamento = DateTime.Now;
                fluxo0214.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0214.DataPagamentoEfetuado = null;
                fluxo0214.Id = 0;
                fluxo0214.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0214.IdCliente = obra.IdCliente;
                fluxo0214.IdDef = def0214.Id;
                fluxo0214.IdFaturamento = null;
                fluxo0214.IdFornecedorBeneficiario = null;
                fluxo0214.IdObra = obra.Id;
                fluxo0214.IdPedidoCompra = null;
                fluxo0214.IdPedidoCompraFatura = null;
                fluxo0214.IdPedidoCompraNotaFiscal = null;
                fluxo0214.IdPedidoInterno = null;
                fluxo0214.IdTipoFluxoCaixa = 1;
                fluxo0214.IdUsuarioBeneficiario = null;
                fluxo0214.IdUsuarioInformouPagamento = null;
                fluxo0214.NomeCliente = cliente?.NomeFantasia ?? "";
                fluxo0214.NumeroNotaFiscalFaturamento = null;
                fluxo0214.NumeroNotaFiscalPedidoCompra = null;
                fluxo0214.PagamentoEfetuado = false;
                fluxo0214.Valor = impostoArt30;

                _context.FluxoCaixa.Add(fluxo0214);
                await _context.SaveChangesAsync();

                #endregion
            }

            return _mapper.Map<ObraDTO>(obra);
        }

        public async Task<ObraDTO> Put(ObraDTO obraDTO)
        {
            var obra = await _context.Obra.Include(x=>x.UsuariosAprovadores).FirstOrDefaultAsync(x => x.Id == obraDTO.Id);

            if (obra == null)
                throw new Exception("Obra não encontrada");

            obra.AprovacaoAutomatica = obraDTO.AprovacaoAutomatica;
            obra.Bloqueada = obraDTO.Bloqueada;
            obra.Finalizada = obraDTO.Finalizada;
            obra.CEP = obraDTO.CEP;
            obra.DataFim = obraDTO.DataFim;
            obra.DataInicio = obraDTO.DataInicio;
            obra.DataUltimaAlteracao = obraDTO.DataUltimaAlteracao;
            obra.Descricao = obraDTO.Descricao;
            obra.DiasDePagamento = obraDTO.DiasDePagamento;
            obra.EnderecoObra = obraDTO.EnderecoObra;
            obra.EnderecoEntrega=obraDTO.EnderecoEntrega;
            obra.IdUsuarioAlteracao = obraDTO.IdUsuarioAlteracao;
            obra.IdUsuarioDiretorAprovador = obraDTO.IdUsuarioDiretorAprovador;
            obra.NumeroPedidoCliente = obraDTO.NumeroPedidoCliente;
            obra.PrazoDias = obraDTO.PrazoDias;
            obra.ValorNaoComissionado = obraDTO.ValorNaoComissionado;
            obra.ValorMaterial = obraDTO.ValorMaterial;
            obra.AliquotaImpostoISS = obraDTO.AliquotaImpostoISS;
            obra.AliquotaImpostoINSS = obraDTO.AliquotaImpostoINSS;
            obra.AliquotaImpostoIR = obraDTO.AliquotaImpostoIR;
            obra.AliquotaImpostoArt30 = obraDTO.AliquotaImpostoArt30;
            obra.ValorSinal = obraDTO.ValorSinal;
            obra.PercentualEquivalenteSinal = obraDTO.PercentualEquivalenteSinal;
            obra.DataRecebimentoSinal = obraDTO.DataRecebimentoSinal;
                        
            obra.UsuariosAprovadores?.ForEach(x =>
            {
                _context.ObraUsuarioAprovacao.Remove(x);
            });

            _context.Entry(obra).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            obraDTO.UsuariosAprovadores.ForEach(usuarioAprovacao =>
            {
                var obraUsuarioAprovacao = new ObraUsuarioAprovacao();
                obraUsuarioAprovacao.Id = 0;
                obraUsuarioAprovacao.IdObra = obra.Id;
                obraUsuarioAprovacao.IdUsuarioAprovacao = usuarioAprovacao.IdUsuarioAprovacao;

                _context.ObraUsuarioAprovacao.Add(obraUsuarioAprovacao);
                _context.SaveChanges();
            });

            return _mapper.Map<ObraDTO>(obra);
        }

        public async Task BloquearDesbloquear(Int64 idObra, Int64 idUsuario, bool status)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra SET Bloqueada = {(status ? 1 : 0)}, IdUsuarioAlteracao = {idUsuario}, DataUltimaAlteracao = GETDATE() WHERE Id = {idObra}");
        }

        public async Task CalculaCustoObra(Int64 idObra)
        {
            var custo = 0.0;

            _context.PedidoCompra.Include(x => x.Fornecedor).Where(x => x.IdCentroCustoObra == idObra && x.IdStatusPedidoCompra < 3).ToList().ForEach(pedidoCompra =>
            {
                custo += pedidoCompra.ValorTotal;
            });

            _context.PedidoInterno
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.UsuarioBeneficiario)
                .Where(x => x.Obras.Any(y => y.IdObra == idObra)).ToList().ForEach(pedidoInterno =>
                {
                    pedidoInterno.Parcelas.ForEach(parcela =>
                    {
                        parcela.ParcelaObras.ForEach(parcelaObra =>
                        {
                            if (parcelaObra.IdObra == idObra)
                            {
                                custo += parcelaObra.Valor;
                            }
                        });
                    });
                });

            _context.PedidoCompra_DevolucaoSaldo.Include(x => x.PedidoCompra).ThenInclude(x => x.Fornecedor).Where(x => x.PedidoCompra.IdCentroCustoObra == idObra).ToList().ForEach(cancelamentoSaldo =>
            {
                custo -= cancelamentoSaldo.Valor;
            });

            await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra SET Gasto = {custo.ToString().Replace(",", ".")} WHERE Id = {idObra}");

           
        }

        public async Task PostMedicoes(List<ObraFaturamentoDTO> medicoesDTO)
        {
            //var idObra = medicoesDTO.FirstOrDefault().IdObra;
            //var obra = _context.Obra.FirstOrDefault(x => x.Id == idObra);

            //await _context.Database.ExecuteSqlRawAsync($"DELETE FROM ObraMedicao WHERE IdObra = {idObra}");

            //medicoesDTO.OrderBy(x => x.Data).ToList().ForEach(x =>
            //{
            //    var obraMedicao = new ObraFaturamento();

            //    obraMedicao.DataCadastro = x.DataCadastro;
            //    obraMedicao.Data = x.Data;
            //    obraMedicao.DataPrevistaRecebimento = x.DataPrevistaRecebimento;
            //    obraMedicao.DataUltimaAlteracao = x.DataUltimaAlteracao;
            //    obraMedicao.Id = 0;
            //    obraMedicao.IdObra = x.IdObra;
            //    obraMedicao.IdObraAjuste = x.IdObraAjuste;
            //    obraMedicao.IdUsuarioAlteracao = x.IdUsuarioAlteracao;
            //    obraMedicao.IdUsuarioCadastro = x.IdUsuarioCadastro;
            //    obraMedicao.Obra = null;
            //    obraMedicao.Valor = x.Valor;
            //    obraMedicao.ValorPrevistoAjustado = x.ValorPrevistoAjustado;
            //    obraMedicao.ValorFaturado = x.ValorFaturado;

            //    _context.ObraMedicao.Add(obraMedicao);
            //    _context.SaveChanges();
            //});

            //await AtualizaMedicoesFluxoCaixa(obra.Id, 0);
            //await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra SET ValorTotalAjustado = (SELECT SUM(OM.ValorPrevistoAjustado) FROM ObraMedicao OM WHERE OM.IdObra = {idObra}) WHERE Id = {idObra}");
        }

        public async Task PostETOs(List<Dominio.Modelos.ObraETODTO> etosDTO)
        {
            //var idObra = etosDTO.FirstOrDefault().IdObra;

            //await _context.Database.ExecuteSqlRawAsync($"DELETE FROM ObraControleCusto WHERE IdObra = {idObra}");

            //etosDTO.OrderBy(x => x.Data).ToList().ForEach(x =>
            //{
            //    var ObraControleCusto = new Modelos.ObraETO();

            //    ObraControleCusto.DataCadastro = x.DataCadastro;
            //    ObraControleCusto.Data = x.Data;
            //    ObraControleCusto.DataUltimaAlteracao = x.DataUltimaAlteracao;
            //    ObraControleCusto.Id = 0;
            //    ObraControleCusto.IdObra = x.IdObra;
            //    ObraControleCusto.IdUsuarioAlteracao = x.IdUsuarioAlteracao;
            //    ObraControleCusto.IdUsuarioCadastro = x.IdUsuarioCadastro;
            //    ObraControleCusto.Obra = null;
            //    ObraControleCusto.ValorPrevisto = x.Valor;
            //    ObraControleCusto.ValorPrevistoAjustado = x.ValorPrevistoAjustado;
            //    ObraControleCusto.ValorMedido = x.ValorMedido;

            //    _context.ObraControleCusto.Add(ObraControleCusto);
            //    _context.SaveChanges();
            //});

            //await AtualizaETOsFluxoCaixa(idObra);
            //await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra SET ValorCustoAjustado = (SELECT SUM(OM.ValorPrevistoAjustado) FROM ObraControleCusto OM WHERE OM.IdObra = {idObra}) WHERE Id = {idObra}");
        }

        public async Task<List<Obra_ItemContaCorrenteDTO>> ObtemDadosETO(Int64 IdObra)
        {
            var result = new List<Obra_ItemContaCorrenteDTO>();

            _context.PedidoCompra.Include(x=>x.Fornecedor).Include(x=>x.NotasFiscais).Include(x=>x.CancelamentosSaldo).Where(x => x.IdCentroCustoObra == IdObra && x.IdStatusPedidoCompra < 3).ToList().ForEach(pedidoCompra =>
            {
                var valorNotas = pedidoCompra.NotasFiscais.Where(x=>x.Aprovada==true).Sum(y => y.Valor);
                var valorCancelamentoDeSaldoParaPedido = pedidoCompra.CancelamentosSaldo.Sum(x => x.Valor);

                var valorFinal = pedidoCompra.ValorTotal - valorCancelamentoDeSaldoParaPedido;

                if (valorFinal > 0)
                    result.Add(new Obra_ItemContaCorrenteDTO()
                    {
                        Credito = false,
                        Data = pedidoCompra.DataCadastro,
                        Fornecedor = pedidoCompra.Fornecedor?.NomeFantasia ?? "",
                        NotaFiscal = "",
                        OrdemCompra = pedidoCompra.CodigoSequencia.ToString().PadLeft(7, '0'),
                        Valor = valorFinal
                    });
            });

            _context.PedidoInterno
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .Include(x=>x.FornecedorBeneficiario)
                .Include(x=>x.UsuarioBeneficiario)
                .Where(x => x.Obras.Any(y => y.IdObra == IdObra)).ToList().ForEach(pedidoInterno =>
            {
                pedidoInterno.Parcelas.ForEach(parcela =>
                {
                    parcela.ParcelaObras.ForEach(parcelaObra =>
                    {
                        if (parcelaObra.IdObra == IdObra)
                        {
                            result.Add(new Obra_ItemContaCorrenteDTO()
                            {
                                Credito = false,
                                Data = parcela.DataPagamento,
                                Fornecedor = pedidoInterno.IdUsuarioBeneficiario.HasValue?pedidoInterno.UsuarioBeneficiario.Nome : pedidoInterno.FornecedorBeneficiario?.NomeFantasia,
                                NotaFiscal = "",
                                OrdemCompra = parcela.CodigoFormatado,
                                Valor = parcelaObra.Valor
                            });
                        }
                    });
                });
            });

            //_context.PedidoCompra_DevolucaoSaldo.Include(x=>x.PedidoCompra).ThenInclude(x=>x.Fornecedor).Where(x => x.PedidoCompra.IdCentroCustoObra == IdObra).ToList().ForEach(cancelamentoSaldo =>
            //{
            //    result.Add(new Obra_ItemContaCorrenteDTO()
            //    {
            //        Credito = true,
            //        Data = cancelamentoSaldo.DataCadastro,
            //        Fornecedor = cancelamentoSaldo.PedidoCompra?.Fornecedor?.NomeFantasia ?? "",
            //        NotaFiscal = "C. SALDO",
            //        OrdemCompra = cancelamentoSaldo.PedidoCompra?.CodigoSequencia.ToString().PadLeft(7, '0'),
            //        Valor = cancelamentoSaldo.Valor
            //    });
            //});

            return result.OrderBy(x=>x.Data).ToList();
        }

        public async Task<List<Obra_ItemContaCorrenteDTO>> ObtemDadosContaCorrente(Int64 IdObra)
        {
            var result = new List<Obra_ItemContaCorrenteDTO>();

            //vincular com as notas fiscais e o saldo restante colocar para as próximas parcelas
            _context.PedidoCompra
                .Include(x => x.NotasFiscais)
                .Include(x => x.Faturas)
                .Include(x => x.Fornecedor)
                .Where(x => x.IdStatusPedidoCompra != 3 && x.IdCentroCustoObra == IdObra && x.ImportadoParaFinanceiro == true).ToList().ForEach(x =>
             {
                 var faturasTemporarias = new List<Obra_ItemContaCorrenteDTO>();

                 x.Faturas.ForEach(y =>
                 {
                     faturasTemporarias.Add(new Obra_ItemContaCorrenteDTO()
                     {
                         Credito = false,
                         Data = y.DataFatura,
                         Fornecedor = x.Fornecedor?.NomeFantasia ?? "",
                         NotaFiscal = "",
                         OrdemCompra = x.Codigo,
                         Valor = y.Valor
                     });
                 });

                 x.NotasFiscais.Where(nf=>nf.ImportadoParaFinanceiro == true).ToList().ForEach(y =>
                 {
                     result.Add(new Obra_ItemContaCorrenteDTO()
                     {
                         Credito = false,
                         Data = _context.FluxoCaixa.FirstOrDefault(z => z.IdPedidoCompraNotaFiscal == y.Id)?.DataPagamento ?? y.DataVencimento,
                         Fornecedor = x.Fornecedor?.NomeFantasia ?? "",
                         NotaFiscal = y.NumeroNotaFiscal,
                         OrdemCompra = x.Codigo,
                         Valor = y.Valor
                     });

                     var valorParaDeduzir = y.Valor;

                     while (valorParaDeduzir > 0)
                     {
                         if (faturasTemporarias.Where(x => x.Valor > 0).ToList().Count == 0)
                             break;

                         faturasTemporarias.Where(x => x.Valor > 0).ToList().ForEach(faturaTemporaria =>
                         {
                             if (faturaTemporaria.Valor >= valorParaDeduzir)
                             {
                                 faturaTemporaria.Valor -= valorParaDeduzir; //Apenas subtrai o valor
                                 valorParaDeduzir = 0;
                             }
                             else
                             {
                                 valorParaDeduzir -= faturaTemporaria.Valor;
                                 faturaTemporaria.Valor = 0;
                             }
                         });
                     }
                 });

                 result.AddRange(faturasTemporarias.Where(x => x.Valor > 0).ToList());
             });

            _context.PedidoInterno
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .Where(x => x.Aprovado == true && x.Obras.Any(y => y.IdObra == IdObra)).ToList().ForEach(x =>
            {
                x.Parcelas.ForEach(parcela =>
                {
                    parcela.ParcelaObras.Where(y => y.IdObra == IdObra).ToList().ForEach(parcelaObra =>
                    {
                        result.Add(new Obra_ItemContaCorrenteDTO()
                        {
                            Credito = false,
                            Data = parcela.DataPagamento,
                            Fornecedor = (x.IdFornecedorBeneficiario.HasValue ? x.FornecedorBeneficiario.NomeFantasia : x.UsuarioBeneficiario?.Nome),
                            NotaFiscal = parcela.CodigoFormatado,
                            OrdemCompra = parcela.CodigoFormatado,
                            Valor = parcelaObra.Valor
                        });
                    });
                });
            });

            var etoTemporario = new List<Obra_ItemContaCorrenteDTO>();

            var dataUltimoETO = new DateTime(2000, 1, 1);


            //_context.ObraControleCusto.Where(x => x.IdObra == IdObra && x.ValorMedido<x.ValorPrevistoAjustado).OrderBy(x=>x.DataPrevista).ToList()
            _context.FluxoCaixa.Where(x => x.IdObra == IdObra && x.CodigoDef == "03.03").OrderBy(x => x.DataPagamento).ToList().ForEach(x =>
            {
                etoTemporario.Add(new Obra_ItemContaCorrenteDTO()
                {
                    Credito = false,
                    Fornecedor = "ETO",
                    NotaFiscal = "",
                    OrdemCompra = "",
                    ValorPrevisto = x.Valor,
                    Valor = x.Valor,
                    Data = x.DataPagamento,
                    DataRecebimentoPrevisto = x.DataPagamento
                });

                dataUltimoETO = x.DataPagamento;
            });

            result.AddRange(etoTemporario);


            _context.Faturamento.Where(x => x.IdStatusFaturamento != 3 && x.IdObra == IdObra).ToList().ForEach(faturamento =>
            {
                _context.FluxoCaixa.Where(y => y.IdFaturamento == faturamento.Id && y.CodigoDef== "02.09").ToList().ForEach(fluxoCaixa =>
                {
                    result.Add(new Obra_ItemContaCorrenteDTO()
                    {
                        Credito = true,
                        Fornecedor = $"FATURAMENTO - NF {faturamento.NumeroNF}",
                        NotaFiscal = faturamento.NumeroNF,
                        OrdemCompra = "",
                        Valor = fluxoCaixa.Valor,
                        Data = faturamento.DataFaturamento,
                        DataRecebimentoPrevisto = fluxoCaixa.PagamentoEfetuado ? fluxoCaixa.DataPagamento : faturamento.DataRecebimentoPrevisto
                    });
                });
                
            });

            var obra = await _context.Obra.FirstOrDefaultAsync(x => x.Id == IdObra);

            var def0210 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");
            _context.FluxoCaixa.Where(x => x.IdObra == IdObra && x.IdDef == def0210.Id).OrderBy(x => x.DataPagamento).ToList();

            _context.FluxoCaixa.Where(x => x.IdObra == IdObra && x.IdDef == def0210.Id).OrderBy(x => x.DataPagamento).ToList().ForEach(x =>
            {
                result.Add(new Obra_ItemContaCorrenteDTO()
                {
                    Credito = true,
                    Fornecedor = "A FATURAR",
                    NotaFiscal = "",
                    OrdemCompra = "",
                    Valor = x.Valor,
                    Data = x.DataPagamento,
                    DataRecebimentoPrevisto = x.DataPagamento.AddDays(obra.DiasDePagamento)
                });
            });



            return result.OrderBy(x => x.Data?.Date).ToList();
        }

        public async Task<List<Obra_ItemResumoETODTO>> ObtemDadosResumoETO()
        { 
            var result = new List<Obra_ItemResumoETODTO>();

            _context.Obra.Where(x => !x.Cancelada && !x.Finalizada && !x.Bloqueada).Select(x => new { x.Id, x.Codigo, x.Cliente.NomeFantasia, x.Descricao, x.ValorTotal, x.ETO }).ToList().ForEach(obra =>
            {
                double gastoObra = 0;

                _context.PedidoCompra
                    .Include(x => x.NotasFiscais)
                    .Include(x => x.Faturas)
                    .Include(x => x.Fornecedor)
                    .Where(x => x.IdStatusPedidoCompra != 3 && x.IdCentroCustoObra == obra.Id && x.ImportadoParaFinanceiro == true).ToList().ForEach(x =>
                    {
                        var faturasTemporarias = new List<Obra_ItemContaCorrenteDTO>();

                        x.Faturas.ForEach(y =>
                        {
                            faturasTemporarias.Add(new Obra_ItemContaCorrenteDTO()
                            {
                                Credito = false,
                                Data = y.DataFatura,
                                Fornecedor = x.Fornecedor?.NomeFantasia ?? "",
                                NotaFiscal = "",
                                OrdemCompra = x.Codigo,
                                Valor = y.Valor
                            });
                        });

                        x.NotasFiscais.Where(nf => nf.ImportadoParaFinanceiro == true).ToList().ForEach(y =>
                        {
                            gastoObra += y.Valor;

                            var valorParaDeduzir = y.Valor;

                            while (valorParaDeduzir > 0)
                            {
                                if (faturasTemporarias.Where(x => x.Valor > 0).ToList().Count == 0)
                                    break;

                                faturasTemporarias.Where(x => x.Valor > 0).ToList().ForEach(faturaTemporaria =>
                                {
                                    if (faturaTemporaria.Valor >= valorParaDeduzir)
                                    {
                                        faturaTemporaria.Valor -= valorParaDeduzir; //Apenas subtrai o valor
                                        valorParaDeduzir = 0;
                                    }
                                    else
                                    {
                                        valorParaDeduzir -= faturaTemporaria.Valor;
                                        faturaTemporaria.Valor = 0;
                                    }
                                });
                            }
                        });

                        gastoObra += faturasTemporarias.Sum(w => w.Valor);
                    });

                _context.PedidoInterno
                    .Include(x => x.UsuarioBeneficiario)
                    .Include(x => x.FornecedorBeneficiario)
                    .Include(x => x.Parcelas)
                    .ThenInclude(x => x.ParcelaObras)
                    .Where(x => x.Aprovado == true && x.Obras.Any(y => y.IdObra == obra.Id)).ToList().ForEach(x =>
                    {
                        x.Parcelas.ForEach(parcela =>
                        {
                            parcela.ParcelaObras.Where(y => y.IdObra == obra.Id).ToList().ForEach(parcelaObra =>
                            {
                                gastoObra += parcelaObra.Valor;
                            });
                        });
                    });

                var cancelamentoSaldo = _context.PedidoCompra_DevolucaoSaldo.Where(y => y.PedidoCompra.IdCentroCustoObra == obra.Id).Sum(y => y.Valor);

                result.Add(new Obra_ItemResumoETODTO()
                {
                    Cliente = obra.NomeFantasia,
                    CodigoObra = obra.Codigo,
                    Escopo = obra.Descricao,
                    ETO = obra.ETO,
                    Gasto = gastoObra,
                    Saldo = obra.ETO - gastoObra + cancelamentoSaldo
                });
            });

            return result;
        }

        public async Task AtualizaMedicoesFluxoCaixa(Int64 idObra, double saldoParaDescontarDaMedicao)
        {
            var obra = await _context.Obra.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idObra);
            var def0210 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");

            var defsDeFaturamento = await _context.FluxoCaixa.Include(x=>x.FluxosCaixaFilhos).Where(x => x.IdObra == idObra && x.IdDef == def0210.Id).OrderBy(x=>x.DataPagamento).ToListAsync();

            defsDeFaturamento.ForEach(defFaturamento =>
            {
                if (saldoParaDescontarDaMedicao > 0)
                {
                    if (defFaturamento.Valor < saldoParaDescontarDaMedicao)
                    {
                        saldoParaDescontarDaMedicao -= defFaturamento.Valor;
                        defFaturamento.Valor = 0;

                        _context.Entry(defFaturamento).State = EntityState.Deleted;
                    }
                    else
                    {
                        defFaturamento.Valor -= saldoParaDescontarDaMedicao;
                        saldoParaDescontarDaMedicao = 0;

                        if(defFaturamento.Valor==0)
                            _context.Entry(defFaturamento).State = EntityState.Deleted;
                        else
                            _context.Entry(defFaturamento).State = EntityState.Modified;
                    }
                }

                foreach (var item in defFaturamento.FluxosCaixaFilhos)
                {
                    _context.Entry(item).State = EntityState.Deleted;
                }

            });

            await _context.SaveChangesAsync();

            var def0211 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.11"); //INSS
            var def0212 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.12"); //IR
            var def0213 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.13"); //ISS
            var def0214 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.14"); //ART30

            defsDeFaturamento = await _context.FluxoCaixa.Include(x => x.FluxosCaixaFilhos).Where(x => x.IdObra == idObra && x.IdDef == def0210.Id).ToListAsync();

            foreach (var fluxo0210 in defsDeFaturamento)
            {
                var impostoArt30 = (fluxo0210.Valor * obra.AliquotaImpostoArt30 ?? 0) / 100;
                var impostoINSS = (fluxo0210.Valor * obra.AliquotaImpostoINSS ?? 0) / 100;
                var impostoIR = (fluxo0210.Valor * obra.AliquotaImpostoIR ?? 0) / 100;
                var impostoISS = (fluxo0210.Valor * obra.AliquotaImpostoISS) / 100;

                #region 02.11 - INSS

                var fluxo0211 = new FluxoCaixa();
                fluxo0211.CodigoDef = def0211.Codigo;
                fluxo0211.CodigoFatura = null;
                fluxo0211.CodigoObra = fluxo0210.CodigoObra;
                fluxo0211.CodigoPedidoCompra = null;
                fluxo0211.CodigoPedidoInterno = null;
                fluxo0211.DataLancamento = DateTime.Now;
                fluxo0211.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0211.DataPagamentoEfetuado = null;
                fluxo0211.Id = 0;
                fluxo0211.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0211.IdCliente = fluxo0210.IdCliente;
                fluxo0211.IdDef = def0211.Id;
                fluxo0211.IdFaturamento = null;
                fluxo0211.IdFornecedorBeneficiario = null;
                fluxo0211.IdObra = fluxo0210.IdObra;
                fluxo0211.IdPedidoCompra = null;
                fluxo0211.IdPedidoCompraFatura = null;
                fluxo0211.IdPedidoCompraNotaFiscal = null;
                fluxo0211.IdPedidoInterno = null;
                fluxo0211.IdTipoFluxoCaixa = 1;
                fluxo0211.IdUsuarioBeneficiario = null;
                fluxo0211.IdUsuarioInformouPagamento = null;
                fluxo0211.NomeCliente = fluxo0210.NomeCliente;
                fluxo0211.NumeroNotaFiscalFaturamento = null;
                fluxo0211.NumeroNotaFiscalPedidoCompra = null;
                fluxo0211.PagamentoEfetuado = false;
                fluxo0211.Valor = impostoINSS;

                _context.FluxoCaixa.Add(fluxo0211);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.12 - IR

                var fluxo0212 = new FluxoCaixa();
                fluxo0212.CodigoDef = def0212.Codigo;
                fluxo0212.CodigoFatura = null;
                fluxo0212.CodigoObra = fluxo0210.CodigoObra;
                fluxo0212.CodigoPedidoCompra = null;
                fluxo0212.CodigoPedidoInterno = null;
                fluxo0212.DataLancamento = DateTime.Now;
                fluxo0212.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0212.DataPagamentoEfetuado = null;
                fluxo0212.Id = 0;
                fluxo0212.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0212.IdCliente = fluxo0210.IdCliente;
                fluxo0212.IdDef = def0212.Id;
                fluxo0212.IdFaturamento = null;
                fluxo0212.IdFornecedorBeneficiario = null;
                fluxo0212.IdObra = fluxo0210.IdObra;
                fluxo0212.IdPedidoCompra = null;
                fluxo0212.IdPedidoCompraFatura = null;
                fluxo0212.IdPedidoCompraNotaFiscal = null;
                fluxo0212.IdPedidoInterno = null;
                fluxo0212.IdTipoFluxoCaixa = 1;
                fluxo0212.IdUsuarioBeneficiario = null;
                fluxo0212.IdUsuarioInformouPagamento = null;
                fluxo0212.NomeCliente = fluxo0210.NomeCliente;
                fluxo0212.NumeroNotaFiscalFaturamento = null;
                fluxo0212.NumeroNotaFiscalPedidoCompra = null;
                fluxo0212.PagamentoEfetuado = false;
                fluxo0212.Valor = impostoIR;

                _context.FluxoCaixa.Add(fluxo0212);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.13 - ISS

                var fluxo0213 = new FluxoCaixa();
                fluxo0213.CodigoDef = def0213.Codigo;
                fluxo0213.CodigoFatura = null;
                fluxo0213.CodigoObra = fluxo0210.CodigoObra;
                fluxo0213.CodigoPedidoCompra = null;
                fluxo0213.CodigoPedidoInterno = null;
                fluxo0213.DataLancamento = DateTime.Now;
                fluxo0213.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0213.DataPagamentoEfetuado = null;
                fluxo0213.Id = 0;
                fluxo0213.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0213.IdCliente = fluxo0210.IdCliente;
                fluxo0213.IdDef = def0213.Id;
                fluxo0213.IdFaturamento = null;
                fluxo0213.IdFornecedorBeneficiario = null;
                fluxo0213.IdObra = fluxo0210.IdObra;
                fluxo0213.IdPedidoCompra = null;
                fluxo0213.IdPedidoCompraFatura = null;
                fluxo0213.IdPedidoCompraNotaFiscal = null;
                fluxo0213.IdPedidoInterno = null;
                fluxo0213.IdTipoFluxoCaixa = 1;
                fluxo0213.IdUsuarioBeneficiario = null;
                fluxo0213.IdUsuarioInformouPagamento = null;
                fluxo0213.NomeCliente = fluxo0210.NomeCliente;
                fluxo0213.NumeroNotaFiscalFaturamento = null;
                fluxo0213.NumeroNotaFiscalPedidoCompra = null;
                fluxo0213.PagamentoEfetuado = false;
                fluxo0213.Valor = impostoISS;

                _context.FluxoCaixa.Add(fluxo0213);
                await _context.SaveChangesAsync();

                #endregion

                #region 02.14 - ART30

                var fluxo0214 = new FluxoCaixa();
                fluxo0214.CodigoDef = def0214.Codigo;
                fluxo0214.CodigoFatura = null;
                fluxo0214.CodigoObra = fluxo0210.CodigoObra;
                fluxo0214.CodigoPedidoCompra = null;
                fluxo0214.CodigoPedidoInterno = null;
                fluxo0214.DataLancamento = DateTime.Now;
                fluxo0214.DataPagamento = new DateTime(fluxo0210.DataPagamento.Year, fluxo0210.DataPagamento.Month, 20).AddMonths(1);
                fluxo0214.DataPagamentoEfetuado = null;
                fluxo0214.Id = 0;
                fluxo0214.IdFluxoCaixaPai = fluxo0210.Id;
                fluxo0214.IdCliente = fluxo0210.IdCliente;
                fluxo0214.IdDef = def0214.Id;
                fluxo0214.IdFaturamento = null;
                fluxo0214.IdFornecedorBeneficiario = null;
                fluxo0214.IdObra = fluxo0210.IdObra;
                fluxo0214.IdPedidoCompra = null;
                fluxo0214.IdPedidoCompraFatura = null;
                fluxo0214.IdPedidoCompraNotaFiscal = null;
                fluxo0214.IdPedidoInterno = null;
                fluxo0214.IdTipoFluxoCaixa = 1;
                fluxo0214.IdUsuarioBeneficiario = null;
                fluxo0214.IdUsuarioInformouPagamento = null;
                fluxo0214.NomeCliente = fluxo0210.NomeCliente;
                fluxo0214.NumeroNotaFiscalFaturamento = null;
                fluxo0214.NumeroNotaFiscalPedidoCompra = null;
                fluxo0214.PagamentoEfetuado = false;
                fluxo0214.Valor = impostoArt30;

                _context.FluxoCaixa.Add(fluxo0214);
                await _context.SaveChangesAsync();

                #endregion
            }

        }

        public List<Obra_AjusteETOResultDTO> ObtemETOsAjuste()
        {
            var result = new List<Obra_AjusteETOResultDTO>();

            _context.Obra.Where(x => !x.Cancelada && x.Finalizada == false).Select(x => new { x.Id, x.Codigo, x.Cliente.NomeFantasia, x.Bloqueada, x.ETO, x.Gasto }).ToList().ForEach(x =>
            {
                var obra = new Obra_AjusteETOResultDTO();
                obra.IdObra = x.Id;
                obra.Cliente = x.NomeFantasia;
                obra.CodigoObra = x.Codigo;
                obra.Status = x.Bloqueada ? "Bloqueada" : "";
                obra.Custo = x.ETO;
                obra.Gasto = x.Gasto;
                obra.Saldo = obra.Custo - obra.Gasto;
                obra.ETO = new List<Tuple<DateTime, double>>();

                result.Add(obra);
            });

            result.ForEach(obra =>
            {
                _context.FluxoCaixa.Where(x => x.IdObra == obra.IdObra && x.CodigoDef == "03.03").Select(x => new { x.DataPagamento, x.Valor }).OrderBy(x => x.DataPagamento).ToList().ForEach(x =>
                {
                    obra.ETO.Add(new Tuple<DateTime, double>(x.DataPagamento, x.Valor));
                });
            });

            return result;
        }

        public async Task RetiraValorDef0302(Int64 idPedidoCompra, double valor)
        {
            var saldoTotal = valor;

            var fluxosCaixa = _context.FluxoCaixa.Where(x => x.IdPedidoCompra == idPedidoCompra && x.CodigoDef == "03.02").ToList();

            fluxosCaixa.OrderBy(x => x.DataPagamento).ToList().ForEach(fluxo =>
            {
                if (saldoTotal > 0)
                {
                    var saldoDoFluxo = fluxo.Valor;

                    if (saldoDoFluxo > 0)
                    {
                        //Ainda tem saldo a ser descontado

                        if (saldoDoFluxo > saldoTotal)
                        {
                            //Saldo do eto é maior que o saldo do pedido
                            fluxo.Valor -= saldoTotal;
                            saldoTotal = 0;
                        }
                        else
                        {
                            //Saldo do eto é menor que o saldo do pedido
                            saldoTotal = saldoTotal - saldoDoFluxo;
                            fluxo.Valor = 0;
                        }

                        _context.Entry(fluxo).State = EntityState.Modified;
                        
                    }
                }
            });

            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdPedidoCompra = {idPedidoCompra} AND CodigoDef = '03.02' AND Valor = 0");
        }

        public async Task RetiraValorDef0303(Int64 idObra, double valor)
        {
            var obra = await _context.Obra.Include(x => x.ETOs).FirstOrDefaultAsync(x => x.Id == idObra);

            var saldoTotal = valor;

            obra.ETOs.OrderBy(x => x.Data).ToList().ForEach(eto =>
            {
                if (saldoTotal > 0)
                {
                    var saldoDoETO = eto.Valor - eto.ValorConsumido;

                    if (saldoDoETO > 0)
                    {
                        //Ainda tem saldo a ser descontado

                        if (saldoDoETO > saldoTotal)
                        {
                            //Saldo do eto é maior que o saldo do pedido
                            eto.ValorConsumido = eto.ValorConsumido + saldoTotal;
                            saldoTotal = 0;
                        }
                        else
                        {
                            //Saldo do eto é menor que o saldo do pedido
                            saldoTotal = saldoTotal - saldoDoETO;
                            eto.ValorConsumido = eto.Valor;
                        }

                        _context.Entry(eto).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            });

            if (saldoTotal > 0)
            {
                var ultimoETO = obra.ETOs.OrderByDescending(e => e.Data).First();
                ultimoETO.ValorConsumido += saldoTotal;
            }

            saldoTotal = valor;

            var fluxosCaixa = _context.FluxoCaixa.Where(x => x.IdObra == idObra && x.CodigoDef == "03.03").ToList();

            fluxosCaixa.OrderBy(x => x.DataPagamento).ToList().ForEach(fluxo =>
            {
                if (saldoTotal > 0)
                {
                    var saldoDoFluxo = fluxo.Valor;

                    if (saldoDoFluxo > 0)
                    {
                        //Ainda tem saldo a ser descontado

                        if (saldoDoFluxo > saldoTotal)
                        {
                            //Saldo do eto é maior que o saldo do pedido
                            fluxo.Valor -= saldoTotal;
                            saldoTotal = 0;
                        }
                        else
                        {
                            //Saldo do eto é menor que o saldo do pedido
                            saldoTotal = saldoTotal - saldoDoFluxo;
                            fluxo.Valor = 0;
                        }

                        _context.Entry(fluxo).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            });

            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE IdObra = {idObra} AND CodigoDef = '03.03' AND Valor = 0");
        }

        public void AtualizaPrevisoesETO(Obra_AjusteETODTO previsao, Int64 IdUsuario)
        {
            var obra = _context.Obra.FirstOrDefault(x => x.Codigo == previsao.CodigoObra);

            _context.Database.ExecuteSqlRaw($"DELETE FROM FluxoCaixa WHERE IdObra = {obra.Id} AND CodigoDef = '03.03'");
            _context.Database.ExecuteSqlRaw($"UPDATE Obra_ETO SET Valor = ValorConsumido WHERE IdObra = {obra.Id} AND ValorConsumido < Valor AND ValorConsumido > 0");
            _context.Database.ExecuteSqlRaw($"DELETE FROM Obra_ETO WHERE IdObra = {obra.Id} AND ValorConsumido = 0");

            var def0303 = _context.DEF.FirstOrDefault(x => x.Codigo == "03.03");

            previsao.Valores.OrderBy(x=>x.Item1).ToList().ForEach(x =>
            {
                var controleCusto = new Modelos.ObraETO();
                controleCusto.DataCadastro = DateTime.Now;
                controleCusto.Data = x.Item1;
                controleCusto.DataUltimaAlteracao = DateTime.Now;
                controleCusto.Id = 0;
                controleCusto.IdObra = obra.Id;
                controleCusto.IdUsuarioAlteracao = IdUsuario;
                controleCusto.IdUsuarioCadastro = IdUsuario;
                controleCusto.ValorConsumido = 0;
                controleCusto.Valor = x.Item2;

                _context.ObraControleCusto.Add(controleCusto);
                _context.SaveChanges();

                var fluxo0303 = new FluxoCaixa();
                fluxo0303.CodigoDef = def0303.Codigo;
                fluxo0303.CodigoFatura = null;
                fluxo0303.CodigoObra = obra.Codigo;
                fluxo0303.CodigoPedidoCompra = null;
                fluxo0303.CodigoPedidoInterno = null;
                fluxo0303.DataLancamento = DateTime.Now;
                fluxo0303.DataPagamento = x.Item1;
                fluxo0303.DataPagamentoEfetuado = null;
                fluxo0303.Id = 0;
                fluxo0303.IdCliente = obra.IdCliente;
                fluxo0303.IdDef = def0303.Id;
                fluxo0303.IdFaturamento = null;
                fluxo0303.IdFornecedorBeneficiario = null;
                fluxo0303.IdObra = obra.Id;
                fluxo0303.IdPedidoCompra = null;
                fluxo0303.IdPedidoCompraFatura = null;
                fluxo0303.IdPedidoCompraNotaFiscal = null;
                fluxo0303.IdPedidoInterno = null;
                fluxo0303.IdTipoFluxoCaixa = 2;
                fluxo0303.IdUsuarioBeneficiario = null;
                fluxo0303.IdUsuarioInformouPagamento = null;
                fluxo0303.NomeCliente = "";
                fluxo0303.NumeroNotaFiscalFaturamento = null;
                fluxo0303.NumeroNotaFiscalPedidoCompra = null;
                fluxo0303.PagamentoEfetuado = false;
                fluxo0303.Valor = x.Item2;
                _context.FluxoCaixa.Add(fluxo0303);
                _context.SaveChanges();
            });
        }

        public void SalvaAjuste(ObraAjusteDTO ajusteDTO)
        {
            var novoAjuste = new ObraAjuste();
            novoAjuste.DataCadastro = ajusteDTO.DataCadastro;
            novoAjuste.Id = 0;
            novoAjuste.IdUsuarioCadastro = ajusteDTO.IdUsuarioCadastro;
            novoAjuste.NomeLogicoAntes = ajusteDTO.NomeLogicoAntes;
            novoAjuste.NomeLogicoDepois = ajusteDTO.NomeLogicoDepois;

            _context.Database.ExecuteSqlRaw($"INSERT INTO Obra_Ajuste VALUES ('{novoAjuste.NomeLogicoAntes}', '{novoAjuste.NomeLogicoDepois}', {novoAjuste.IdUsuarioCadastro}, GETDATE())");
        }

        public async Task<List<ObraAjusteDTO>> ObtemAjustes()
        {
            return _mapper.Map<List<ObraAjusteDTO>>(await _context.ObraAjuste.AsNoTracking().Include(x=>x.Usuario).ToListAsync());
        }

        public async Task<string> GetFile(Int64 id, int tipo)
        {
            if (tipo == 0)
                return (await _context.ObraAjuste.FirstOrDefaultAsync(x => x.Id == id))?.NomeLogicoAntes ?? "";
            else
                return (await _context.ObraAjuste.FirstOrDefaultAsync(x => x.Id == id))?.NomeLogicoDepois ?? "";
        }

        public async Task Cancelar(Int64 idObra)
        {
            //Cancela a obra
            await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra SET Cancelada = 1, Bloqueada = 1, Finalizada = 1 WHERE Id = {idObra}");

            //Cancela todos as solicitações de compra que nao estão finalizadas ou canceladas
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 5 WHERE IdCentroCustoObra = {idObra} AND IdStatusSolicitacaoCompra NOT IN (4, 5, 6)");

            //Cancela todos os pedidos de compra que nao estão finalizados ou cancelados
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra SET IdStatusPedidoCompra = 3 WHERE IdCentroCustoObra = {idObra} AND IdStatusPedidoCompra = 1");

            //Cancela todos os pedidos internos
            //await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoInterno SET Cancelado = 1 WHERE IdObra = {idObra}");

            //Cancela todas as notas fiscais
            //await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET Cancelada = 1 WHERE IdPedidoCompra IN (SELECT PC.Id FROM PedidoCompra PC WHERE PC.IdCentroCustoObra = {idObra})");

            //Cancela todos os fluxos de caixa
            await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Cancelado = 1 WHERE IdObra = {idObra}");

        }
    }
}
