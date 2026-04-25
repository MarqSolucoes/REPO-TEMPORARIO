using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.Material;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoMaterial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioMaterial _repositorioMaterial;

        public ServicoMaterial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioMaterial = new RepositorioMaterial(_context, _mapper);
        }

        public async Task<List<MaterialDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioMaterial.Get(apenasAtivos);
        }

        public async Task<PaginacaoResultDTO<MaterialDTO>> ObtemMateriaisPaginados(PaginacaoDTO paginacao)
        {
            return await _repositorioMaterial.ObtemMateriaisPaginados(paginacao);
        }

        public async Task<MaterialDTO> Get(Int64 Id)
        {
            return await _repositorioMaterial.Get(Id);
        }

        public async Task<MaterialDTO> Post(MaterialDTO materialDTO)
        {
            if (string.IsNullOrEmpty(materialDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioMaterial.VerificaDescricaoExistente(materialDTO.Descricao))
                throw new Exception($"A descrição '{materialDTO.Descricao}' já existe");

            return await _repositorioMaterial.Post(materialDTO);
        }

        public async Task<MaterialDTO> Put(MaterialDTO materialDTO)
        {
            if (string.IsNullOrEmpty(materialDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioMaterial.VerificaDescricaoExistente(materialDTO.Descricao, materialDTO.Id))
                throw new Exception($"A descrição '{materialDTO.Descricao}' já existe");

            return await _repositorioMaterial.Put(materialDTO);
        }

        public async Task<List<UnidadeMaterialDTO>> Unidades()
        {
            return await _repositorioMaterial.Unidades();
        }

        public async Task<List<MaterialDTO>> BuscaMaterialPorTexto(string texto)
        {
            return await _repositorioMaterial.BuscaMaterialPorTexto(texto);
        }

        public async Task<List<Material_HistoricoCompraDTO>> ObtemHistoricoDeCompra(Int64 idMaterial)
        {
            return await _repositorioMaterial.ObtemHistoricoDeCompra(idMaterial);
        }
    }
}
