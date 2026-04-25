using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.Dominio.Modelos.Genericos.Faturamento;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioAgenda
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioAgenda(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AgendaDTO> CadastraAgenda(AgendaDTO agendaDTO)
        {
            var agenda = new Agenda();
            agenda.DataLancamento = agendaDTO.DataLancamento;
            agenda.DataPagamento = agendaDTO.DataPagamento;
            agenda.Id = 0;
            agenda.IdCliente = agendaDTO.IdCliente;
            agenda.NomeCliente = agendaDTO.NomeCliente;
            agenda.IdDef = agendaDTO.IdDef;
            agenda.CodigoDef = agendaDTO.CodigoDef;
            agenda.IdFornecedorBeneficiario = agendaDTO.IdFornecedorBeneficiario;
            agenda.NomeFantasiaFornecedor = agendaDTO.NomeFantasiaFornecedor;
            agenda.IdObra = agendaDTO.IdObra;
            agenda.CodigoObra = agendaDTO.CodigoObra;
            agenda.IdPedidoCompra = agendaDTO.IdPedidoCompra;
            agenda.CodigoPedidoCompra = agendaDTO.CodigoPedidoCompra;
            agenda.IdPedidoInterno = agendaDTO.IdPedidoInterno;
            agenda.CodigoPedidoInterno = agendaDTO.CodigoPedidoInterno;
            agenda.IdUsuarioBeneficiario = agendaDTO.IdUsuarioBeneficiario;
            agenda.NomeUsuarioBeneficiario = agendaDTO.NomeUsuarioBeneficiario;
            agenda.NumeroNF = agendaDTO.NumeroNF;
            agenda.Valor = agendaDTO.Valor;
            agenda.PagamentoEfetuado = false;

            await _context.Agenda.AddAsync(agenda);
            await _context.SaveChangesAsync();

            return _mapper.Map<AgendaDTO>(agenda);
        }

        public bool DefEhDaAgenda(Int64 IdDef)
        {
            var listaDefs = new List<string>() { "03.01", "03.02", "03.03", "03.16", "03.29", "03.30", "03.31", "03.33", "03.34", "03.36", "05.01", "05.02", "05.04", "05.05", "05.06", "05.07", "05.08", "05.09", "05.10", "05.12", "05.13", "05.14", "05.15", "05.16", "05.17", "05.18", "05.19", "05.23", "05.24", "05.26", "05.29", "05.30", "05.31", "05.32", "05.33" };

            if (listaDefs.Contains(_context.DEF.FirstOrDefault(x => x.Id == IdDef).Codigo))
                return true;
            else
                return false;
        }

        //private List<AgendaDTO> ObtemItensPedidoInterno(DEF def, Agenda_RequestDTO parametros)
        //{
        //    var result = new List<Agenda_ItemDTO>();

        //    if (parametros.idsDefs.Count == 0 || parametros.idsDefs.Any(x => x == def.Id))
        //    {
        //        var queryDef = _context.PedidoInternoParcelas
        //            .Include(x => x.PedidoInterno)
        //            .ThenInclude(x => x.UsuarioBeneficiario)
        //            .Include(x => x.PedidoInterno)
        //            .ThenInclude(x => x.FornecedorBeneficiario)
        //            .Include(x => x.ParcelaObras)
        //            .ThenInclude(x => x.Obra).AsNoTracking().AsQueryable();

        //        queryDef = queryDef.Where(x => x.PedidoInterno.IdDEF == def.Id && x.PedidoInterno.Aprovado == true);

        //        if (parametros.dataInicial.HasValue)
        //            queryDef = queryDef.Where(x => x.DataPagamento.Date >= parametros.dataInicial.Value.Date);

        //        if (parametros.dataFinal.HasValue)
        //            queryDef = queryDef.Where(x => x.DataPagamento.Date <= parametros.dataFinal.Value.Date);

        //        if (!parametros.dataFinal.HasValue && !parametros.dataInicial.HasValue)
        //            queryDef = queryDef.Where(x => x.DataPagamento.Date.Month == DateTime.Now.Date.Month);

        //        //if (parametros.idsClientes != null && parametros.idsClientes.Count > 0)
        //        //    queryDef = queryDef.Where(x => x.ParcelaObras.Any(y => parametros.idsClientes.Contains(y.Obra.IdCliente)));

        //        //if (parametros.idsFornecedores != null && parametros.idsFornecedores.Count > 0)
        //        //    queryDef = queryDef.Where(x => parametros.idsFornecedores.Contains(x.PedidoInterno.IdFornecedorBeneficiario.Value));

        //        //if (parametros.idsObras != null && parametros.idsObras.Count > 0)
        //        //    queryDef = queryDef.Where(x => x.ParcelaObras.Any(y => parametros.idsObras.Contains(y.IdObra)));

        //        queryDef.ToList().ForEach(x =>
        //        {
        //            result.Add(new Agenda_ItemDTO()
        //            {
        //                codigoDef = def.Codigo,
        //                codigoObra = string.Join(",", x.ParcelaObras.Select(x => x.Obra.Codigo)),
        //                codigoPedidoCompra = x.CodigoFormatado,
        //                dataLancamento = x.PedidoInterno.DataCadastro,
        //                dataPagamento = x.DataPagamento,
        //                descricaoFornecedor = x.PedidoInterno.IdUsuarioBeneficiario.HasValue ? x.PedidoInterno.UsuarioBeneficiario?.Nome : x.PedidoInterno.FornecedorBeneficiario?.NomeFantasia,
        //                idDef = def.Id,
        //                idFornecedor = x.PedidoInterno.IdFornecedorBeneficiario ?? 0,
        //                idObra = 0,
        //                idPedidoCompra = 0,
        //                notaFiscal = x.PedidoInterno.Descricao,
        //                valor = x.Valor
        //            });
        //        });

        //        result.OrderBy(x => x.dataPagamento).ToList();

        //        result.Add(new Agenda_ItemDTO()
        //        {
        //            codigoDef = def.Codigo,
        //            codigoObra = "TOTAL",
        //            codigoPedidoCompra = "",
        //            dataLancamento = null,
        //            dataPagamento = null,
        //            descricaoFornecedor = "",
        //            idDef = 0,
        //            idFornecedor = 0,
        //            idObra = 0,
        //            idPedidoCompra = 0,
        //            notaFiscal = "",
        //            valor = result.Where(x => x.idDef == def.Id).Sum(x => x.valor)
        //        });
        //    }

        //    return result;
        //}

        public async Task AlteraDef(Int64 id, Int64 idDef, Int64 IdUsuario, string NomeUsuario)
        {
            var codigoDef = (await _context.DEF.FirstOrDefaultAsync(x => x.Id==idDef))?.Codigo ?? "";
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == id);

            var codigoDefAnterior = _context.DEF.FirstOrDefault(x => x.Id == fluxoCaixa.IdDef)?.Codigo ?? "";

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Valor de pagamento ou recebimento";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
            novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = IdUsuario;
            novoHistorico.Usuario = NomeUsuario;
            novoHistorico.ValorAntigo = $"{codigoDef}";
            novoHistorico.ValorNovo = $"{codigoDefAnterior}";

            fluxoCaixa.IdDef = idDef;
            fluxoCaixa.CodigoDef = codigoDef;

            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            await _context.Historico.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET IdDef = {idDef}, CodigoDef = (SELECT Codigo FROM DEF D WHERE D.Id = {idDef}) WHERE Id = {id}");
        }

        public async Task AlteraValor(Agenda_AlteracaoValorDTO parametros)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == parametros.Id);

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Valor de pagamento ou recebimento";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
            novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = parametros.IdUsuario;
            novoHistorico.Usuario = parametros.NomeUsuario;
            novoHistorico.ValorAntigo = $"R$ {fluxoCaixa.Valor}";
            novoHistorico.ValorNovo = $"R$ {parametros.Valor}";

            fluxoCaixa.Valor = parametros.Valor;

            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            await _context.Historico.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET Valor = {valor.ToString().Replace(".", "").Replace(",", ".")} WHERE Id = {id}");
        }

        public async Task AlteraDataPagamento(Agenda_AlteracaoDataDTO parametros)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == parametros.Id);

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Data pagamento ou recebimento";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
            novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = parametros.IdUsuario;
            novoHistorico.Usuario = parametros.NomeUsuario;
            novoHistorico.ValorAntigo = $"{fluxoCaixa.DataPagamento.ToString("dd/MM/yyyy")}";
            novoHistorico.ValorNovo = $"{parametros.Data.ToString("dd/MM/yyyy")}";

            fluxoCaixa.DataPagamento = parametros.Data;

            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            await _context.Historico.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET DataPagamento = '{data.ToString("MM/dd/yyyy 00:00:00")}' WHERE Id = {id}");
        }

        public async Task AlteraDefEmLote(Agenda_AlteracaoDefLoteDTO lote)
        {
            var codigoDef = (await _context.DEF.FirstOrDefaultAsync(x => x.Id == lote.IdDef))?.Codigo ?? "";
            var fluxosCaixa = await _context.FluxoCaixa.Where(x => lote.Ids.Contains(x.Id)).ToListAsync();

            fluxosCaixa.ForEach(fluxoCaixa =>
            {
                var codigoDefAnterior = _context.DEF.FirstOrDefault(x => x.Id == fluxoCaixa.IdDef)?.Codigo ?? "";

                var novoHistorico = new Historico();
                novoHistorico.Id = Guid.NewGuid();
                novoHistorico.Campo = $"DEF de pagamento ou recebimento";
                novoHistorico.Data = DateTime.Now;
                novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
                novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
                novoHistorico.IdSolicitacaoCompra = null;
                novoHistorico.IdUsuario = lote.IdUsuario;
                novoHistorico.Usuario = lote.NomeUsuario;
                novoHistorico.ValorAntigo = $"{codigoDefAnterior}";
                novoHistorico.ValorNovo = $"{codigoDef}";

                fluxoCaixa.IdDef = lote.IdDef;
                fluxoCaixa.CodigoDef = codigoDef;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                _context.Historico.Add(novoHistorico);
                _context.SaveChanges();
            });

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET IdDef = {lote.IdDef}, CodigoDef = (SELECT Codigo FROM DEF D WHERE D.Id = {lote.IdDef}) WHERE Id IN ({string.Join(',', lote.Ids)})");
        }

        public async Task AlteraDataPagamentoEmLote(Agenda_AlteracaoDataLoteDTO lote)
        {
            var fluxosCaixa = await _context.FluxoCaixa.Where(x => lote.Ids.Contains(x.Id)).ToListAsync();

            fluxosCaixa.ForEach(fluxoCaixa =>
            {
                var novoHistorico = new Historico();
                novoHistorico.Id = Guid.NewGuid();
                novoHistorico.Campo = $"Data prevista pagamento/recebimento";
                novoHistorico.Data = DateTime.Now;
                novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
                novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
                novoHistorico.IdSolicitacaoCompra = null;
                novoHistorico.IdUsuario = lote.IdUsuario;
                novoHistorico.Usuario = lote.NomeUsuario;
                novoHistorico.ValorAntigo = $"{fluxoCaixa.DataPagamento.ToString("dd/MM/yyyy")}";
                novoHistorico.ValorNovo = $"{lote.Data.ToString("dd/MM/yyyy")}";

                fluxoCaixa.DataPagamento = lote.Data;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                _context.Historico.Add(novoHistorico);
                _context.SaveChanges();
            });

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET DataPagamento = '{lote.Data.ToString("MM/dd/yyyy 00:00:00")}' WHERE Id IN ({string.Join(',', lote.Ids)})");
        }

        public async Task InformarPagamentoRecebimentoLote(Agenda_PagamentoRecebimentoLoteDTO lote)
        {
            var fluxosCaixa = await _context.FluxoCaixa.Where(x => lote.Ids.Contains(x.Id)).ToListAsync();

            fluxosCaixa.ForEach(fluxoCaixa =>
            {
                fluxoCaixa.PagamentoEfetuado = true;
                fluxoCaixa.DataPagamentoEfetuado = DateTime.Now;
                fluxoCaixa.IdUsuarioInformouPagamento = lote.IdUsuario;

                _context.Entry(fluxoCaixa).State = EntityState.Modified;
                _context.SaveChanges();

                var novoHistorico = new Historico();
                novoHistorico.Id = Guid.NewGuid();
                novoHistorico.Campo = $"Pagamento ou recebimento com data '{fluxoCaixa.DataPagamento.ToString("dd/MM/yyyy")}' no valor de R$ {fluxoCaixa.Valor} pago/recebido";
                novoHistorico.Data = DateTime.Now;
                novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
                novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
                novoHistorico.IdSolicitacaoCompra = null;
                novoHistorico.IdUsuario = lote.IdUsuario;
                novoHistorico.Usuario = lote.NomeUsuario;
                novoHistorico.ValorAntigo = $"";
                novoHistorico.ValorNovo = $"";

                _context.Historico.Add(novoHistorico);
                _context.SaveChanges();
            });

            //await _context.Database.ExecuteSqlRawAsync($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamentoEfetuado = GETDATE(), IdUsuarioInformouPagamento = {lote.IdUsuario} WHERE Id IN ({string.Join(',', lote.Ids)})");
        }

        public async Task CancelarPagamentoRecebimento(Agenda_CancelamentoPagamentoRecebimentoDTO parametros)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == parametros.Id);

            fluxoCaixa.PagamentoEfetuado = false;
            fluxoCaixa.DataPagamentoEfetuado = null;
            fluxoCaixa.IdUsuarioInformouPagamento = null;

            _context.Entry(fluxoCaixa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Pagamento ou recebimento com data '{fluxoCaixa.DataPagamento.ToString("dd/MM/yyyy")}' no valor de R$ {fluxoCaixa.Valor} cancelado";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
            novoHistorico.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
            novoHistorico.IdSolicitacaoCompra = null;
            novoHistorico.IdUsuario = parametros.IdUsuario;
            novoHistorico.Usuario = parametros.NomeUsuario;
            novoHistorico.ValorAntigo = $"";
            novoHistorico.ValorNovo = $"";

            await _context.Historico.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();

            //_context.Database.ExecuteSqlRaw($"UPDATE FluxoCaixa SET PagamentoEfetuado = 1, DataPagamentoEfetuado = GETDATE(), IdUsuarioInformouPagamento = {parametros.IdUsuario} WHERE Id = {parametros.Id}");
        }




        public async Task<List<AgendaDTO>> ObtemAgendaFaturamento(Agenda_RequestDTO parametros)
        {
            var result = new List<AgendaDTO>();

            var query = _context.FluxoCaixa.Include(x => x.Fornecedor).Include(x => x.UsuarioBeneficiario).AsQueryable();

            query = query.Where(x => x.IdTipoFluxoCaixa == 1);

            if (parametros.dataInicial.HasValue)
                query = query.Where(x => x.DataPagamento.Date >= parametros.dataInicial.Value.Date);
            else
            {
                var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                query = query.Where(x => x.DataPagamento >= primeiroDiaMes);
            }

            if (parametros.dataFinal.HasValue)
                query = query.Where(x => x.DataPagamento.Date <= parametros.dataFinal.Value.Date);

            if (parametros.idsClientes != null && parametros.idsClientes.Count > 0)
                query = query.Where(x => parametros.idsClientes.Contains(x.IdCliente.Value));

            if (parametros.idsFornecedores != null && parametros.idsFornecedores.Count > 0)
                query = query.Where(x => parametros.idsFornecedores.Contains(x.IdFornecedorBeneficiario.Value));

            if (parametros.idsObras != null && parametros.idsObras.Count > 0)
                query = query.Where(x => parametros.idsObras.Contains(x.IdObra.Value));

            if (parametros.idsDefs != null && parametros.idsDefs.Count > 0)
                query = query.Where(x => parametros.idsDefs.Contains(x.IdDef));

            if (!string.IsNullOrEmpty(parametros.notaFiscal))
                query = query.Where(x => x.NumeroNotaFiscalPedidoCompra.Contains(parametros.notaFiscal));

            if (!string.IsNullOrEmpty(parametros.pedidoInterno))
                query = query.Where(x => x.CodigoPedidoInterno.Contains(parametros.pedidoInterno));

            if (!string.IsNullOrEmpty(parametros.ordemCompra))
                query = query.Where(x => x.CodigoPedidoCompra.Contains(parametros.ordemCompra));

            foreach (var agendaAgrupada in query.ToList().OrderBy(x => x.CodigoDef).GroupBy(x => x.CodigoDef))
            {
                foreach (var item in agendaAgrupada.OrderBy(x => x.DataPagamento))
                {

                    result.Add(new AgendaDTO()
                    {
                        CodigoDef = item.CodigoDef,
                        CodigoObra = item.CodigoObra,
                        CodigoPedidoCompra = item.CodigoPedidoCompra,
                        CodigoPedidoInterno = item.CodigoPedidoInterno,
                        DataLancamento = item.DataLancamento,
                        DataPagamento = item.DataPagamento,
                        Id = item.Id,
                        IdCliente = item.IdCliente,
                        IdDef = item.IdDef,
                        IdFornecedorBeneficiario = item.IdFornecedorBeneficiario,
                        IdObra = item.IdObra,
                        IdPedidoCompra = item.IdPedidoCompra,
                        IdPedidoInterno = item.IdPedidoInterno,
                        IdUsuarioBeneficiario = item.IdUsuarioBeneficiario,
                        NomeCliente = item.NomeCliente,
                        NomeFantasiaFornecedor = item.Fornecedor?.NomeFantasia ?? "",
                        NomeUsuarioBeneficiario = item.UsuarioBeneficiario?.Nome ?? "",
                        NumeroNF = item.NumeroNotaFiscalPedidoCompra,
                        PagamentoEfetuado = item.PagamentoEfetuado,
                        Valor = item.Valor
                    });
                }

                result.Add(new AgendaDTO()
                {
                    CodigoObra = "TOTAL",
                    CodigoDef = agendaAgrupada.Key,
                    Valor = result.Where(x => x.CodigoDef == agendaAgrupada.Key).Sum(x => x.Valor),
                    DataLancamento = new DateTime(2500, 1, 1, 0, 0, 0),
                    DataPagamento = new DateTime(2500, 1, 1, 0, 0, 0)
                });
            }

            return result.OrderBy(x => x.CodigoDef).ThenBy(x => x.DataPagamento).ToList();
        }

        public async Task<List<AgendaDTO>> Get(Agenda_RequestDTO parametros)
        {
            var defs = _mapper.Map<List<DEFDTO>>(_context.DEF.ToList());

            var result = new List<AgendaDTO>();

            var query = _context.FluxoCaixa.AsNoTracking().Include(x=>x.Fornecedor).Include(x=>x.UsuarioBeneficiario).AsQueryable();

            if(parametros.tipoRelatorio ==1)
            {
                //Agenda de pagamento
                query = query.Where(x => x.IdTipoFluxoCaixa == 2);
            }
            else if(parametros.tipoRelatorio==2)
            {
                //Agenda recebimento
                query = query.Where(x => x.IdTipoFluxoCaixa == 1);
            }
            else
            {
                //Fluxo de caixa
            }
                

            if (parametros.dataInicial.HasValue)
                query = query.Where(x => x.DataPagamento.Date >= parametros.dataInicial.Value.Date);
            else
            {
                var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                query = query.Where(x => x.DataPagamento >= primeiroDiaMes);
            }

            if (parametros.dataFinal.HasValue)
                query = query.Where(x => x.DataPagamento.Date <= parametros.dataFinal.Value.Date);

            if (parametros.idsClientes != null && parametros.idsClientes.Count > 0)
                query = query.Where(x => parametros.idsClientes.Contains(x.IdCliente.Value));

            if (parametros.idsFornecedores != null && parametros.idsFornecedores.Count > 0)
                query = query.Where(x => parametros.idsFornecedores.Contains(x.IdFornecedorBeneficiario.Value));

            if (parametros.idsObras != null && parametros.idsObras.Count > 0)
                query = query.Where(x => parametros.idsObras.Contains(x.IdObra.Value));

            if (parametros.idsDefs != null && parametros.idsDefs.Count > 0)
                query = query.Where(x => parametros.idsDefs.Contains(x.IdDef));

            if (!string.IsNullOrEmpty(parametros.notaFiscal))
                query = query.Where(x => x.NumeroNotaFiscalPedidoCompra.Contains(parametros.notaFiscal));

            if (!string.IsNullOrEmpty(parametros.pedidoInterno))
                query = query.Where(x => x.CodigoPedidoInterno.Contains(parametros.pedidoInterno));

            if (!string.IsNullOrEmpty(parametros.ordemCompra))
                query = query.Where(x => x.CodigoPedidoCompra.Contains(parametros.ordemCompra));

            var queryResult = _mapper.Map<List<FluxoCaixaDTO>>(await query.ToListAsync());

            var idsPedidosComNota = queryResult.Where(x => x.IdPedidoCompraNotaFiscal.HasValue).Select(x => x.IdPedidoCompra).Distinct().ToList();

            var relacaoPedidoCompraTipoServico = _context.PedidoCompra.Where(x => idsPedidosComNota.Contains(x.Id)).Select(x => new { x.Id, x.SolicitacaoCompra.Servico }).ToList();

            queryResult.OrderBy(x=>x.DataPagamento).ToList().ForEach(item =>
            {
                result.Add(new AgendaDTO()
                {
                    CodigoDef = item.CodigoDef,
                    CodigoObra = item.CodigoObra,
                    CodigoPedidoCompra = item.CodigoPedidoCompra,
                    CodigoPedidoInterno = item.CodigoPedidoInterno,
                    DataLancamento = item.DataLancamento,
                    DataPagamento = item.DataPagamento,
                    Id = item.Id,
                    IdCliente = item.IdCliente,
                    IdDef = item.IdDef,
                    IdFornecedorBeneficiario = item.IdFornecedorBeneficiario,
                    IdObra = item.IdObra,
                    IdPedidoCompra = item.IdPedidoCompra,
                    IdPedidoInterno = item.IdPedidoInterno,
                    IdUsuarioBeneficiario = item.IdUsuarioBeneficiario,
                    NomeCliente = item.NomeCliente,
                    NomeFantasiaFornecedor = item.Fornecedor?.NomeFantasia ?? "",
                    NomeUsuarioBeneficiario = item.UsuarioBeneficiario?.Nome ?? "",
                    IdPedidoCompraNotaFiscal = item.IdPedidoCompraNotaFiscal,
                    NotaFiscalServico = relacaoPedidoCompraTipoServico.FirstOrDefault(x => x.Id == (item.IdPedidoCompra ?? 0))?.Servico ?? false,
                    NumeroNF = item.NumeroNotaFiscalPedidoCompra,
                    PagamentoEfetuado = item.PagamentoEfetuado,
                    Valor = item.Valor,
                    Def = defs.FirstOrDefault(x => x.Id == item.IdDef),
                    DataPagamentoEfetuado = item.DataPagamentoEfetuado,
                    Cancelado=item.Cancelado
                });
            });

            return result;
        }

        public List<Agenda_CarimboResultDTO> ObtemNotasComCarimbo(Agenda_ExcelCarimboRequestDTO parametros)
        {
            var dataInicial = parametros.DataInicial?.Date ?? DateTime.Today;
            var dataFinal = parametros.DataFinal?.Date ?? dataInicial.AddMonths(1);

            var sql = @"SELECT
                            O.Codigo AS CodigoObra,
                            P.Codigo AS CodigoPedido,
                            PNF.NumeroNotaFiscal,
                            PNFP.ValorBruto,
                            PNFP.ValorMaterialAbatido,
                            PNFP.ValorArt30,
                            PNFP.ValorINSS,
                            PNFP.DataPagamentoINSS,
                            PNFP.ValorISS,
                            PNFP.DataPagamentoISS,      
                            PNFP.ValorIR,
                            PNFP.DataPagamentoIR,
                            (PNFP.ValorBruto - PNFP.ValorArt30 - PNFP.ValorINSS - PNFP.ValorISS - PNFP.ValorIR) AS ValorLiquido,
                            PNFP.DataPagamentoArt30,
							F.RazaoSocial as fornecedor,
							F.CNPJ as cnpj
                        FROM PedidoCompra P
						INNER JOIN Fornecedor F on F.Id = P.IdFornecedor
                        INNER JOIN Obra O ON O.Id = P.IdCentroCustoObra
                        INNER JOIN PedidoCompra_NotaFiscal PNF on PNF.IdPedidoCompra = P.Id
                        INNER JOIN PedidoCompra_NotaFiscal_Pagamento PNFP on PNFP.IdPedidoCompraNotaFiscal = PNF.Id
                        WHERE PNFP.DataPagamentoArt30 >= @dataInicial AND PNFP.DataPagamentoArt30 <= @dataFinal";

            var result = new List<Agenda_CarimboResultDTO>();

            using var connection = _context.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@dataInicial", dataInicial.ToString("yyyy-MM-dd HH:mm:ss")));
            command.Parameters.Add(new SqlParameter("@dataFinal", dataFinal.ToString("yyyy-MM-dd HH:mm:ss")));

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dto = new Agenda_CarimboResultDTO
                {
                    // Ajuste conforme os nomes reais das propriedades do seu DTO
                    Obra = reader["CodigoObra"]?.ToString(),
                    PedidoCompra = reader["CodigoPedido"]?.ToString(),
                    NotaFiscal = reader["NumeroNotaFiscal"]?.ToString(),
                    fornecedor = reader["fornecedor"]?.ToString(),
                    cnpj = reader["cnpj"]?.ToString(),
                    ValorBruto = reader["ValorBruto"] as double? ?? 0,
                    ValorMaterialAbatido = reader["ValorMaterialAbatido"] as double? ?? 0,
                    ValorArt30 = reader["ValorArt30"] as double? ?? 0,
                    ValorINSS = reader["ValorINSS"] as double? ?? 0,
                    ValorISS = reader["ValorISS"] as double? ?? 0,
                    ValorIR = reader["ValorIR"] as double? ?? 0,
                    ValorLiquido = reader["ValorLiquido"] as double? ?? 0,
                    DataPagamentoINSS = reader["DataPagamentoINSS"] as DateTime? ?? DateTime.MinValue,
                    DataPagamentoIR = reader["DataPagamentoIR"] as DateTime? ?? DateTime.MinValue,
                    DataPagamentoISS = reader["DataPagamentoISS"] as DateTime? ?? DateTime.MinValue,
                    DataPagamentoArt30 = reader["DataPagamentoArt30"] as DateTime? ?? DateTime.MinValue
                };

                result.Add(dto);
            }

            return result;
        }

        public async Task<List<long>> ObtemIdsPedidosComNotasComCarimbos(Agenda_ExcelCarimboRequestDTO parametros)
        {
            var dataInicial = parametros.DataInicial?.Date ?? DateTime.Today;
            var dataFinal = parametros.DataFinal?.Date ?? dataInicial.AddMonths(1);

            return await _context.PedidoCompra_NotaFiscal_Pagamentos.Where(x =>  x.DataPagamentoArt30.Value.Date >= dataInicial.Date && x.DataPagamentoArt30.Value.Date <= dataFinal.Date).Select(x => x.IdPedidoCompraNotaFiscal).ToListAsync();
        }

        public async Task AjusteManual(Int64 idFluxoCaixa, List<Agenda_AjusteManualRequestDTO> parametros)
        {
            var fluxoCaixa = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == idFluxoCaixa);

            if (fluxoCaixa == null)
                throw new Exception("Registro inexistente");

            parametros.OrderBy(x => x.data).ToList().ForEach(x =>
            {
                var novoFluxoCaixa = new FluxoCaixa();
                novoFluxoCaixa.Id = 0;
                novoFluxoCaixa.CodigoDef = fluxoCaixa.CodigoDef;
                novoFluxoCaixa.CodigoFatura = fluxoCaixa.CodigoFatura;
                novoFluxoCaixa.CodigoObra = fluxoCaixa.CodigoObra;
                novoFluxoCaixa.CodigoPedidoCompra = fluxoCaixa.CodigoPedidoCompra;
                novoFluxoCaixa.CodigoPedidoInterno = fluxoCaixa.CodigoPedidoInterno;
                novoFluxoCaixa.DataLancamento = fluxoCaixa.DataLancamento;
                novoFluxoCaixa.DataPagamento = x.data;
                novoFluxoCaixa.DataPagamentoEfetuado = null;
                novoFluxoCaixa.Fornecedor = fluxoCaixa.Fornecedor;
                novoFluxoCaixa.IdCliente = fluxoCaixa.IdCliente;
                novoFluxoCaixa.IdDef = fluxoCaixa.IdDef;
                novoFluxoCaixa.IdFaturamento = fluxoCaixa.IdFaturamento;
                novoFluxoCaixa.IdFornecedorBeneficiario = fluxoCaixa.IdFornecedorBeneficiario;
                novoFluxoCaixa.IdObra = fluxoCaixa.IdObra;
                novoFluxoCaixa.IdPedidoCompra = fluxoCaixa.IdPedidoCompra;
                novoFluxoCaixa.IdPedidoCompraFatura = fluxoCaixa.IdPedidoCompraFatura;
                novoFluxoCaixa.IdPedidoCompraNotaFiscal = fluxoCaixa.IdPedidoCompraNotaFiscal;
                novoFluxoCaixa.IdPedidoInterno = fluxoCaixa.IdPedidoInterno;
                novoFluxoCaixa.IdTipoFluxoCaixa = fluxoCaixa.IdTipoFluxoCaixa;
                novoFluxoCaixa.IdUsuarioBeneficiario = fluxoCaixa.IdUsuarioBeneficiario;
                novoFluxoCaixa.IdUsuarioInformouPagamento = fluxoCaixa.IdUsuarioInformouPagamento;
                novoFluxoCaixa.NomeCliente = fluxoCaixa.NomeCliente;
                novoFluxoCaixa.NumeroNotaFiscalFaturamento = fluxoCaixa.NumeroNotaFiscalFaturamento;
                novoFluxoCaixa.NumeroNotaFiscalPedidoCompra = fluxoCaixa.NumeroNotaFiscalPedidoCompra;
                novoFluxoCaixa.PagamentoEfetuado = fluxoCaixa.PagamentoEfetuado;
                novoFluxoCaixa.UsuarioBeneficiario = fluxoCaixa.UsuarioBeneficiario;
                novoFluxoCaixa.UsuarioPagamento = fluxoCaixa.UsuarioPagamento;
                novoFluxoCaixa.Valor = x.valor;

                _context.FluxoCaixa.Add(novoFluxoCaixa);
            });

            _context.FluxoCaixa.Remove(fluxoCaixa);

            await _context.SaveChangesAsync();
        }

        public async Task<Agenda_ExcelFaturamento_ResponseDTO> ExcelFaturamento(Agenda_ExcelFaturamento_RequestDTO parametros)
        {
            var result = new Agenda_ExcelFaturamento_ResponseDTO();

            #region AFaturar 

            var def = await _context.DEF.FirstOrDefaultAsync(x => x.Codigo == "02.10");

            var queryAFaturar = _context.FluxoCaixa
                .Include(x => x.Obra)
                .ThenInclude(Obra => Obra.Cliente)
                .Include(x => x.FluxosCaixaFilhos)
                .AsQueryable();

            if (parametros.dataInicial.HasValue)
                queryAFaturar = queryAFaturar.Where(x => x.DataPagamento.Date >= parametros.dataInicial.Value.Date);

            if (parametros.dataFinal.HasValue)
                queryAFaturar = queryAFaturar.Where(x => x.DataPagamento.Date <= parametros.dataFinal.Value.Date);

            if (parametros.idsObras != null && parametros.idsObras.Count>0)
                queryAFaturar = queryAFaturar.Where(x => parametros.idsObras.Contains(x.IdObra.Value));

            queryAFaturar = queryAFaturar
                .Where(fc => fc.IdDef == def.Id)
                .OrderBy(x => x.DataPagamento);


            result.AFaturar = await queryAFaturar.AsNoTracking().Select(fc => new Faturamento_ValoresAFaturarDTO
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

            #endregion

            #region Faturado

            var queryFaturado = _context.Faturamento.AsQueryable();

            queryFaturado = queryFaturado.Where(x => x.Obra.Cancelada == false);

            if(parametros.dataInicial.HasValue)
                queryFaturado = queryFaturado.Where(x => x.DataFaturamento.Date >= parametros.dataInicial.Value.Date);

            if(parametros.dataFinal.HasValue)
                queryFaturado = queryFaturado.Where(x => x.DataFaturamento.Date <= parametros.dataFinal.Value.Date);

            if (parametros.idsObras != null && parametros.idsObras.Count > 0)
                queryFaturado = queryFaturado.Where(x => parametros.idsObras.Contains(x.IdObra));

            queryFaturado = queryFaturado.AsNoTracking();

            queryFaturado = queryFaturado
                .Include(x => x.Obra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Status);

            result.FaturadoRecebido = _mapper.Map<List<FaturamentoDTO>>(await queryFaturado.Where(x => x.IdStatusFaturamento == 1).ToListAsync());
            result.FaturadoAReceber = _mapper.Map<List<FaturamentoDTO>>(await queryFaturado.Where(x => x.IdStatusFaturamento == 2).ToListAsync());

            #endregion

            return result;
        }
    }
}
