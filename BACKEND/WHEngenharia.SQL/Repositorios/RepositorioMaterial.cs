using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.Material;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioMaterial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioMaterial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MaterialDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<MaterialDTO>>(await _context.Material.Include(x => x.CategoriaMaterial).Include(x=>x.UnidadeMaterial).AsNoTracking().Where(x=>x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<MaterialDTO>>(await _context.Material.Include(x => x.CategoriaMaterial).Include(x => x.UnidadeMaterial).AsNoTracking().ToListAsync());
        }

        public async Task<PaginacaoResultDTO<MaterialDTO>> ObtemMateriaisPaginados(PaginacaoDTO paginacao)
        {
            // Inicializa a consulta básica
            var query = _context.Material.AsQueryable();

            // Inclui as tabelas relacionadas (CategoriaMaterial e UnidadeMaterial)
            query = query.Include(x => x.CategoriaMaterial).Include(x => x.UnidadeMaterial).AsNoTracking();

            if (!string.IsNullOrEmpty(paginacao.descricao))
                query = query.Where(x => x.Descricao.StartsWith(paginacao.descricao));

            // Conta o total de itens antes de aplicar a paginação
            var totalItems = await query.CountAsync();

            // Aplica a paginação na consulta
            var materiais = await query
                .Skip(paginacao.skip)
                .Take(paginacao.take)
                .ToListAsync();

            // Mapeia os dados para o DTO
            var mappedItems = _mapper.Map<List<MaterialDTO>>(materiais);

            // Cria o resultado com total de itens e a lista de itens mapeados
            var result = new PaginacaoResultDTO<MaterialDTO>
            {
                total = totalItems, // Total de itens da tabela
                items = mappedItems // Lista de materiais paginados
            };

            return result;
        }

        public async Task<MaterialDTO> Get(Int64 Id)
        {
            return _mapper.Map<MaterialDTO>(await _context.Material.Include(x=>x.CategoriaMaterial).Include(x => x.UnidadeMaterial).AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<MaterialDTO> Post(MaterialDTO materialDTO)
        {
            var material = new Material();
            material.Ativo = materialDTO.Ativo;
            material.DataCadastro = materialDTO.DataCadastro;
            material.DataUltimaAlteracao = materialDTO.DataUltimaAlteracao;
            material.Descricao = materialDTO.Descricao;
            material.Id = 0;
            material.IdUnidadeMaterial = materialDTO.IdUnidadeMaterial;
            material.IdCategoriaMaterial = materialDTO.IdCategoriaMaterial;
            material.IdUsuarioAlteracao = materialDTO.IdUsuarioAlteracao;
            material.IdUsuarioCadastro = materialDTO.IdUsuarioCadastro;
            material.Servico = materialDTO.Servico;

            await _context.Material.AddAsync(material);
            await _context.SaveChangesAsync();

            return _mapper.Map<MaterialDTO>(material);
        }

        public async Task<MaterialDTO> Put(MaterialDTO materialDTO)
        {
            var material = await _context.Material.FirstOrDefaultAsync(x => x.Id == materialDTO.Id);

            if (material == null)
                throw new Exception("Material não encontrado");

            material.Ativo = materialDTO.Ativo;
            material.Descricao = materialDTO.Descricao;
            material.DataUltimaAlteracao = materialDTO.DataUltimaAlteracao;
            material.IdUnidadeMaterial = materialDTO.IdUnidadeMaterial;
            material.IdUsuarioAlteracao = materialDTO.IdUsuarioAlteracao;
            material.IdCategoriaMaterial = materialDTO.IdCategoriaMaterial;
            material.Servico = materialDTO.Servico;

            _context.Entry(material).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<MaterialDTO>(material);
        }

        public async Task<bool> VerificaDescricaoExistente(string descricao, Int64? id = null)
        {
            if (id.HasValue)
                return await _context.Material.AnyAsync(x => x.Descricao == descricao && x.Id != id);
            else
                return await _context.Material.AnyAsync(x => x.Descricao == descricao);
        }

        public async Task<List<UnidadeMaterialDTO>> Unidades()
        {
            return _mapper.Map<List<UnidadeMaterialDTO>>(await _context.UnidadeMaterial.ToListAsync());
        }

        public async Task<List<MaterialDTO>> BuscaMaterialPorTexto(string texto)
        {
            var listaDeMateriais = await _context.Material.Include(x => x.CategoriaMaterial).Include(x => x.UnidadeMaterial).AsNoTracking().Where(x => x.Ativo && x.Descricao.StartsWith(texto)).OrderBy(x => x.Descricao).ToListAsync();
            var listaDeMateriaisContem = await _context.Material.Include(x => x.CategoriaMaterial).Include(x => x.UnidadeMaterial).AsNoTracking().Where(x => x.Ativo && x.Descricao.Contains(texto)).OrderBy(x => x.Descricao).ToListAsync();

            listaDeMateriais.ForEach(x =>
            {
                listaDeMateriaisContem.Remove(x);
            });

            listaDeMateriais.AddRange(listaDeMateriaisContem);

            return _mapper.Map<List<MaterialDTO>>(listaDeMateriais);
        }

        public async Task<List<Material_HistoricoCompraDTO>> ObtemHistoricoDeCompra(Int64 idMaterial)
        {
            var query =
            from nfm in _context.PedidoCompra_NotaFiscal_Materiais
            join pcm in _context.PedidoCompra_Materiais on nfm.IdPedidoCompraMateriais equals pcm.Id
            join pc in _context.PedidoCompra on pcm.IdPedidoCompra equals pc.Id
            join f in _context.Fornecedor on pc.IdFornecedor equals f.Id
            join m in _context.Material on pcm.IdMaterial equals m.Id
            join o in _context.Obra on pc.IdCentroCustoObra equals o.Id
            join c in _context.Cliente on o.IdCliente equals c.Id
            orderby pcm.DataCadastro
            where m.Id == idMaterial
            select new Material_HistoricoCompraDTO
            {
                Obra = o.Codigo,
                Cliente = c.NomeFantasia + " - " + c.CNPJ,
                Pedido = pc.Codigo,
                Material = m.Descricao,
                Fornecedor=f.NomeFantasia,
                Data = pcm.DataCadastro,
                QuantidadePedida = pcm.Quantidade,
                QuantidadeConciliada = nfm.Quantidade,
                Valor = nfm.Valor
            };

            return await query.ToListAsync();
        }
    }
}
