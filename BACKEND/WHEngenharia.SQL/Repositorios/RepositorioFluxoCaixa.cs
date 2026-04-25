using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioFluxoCaixa
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioFluxoCaixa(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<double> ObtemNotasFiscais(DateTime data)
        {
            return await _context.PedidoCompra_NotaFiscal.Where(x => x.DataVencimento.Date == data.Date).SumAsync(x => x.Valor);
        }

        public async Task<double> ObtemETO(DateTime data, Int64 idDef)
        {
            var valorPrevisto = await _context.ObraControleCusto.Where(x => x.Data.Date == data.Date).SumAsync(x => x.Valor);
            var ordensCompra = await _context.FluxoCaixa.Where(x => x.DataPagamento.Date == data.Date && x.IdDef == idDef).SumAsync(x => x.Valor);

            return valorPrevisto - ordensCompra;

            //PREVISÃO DO MÊS - ORDENS DE COMPRA (PARA TODAS AS OBRAS ATIVAS)
        }

        public async Task<double> ObtemOrdemCompraPedidoInterno(DateTime data)
        {
            return _context.PedidoCompra_Faturas.Where(x => x.DataFatura.Date == data.Date).Sum(x => x.Valor);

            //USAR COMO CALCULO A CONDIÇÃO DE PAGAMENTO PARA O FORNECEDOR DO PEDIDO DE COMPRA. A CONDIÇÃO SERÁ ACRESCIDA DA DATA DE ENTREGA, E QUANDO HOUVER DUAS DATAS OU MAIS, DIVIDIR O VALOR IGUALMENTE.
        }

        public bool DefEhDoFluxoCaixa(Int64 IdDef)
        {
            var listaDefs = new List<string>() { "02.09", "02.10", "02.16", "03.01", "03.02", "03.03", "03.04", "03.13", "03.16","03.20", "03.26", "03.27", "03.28", "03.29", "03.30", "03.31", "03.33", "03.34", "03.36", "03.38", "03.41", "05.01", "05.01.01", "05.01.02", "05.01.03", "05.01.04", "05.02", "05.02.01", "05.02.02", "05.02.03", "05.02.04", "05.02.05", "05.02.06", "05.02.07", "05.04", "05.05", "05.06", "05.07", "05.08", "05.09", "05.10", "05.11", "05.12", "05.13", "05.14", "05.15", "05.16", "05.17", "05.18", "05.19", "05.20", "05.21", "05.22", "05.23", "05.24", "05.25", "05.26", "05.27", "05.28", "05.29", "05.30", "05.31", "05.32", "05.33"
            };

            if (listaDefs.Contains(_context.DEF.FirstOrDefault(x => x.Id == IdDef).Codigo))
                return true;
            else
                return false;
        }

        public double ObtemValoresFluxoCaixa(List<FluxoCaixa> registrosFluxoCaixa, DateTime dia, Int64 idDef, List<Int64> idsDefsQueNaoSomamValoresDefsFilhos)
        {
            if (idsDefsQueNaoSomamValoresDefsFilhos.Contains(idDef))
                return registrosFluxoCaixa.Where(x => x.DataPagamento.Date == dia.Date && x.IdDef == idDef && !x.Cancelado).Sum(x => x.Valor);
            else
                return registrosFluxoCaixa.Where(x => x.DataPagamento.Date == dia.Date && x.IdDef == idDef && !x.Cancelado).Sum(x => x.Valor - x.FluxosCaixaFilhos.Sum(y => y.Valor));
        }

        public async Task<FluxoCaixaDTO> CadastraFluxoCaixa(FluxoCaixaDTO fluxoCaixaDTO)
        {
            var fluxoCaixa = new FluxoCaixa();
            fluxoCaixa.Id = 0;
            fluxoCaixa.Cancelado = false;
            fluxoCaixa.IdTipoFluxoCaixa = fluxoCaixaDTO.IdTipoFluxoCaixa;
            fluxoCaixa.IdObra = fluxoCaixaDTO.IdObra;
            fluxoCaixa.CodigoObra = fluxoCaixaDTO.CodigoObra;
            fluxoCaixa.IdDef = fluxoCaixaDTO.IdDef;
            fluxoCaixa.CodigoDef = fluxoCaixaDTO.CodigoDef;
            fluxoCaixa.IdPedidoCompra = fluxoCaixaDTO.IdPedidoCompra;
            fluxoCaixa.IdPedidoCompraFatura = fluxoCaixaDTO.IdPedidoCompraFatura;
            fluxoCaixa.IdPedidoCompraNotaFiscal = fluxoCaixaDTO.IdPedidoCompraNotaFiscal;
            fluxoCaixa.CodigoPedidoCompra = fluxoCaixaDTO.CodigoPedidoCompra;
            fluxoCaixa.IdPedidoInterno = fluxoCaixaDTO.IdPedidoInterno;
            fluxoCaixa.CodigoPedidoInterno = fluxoCaixaDTO.CodigoPedidoInterno;
            fluxoCaixa.IdCliente = fluxoCaixaDTO.IdCliente;
            fluxoCaixa.NomeCliente = fluxoCaixaDTO.NomeCliente;
            fluxoCaixa.IdFaturamento = fluxoCaixaDTO.IdFaturamento;
            fluxoCaixa.NumeroNotaFiscalPedidoCompra = fluxoCaixaDTO.NumeroNotaFiscalPedidoCompra;
            fluxoCaixa.NumeroNotaFiscalFaturamento = fluxoCaixaDTO.NumeroNotaFiscalFaturamento;
            fluxoCaixa.DataLancamento = fluxoCaixaDTO.DataLancamento;
            fluxoCaixa.DataPagamento = fluxoCaixaDTO.DataPagamento;
            fluxoCaixa.Valor = fluxoCaixaDTO.Valor;
            fluxoCaixa.PagamentoEfetuado = false;
            fluxoCaixa.IdFornecedorBeneficiario = fluxoCaixaDTO.IdFornecedorBeneficiario;
            fluxoCaixa.IdUsuarioBeneficiario = fluxoCaixaDTO.IdUsuarioBeneficiario;
            fluxoCaixa.IdFluxoCaixaPai = fluxoCaixaDTO.IdFluxoCaixaPai;

            await _context.FluxoCaixa.AddAsync(fluxoCaixa);
            await _context.SaveChangesAsync();

            return _mapper.Map<FluxoCaixaDTO>(fluxoCaixa);
        }

        public async Task<List<FluxoCaixaDTO>> ObtemFluxosDoDia(DateTime data, bool credito)
        {


            var defsParaNaoDescontarValorDesfsFilhos = new List<string>() { "02.09" };

            var listaDefsCredito = await _context.DEF.Where(x => x.Tipo == 'R').Select(x => x.Codigo).ToListAsync();

            if (credito)
            {
                var fluxos = _mapper.Map<List<FluxoCaixaDTO>>(await _context.FluxoCaixa
                .Include(x => x.Fornecedor)
                .Include(x => x.UsuarioBeneficiario)
                .AsNoTracking()
                .Where(x => x.DataPagamento.Date == data.Date && listaDefsCredito.Contains(x.CodigoDef))
                .ToListAsync());

                var ids = fluxos.Select(x => x.Id).ToList();

                var filhos = await _context.FluxoCaixa
                .AsNoTracking()
                .Where(x => x.IdFluxoCaixaPai != null && ids.Contains(x.IdFluxoCaixaPai.Value) && !x.Cancelado)
                .GroupBy(x => x.IdFluxoCaixaPai.Value)
                .Select(g => new { PaiId = g.Key, Total = g.Sum(x => x.Valor) })
                .ToListAsync();

                fluxos.Where(x=>!defsParaNaoDescontarValorDesfsFilhos.Contains(x.CodigoDef)).ToList().ForEach(x =>
                {
                    var filho = filhos.FirstOrDefault(f => f.PaiId == x.Id);
                    if (filho != null)
                        x.Valor -= filho.Total;
                });

                return fluxos;
            }
            else
                return _mapper.Map<List<FluxoCaixaDTO>>(await _context.FluxoCaixa.Include(x => x.Fornecedor).Include(x => x.UsuarioBeneficiario).Where(x => x.DataPagamento.Date == data.Date && !listaDefsCredito.Contains(x.CodigoDef)).ToListAsync());
        }

        public async Task InformarPagamentoOuRecebimento(FluxoCaixa_DataPagamentoRecebimentoParameterDTO parameterDTO)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == parameterDTO.Id);

            if (fluxoCaixa == null)
                throw new Exception("Registro não encontrado");

            if(fluxoCaixa.IdPedidoCompraNotaFiscal.HasValue)
            {
                //Informar pagamento de uma nota fiscal de pedido de compra
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamento = '{parameterDTO.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE IdPedidoCompraNotaFiscal = {fluxoCaixa.IdPedidoCompraNotaFiscal}");
            }
            else if (fluxoCaixa.IdFaturamento.HasValue)
            {
                //Informar recebimento de um faturamento
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamento = '{parameterDTO.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE IdFaturamento = {fluxoCaixa.IdFaturamento}");
                await _context.Database.ExecuteSqlRawAsync($"UPDATE Faturamento SET DataRecebimentoRealizado = '{parameterDTO.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE Id = {fluxoCaixa.IdFaturamento}");
            }
            else if(fluxoCaixa.IdPedidoInterno.HasValue)
            {
                //Informar pagamento de um pedido interno
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamento = '{parameterDTO.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE IdPedidoInterno = {fluxoCaixa.IdPedidoInterno} AND CodigoPedidoInterno = '{fluxoCaixa.CodigoPedidoInterno}'");
            }
            else if (fluxoCaixa.IdPedidoCompra.HasValue)
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_Faturas SET PagamentoEfetuado = 1 WHERE Id = {fluxoCaixa.IdPedidoCompraFatura}");
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamento = '{parameterDTO.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE Id = {fluxoCaixa.Id}");
            }
        }

        public async Task<FluxoCaixaDTO> Put(FluxoCaixaDTO fluxoCaixaDTO)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == fluxoCaixaDTO.Id);

            if (fluxoCaixa != null)
            {
                var def = await _context.DEF.FirstOrDefaultAsync(x => x.Id == fluxoCaixaDTO.IdDef);

                fluxoCaixa.IdDef = def.Id;
                fluxoCaixa.CodigoDef = def.Codigo;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //if (fluxoCaixa.IdPedidoCompraFatura.HasValue)
                //    _context.Database.ExecuteSqlRaw($"UPDATE PedidoCompra_Faturas SET Valor = {fluxoCaixaDTO.Valor.ToString().Replace(',', '.')}, DataFatura = '{fluxoCaixaDTO.DataPagamento.ToString("yyyy-dd-MM 00:00:00")}' WHERE Id = {fluxoCaixa.IdPedidoCompraFatura}");

                //if (fluxoCaixa.IdPedidoInterno.HasValue)
                //    _context.Database.ExecuteSqlRaw($"UPDATE PedidoInterno_Parcelas SET Valor = {fluxoCaixaDTO.Valor.ToString().Replace(',', '.')}, DataPagamento = '{fluxoCaixaDTO.DataPagamento.ToString("yyyy-dd-MM 00:00:00")}' WHERE IdPedidoInterno = {fluxoCaixa.IdPedidoInterno} AND CodigoFormatado = '{fluxoCaixa.CodigoPedidoInterno}'");

                //if (fluxoCaixa.IdFaturamento.HasValue)
                    //_context.Database.ExecuteSqlRaw($"UPDATE Faturamento SET DataRecebimentoRealizado = '{fluxoCaixaDTO.DataPagamento.ToString("yyyy-dd-MM 00:00:00")}' WHERE Id =  {fluxoCaixaDTO.IdFaturamento}");

                return _mapper.Map<FluxoCaixaDTO>(fluxoCaixa);
            }

            return null;
        }

        public async Task CadastraNovosValoresOCPI(FluxoCaixa_NovosValoresOCPIDTO fluxoCaixa_NovosValoresOCPIDTO)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == fluxoCaixa_NovosValoresOCPIDTO.IdFluxoCaixa);

            var valorDoFluxoNoArray = fluxoCaixa_NovosValoresOCPIDTO.Valores.FirstOrDefault(x => x.DataPrevista == fluxoCaixa.DataPagamento);

            if (valorDoFluxoNoArray!=null)
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {valorDoFluxoNoArray.ValorPrevisto}, DataPagamento = '{fluxoCaixa.DataPagamento}' WHERE Id = {fluxoCaixa.Id}");

                fluxoCaixa_NovosValoresOCPIDTO.Valores.Remove(valorDoFluxoNoArray);
            }
            else
            {
                await _context.Database.ExecuteSqlRawAsync($"DELETE FROM FluxoCaixa WHERE Id = {fluxoCaixa.Id}");
            }

            fluxoCaixa_NovosValoresOCPIDTO.Valores.ForEach(novoValor =>
            {
                CadastraFluxoCaixa(new FluxoCaixaDTO()
                {
                    CodigoDef = fluxoCaixa.CodigoDef,
                    CodigoObra = fluxoCaixa.CodigoObra,
                    CodigoPedidoCompra = fluxoCaixa.CodigoPedidoCompra,
                    CodigoPedidoInterno = fluxoCaixa.CodigoPedidoInterno,
                    DataLancamento = DateTime.Now,
                    DataPagamento = novoValor.DataPrevista,
                    Id = 0,
                    IdCliente = fluxoCaixa.IdCliente,
                    IdDef = fluxoCaixa.IdDef,
                    IdFaturamento = fluxoCaixa.IdFaturamento,
                    IdObra = fluxoCaixa.IdObra,
                    IdPedidoCompra = fluxoCaixa.IdPedidoCompra,
                    IdPedidoCompraFatura = fluxoCaixa.IdPedidoCompraFatura,
                    IdPedidoCompraNotaFiscal = fluxoCaixa.IdPedidoCompraNotaFiscal,
                    IdPedidoInterno = fluxoCaixa.IdPedidoInterno,
                    IdTipoFluxoCaixa = fluxoCaixa.IdTipoFluxoCaixa,
                    NomeCliente = fluxoCaixa.NomeCliente,
                    NumeroNotaFiscalPedidoCompra = fluxoCaixa.NumeroNotaFiscalPedidoCompra,
                    PagamentoEfetuado = fluxoCaixa.PagamentoEfetuado,
                    Valor = novoValor.ValorPrevisto,
                    CodigoFatura = fluxoCaixa.CodigoFatura,
                    IdFornecedorBeneficiario = fluxoCaixa.IdFornecedorBeneficiario,
                    IdUsuarioBeneficiario = fluxoCaixa.IdUsuarioBeneficiario,
                    NumeroNotaFiscalFaturamento = fluxoCaixa.NumeroNotaFiscalFaturamento,
                }).Wait();
            });

            if(fluxoCaixa.IdFaturamento.HasValue)
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE Faturamento SET DataRecebimentoPrevisto = (SELECT DataPagamento FROM FluxoCaixa WHERE IdFaturamento = {fluxoCaixa.IdFaturamento}) WHERE Id = {fluxoCaixa.IdFaturamento}");
            }
        }

        public async Task GeraPrevisaoFluxoCaixa()
        {
            var codigosDef = new List<string>();

            var fluxoCaixaEstimativaMensal = await _context.FluxoCaixaEstimativaMensal.FirstOrDefaultAsync(x => x.Mes == DateTime.Now.Month && x.Ano == DateTime.Now.Year);

            if (fluxoCaixaEstimativaMensal == null)
            {
                fluxoCaixaEstimativaMensal = new FluxoCaixaEstimativaMensal();
                fluxoCaixaEstimativaMensal.Id = 0;
                fluxoCaixaEstimativaMensal.Mes = DateTime.Now.Month;
                fluxoCaixaEstimativaMensal.Ano = DateTime.Now.Year;
            }

            codigosDef = new List<string>() { "03.02" };
            fluxoCaixaEstimativaMensal.OrdemCompraPedidoInterno = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.01", "03.04" };
            fluxoCaixaEstimativaMensal.NotaFiscal = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.03"};
            fluxoCaixaEstimativaMensal.ETO = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.16", "03.17", "03.18", "03.19", "03.20", "03.21", "03.22", "03.23", "03.24", "03.25", "03.26", "03.27", "03.28", "03.29", "03.29.01", "03.30", "03.30.01", "03.31", "03.31.01", "03.32", "03.32", "03.32.01", "03.33", "03.33.01", "03.34", "03.34.01", "03.35", "03.36", "03.38", "03.40", "03.41" };
            fluxoCaixaEstimativaMensal.FolhaPagamento = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.29", "03.30", "03.31", "03.33", "03.34", "03.38" };
            fluxoCaixaEstimativaMensal.Imposto = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "05.01", "05.01.01", "05.01.02", "05.01.03", "05.01.04", "05.02", "05.02.01", "05.02.02", "05.02.03", "05.02.04", "05.02.05", "05.02.06", "05.02.07", "05.04", "05.05", "05.06", "05.07", "05.08", "05.09", "05.10", "05.11", "05.12", "05.13", "05.14", "05.15", "05.16", "05.17", "05.18", "05.19", "05.20", "05.21", "05.22", "05.23", "05.24", "05.25", "05.26", "05.27", "05.28", "05.29", "05.30", "05.31", "05.32" };
            fluxoCaixaEstimativaMensal.DespesasFixas = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.20","03.21","03.22","03.23","03.24","03.25","03.26" };
            fluxoCaixaEstimativaMensal.Reserva = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.36", "05.33" };
            fluxoCaixaEstimativaMensal.Outros = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "03.27", "03.28" };
            fluxoCaixaEstimativaMensal.Transferencias = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            fluxoCaixaEstimativaMensal.TotalDiario
                += fluxoCaixaEstimativaMensal.OrdemCompraPedidoInterno
                += fluxoCaixaEstimativaMensal.NotaFiscal
                += fluxoCaixaEstimativaMensal.ETO
                += fluxoCaixaEstimativaMensal.FolhaPagamento
                += fluxoCaixaEstimativaMensal.Imposto
                += fluxoCaixaEstimativaMensal.DespesasFixas
                += fluxoCaixaEstimativaMensal.Reserva
                += fluxoCaixaEstimativaMensal.Outros
                += fluxoCaixaEstimativaMensal.Transferencias;

            codigosDef = new List<string>() { "02.09" };
            fluxoCaixaEstimativaMensal.AReceberFaturado = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "02.10" };
            fluxoCaixaEstimativaMensal.AReceberAFaturar = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            codigosDef = new List<string>() { "02.16" };
            fluxoCaixaEstimativaMensal.Estornos = await _context.FluxoCaixa.Where(x => codigosDef.Contains(x.CodigoDef) && x.DataPagamento.Month == DateTime.Now.Month && x.DataPagamento.Year == DateTime.Now.Year).SumAsync(x => x.Valor);

            fluxoCaixaEstimativaMensal.Saldo = 0;

            if(fluxoCaixaEstimativaMensal.Id==0)
            {
                await _context.FluxoCaixaEstimativaMensal.AddAsync(fluxoCaixaEstimativaMensal);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Entry(fluxoCaixaEstimativaMensal).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
