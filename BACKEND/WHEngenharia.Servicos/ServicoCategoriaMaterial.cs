using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoCategoriaMaterial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioCategoriaMaterial _repositorioCategoriaMaterial;

        public ServicoCategoriaMaterial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCategoriaMaterial = new RepositorioCategoriaMaterial(_context, _mapper);
        }

        public async Task<List<CategoriaMaterialDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCategoriaMaterial.Get(apenasAtivos);
        }

        public async Task<CategoriaMaterialDTO> Get(Int64 Id)
        {
            return await _repositorioCategoriaMaterial.Get(Id);
        }

        public async Task<CategoriaMaterialDTO> Post(CategoriaMaterialDTO categoriaMaterialDTO)
        {
            if (string.IsNullOrEmpty(categoriaMaterialDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioCategoriaMaterial.VerificaDescricaoExistente(categoriaMaterialDTO.Descricao))
                throw new Exception($"A descrição '{categoriaMaterialDTO.Descricao}' já existe");

            return await _repositorioCategoriaMaterial.Post(categoriaMaterialDTO);
        }

        public async Task<CategoriaMaterialDTO> Put(CategoriaMaterialDTO categoriaMaterialDTO)
        {
            if (string.IsNullOrEmpty(categoriaMaterialDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioCategoriaMaterial.VerificaDescricaoExistente(categoriaMaterialDTO.Descricao, categoriaMaterialDTO.Id))
                throw new Exception($"A descrição '{categoriaMaterialDTO.Descricao}' já existe");

            return await _repositorioCategoriaMaterial.Put(categoriaMaterialDTO);
        }
    }
}
