using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.Dominio.Modelos.Genericos.Faturamento;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioFaturamento
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;
        private RepositorioObra _repositorioObra;

        public RepositorioFaturamento(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
            _repositorioObra = new RepositorioObra(_context, _mapper);
        }

        public async Task<List<FaturamentoDTO>> Get()
        {
            return _mapper.Map<List<FaturamentoDTO>>(
                await _context.Faturamento.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.Obra)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Status)
                .ToListAsync());
        }

        public async Task<Faturamento_FiltradoReturnDTO> GetFiltrado(FaturamentoRequestDTO parametros)
        {
            var result = new Faturamento_FiltradoReturnDTO();
            result.MedicoesDTO = new List<ObraFaturamentoDTO>();
            result.FaturamentosDTO = new List<FaturamentoDTO>();

            var query = _context.Faturamento.AsQueryable();

            query = query.Where(x => x.Obra.Cancelada == false);

            if (parametros.IdObra.HasValue)
                query = query.Where(x => x.IdObra == parametros.IdObra);

            if (parametros.IdCliente.HasValue)
                query = query.Where(x => x.Obra.IdCliente == parametros.IdCliente);

            if (parametros.IdStatusFaturamento > 0)
                query = query.Where(x => x.IdStatusFaturamento == parametros.IdStatusFaturamento);

            if (!string.IsNullOrEmpty(parametros.NumeroNotaFiscal))
                query = query.Where(x => x.NumeroNF.Contains(parametros.NumeroNotaFiscal));

            if (!string.IsNullOrEmpty(parametros.Observacao))
                query = query.Where(x => x.Observacao.Contains(parametros.Observacao));

            if (parametros.DataFaturamentoInicial.HasValue)
                query = query.Where(x => x.DataFaturamento >= parametros.DataFaturamentoInicial.Value.Date);

            if (parametros.DataFaturamentoFinal.HasValue)
                query = query.Where(x => x.DataFaturamento <= parametros.DataFaturamentoFinal.Value.Date);

            if (parametros.DataRecebimentoInicial.HasValue)
                query = query.Where(x => x.DataRecebimentoPrevisto.Date >= parametros.DataRecebimentoInicial.Value.Date);

            if (parametros.DataRecebimentoFinal.HasValue)
                query = query.Where(x => x.DataRecebimentoPrevisto.Date <= parametros.DataRecebimentoFinal.Value.Date);

            query = query.AsNoTracking();

            query = query.Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.Obra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Status);

            result.FaturamentosDTO = _mapper.Map<List<FaturamentoDTO>>(await query.ToListAsync());

            if (parametros.IdStatusFaturamento <= 0)
            {
                //Obtem a lista de medições previstas

                var queryMedicoes = _context.ObraMedicao.AsQueryable();

                queryMedicoes = queryMedicoes.Where(x => x.Obra.Cancelada == false);

                if (parametros.IdObra.HasValue)
                    queryMedicoes = queryMedicoes.Where(x => x.IdObra == parametros.IdObra);

                if (parametros.IdCliente.HasValue)
                    queryMedicoes = queryMedicoes.Where(x => x.Obra.IdCliente == parametros.IdCliente);

                if (parametros.DataFaturamentoInicial.HasValue)
                    queryMedicoes = queryMedicoes.Where(x => x.DataPrevistaRecebimento.Date >= parametros.DataFaturamentoInicial.Value.Date);

                if (parametros.DataFaturamentoFinal.HasValue)
                    queryMedicoes = queryMedicoes.Where(x => x.DataPrevistaRecebimento.Date <= parametros.DataFaturamentoFinal.Value.Date);

                result.MedicoesDTO = _mapper.Map<List<ObraFaturamentoDTO>>(await queryMedicoes.AsNoTracking().Include(x => x.Obra).ToListAsync());
                result.SomaAFaturar = result.MedicoesDTO.Sum(x => x.Valor);

                var idsObras = result.MedicoesDTO.Select(x => x.IdObra).Distinct().ToList();
                var obrasDTO = _mapper.Map<List<ObraDTO>>(_context.Obra.Include(x => x.Cliente).Where(x => idsObras.Contains(x.Id))).ToList();

                result.MedicoesDTO.ForEach(x =>
                {
                    x.ObraDaMedicao = obrasDTO.FirstOrDefault(y => y.Id == x.IdObra);
                });

                var def020901 = _context.DEF.FirstOrDefault(x => x.Codigo == "02.09.01");
                result.SinalRecebido = _context.FluxoCaixa.Where(x => x.IdDef == def020901.Id && idsObras.Contains(x.IdObra.Value)).Sum(x=>x.Valor);
                result.SinalDescontado = _context.Faturamento.Where(x => idsObras.Contains(x.IdObra)).Sum(x => x.ValorSinal);
                result.SaldoSinal = result.SinalRecebido - result.SinalDescontado;

                result.ValorNaoComissionado = _context.Obra.Where(x => idsObras.Contains(x.Id)).Sum(x => x.ValorNaoComissionado);
                result.ValorNaoComissionadoUtilizado= _context.Faturamento.Where(x => idsObras.Contains(x.IdObra)).Sum(x => x.ValorNaoComissionado);
                result.SaldoValorNaoComissionado = result.ValorNaoComissionado - result.ValorNaoComissionadoUtilizado;
            }

            result.SomaFaturadoAReceber = result.FaturamentosDTO.Where(x => x.IdStatusFaturamento == 2).Sum(x => x.ValorBruto);
            result.SomaFaturadoAReceberLiquido = result.FaturamentosDTO.Where(x => x.IdStatusFaturamento == 2).Sum(x => x.ValorLiquido);
            result.SomaFaturadoRecebido = result.FaturamentosDTO.Where(x => x.IdStatusFaturamento == 1).Sum(x => x.ValorBruto);

            return result;
        }

        public async Task CancelarFaturamento(Faturamento_CancelarFaturamentoDTO parametros)
        {
            var def0210 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10"); //A FATURAR
            var def0211 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.11"); //INSS
            var def0212 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.12"); //IR
            var def0213 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.13"); //ISS
            var def0214 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.14"); //ART30

            var faturamento = await _context.Faturamento.FirstOrDefaultAsync(x => x.Id == parametros.idFaturamento);
            faturamento.IdStatusFaturamento = 3;
            _context.Entry(faturamento).State = EntityState.Modified;

            var obra = await _context.Obra.AsNoTracking().Include(x=>x.Cliente).FirstOrDefaultAsync(x => x.Id == faturamento.IdObra);

            //Obtem os fluxos a faturar daquela obra naquela data
            var fluxosAFaturar = await _context.FluxoCaixa.Include(x=>x.FluxosCaixaFilhos).Where(x => x.IdObra == faturamento.IdObra && x.IdDef == def0210.Id && x.DataLancamento.Date == parametros.dataRetornoFaturamento.Date).ToListAsync();

            //Compoe o novo valor a faturar, que é o valor do faturamento cancelado mais os valores dos possiveis defs
            var valorParaAFaturar = faturamento.ValorBruto + fluxosAFaturar.Sum(x => x.Valor);
            
            //Limpar os dados do fluxo de caixa pois o faturamento foi cancelado (isso exclui tb os impostos)
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM FluxoCaixa WHERE IdFaturamento = {faturamento.Id}");

            fluxosAFaturar.ForEach(x =>
            {
                x.FluxosCaixaFilhos.ToList().ForEach(y =>
                {
                    _context.Entry(y).State = EntityState.Deleted;
                });

                _context.Entry(x).State = EntityState.Deleted;
            });

            _context.SaveChanges();

            var impostoArt30 = (valorParaAFaturar * obra.AliquotaImpostoArt30 ?? 0) / 100;
            var impostoINSS = (valorParaAFaturar * obra.AliquotaImpostoINSS ?? 0) / 100;
            var impostoIR = (valorParaAFaturar * obra.AliquotaImpostoIR ?? 0) / 100;
            var impostoISS = (valorParaAFaturar * obra.AliquotaImpostoISS) / 100;

            #region 02.10 - A faturar 

            var fluxo0210 = new FluxoCaixa();
            fluxo0210.CodigoDef = def0210.Codigo;
            fluxo0210.CodigoFatura = null;
            fluxo0210.CodigoObra = obra.Codigo;
            fluxo0210.CodigoPedidoCompra = null;
            fluxo0210.CodigoPedidoInterno = null;
            fluxo0210.DataLancamento = parametros.dataRetornoFaturamento;
            fluxo0210.DataPagamento = parametros.dataRetornoFaturamento.AddDays(obra.DiasDePagamento);
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
            fluxo0210.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
            fluxo0210.NumeroNotaFiscalFaturamento = null;
            fluxo0210.NumeroNotaFiscalPedidoCompra = null;
            fluxo0210.PagamentoEfetuado = false;
            fluxo0210.Valor = valorParaAFaturar;
            _context.FluxoCaixa.Add(fluxo0210);
            _context.SaveChanges();

            #endregion

            #region 02.11 - INSS 

            var fluxo0211 = new FluxoCaixa();
            fluxo0211.CodigoDef = def0211.Codigo;
            fluxo0211.CodigoFatura = null;
            fluxo0211.CodigoObra = obra.Codigo;
            fluxo0211.CodigoPedidoCompra = null;
            fluxo0211.CodigoPedidoInterno = null;
            fluxo0211.DataLancamento = parametros.dataRetornoFaturamento;
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
            fluxo0211.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
            fluxo0211.NumeroNotaFiscalFaturamento = null;
            fluxo0211.NumeroNotaFiscalPedidoCompra = null;
            fluxo0211.PagamentoEfetuado = false;
            fluxo0211.Valor = impostoINSS;
            _context.FluxoCaixa.Add(fluxo0211);
            _context.SaveChanges();

            #endregion

            #region 02.12 - IR

            var fluxo0212 = new FluxoCaixa();
            fluxo0212.CodigoDef = def0212.Codigo;
            fluxo0212.CodigoFatura = null;
            fluxo0212.CodigoObra = obra.Codigo;
            fluxo0212.CodigoPedidoCompra = null;
            fluxo0212.CodigoPedidoInterno = null;
            fluxo0212.DataLancamento = parametros.dataRetornoFaturamento;
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
            fluxo0212.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
            fluxo0212.NumeroNotaFiscalFaturamento = null;
            fluxo0212.NumeroNotaFiscalPedidoCompra = null;
            fluxo0212.PagamentoEfetuado = false;
            fluxo0212.Valor = impostoIR;
            _context.FluxoCaixa.Add(fluxo0212);
            _context.SaveChanges();

            #endregion

            #region 02.13 - ISS

            var fluxo0213 = new FluxoCaixa();
            fluxo0213.CodigoDef = def0213.Codigo;
            fluxo0213.CodigoFatura = null;
            fluxo0213.CodigoObra = obra.Codigo;
            fluxo0213.CodigoPedidoCompra = null;
            fluxo0213.CodigoPedidoInterno = null;
            fluxo0213.DataLancamento = parametros.dataRetornoFaturamento;
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
            fluxo0213.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
            fluxo0213.NumeroNotaFiscalFaturamento = null;
            fluxo0213.NumeroNotaFiscalPedidoCompra = null;
            fluxo0213.PagamentoEfetuado = false;
            fluxo0213.Valor = impostoISS;
            _context.FluxoCaixa.Add(fluxo0213);
            _context.SaveChanges();

            #endregion

            #region 02.14 - ART30

            var fluxo0214 = new FluxoCaixa();
            fluxo0214.CodigoDef = def0214.Codigo;
            fluxo0214.CodigoFatura = null;
            fluxo0214.CodigoObra = obra.Codigo;
            fluxo0214.CodigoPedidoCompra = null;
            fluxo0214.CodigoPedidoInterno = null;
            fluxo0214.DataLancamento = parametros.dataRetornoFaturamento;
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
            fluxo0214.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
            fluxo0214.NumeroNotaFiscalFaturamento = null;
            fluxo0214.NumeroNotaFiscalPedidoCompra = null;
            fluxo0214.PagamentoEfetuado = false;
            fluxo0214.Valor = impostoArt30;
            _context.FluxoCaixa.Add(fluxo0214);
            _context.SaveChanges();

            #endregion
        }

        public async Task<List<Faturamento_ValoresAFaturarDTO>> ObtemValoresAFaturar(FaturamentoRequestDTO parametros)
        {
            var def = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");

            var query = _context.FluxoCaixa
                .Include(x => x.Obra)
                .ThenInclude(Obra => Obra.Cliente)
                .Include(x => x.FluxosCaixaFilhos)
                .AsQueryable();

            if (parametros.IdObra.HasValue)
                query = query.Where(x => x.IdObra == parametros.IdObra);

            if(parametros.DataFaturamentoInicial.HasValue)
                query = query.Where(x => x.DataPagamento.Date >= parametros.DataFaturamentoInicial.Value.Date);

            if(parametros.DataFaturamentoFinal.HasValue)
                query = query.Where(x => x.DataPagamento.Date <= parametros.DataFaturamentoFinal.Value.Date);

            query = query
                .Where(fc => fc.IdDef == def.Id)
                .OrderBy(x=>x.DataPagamento);

            return await query.AsNoTracking().Select(fc => new Faturamento_ValoresAFaturarDTO
            {
                Id = fc.Id,
                IdObra = fc.IdObra ?? 0,
                Codigo = fc.Obra.Codigo,
                Descricao = fc.Obra.Descricao,
                CNPJ = fc.Obra.Cliente.CNPJ,
                RazaoSocial = fc.Obra.Cliente.RazaoSocial,
                Valor = fc.Valor,
                DataLancamento = fc.DataLancamento,
                DataPagamento = fc.DataPagamento,
                INSS = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.11").Sum(x => x.Valor),
                IR = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.12").Sum(x => x.Valor),
                ISS = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.13").Sum(x => x.Valor),
                ART30 = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.14").Sum(x => x.Valor),
                ValorLiquido = fc.Valor - fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.11" || x.CodigoDef == "02.12" || x.CodigoDef == "02.13" || x.CodigoDef == "02.14").Sum(x => x.Valor)
            }).ToListAsync();
        }

        public async Task<List<Faturamento_ValoresAFaturarDTO>> ObtemValoresAFaturar(Int64 idObra)
        {
            var def = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");

            var query = _context.FluxoCaixa
                .Include(x => x.Obra)
                .ThenInclude(Obra => Obra.Cliente)
                .Include(x => x.FluxosCaixaFilhos)
                .Where(fc => fc.IdDef == def.Id && fc.IdObra==idObra)
                .Select(fc => new Faturamento_ValoresAFaturarDTO
                {
                    Id = fc.Id,
                    IdObra = fc.IdObra ?? 0,
                    Codigo = fc.Obra.Codigo,
                    Descricao = fc.Obra.Descricao,
                    CNPJ = fc.Obra.Cliente.CNPJ,
                    RazaoSocial = fc.Obra.Cliente.RazaoSocial,
                    Valor = fc.Valor,
                    DataLancamento = fc.DataLancamento,
                    DataPagamento = fc.DataPagamento,
                    INSS = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.11").Sum(x => x.Valor),
                    IR = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.12").Sum(x => x.Valor),
                    ISS = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.13").Sum(x => x.Valor),
                    ART30 = fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.14").Sum(x => x.Valor),
                    ValorLiquido = fc.Valor - fc.FluxosCaixaFilhos.Where(x => x.CodigoDef == "02.11" || x.CodigoDef == "02.12" || x.CodigoDef == "02.13" || x.CodigoDef == "02.14").Sum(x => x.Valor)
                });

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AjustaFaturamento(Faturamento_AjusteFaturamentoDTO faturamentos)
        {
            var def0210 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");
            var def0211 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.11"); //INSS
            var def0212 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.12"); //IR
            var def0213 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.13"); //ISS
            var def0214 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.14"); //ART30

            var obra = await _context.Obra.AsNoTracking().Include(x => x.Cliente).FirstOrDefaultAsync(x => x.Id == faturamentos.IdObra);

            _context.FluxoCaixa.Where(x => x.IdObra == faturamentos.IdObra && x.IdDef == def0210.Id).Include(x => x.FluxosCaixaFilhos).ToList().ForEach(fluxo =>
            {
                fluxo.FluxosCaixaFilhos.ToList().ForEach(filho =>
                {
                    _context.FluxoCaixa.Remove(filho);
                });
                _context.SaveChanges();

                _context.FluxoCaixa.Remove(fluxo);
                _context.SaveChanges();
            });

            faturamentos.DatasValores.ForEach(faturamento =>
            {
                var impostoArt30 = (faturamento.Valor * obra.AliquotaImpostoArt30 ?? 0) / 100;
                var impostoINSS = (faturamento.Valor * obra.AliquotaImpostoINSS ?? 0) / 100;
                var impostoIR = (faturamento.Valor * obra.AliquotaImpostoIR ?? 0) / 100;
                var impostoISS = (faturamento.Valor * obra.AliquotaImpostoISS) / 100;

                #region 02.10 - A faturar 

                var fluxo0210 = new FluxoCaixa();
                fluxo0210.CodigoDef = def0210.Codigo;
                fluxo0210.CodigoFatura = null;
                fluxo0210.CodigoObra = obra.Codigo;
                fluxo0210.CodigoPedidoCompra = null;
                fluxo0210.CodigoPedidoInterno = null;
                fluxo0210.DataLancamento = faturamento.DataFaturamento;
                fluxo0210.DataPagamento = faturamento.DataFaturamento.AddDays(obra.DiasDePagamento);
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
                fluxo0210.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
                fluxo0210.NumeroNotaFiscalFaturamento = null;
                fluxo0210.NumeroNotaFiscalPedidoCompra = null;
                fluxo0210.PagamentoEfetuado = false;
                fluxo0210.Valor = faturamento.Valor;
                _context.FluxoCaixa.Add(fluxo0210);
                _context.SaveChanges();

                #endregion

                #region 02.11 - INSS 

                var fluxo0211 = new FluxoCaixa();
                fluxo0211.CodigoDef = def0211.Codigo;
                fluxo0211.CodigoFatura = null;
                fluxo0211.CodigoObra = obra.Codigo;
                fluxo0211.CodigoPedidoCompra = null;
                fluxo0211.CodigoPedidoInterno = null;
                fluxo0211.DataLancamento = faturamento.DataFaturamento;
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
                fluxo0211.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
                fluxo0211.NumeroNotaFiscalFaturamento = null;
                fluxo0211.NumeroNotaFiscalPedidoCompra = null;
                fluxo0211.PagamentoEfetuado = false;
                fluxo0211.Valor = impostoINSS;
                _context.FluxoCaixa.Add(fluxo0211);
                _context.SaveChanges();

                #endregion

                #region 02.12 - IR

                var fluxo0212 = new FluxoCaixa();
                fluxo0212.CodigoDef = def0212.Codigo;
                fluxo0212.CodigoFatura = null;
                fluxo0212.CodigoObra = obra.Codigo;
                fluxo0212.CodigoPedidoCompra = null;
                fluxo0212.CodigoPedidoInterno = null;
                fluxo0212.DataLancamento = faturamento.DataFaturamento;
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
                fluxo0212.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
                fluxo0212.NumeroNotaFiscalFaturamento = null;
                fluxo0212.NumeroNotaFiscalPedidoCompra = null;
                fluxo0212.PagamentoEfetuado = false;
                fluxo0212.Valor = impostoIR;
                _context.FluxoCaixa.Add(fluxo0212);
                _context.SaveChanges();

                #endregion

                #region 02.13 - ISS

                var fluxo0213 = new FluxoCaixa();
                fluxo0213.CodigoDef = def0213.Codigo;
                fluxo0213.CodigoFatura = null;
                fluxo0213.CodigoObra = obra.Codigo;
                fluxo0213.CodigoPedidoCompra = null;
                fluxo0213.CodigoPedidoInterno = null;
                fluxo0213.DataLancamento = faturamento.DataFaturamento;
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
                fluxo0213.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
                fluxo0213.NumeroNotaFiscalFaturamento = null;
                fluxo0213.NumeroNotaFiscalPedidoCompra = null;
                fluxo0213.PagamentoEfetuado = false;
                fluxo0213.Valor = impostoISS;
                _context.FluxoCaixa.Add(fluxo0213);
                _context.SaveChanges();

                #endregion

                #region 02.14 - ART30

                var fluxo0214 = new FluxoCaixa();
                fluxo0214.CodigoDef = def0214.Codigo;
                fluxo0214.CodigoFatura = null;
                fluxo0214.CodigoObra = obra.Codigo;
                fluxo0214.CodigoPedidoCompra = null;
                fluxo0214.CodigoPedidoInterno = null;
                fluxo0214.DataLancamento = faturamento.DataFaturamento;
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
                fluxo0214.NomeCliente = obra.Cliente?.NomeFantasia ?? "";
                fluxo0214.NumeroNotaFiscalFaturamento = null;
                fluxo0214.NumeroNotaFiscalPedidoCompra = null;
                fluxo0214.PagamentoEfetuado = false;
                fluxo0214.Valor = impostoArt30;
                _context.FluxoCaixa.Add(fluxo0214);
                _context.SaveChanges();

                #endregion
            });
        }

        public async Task<FaturamentoDTO> Get(Int64 Id)
        {
            return _mapper.Map<FaturamentoDTO>(await _context.Faturamento.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.Obra)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<List<FaturamentoDTO>> GetObra(Int64 idObra)
        {
            return _mapper.Map<List<FaturamentoDTO>>(await _context.Faturamento.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.Obra)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Status)
                .Where(x => x.IdObra == idObra)
                .ToListAsync());
        }

        public async Task<Faturamento_DetalheClienteDTO> GetClienteDetalhe(Int64 idCliente)
        {
            var result = new Faturamento_DetalheClienteDTO();
            result.NumeroObrasAtivas = await _context.Obra.CountAsync(x => x.IdCliente == idCliente && x.Bloqueada);
            result.ValorTotalObrasAtivas = await _context.Obra.Where(x => x.IdCliente == idCliente && x.Bloqueada).SumAsync(x => x.ValorTotal);
            result.ValorFaturadoRecebidoObrasAtivas = await _context.Faturamento.Where(x => x.Obra.IdCliente == idCliente && x.IdStatusFaturamento == 1).SumAsync(x => x.ValorBruto);
            result.ValorFaturadoAReceberObrasAtivas = await _context.Faturamento.Where(x => x.Obra.IdCliente == idCliente && x.IdStatusFaturamento == 2).SumAsync(x => x.ValorBruto);
            result.ValorAFaturarObrasAtivas = result.ValorTotalObrasAtivas - result.ValorFaturadoAReceberObrasAtivas - result.ValorFaturadoRecebidoObrasAtivas;

            return result;
        }

        public async Task<FaturamentoDTO> Post(FaturamentoDTO faturamentoDTO)
        {
            var def = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.09");
            var obra = _context.Obra.Include(x => x.Cliente).Include(x => x.Faturamentos).AsNoTracking().FirstOrDefault(x => x.Id == faturamentoDTO.IdObra);

            var faturamento = new Faturamento();
            faturamento.DataCadastro = faturamentoDTO.DataCadastro;
            faturamento.DataFaturamento = faturamentoDTO.DataFaturamento;
            faturamento.DataRecebimentoPrevisto = faturamentoDTO.DataRecebimentoPrevisto;
            faturamento.DataRecebimentoRealizado = faturamentoDTO.DataRecebimentoRealizado;
            faturamento.DataUltimaAlteracao = faturamentoDTO.DataUltimaAlteracao;
            faturamento.Id = 0;
            faturamento.IdObra = faturamentoDTO.IdObra;
            faturamento.IdStatusFaturamento = faturamentoDTO.IdStatusFaturamento;
            faturamento.IdUsuarioAlteracao = faturamentoDTO.IdUsuarioAlteracao;
            faturamento.IdUsuarioCadastro = faturamentoDTO.IdUsuarioCadastro;
            faturamento.NumeroNF = faturamentoDTO.NumeroNF;
            faturamento.Observacao = faturamentoDTO.Observacao;
            faturamento.ValorArt30 = faturamentoDTO.ValorArt30;
            faturamento.ValorBruto = faturamentoDTO.ValorBruto;
            faturamento.ValorDesconto = faturamentoDTO.ValorDesconto;
            faturamento.ValorMaterial = faturamentoDTO.ValorMaterial;
            faturamento.ValorINSS = faturamentoDTO.ValorINSS;
            faturamento.ValorIR = faturamentoDTO.ValorIR;
            faturamento.ValorISS = faturamentoDTO.ValorISS;
            faturamento.ValorLiquido = faturamentoDTO.ValorLiquido;
            faturamento.ValorSinal = faturamentoDTO.ValorSinal;
            faturamento.ValorRecebido = 0;
            faturamento.ValorNaoComissionado = faturamentoDTO.ValorNaoComissionado;

            await _context.Faturamento.AddAsync(faturamento);
            await _context.SaveChangesAsync();

            //Valor Liquido
            var fluxo0209 = _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
            {
                CodigoDef = def.Codigo,
                CodigoObra = obra?.Codigo,
                CodigoPedidoCompra = null,
                CodigoPedidoInterno = null,
                DataLancamento = faturamento.DataFaturamento,
                DataPagamento = faturamento.DataRecebimentoPrevisto,
                Id = 0,
                IdCliente = obra?.IdCliente,
                IdDef = def.Id,
                IdObra = faturamento.IdObra,
                IdPedidoCompra = null,
                IdPedidoCompraFatura = null,
                IdPedidoCompraNotaFiscal = null,
                IdPedidoInterno = null,
                NomeCliente = obra?.Cliente?.NomeFantasia,
                NumeroNotaFiscalFaturamento = faturamento.NumeroNF,
                Valor = faturamento.ValorLiquido,
                IdFaturamento = faturamento.Id,
                IdTipoFluxoCaixa = 1,
                PagamentoEfetuado = false,
                CodigoFatura = faturamento.NumeroNF,
            }).Result;

            //Art30
            var defArt30 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.03");
            _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
            {
                CodigoDef = defArt30.Codigo,
                CodigoObra = obra?.Codigo,
                CodigoPedidoCompra = null,
                CodigoPedidoInterno = null,
                DataLancamento = faturamento.DataFaturamento,
                DataPagamento = faturamento.DataRecebimentoPrevisto,
                Id = 0,
                IdCliente = obra?.IdCliente,
                IdDef = defArt30.Id,
                IdObra = faturamento.IdObra,
                IdPedidoCompra = null,
                IdPedidoCompraFatura = null,
                IdPedidoCompraNotaFiscal = null,
                IdPedidoInterno = null,
                NomeCliente = obra?.Cliente?.NomeFantasia,
                NumeroNotaFiscalFaturamento = faturamento.NumeroNF,
                Valor = faturamento.ValorArt30,
                IdFaturamento = faturamento.Id,
                IdTipoFluxoCaixa = 1,
                PagamentoEfetuado = false,
                CodigoFatura = faturamento.NumeroNF,
                IdFluxoCaixaPai= fluxo0209.Id
            }).Wait();

            //INSS
            var defINSS = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.01");
            _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
            {
                CodigoDef = defINSS.Codigo,
                CodigoObra = obra?.Codigo,
                CodigoPedidoCompra = null,
                CodigoPedidoInterno = null,
                DataLancamento = faturamento.DataFaturamento,
                DataPagamento = faturamento.DataRecebimentoPrevisto,
                Id = 0,
                IdCliente = obra?.IdCliente,
                IdDef = defINSS.Id,
                IdObra = faturamento.IdObra,
                IdPedidoCompra = null,
                IdPedidoCompraFatura = null,
                IdPedidoCompraNotaFiscal = null,
                IdPedidoInterno = null,
                NomeCliente = obra?.Cliente?.NomeFantasia,
                NumeroNotaFiscalFaturamento = faturamento.NumeroNF,
                Valor = faturamento.ValorINSS,
                IdFaturamento = faturamento.Id,
                IdTipoFluxoCaixa = 1,
                PagamentoEfetuado = false,
                CodigoFatura = faturamento.NumeroNF,
                IdFluxoCaixaPai = fluxo0209.Id
            }).Wait();

            //IR
            var defIR = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.02");
            _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
            {
                CodigoDef = defIR.Codigo,
                CodigoObra = obra?.Codigo,
                CodigoPedidoCompra = null,
                CodigoPedidoInterno = null,
                DataLancamento = faturamento.DataFaturamento,
                DataPagamento = faturamento.DataRecebimentoPrevisto,
                Id = 0,
                IdCliente = obra?.IdCliente,
                IdDef = defIR.Id,
                IdObra = faturamento.IdObra,
                IdPedidoCompra = null,
                IdPedidoCompraFatura = null,
                IdPedidoCompraNotaFiscal = null,
                IdPedidoInterno = null,
                NomeCliente = obra?.Cliente?.NomeFantasia,
                NumeroNotaFiscalFaturamento = faturamento.NumeroNF,
                Valor = faturamento.ValorIR,
                IdFaturamento = faturamento.Id,
                IdTipoFluxoCaixa = 1,
                PagamentoEfetuado = false,
                CodigoFatura = faturamento.NumeroNF,
                IdFluxoCaixaPai = fluxo0209.Id
            }).Wait();

            //ISS
            var defISS = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "03.01.04");
            _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
            {
                CodigoDef = defISS.Codigo,
                CodigoObra = obra?.Codigo,
                CodigoPedidoCompra = null,
                CodigoPedidoInterno = null,
                DataLancamento = faturamento.DataFaturamento,
                DataPagamento = faturamento.DataRecebimentoPrevisto,
                Id = 0,
                IdCliente = obra?.IdCliente,
                IdDef = defISS.Id,
                IdObra = faturamento.IdObra,
                IdPedidoCompra = null,
                IdPedidoCompraFatura = null,
                IdPedidoCompraNotaFiscal = null,
                IdPedidoInterno = null,
                NomeCliente = obra?.Cliente?.NomeFantasia,
                NumeroNotaFiscalFaturamento = faturamento.NumeroNF,
                Valor = faturamento.ValorISS,
                IdFaturamento = faturamento.Id,
                IdTipoFluxoCaixa = 1,
                PagamentoEfetuado = false,
                CodigoFatura = faturamento.NumeroNF,
                IdFluxoCaixaPai = fluxo0209.Id
            }).Wait();

            await _repositorioObra.AtualizaMedicoesFluxoCaixa(obra.Id, faturamento.ValorBruto);

            return _mapper.Map<FaturamentoDTO>(faturamento);
        }

        public async Task<FaturamentoDTO> Put(FaturamentoDTO faturamentoDTO)
        {
            var faturamento = await _context.Faturamento.AsNoTracking().FirstOrDefaultAsync(x => x.Id == faturamentoDTO.Id);

            if (faturamento == null)
                throw new Exception("Faturamento não encontrada");

            faturamento.DataRecebimentoPrevisto = faturamentoDTO.DataRecebimentoPrevisto;
            faturamento.DataRecebimentoRealizado = faturamentoDTO.DataRecebimentoRealizado;
            faturamento.DataUltimaAlteracao = faturamentoDTO.DataUltimaAlteracao;
            faturamento.IdStatusFaturamento = faturamentoDTO.IdStatusFaturamento;
            faturamento.IdUsuarioAlteracao = faturamentoDTO.IdUsuarioAlteracao;
            faturamento.NumeroNF = faturamentoDTO.NumeroNF;
            faturamento.Observacao = faturamentoDTO.Observacao;
            faturamento.ValorArt30 = faturamentoDTO.ValorArt30;
            faturamento.ValorBruto = faturamentoDTO.ValorBruto;
            faturamento.ValorDesconto = faturamentoDTO.ValorDesconto;
            faturamento.ValorINSS = faturamentoDTO.ValorINSS;
            faturamento.ValorIR = faturamentoDTO.ValorIR;
            faturamento.ValorISS = faturamentoDTO.ValorISS;
            faturamento.ValorLiquido = faturamentoDTO.ValorLiquido;
            faturamento.ValorSinal = faturamentoDTO.ValorSinal;

            if (faturamentoDTO.IdStatusFaturamento == 1)
            {
                //Faturado Recebido
                var sql = $"UPDATE FluxoCaixa SET DataPagamento = '{faturamentoDTO.DataRecebimentoRealizado?.ToString("MM-dd-yyyy HH:mm:ss")}' WHERE IdFaturamento = {faturamento.Id}";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            else if (faturamentoDTO.IdStatusFaturamento == 2)
            {
                //Faturado a Receber
                var sql = $"UPDATE FluxoCaixa SET IdDef = (SELECT D.Id FROM Def D WHERE D.Codigo = '02.09' ), CodigoDef = '02.09' WHERE IdFaturamento = {faturamento.Id}";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            else if (faturamentoDTO.IdStatusFaturamento == 3)
            {
                //Cancelado
               
            }


            _context.Entry(faturamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<FaturamentoDTO>(faturamento);
        }

        public async Task<FaturamentoArquivosDTO> Upload(FaturamentoArquivosDTO faturamentoArquivosDTO)
        {
            if (_context.Faturamento_Arquivos.Any(x => x.IdFaturamento == faturamentoArquivosDTO.IdFaturamento && x.Nome == faturamentoArquivosDTO.Nome))
                throw new Exception("Já existe um arquivo com este nome");

            var faturamentoArquivos = new FaturamentoArquivos();
            faturamentoArquivos.DataCadastro = faturamentoArquivosDTO.DataCadastro;
            faturamentoArquivos.Extensao = faturamentoArquivosDTO.Extensao;
            faturamentoArquivos.Id = faturamentoArquivosDTO.Id;
            faturamentoArquivos.IdFaturamento = faturamentoArquivosDTO.IdFaturamento;
            faturamentoArquivos.IdUsuarioCadastro = faturamentoArquivosDTO.IdUsuarioCadastro;
            faturamentoArquivos.Nome = faturamentoArquivosDTO.Nome;
            faturamentoArquivos.NomeLogico = faturamentoArquivosDTO.NomeLogico;
            faturamentoArquivos.TamanhoMB = faturamentoArquivosDTO.TamanhoMB;

            await _context.Faturamento_Arquivos.AddAsync(faturamentoArquivos);
            await _context.SaveChangesAsync();

            return _mapper.Map<FaturamentoArquivosDTO>(faturamentoArquivos);
        }

        public async Task<FaturamentoArquivosDTO> GetFile(Int64 id)
        {
            return _mapper.Map<FaturamentoArquivosDTO>(await _context.Faturamento_Arquivos.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            var arquivo = await _context.Faturamento_Arquivos.FirstOrDefaultAsync(x => x.Id == idArquivo);

            if (arquivo == null)
                throw new Exception("Arquivo não encontrado");

            _context.Faturamento_Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FaturamentoArquivosDTO>> ObtemArquivos(Int64 idFaturamento)
        {
            return _mapper.Map<List<FaturamentoArquivosDTO>>(await _context.Faturamento_Arquivos.Include(x => x.UsuarioCadastro).Where(x => x.IdFaturamento == idFaturamento).ToListAsync());
        }

        public async Task AjustaDataFaturamento()
        {
            var dataVerificacao = DateTime.Now.AddDays(-1).Date;

            _context.FluxoCaixa.Where(x=>x.CodigoDef == "02.10" && x.DataLancamento.Date <= dataVerificacao.Date).Include(x => x.FluxosCaixaFilhos).ToList().ForEach(fluxo =>
            {
                var dias = (DateTime.Now - fluxo.DataLancamento.Date).Days;

                fluxo.DataLancamento = fluxo.DataLancamento.AddDays(dias);
                fluxo.DataPagamento = fluxo.DataPagamento.AddDays(dias);

                fluxo.FluxosCaixaFilhos.ToList().ForEach(filho =>
                {
                    filho.DataLancamento = filho.DataLancamento.AddDays(dias);
                    filho.DataPagamento = filho.DataPagamento.AddDays(dias);
                    _context.Entry(filho).State = EntityState.Modified;
                });

                _context.Entry(fluxo).State = EntityState.Modified;
            });
            await _context.SaveChangesAsync();
        }


        public async Task AlteraData(Faturamento_AlteracaoDataDTO parametros)
        {

            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.IdFaturamento == parametros.Id);
            var faturamento = await _context.Faturamento.FirstOrDefaultAsync(x => x.Id == parametros.Id);

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Data faturamento - {faturamento.NumeroNF}";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = null;
            novoHistorico.IdPedidoInterno = null;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = parametros.IdUsuario;
            novoHistorico.Usuario = parametros.NomeUsuario;
            novoHistorico.ValorAntigo = $"{fluxoCaixa.DataPagamento.ToString("dd/MM/yyyy")}";
            novoHistorico.ValorNovo = $"{parametros.Data.ToString("dd/MM/yyyy")}";

            if (parametros.TipoData == 1)
            {
                //Data em que foi faturado (criado)
            }
            else
            {
                //Data de Faturamento (quando vai receber)

                fluxoCaixa.DataPagamento = parametros.Data;

                faturamento.DataRecebimentoPrevisto = parametros.Data;
                faturamento.IdUsuarioAlteracao = parametros.IdUsuario;
                faturamento.DataUltimaAlteracao = DateTime.Now;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                _context.Entry(faturamento).State = EntityState.Modified;

                await _context.Historico.AddAsync(novoHistorico);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AlteraValor(Faturamento_AlteracaoValorDTO parametros)
        {

            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.IdFaturamento == parametros.Id);
            var faturamento = await _context.Faturamento.FirstOrDefaultAsync(x => x.Id == parametros.Id);

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Valor {parametros.Campo} faturamento - {faturamento.NumeroNF}";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = null;
            novoHistorico.IdPedidoInterno = null;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = parametros.IdUsuario;
            novoHistorico.Usuario = parametros.NomeUsuario;
            novoHistorico.ValorNovo = $"{parametros.Valor}";

            if (parametros.Campo == "INSS")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorINSS}";
                faturamento.ValorINSS = parametros.Valor;
            }
            else if (parametros.Campo == "ISS")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorISS}";
                faturamento.ValorISS = parametros.Valor;
            }
            else if (parametros.Campo == "IR")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorIR}";
                faturamento.ValorIR = parametros.Valor;
            }
            else if (parametros.Campo == "Art30")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorArt30}";
                faturamento.ValorArt30 = parametros.Valor;
            }
            else if (parametros.Campo == "Desconto")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorDesconto}";
                faturamento.ValorDesconto = parametros.Valor;
            }
            else if (parametros.Campo == "Sinal")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorSinal}";
                faturamento.ValorSinal = parametros.Valor;
            }
            else if (parametros.Campo == "NaoComissionado")
            {
                novoHistorico.ValorAntigo = $"{faturamento.ValorNaoComissionado}";
                faturamento.ValorNaoComissionado = parametros.Valor;
            }

            faturamento.ValorLiquido = faturamento.ValorBruto - faturamento.ValorINSS - faturamento.ValorISS - faturamento.ValorIR - faturamento.ValorArt30 - faturamento.ValorDesconto - faturamento.ValorSinal;
            faturamento.IdUsuarioAlteracao = parametros.IdUsuario;
            faturamento.DataUltimaAlteracao = DateTime.Now;

            fluxoCaixa.Valor = faturamento.ValorLiquido;

            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            _context.Entry(faturamento).State = EntityState.Modified;

            await _context.Historico.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();
        }

        public async Task InformarFaturamento(Faturamento_InformarFaturamentoDTO _faturamento, Int64 idUsuario)
        {
            var faturamento = await _context.Faturamento.FirstOrDefaultAsync(x => x.Id == _faturamento.idFaturamento);

            if (faturamento == null)
                return;

            if(_faturamento.valorRestante>0)
            {
                var porcentagemRecebida = ((_faturamento.valorRecebido * 100) / faturamento.ValorLiquido) / 100;
                var porcentagemRestante = 1 - porcentagemRecebida;

                var novoFaturamento = new Faturamento();
                novoFaturamento.DataCadastro = DateTime.Now;
                novoFaturamento.DataFaturamento = faturamento.DataFaturamento;
                novoFaturamento.DataRecebimentoPrevisto = _faturamento.dataProximoRecebimento ?? DateTime.Now;
                novoFaturamento.DataUltimaAlteracao = DateTime.Now;
                novoFaturamento.Id = 0;
                novoFaturamento.IdObra = faturamento.IdObra;
                novoFaturamento.IdStatusFaturamento = 2;
                novoFaturamento.IdUsuarioAlteracao = idUsuario;
                novoFaturamento.IdUsuarioCadastro = idUsuario;
                novoFaturamento.NumeroNF=faturamento.NumeroNF;
                novoFaturamento.Observacao = faturamento.Observacao;
                novoFaturamento.ValorArt30 = 0;
                novoFaturamento.ValorBruto=faturamento.ValorBruto * porcentagemRestante;
                novoFaturamento.ValorIR= 0;
                novoFaturamento.ValorDesconto = faturamento.ValorDesconto * porcentagemRestante;
                novoFaturamento.ValorINSS = 0;
                novoFaturamento.ValorISS = 0;
                novoFaturamento.ValorLiquido = _faturamento.valorRestante;
                novoFaturamento.ValorRecebido = 0;
                novoFaturamento.ValorSinal = faturamento.ValorSinal * porcentagemRestante;

                await _context.AddAsync(novoFaturamento);
                await _context.SaveChangesAsync();

                var def0209 = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.09");
                var obra = _context.Obra.Include(x => x.Cliente).Include(x => x.Faturamentos).FirstOrDefault(x => x.Id == faturamento.IdObra);

                _repositorioFluxoCaixa.CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    CodigoDef = def0209.Codigo,
                    CodigoObra = obra?.Codigo,
                    CodigoPedidoCompra = null,
                    CodigoPedidoInterno = null,
                    DataLancamento = novoFaturamento.DataFaturamento,
                    DataPagamento = novoFaturamento.DataRecebimentoPrevisto,
                    Id = 0,
                    IdCliente = obra?.IdCliente,
                    IdDef = def0209.Id,
                    IdObra = novoFaturamento.IdObra,
                    IdPedidoCompra = null,
                    IdPedidoCompraFatura = null,
                    IdPedidoCompraNotaFiscal = null,
                    IdPedidoInterno = null,
                    NomeCliente = obra?.Cliente?.NomeFantasia,
                    NumeroNotaFiscalFaturamento = novoFaturamento.NumeroNF,
                    Valor = novoFaturamento.ValorLiquido,
                    IdFaturamento = novoFaturamento.Id,
                    IdTipoFluxoCaixa = 1,
                    PagamentoEfetuado = false,
                    CodigoFatura = novoFaturamento.NumeroNF,
                }).Wait();

                faturamento.ValorBruto = faturamento.ValorBruto * porcentagemRecebida;
                faturamento.ValorDesconto = faturamento.ValorDesconto * porcentagemRecebida;
                faturamento.ValorSinal = faturamento.ValorSinal * porcentagemRecebida;
                faturamento.ValorLiquido = faturamento.ValorLiquido * porcentagemRecebida;
                faturamento.ValorRecebido = _faturamento.valorRecebido;
            }

            faturamento.IdStatusFaturamento = 1;
            faturamento.DataRecebimentoRealizado = DateTime.Now;
            faturamento.DataUltimaAlteracao = DateTime.Now;
            faturamento.IdUsuarioAlteracao = idUsuario;

            var fluxoCaixa = _context.FluxoCaixa.FirstOrDefault(x => x.IdFaturamento == faturamento.Id && x.CodigoDef=="02.09");
            fluxoCaixa.DataPagamentoEfetuado = _faturamento.dataRecebimento;
            fluxoCaixa.DataPagamento = _faturamento.dataRecebimento;
            fluxoCaixa.IdUsuarioInformouPagamento = idUsuario;
            fluxoCaixa.PagamentoEfetuado = true;
            fluxoCaixa.Valor = _faturamento.valorRecebido;

            _context.Entry(faturamento).State = EntityState.Modified;
            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}