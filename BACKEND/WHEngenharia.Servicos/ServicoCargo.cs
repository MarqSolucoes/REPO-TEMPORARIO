using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoCargo
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioCargo _repositorioCargo;

        public ServicoCargo(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCargo = new RepositorioCargo(_context, _mapper);
        }

        public async Task<List<CargoDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCargo.Get(apenasAtivos);
        }

        public async Task<CargoDTO> Get(Int64 Id)
        {
            return await _repositorioCargo.Get(Id);
        }

        public async Task<CargoDTO> Post(CargoDTO cargoDTO)
        {
            if (string.IsNullOrEmpty(cargoDTO.Descricao.Trim()))
                throw new Exception("O campo'Descrição' deve ser informado");

            if (await _repositorioCargo.VerificaDescricaoExistente(cargoDTO.Descricao))
                throw new Exception($"A descrição '{cargoDTO.Descricao}' já existe");

            return await _repositorioCargo.Post(cargoDTO);
        }

        public async Task<CargoDTO> Put(CargoDTO cargoDTO)
        {
            if (string.IsNullOrEmpty(cargoDTO.Descricao.Trim()))
                throw new Exception("O campo'Descrição' deve ser informado");

            if (await _repositorioCargo.VerificaDescricaoExistente(cargoDTO.Descricao, cargoDTO.Id))
                throw new Exception($"A descrição '{cargoDTO.Descricao}' já existe");

            return await _repositorioCargo.Put(cargoDTO);
        }
    }
}
