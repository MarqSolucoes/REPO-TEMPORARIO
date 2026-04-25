using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoFluxoCaixa
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFluxoCaixa _repositorioFluxoCaixa;


        private List<string> defsParaNaoDescontarValorDesfsFilhos = new List<string>() { "02.09" };

        public ServicoFluxoCaixa(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
        }

        public async Task<FluxoCaixaDTO> Put(FluxoCaixaDTO fluxoCaixaDTO)
        {
            return await _repositorioFluxoCaixa.Put(fluxoCaixaDTO);
        }

        public async Task CadastraNovosValoresOCPI(FluxoCaixa_NovosValoresOCPIDTO fluxoCaixa_NovosValoresOCPIDTO)
        {
            await _repositorioFluxoCaixa.CadastraNovosValoresOCPI(fluxoCaixa_NovosValoresOCPIDTO);
        }

        public async Task<List<FluxoCaixaDTO>> ObtemFluxosDoDia(DateTime data, bool credito)
        {
            return await _repositorioFluxoCaixa.ObtemFluxosDoDia(data, credito);
        }

        public async Task InformarPagamentoOuRecebimento(FluxoCaixa_DataPagamentoRecebimentoParameterDTO parameterDTO)
        {
            await _repositorioFluxoCaixa.InformarPagamentoOuRecebimento(parameterDTO);
        }

        private async Task CalculaSaldoDoMes(int mes, int ano)
        {
            double saldoMes = 0;
            var dataAuxiliar = new DateTime(ano, mes, 1);
            var defs = _context.DEF.ToList();
            var registrosFluxoCaixa = _context.FluxoCaixa.Where(x => x.DataPagamento.Date.Month == dataAuxiliar.Month && x.DataPagamento.Date.Year == dataAuxiliar.Year).ToList();

            var idsDefsQueNaoSomamValoresDefsFilhos = _context.DEF.Where(x => defsParaNaoDescontarValorDesfsFilhos.Contains(x.Codigo)).Select(x => x.Id).ToList();

            while (dataAuxiliar.Month==mes && dataAuxiliar.Year==ano)
            {
                double saldoParcial = 0;
                var fluxoCaixaDTO = new FluxoCaixa_ResponseDTO();
                fluxoCaixaDTO.Dia = dataAuxiliar;

                fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.OrdemCompraPedidoInterno = _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.folhaPagamento += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.16").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.folhaPagamento += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.41").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.29").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.30").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.31").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.34").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.38").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.01.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.01.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.01.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.01.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.05").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.06").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.02.07").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.05").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.06").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.07").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.08").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.09").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.10").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.11").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.12").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.13").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.14").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.15").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.16").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.17").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.18").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.19").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.20").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.21").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.22").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.23").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.24").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.25").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.26").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.27").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.28").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.29").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.30").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.31").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.32").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.reserva += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.20").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.reserva += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.26").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                //fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.33").Id);
                //fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36").Id);

                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.01.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.01.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.02.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36.02.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);


                fluxoCaixaDTO.transferencia += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.27").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.transferencia += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.28").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.totalDiario = fluxoCaixaDTO.NotaFiscal + fluxoCaixaDTO.Eto + fluxoCaixaDTO.OrdemCompraPedidoInterno + fluxoCaixaDTO.folhaPagamento + fluxoCaixaDTO.imposto + fluxoCaixaDTO.despesasFixas + fluxoCaixaDTO.reserva + fluxoCaixaDTO.outros + fluxoCaixaDTO.transferencia;

                fluxoCaixaDTO.aReceberFaturado += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.09").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.aReceberAFaturar += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.10").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                //fluxoCaixaDTO.estornos += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.16.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                //fluxoCaixaDTO.estornos += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.16.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                saldoParcial = saldoParcial - fluxoCaixaDTO.totalDiario + fluxoCaixaDTO.aReceberAFaturar + fluxoCaixaDTO.aReceberFaturado + fluxoCaixaDTO.estornos;

                saldoMes += saldoParcial;
                dataAuxiliar = dataAuxiliar.AddDays(1);
            }

            var fluxoCaixaSaldoInicial = await _context.FluxoCaixaSaldoInicial.FirstOrDefaultAsync(x => x.Mes == mes && x.Ano == ano);
            
            if(fluxoCaixaSaldoInicial==null)
            {
                fluxoCaixaSaldoInicial = new FluxoCaixaSaldoInicial();
                fluxoCaixaSaldoInicial.Ano = ano;
                fluxoCaixaSaldoInicial.Id = 0;
                fluxoCaixaSaldoInicial.Mes = mes;
                fluxoCaixaSaldoInicial.Saldo = 0;
                fluxoCaixaSaldoInicial.SaldoCalculado = saldoMes;

                await _context.FluxoCaixaSaldoInicial.AddAsync(fluxoCaixaSaldoInicial);
                await _context.SaveChangesAsync();
            }
            else
            {
                saldoMes += fluxoCaixaSaldoInicial.Saldo;
                fluxoCaixaSaldoInicial.SaldoCalculado = saldoMes;

                _context.Entry(fluxoCaixaSaldoInicial).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FluxoCaixa_ResponseDTO>> ObtemFluxoCaixaConsolidado(DateTime data, DateTime? dataFinal)
        {
            var idsDefsQueNaoSomamValoresDefsFilhos = _context.DEF.Where(x => defsParaNaoDescontarValorDesfsFilhos.Contains(x.Codigo)).Select(x => x.Id).ToList();

            var dataCalculoSaldo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            double saldoAnteior = 0;

            while(dataCalculoSaldo<data)
            {
                if (dataCalculoSaldo.Day == 1)
                {
                    CalculaSaldoDoMes(dataCalculoSaldo.Month, dataCalculoSaldo.Year).Wait();
                    saldoAnteior += _context.FluxoCaixaSaldoInicial.FirstOrDefault(x => x.Mes == dataCalculoSaldo.Month && x.Ano == dataCalculoSaldo.Year).SaldoCalculado;
                }

                dataCalculoSaldo = dataCalculoSaldo.AddDays(1);
            }

            
            var defs = _context.DEF.ToList();
            var registrosFluxoCaixa = new List<FluxoCaixa>();

                if (!dataFinal.HasValue)
                    registrosFluxoCaixa = _context.FluxoCaixa.Include(x=>x.FluxosCaixaFilhos).Where(x => x.DataPagamento.Date.Month == data.Month && x.DataPagamento.Date.Year == data.Year).ToList();
                else
                    registrosFluxoCaixa = _context.FluxoCaixa.Include(x => x.FluxosCaixaFilhos).Where(x => x.DataPagamento.Date >= data.Date && x.DataPagamento.Date <= dataFinal.Value.Date).ToList();

            var dataAuxiliar = new DateTime(data.Year, data.Month, 1);

            var result = new List<FluxoCaixa_ResponseDTO>();

            double eto = 0;

            double saldo = saldoAnteior;

            var dataControle = dataFinal.HasValue ? dataFinal.Value : data.AddMonths(1).AddMilliseconds(-1);
            var mesAnterior = data.Month;
            var totalParcial = new FluxoCaixa_ResponseDTO();
            var estimativaMensal = new FluxoCaixaEstimativaMensal();

            while (dataAuxiliar <= dataControle)
            {
                if(dataAuxiliar.Day==1)
                {
                    if (result.Count > 0)
                    {
                        var dataMesAnterior = dataAuxiliar.AddMilliseconds(-1);
                        var resultComFiltro = result.Where(x => x.Dia?.Date.Month == dataMesAnterior.Date.Month && x.Dia?.Date.Year == dataMesAnterior.Date.Year).ToList();

                        totalParcial = new FluxoCaixa_ResponseDTO()
                        {
                            DiaObservacao = "Total R.",
                            aReceberAFaturar = resultComFiltro.Sum(x => x.aReceberAFaturar),
                            aReceberFaturado = resultComFiltro.Sum(x => x.aReceberFaturado),
                            despesasFixas = resultComFiltro.Sum(x => x.despesasFixas),
                            Dia = null,
                            estornos = resultComFiltro.Sum(x => x.estornos),
                            Eto = resultComFiltro.Sum(x => x.Eto),
                            folhaPagamento = resultComFiltro.Sum(x => x.folhaPagamento),
                            imposto = resultComFiltro.Sum(x => x.imposto),
                            NotaFiscal = resultComFiltro.Sum(x => x.NotaFiscal),
                            OrdemCompraPedidoInterno = resultComFiltro.Sum(x => x.OrdemCompraPedidoInterno),
                            outros = resultComFiltro.Sum(x => x.outros),
                            reserva = resultComFiltro.Sum(x => x.reserva),
                            saldo = resultComFiltro?.OrderBy(x => x.Dia)?.LastOrDefault()?.saldo ?? 0,
                            totalDiario = resultComFiltro.Sum(x => x.totalDiario),
                            transferencia = resultComFiltro.Sum(x => x.transferencia),
                        };

                        result.Add(totalParcial);

                        //Adiciona a estimativa mensal calculada
                        estimativaMensal = _context.FluxoCaixaEstimativaMensal.FirstOrDefault(x => x.Mes == dataMesAnterior.Month && x.Ano == dataMesAnterior.Year);

                        result.Add(new FluxoCaixa_ResponseDTO()
                        {
                            DiaObservacao = "Total P.",
                            aReceberAFaturar = estimativaMensal.AReceberAFaturar,
                            aReceberFaturado = estimativaMensal.AReceberFaturado,
                            despesasFixas = estimativaMensal.DespesasFixas,
                            Dia = null,
                            estornos = estimativaMensal.Estornos,
                            Eto = estimativaMensal.ETO,
                            folhaPagamento = estimativaMensal.FolhaPagamento,
                            imposto = estimativaMensal.Imposto,
                            NotaFiscal = estimativaMensal.NotaFiscal,
                            OrdemCompraPedidoInterno = estimativaMensal.OrdemCompraPedidoInterno,
                            outros = estimativaMensal.Outros,
                            reserva = estimativaMensal.Reserva,
                            saldo = estimativaMensal.Saldo,
                            totalDiario = estimativaMensal.TotalDiario,
                            transferencia = estimativaMensal.Transferencias
                        });
                    }

                    eto = 0;
                    saldo += _context.FluxoCaixaSaldoInicial.FirstOrDefault(x => x.Mes == dataAuxiliar.Month && x.Ano == dataAuxiliar.Year)?.Saldo ?? 0;
                }

                eto = _context.FluxoCaixa.Where(x => x.DataPagamento.Date == dataAuxiliar.Date && x.CodigoDef == "03.03" && !x.Cancelado).Sum(x => x.Valor);

                var fluxoCaixaDTO = new FluxoCaixa_ResponseDTO();
                fluxoCaixaDTO.Dia = dataAuxiliar;

                fluxoCaixaDTO.Eto = eto;

                if (dataAuxiliar.Day % 10 == 0)
                {
                    
                    eto = 0;
                }

                //fluxoCaixaDTO.NotaFiscal +=_repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                //fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                //fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                //fluxoCaixaDTO.NotaFiscal += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.01.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.OrdemCompraPedidoInterno = _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.folhaPagamento += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.16").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.folhaPagamento += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.41").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.29").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.30").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.31").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.34").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.imposto += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.38").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.01.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.01.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.01.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.01.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.01").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.02").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.03").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.05").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.06").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.02.07").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.04").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.05").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.06").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.07").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.08").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.09").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.10").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.11").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.12").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.13").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.14").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.15").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.16").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.17").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.18").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.19").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.20").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.21").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.22").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.23").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.24").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.25").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.26").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.27").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.28").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.29").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.30").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x=>x.Codigo == "05.31").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.despesasFixas += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "05.32").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.reserva += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.20").Id, idsDefsQueNaoSomamValoresDefsFilhos );
                fluxoCaixaDTO.reserva += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.26").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.33").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.outros += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.36").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.transferencia += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.27").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.transferencia += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "03.28").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                if (fluxoCaixaDTO.Eto > 0)
                {
                    //fluxoCaixaDTO.Eto -= fluxoCaixaDTO.OrdemCompraPedidoInterno;
                    //fluxoCaixaDTO.Eto -= fluxoCaixaDTO.NotaFiscal;
                }

                fluxoCaixaDTO.totalDiario = fluxoCaixaDTO.NotaFiscal + fluxoCaixaDTO.Eto + fluxoCaixaDTO.OrdemCompraPedidoInterno + fluxoCaixaDTO.folhaPagamento + fluxoCaixaDTO.imposto + fluxoCaixaDTO.despesasFixas + fluxoCaixaDTO.reserva + fluxoCaixaDTO.outros + fluxoCaixaDTO.transferencia;

                fluxoCaixaDTO.aReceberFaturado += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.09").Id, idsDefsQueNaoSomamValoresDefsFilhos);
                fluxoCaixaDTO.aReceberAFaturar += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.10").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                fluxoCaixaDTO.estornos += _repositorioFluxoCaixa.ObtemValoresFluxoCaixa(registrosFluxoCaixa, dataAuxiliar, defs.FirstOrDefault(x => x.Codigo == "02.16").Id, idsDefsQueNaoSomamValoresDefsFilhos);

                saldo = saldo - fluxoCaixaDTO.totalDiario + fluxoCaixaDTO.aReceberAFaturar + fluxoCaixaDTO.aReceberFaturado + fluxoCaixaDTO.estornos;
                fluxoCaixaDTO.saldo = saldo;

                
                result.Add(fluxoCaixaDTO);

                //A SOMA DOS TRÊS CAMPOS É O VALOR DE CUSTO DA OBRA
                dataAuxiliar = dataAuxiliar.AddDays(1);
            }

            var dataMesAnterior2 = dataAuxiliar.AddMilliseconds(-1);
            var resultComFiltro2 = result.Where(x => x.Dia?.Date.Month == dataMesAnterior2.Date.Month && x.Dia?.Date.Year == dataMesAnterior2.Date.Year).ToList();

            totalParcial = new FluxoCaixa_ResponseDTO()
            {
                DiaObservacao = "Total R.",
                aReceberAFaturar = resultComFiltro2.Sum(x => x.aReceberAFaturar),
                aReceberFaturado = resultComFiltro2.Sum(x => x.aReceberFaturado),
                despesasFixas = resultComFiltro2.Sum(x => x.despesasFixas),
                Dia = null,
                estornos = resultComFiltro2.Sum(x => x.estornos),
                Eto = resultComFiltro2.Sum(x => x.Eto),
                folhaPagamento = resultComFiltro2.Sum(x => x.folhaPagamento),
                imposto = resultComFiltro2.Sum(x => x.imposto),
                NotaFiscal = resultComFiltro2.Sum(x => x.NotaFiscal),
                OrdemCompraPedidoInterno = resultComFiltro2.Sum(x => x.OrdemCompraPedidoInterno),
                outros = resultComFiltro2.Sum(x => x.outros),
                reserva = resultComFiltro2.Sum(x => x.reserva),
                saldo = resultComFiltro2?.OrderBy(x => x.Dia)?.LastOrDefault()?.saldo ?? 0,
                totalDiario = resultComFiltro2.Sum(x => x.totalDiario),
                transferencia = resultComFiltro2.Sum(x => x.transferencia),
            };

            result.Add(totalParcial);

            //Adiciona a estimativa mensal calculada
            estimativaMensal = _context.FluxoCaixaEstimativaMensal.FirstOrDefault(x => x.Mes == dataMesAnterior2.Month && x.Ano == dataMesAnterior2.Year);

            result.Add(new FluxoCaixa_ResponseDTO()
            {
                DiaObservacao = "Total P.",
                aReceberAFaturar = estimativaMensal.AReceberAFaturar,
                aReceberFaturado = estimativaMensal.AReceberFaturado,
                despesasFixas = estimativaMensal.DespesasFixas,
                Dia = null,
                estornos = estimativaMensal.Estornos,
                Eto = estimativaMensal.ETO,
                folhaPagamento = estimativaMensal.FolhaPagamento,
                imposto = estimativaMensal.Imposto,
                NotaFiscal = estimativaMensal.NotaFiscal,
                OrdemCompraPedidoInterno = estimativaMensal.OrdemCompraPedidoInterno,
                outros = estimativaMensal.Outros,
                reserva = estimativaMensal.Reserva,
                saldo = estimativaMensal.Saldo,
                totalDiario = estimativaMensal.TotalDiario,
                transferencia = estimativaMensal.Transferencias
            });


            return result;
        }

        private void AdicionaCabecalhoPDF(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath, string periodo)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 5);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"FLUXO DE CAIXA CONSOLIDADO", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento;
            
            graphics.DrawString($"{periodo.ToUpper()}", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.15) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.15) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.20) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.20) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.25) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.25) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.30) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.30) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.45) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.45) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.50) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.50) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.55) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.55) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.65) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.65) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.75) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.75) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.90) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.90) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"DIA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"OC/PI", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.05), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"NF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.10), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"ETO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.15), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FOLHA PAG", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.20), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"IMPOSTO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.25), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DESP. FIXAS", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.30), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"RESERVA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.40), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"OUTROS", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.45), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"TRANSF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.50), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"TOTAL DIÁRIO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.55), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FATURADO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.65), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"A RECEBER", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.75), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"ESTORNO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.85), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"SALDO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.90), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }

        public async Task<byte[]> DownloadPDF(string logoPath, FluxoCaixaRequestDTO parametros)
        {
            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            if (string.IsNullOrEmpty(parametros.Data))
                parametros.Data = DateTime.Now.ToString("MM/yyyy");

            DateTime dataInicial = new DateTime(Convert.ToInt32(parametros.Data.Split('/')[1]), Convert.ToInt32(parametros.Data.Split('/')[0]), 1);
            DateTime? dataFinal = string.IsNullOrEmpty(parametros.DataFinal) ? null : new DateTime(Convert.ToInt32(parametros.DataFinal.Split('/')[1]), Convert.ToInt32(parametros.DataFinal.Split('/')[0]), 1).AddMonths(1).AddMilliseconds(-1);

            var result = ObtemFluxoCaixaConsolidado(dataInicial, dataFinal).Result;

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 7, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalhoPDF(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, $"{result.FirstOrDefault()?.Dia?.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR")) ?? ""} de {result.FirstOrDefault()?.Dia?.Year.ToString() ?? ""}");

            var primeiroDia = true;

            result.ForEach(x =>
            {
                if (x.Dia.HasValue && x.Dia?.Day == 1 && !primeiroDia)
                {
                    pagina.Close();
                    paginaGrafico.Dispose();

                    pagina = documentoPdf.AddPage();
                    pagina.Size = PdfSharpCore.PageSize.A4;
                    pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                    paginaGrafico = XGraphics.FromPdfPage(pagina);

                    posicaoYLinha = margemSuperior;

                    AdicionaCabecalhoPDF(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, $"{x.Dia?.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR")) ?? ""} de {x.Dia?.Year.ToString() ?? ""}");

                }

                if(x.DiaObservacao=="Total R.")
                {
                    posicaoYLinha -= 2;
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width, posicaoYLinha);
                    posicaoYLinha += 2;
                }

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.10) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.10) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.15) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.15) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.20) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.20) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.25) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.25) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.30) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.30) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.40) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.40) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.45) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.45) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.50) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.50) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.55) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.55) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.65) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.65) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.75) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.75) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.90) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.90) - 2, posicaoYLinha + 12);

                paginaGrafico.DrawString($"{x.Dia?.ToString("dd/MM/yy") ?? x.DiaObservacao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.OrdemCompraPedidoInterno == 0 ? "" : x.OrdemCompraPedidoInterno).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.NotaFiscal == 0 ? "" : x.NotaFiscal).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.10), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.Eto == 0 ? "" : x.Eto).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.15), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.folhaPagamento == 0 ? "" : x.folhaPagamento).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.20), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.imposto == 0 ? "" : x.imposto).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.despesasFixas == 0 ? "" : x.despesasFixas).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.30), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.reserva == 0 ? "" : x.reserva).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.40), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.outros == 0 ? "" : x.outros).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.transferencia == 0 ? "" : x.transferencia).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.50), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.totalDiario == 0 ? "" : x.totalDiario).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.55), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.aReceberFaturado == 0 ? "" : x.aReceberFaturado).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.65), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.aReceberAFaturar == 0 ? "" : x.aReceberAFaturar).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.estornos == 0 ? "" : x.estornos).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.85), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{string.Format("{0:C}", x.saldo == 0 ? "" : x.saldo).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.90), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento;

                primeiroDia = false;
            });

            //valores.ToList().ForEach(item =>
            //{
            //    saldo -= item.Valor;

            //    paginaGrafico.DrawString($"{item.Data?.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //    paginaGrafico.DrawString($"{(string.IsNullOrEmpty(item.NotaFiscal) ? item.OrdemCompra : item.NotaFiscal)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.1), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //    paginaGrafico.DrawString($"{string.Format("{0:C}", item.Valor).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.2), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //    paginaGrafico.DrawString($"{item.Fornecedor}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.3), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //    paginaGrafico.DrawString($"{string.Format("{0:C}", saldo).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

            //    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha + 35);
            //    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha + 35);
            //    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha + 35);
            //    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 35);
            //    posicaoYLinha += espacamento;

            //    count++;

            //    if (count % numeroItensPorPagina == 0)
            //    {
            //        pagina.Close();
            //        paginaGrafico.Dispose();

            //        pagina = documentoPdf.AddPage();
            //        pagina.Size = PdfSharpCore.PageSize.A4;
            //        pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            //        paginaGrafico = XGraphics.FromPdfPage(pagina);

            //        posicaoYLinha = margemSuperior;

            //        AdicionaCabecalhoETOObra(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, obraDTO);

            //        count = 0;
            //    }
            //});

            pagina.Close();
            paginaGrafico.Dispose();

            var numeroPaginas = documentoPdf.Pages.Count;
            for (int i = 0; i < documentoPdf.Pages.Count; ++i)
            {
                PdfPage paginaAuxiliar = documentoPdf.Pages[i];

                using (XGraphics paginaGraficoAuxiliar = XGraphics.FromPdfPage(paginaAuxiliar))
                    paginaGraficoAuxiliar.DrawString($"Página {i + 1} de {numeroPaginas}", fonteNormal, XBrushes.Black, new XRect(0, paginaAuxiliar.Height.Value - 30, paginaAuxiliar.Width, paginaAuxiliar.Height), XStringFormats.TopCenter);
            }

            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                documentoPdf.Save(stream, true);
                fileContents = stream.ToArray();
            }

            return fileContents;
        }

        public async Task GeraPrevisaoFluxoCaixa()
        {
            await _repositorioFluxoCaixa.GeraPrevisaoFluxoCaixa();
        }
    }
}
