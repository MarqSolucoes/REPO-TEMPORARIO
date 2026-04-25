using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoInterno;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioPedidoInterno
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioAgenda _repositorioAgenda;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;
        private RepositorioObra _repositorioObra;

        public RepositorioPedidoInterno(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAgenda = new RepositorioAgenda(_context, _mapper);
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
            _repositorioObra = new RepositorioObra(_context, mapper);
        }

        public async Task<List<PedidoInternoDTO>> Get()
        {
            return _mapper.Map<List<PedidoInternoDTO>>(
                await _context.PedidoInterno.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.UsuarioAprovacao)
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Obras)
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .ToListAsync());
        }

        public async Task<List<PedidoInternoDTO>> ObtemPedidosInternosParaAprovacao(Int64 idUsuario)
        {
            return _mapper.Map<List<PedidoInternoDTO>>(
                await _context.PedidoInterno.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.UsuarioAprovacao)
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Obras)
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .Where(x => !x.Aprovado.HasValue && x.IdUsuarioAprovacao == idUsuario)
                .ToListAsync());
        }

        public async Task AprovarReprovarPedidoInterno(Int64 idPedidoInterno, bool valor)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoInterno SET Aprovado = {(valor ? 1 : 0)}, DataUltimaAlteracao = GETDATE(), DataAprovacao = GETDATE() WHERE Id = {idPedidoInterno}");
        }

        public async Task<PedidoInternoDTO> Get(Int64 Id)
        {
            return _mapper.Map<PedidoInternoDTO>(await _context.PedidoInterno.AsNoTracking()
                   .Include(x => x.Arquivos)
                   .ThenInclude(x => x.UsuarioCadastro)
                   .Include(x => x.UsuarioAprovacao)
                   .Include(x => x.FornecedorBeneficiario)
                   .Include(x => x.UsuarioBeneficiario)
                   .Include(x => x.UsuarioCadastro)
                   .Include(x => x.Obras)
                   .Include(x => x.Parcelas)
                   .Include(x=>x.Obra)
                   .Include(x=>x.DEF)
                   .FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<List<PedidoInternoDTO>> GetByUser(Int64 idUsuario)
        {
            return _mapper.Map<List<PedidoInternoDTO>>(await _context.PedidoInterno.AsNoTracking()
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.UsuarioAprovacao)
                .Include(x => x.FornecedorBeneficiario)
                .Include(x => x.UsuarioBeneficiario)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Obras)
                .Include(x => x.Parcelas)
                .ThenInclude(x => x.ParcelaObras)
                .Where(x => x.IdUsuarioCadastro == idUsuario).ToListAsync());
        }

        public async Task<PedidoInternoDTO> Post(PedidoInternoDTO pedidoInternoDTO)
        {
            var pedidoInterno = new PedidoInterno();
            pedidoInterno.Aprovado = pedidoInternoDTO.Aprovado;
            pedidoInterno.Codigo = (_context.PedidoInterno.Max(x => (int?)x.Codigo) ?? 63000)+1;
            pedidoInterno.CodigoFormatado = $"P{pedidoInterno.Codigo.ToString().PadLeft(6, '0')}";
            pedidoInterno.DataCadastro = pedidoInternoDTO.DataCadastro;
            pedidoInterno.DataUltimaAlteracao = pedidoInternoDTO.DataUltimaAlteracao;
            pedidoInterno.Descricao = pedidoInternoDTO.Descricao;
            pedidoInterno.Id = 0;
            pedidoInterno.IdFornecedorBeneficiario = pedidoInternoDTO.IdFornecedorBeneficiario;
            pedidoInterno.IdUsuarioAlteracao = pedidoInternoDTO.IdUsuarioAlteracao;
            pedidoInterno.Cancelado = false;

            // Caso SEJA o DEF 03.04 de garantia de obra, então atribuimos a obra especifica de garantia
            if (pedidoInternoDTO.IdDef == 19)
                pedidoInternoDTO.IdObra = _context.Obra.FirstOrDefault(x => x.Codigo == "O00001")?.Id ?? null;

            //Caso tenha uma obra e NÃO seja a obra de garantia, então busca-se o usuário de aprovação
            if (pedidoInternoDTO.IdObra.HasValue && pedidoInternoDTO.IdDef != 19)
                pedidoInterno.IdUsuarioAprovacao = _context.Obra.FirstOrDefault(x => x.Id == pedidoInternoDTO.IdObra)?.IdUsuarioDiretorAprovador ?? 0; 
            else
                pedidoInterno.IdUsuarioAprovacao = pedidoInternoDTO.IdUsuarioAprovacao;

            pedidoInterno.IdUsuarioBeneficiario = pedidoInternoDTO.IdUsuarioBeneficiario;
            pedidoInterno.IdUsuarioCadastro = pedidoInternoDTO.IdUsuarioCadastro;
            pedidoInterno.NumeroTotalParcelas = pedidoInternoDTO.NumeroTotalParcelas;
            pedidoInterno.ValorTotal = pedidoInternoDTO.ValorTotal;
            pedidoInterno.ImportadoParaFinanceiro = null;
            pedidoInterno.IdDef = pedidoInternoDTO.IdDef;
            pedidoInterno.IdObra = pedidoInternoDTO.IdObra; 

            await _context.PedidoInterno.AddAsync(pedidoInterno);
            await _context.SaveChangesAsync();

            var maisDeUmaParcela = pedidoInternoDTO.Parcelas.Count > 1 ? true : false;
            var intChar = 65;

            pedidoInternoDTO.Parcelas.ForEach(x =>
            {
                var pedidoInternoParcela = new PedidoInternoParcelas();
                pedidoInternoParcela.CodigoFormatado = $"P{pedidoInterno.Codigo.ToString().PadLeft(6, '0')}{(maisDeUmaParcela ? $".{Convert.ToChar(intChar)}" : "")}";
                pedidoInternoParcela.DataPagamento = x.DataPagamento;
                pedidoInternoParcela.Id = 0;
                pedidoInternoParcela.IdPedidoInterno = pedidoInterno.Id;
                pedidoInternoParcela.PagamentoEfetuado = false;
                pedidoInternoParcela.Parcela = x.Parcela;
                pedidoInternoParcela.Valor = x.Valor;

                _context.PedidoInternoParcelas.Add(pedidoInternoParcela);
                _context.SaveChanges();

                //x.ParcelaObras.ForEach(y =>
                //{
                //    var pedidoInternoParcelaObra = new PedidoInternoParcelaObras();
                //    pedidoInternoParcelaObra.Id = 0;
                //    pedidoInternoParcelaObra.IdObra = y.IdObra;
                //    pedidoInternoParcelaObra.IdPedidoInternoParcela = pedidoInternoParcela.Id;
                //    pedidoInternoParcelaObra.Valor = y.Valor;

                //    _context.PedidoInternoParcelaObras.Add(pedidoInternoParcelaObra);
                //    _context.SaveChanges();
                //});

                //x.ParcelaDEFs.ForEach(y =>
                //{
                //    var pedidoInternoParcelaDEF = new PedidoInternoParcelaDEFs();
                //    pedidoInternoParcelaDEF.Id = 0;
                //    pedidoInternoParcelaDEF.IdDef = y.IdDef;
                //    pedidoInternoParcelaDEF.IdPedidoInternoParcela = pedidoInternoParcela.Id;
                //    pedidoInternoParcelaDEF.Valor = y.Valor;

                //    _context.PedidoInternoParcelaDEFs.Add(pedidoInternoParcelaDEF);
                //    _context.SaveChanges();
                //});

                intChar++;
            });


            return _mapper.Map<PedidoInternoDTO>(await Get(pedidoInterno.Id));
        }

        public async Task<PedidoInternoDTO> Put(PedidoInternoDTO pedidoInternoDTO)
        {
            var pedidoInterno = await _context.PedidoInterno.FirstOrDefaultAsync(x => x.Id == pedidoInternoDTO.Id);

            if (pedidoInterno == null)
                throw new Exception("Pedido interno não encontrada");

            pedidoInterno.Aprovado = pedidoInternoDTO.Aprovado;
            pedidoInterno.ImportadoParaFinanceiro = pedidoInternoDTO.ImportadoParaFinanceiro;
            pedidoInterno.DataUltimaAlteracao = pedidoInternoDTO.DataUltimaAlteracao;
            pedidoInterno.Descricao = pedidoInternoDTO.Descricao;
            pedidoInterno.IdUsuarioAlteracao = pedidoInternoDTO.IdUsuarioAlteracao;

            _context.Entry(pedidoInterno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<PedidoInternoDTO>(pedidoInterno);
        }

        public async Task DefinirParcelaPaga(Int64 idParcela)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoInterno_Parcelas SET PagamentoEfetuado = 1 WHERE Id = {idParcela}");
        }

        public async Task AlterarParcela(PedidoInterno_AlteracaoParcelaDTO pedidoInternoAlteracaoDataPagamentoDTO)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoInterno_Parcelas SET Valor = {pedidoInternoAlteracaoDataPagamentoDTO.Valor.ToString().Replace(',','.')}, DataPagamento = '{pedidoInternoAlteracaoDataPagamentoDTO.DataPagamento.ToString("MM/dd/yyyy 00:00:00")}' WHERE Id =  {pedidoInternoAlteracaoDataPagamentoDTO.IdParcela}");
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoInterno SET ValorTotal = (SELECT SUM(PIP1.VALOR) FROM PedidoInterno_Parcelas PIP1 WHERE PIP1.IdPedidoInterno = (SELECT PIP2.IdPedidoInterno FROM PedidoInterno_Parcelas PIP2 WHERE PIP2.Id = {pedidoInternoAlteracaoDataPagamentoDTO.IdParcela}))  WHERE Id = (SELECT PIP3.IdPedidoInterno FROM PedidoInterno_Parcelas PIP3 WHERE PIP3.Id = {pedidoInternoAlteracaoDataPagamentoDTO.IdParcela})");
        }

        public async Task<string> ProximoCodigo()
        {
            return $"P{((await _context.PedidoInterno.MaxAsync(x => (int?)x.Codigo) ?? 0) + 1).ToString().PadLeft(6, '0')}";
        }

        public async Task<PedidoInternoArquivosDTO> Upload(PedidoInternoArquivosDTO pedidoInternoArquivosDTO)
        {
            if (_context.PedidoInterno_Arquivos.Any(x => x.IdPedidoInterno == pedidoInternoArquivosDTO.IdPedidoInterno && x.Nome == pedidoInternoArquivosDTO.Nome))
                throw new Exception("Já existe um arquivo com este nome");

            var pedidoInternoArquivos = new PedidoInternoArquivos();
            pedidoInternoArquivos.DataCadastro = pedidoInternoArquivosDTO.DataCadastro;
            pedidoInternoArquivos.Extensao = pedidoInternoArquivosDTO.Extensao;
            pedidoInternoArquivos.Id = pedidoInternoArquivosDTO.Id;
            pedidoInternoArquivos.IdPedidoInterno = pedidoInternoArquivosDTO.IdPedidoInterno;
            pedidoInternoArquivos.IdUsuarioCadastro = pedidoInternoArquivosDTO.IdUsuarioCadastro;
            pedidoInternoArquivos.Nome = pedidoInternoArquivosDTO.Nome;
            pedidoInternoArquivos.NomeLogico = pedidoInternoArquivosDTO.NomeLogico;
            pedidoInternoArquivos.TamanhoMB = pedidoInternoArquivosDTO.TamanhoMB;

            await _context.PedidoInterno_Arquivos.AddAsync(pedidoInternoArquivos);
            await _context.SaveChangesAsync();

            return _mapper.Map<PedidoInternoArquivosDTO>(pedidoInternoArquivos);
        }

        public async Task<PedidoInternoArquivosDTO> GetFile(Int64 id)
        {
            return _mapper.Map<PedidoInternoArquivosDTO>(await _context.PedidoInterno_Arquivos.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            var arquivo = await _context.PedidoInterno_Arquivos.FirstOrDefaultAsync(x => x.Id == idArquivo);

            if (arquivo == null)
                throw new Exception("Arquivo não encontrado");

            _context.PedidoInterno_Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PedidoInternoArquivosDTO>> ObtemArquivos(Int64 idPedidoInterno)
        {
            return _mapper.Map<List<PedidoInternoArquivosDTO>>(await _context.PedidoInterno_Arquivos.Include(x => x.UsuarioCadastro).Where(x => x.IdPedidoInterno == idPedidoInterno).ToListAsync());
        }
    }
}
