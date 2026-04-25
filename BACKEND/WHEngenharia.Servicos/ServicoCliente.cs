using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoCliente
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioCliente _repositorioCliente;

        public ServicoCliente(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCliente = new RepositorioCliente(_context, _mapper);
        }

        public async Task<List<ClienteDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCliente.Get(apenasAtivos);
        }

        public async Task<ClienteDTO> Get(Int64 Id)
        {
            return await _repositorioCliente.Get(Id);
        }

        public async Task<ClienteDTO> Post(ClienteDTO clienteDTO)
        {
            if (string.IsNullOrEmpty(clienteDTO.RazaoSocial.Trim()))
                throw new Exception("O campo 'Razão Social' deve ser informado");

            if (string.IsNullOrEmpty(clienteDTO.NomeFantasia.Trim()))
                throw new Exception("O campo 'Nome Fantasia' deve ser informado");

            if (string.IsNullOrEmpty(clienteDTO.CNPJ.Trim()))
                throw new Exception("O campo 'CNPJ' deve ser informado");

            if (await _repositorioCliente.VerificaCNPJExistente(clienteDTO.CNPJ))
                throw new Exception($"O CNPJ '{clienteDTO.CNPJ}' já existe");

            return await _repositorioCliente.Post(clienteDTO);
        }

        public async Task<ClienteDTO> Put(ClienteDTO clienteDTO)
        {
            if (string.IsNullOrEmpty(clienteDTO.RazaoSocial.Trim()))
                throw new Exception("O campo 'Razão Social' deve ser informado");

            if (string.IsNullOrEmpty(clienteDTO.NomeFantasia.Trim()))
                throw new Exception("O campo 'Nome Fantasia' deve ser informado");

            return await _repositorioCliente.Put(clienteDTO);
        }
    }
}
