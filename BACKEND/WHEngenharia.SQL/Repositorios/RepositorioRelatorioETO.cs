using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Relatorio;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioRelatorioETO
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioRelatorioETO(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Relatorio_ControleEtoDTO>> Get(bool obraBloqueada)
        {
            var result = new List<Relatorio_ControleEtoDTO>();

            (await _context.Obra.Where(x => !x.Finalizada && (obraBloqueada ? true : !x.Bloqueada)).Select(x => new { x.Id, x.Codigo, x.Cliente.NomeFantasia, x.Descricao, x.ValorTotal, x.Bloqueada }).ToListAsync()).ForEach(x =>
           {
               var controleEtoDTO = new Relatorio_ControleEtoDTO();
               controleEtoDTO.ComentariosNaoVisualizados = false;
               controleEtoDTO.IdObra = x.Id;
               controleEtoDTO.Bloqueada = x.Bloqueada;
               controleEtoDTO.Codigo = x.Codigo;
               controleEtoDTO.Cliente = x.NomeFantasia;
               controleEtoDTO.Descricao = x.Descricao;
               controleEtoDTO.Custo = x.ValorTotal;

               _context.PedidoCompra.Include(y => y.NotasFiscais).Where(y => y.IdCentroCustoObra == x.Id && y.IdStatusPedidoCompra < 3).ToList().ForEach(pedidoCompra =>
               {
                   var valorNotas = pedidoCompra.NotasFiscais.Where(y => y.Aprovada == true).Sum(y => y.Valor);

                   controleEtoDTO.Gasto += valorNotas > pedidoCompra.ValorTotal ? valorNotas : pedidoCompra.ValorTotal;
               });

               _context.PedidoInterno.Include(y=>y.Obras).Where(y => y.Aprovado == true && y.Obras.Any(z => z.IdObra == x.Id)).ToList().ForEach(y =>
               {
                   controleEtoDTO.Gasto += y.Obras.Where(z => z.IdObra == x.Id).Sum(z => z.Valor);
               });

               controleEtoDTO.Saldo = controleEtoDTO.Custo - controleEtoDTO.Gasto;

               result.Add(controleEtoDTO);
           });

            result.Add(new Relatorio_ControleEtoDTO() { Cliente = "", Codigo = "", Custo = result.Sum(x => x.Custo), Descricao = "", Gasto = result.Sum(x => x.Gasto), IdObra = 0, Saldo = result.Sum(x => x.Saldo) });

            return result;
        }

        //public async Task<bool> Ajuste(Relatorio_ControleEtoAjusteDTO relatorioControleEtoAjusteDTO)
        //{
        //    if (string.IsNullOrEmpty(relatorioControleEtoAjusteDTO.Observacao))
        //        relatorioControleEtoAjusteDTO.Observacao = "Sem Observações";

        //    var obraAjuste = new ObraAjuste();
        //    obraAjuste.DataCadastro = DateTime.Now;
        //    obraAjuste.DataUltimaAlteracao = DateTime.Now;
        //    obraAjuste.Id = 0;
        //    obraAjuste.IdObra = relatorioControleEtoAjusteDTO.IdObra;
        //    obraAjuste.IdUsuarioAlteracao = relatorioControleEtoAjusteDTO.IdUsuario;
        //    obraAjuste.IdUsuarioCadastro = relatorioControleEtoAjusteDTO.IdUsuario;
        //    obraAjuste.Observacao = relatorioControleEtoAjusteDTO.Observacao;
        //    obraAjuste.Visualizado = false;

        //    await _context.ObraAjuste.AddAsync(obraAjuste);
        //    await _context.SaveChangesAsync();

        //    _context.ObraMedicao.Where(x => x.IdObra == relatorioControleEtoAjusteDTO.IdObra && x.IdObraAjuste.HasValue && x.Data.Date >= DateTime.Now.Date).ToList().ForEach(x =>
        //      {
        //          _context.ObraMedicao.Remove(x);
        //          _context.SaveChanges();
        //      });

        //    var count = 1;
        //    _context.ObraMedicao.Where(x => x.IdObra == relatorioControleEtoAjusteDTO.IdObra).OrderBy(x => x.Id).ToList().ForEach(x =>
        //      {
        //          _context.Entry(x).State = EntityState.Modified;
        //          _context.SaveChanges();

        //          count++;
        //      });

        //    relatorioControleEtoAjusteDTO.Datas.Where(x => x.Valor > 0).ToList().ForEach(x =>
        //    {
        //        var obraMedicao = new ObraFaturamento();
        //        obraMedicao.DataCadastro = DateTime.Now;
        //        obraMedicao.Data = x.Data;
        //        obraMedicao.DataUltimaAlteracao = DateTime.Now;
        //        obraMedicao.Id = 0;
        //        obraMedicao.IdObra = relatorioControleEtoAjusteDTO.IdObra;
        //        obraMedicao.IdObraAjuste = obraAjuste.Id;
        //        obraMedicao.IdUsuarioAlteracao = relatorioControleEtoAjusteDTO.IdUsuario;
        //        obraMedicao.IdUsuarioCadastro = relatorioControleEtoAjusteDTO.IdUsuario;
        //        obraMedicao.Valor = x.Valor;

        //        _context.ObraMedicao.Add(obraMedicao);
        //        _context.SaveChanges();

        //        count++;
        //    });

        //    return true;
        //}

        public async Task<bool> VisualizarComentario(Int64 idAjuste)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE Obra_Ajuste SET Visualizado = 1 WHERE Id = {idAjuste}");

            return true;
        }

        public async Task<List<ObraFaturamentoDTO>> ObtemAjustes(int idObra)
        {
            return _mapper.Map<List<ObraFaturamentoDTO>>(await _context.ObraMedicao.Where(x => x.IdObra == idObra && x.Data.Date >= DateTime.Now.Date).ToListAsync());
        }

        public async Task<List<ObraFaturamentoDTO>> ObtemAjustes(List<Int64> idsObras)
        {
            return _mapper.Map<List<ObraFaturamentoDTO>>(await _context.ObraMedicao.Where(x => idsObras.Contains(x.IdObra) && x.Data.Date >= DateTime.Now.Date).ToListAsync());
        }
    }
}
