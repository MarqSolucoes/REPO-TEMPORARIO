using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoMotivoDevolucao
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioMotivoDevolucao _repositorioMotivoDevolucao;

        public ServicoMotivoDevolucao(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioMotivoDevolucao = new RepositorioMotivoDevolucao(_context, _mapper);
        }

        public async Task<List<MotivoDevolucaoSaldoDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioMotivoDevolucao.Get(apenasAtivos);
        }

        public async Task<MotivoDevolucaoSaldoDTO> Get(Int64 Id)
        {
            return await _repositorioMotivoDevolucao.Get(Id);
        }

        public async Task<MotivoDevolucaoSaldoDTO> Post(MotivoDevolucaoSaldoDTO motivoDevolucaoSaldoDTO)
        {
            if (string.IsNullOrEmpty(motivoDevolucaoSaldoDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioMotivoDevolucao.VerificaDescricaoExistente(motivoDevolucaoSaldoDTO.Descricao))
                throw new Exception($"A descrição '{motivoDevolucaoSaldoDTO.Descricao}' já existe");

            return await _repositorioMotivoDevolucao.Post(motivoDevolucaoSaldoDTO);
        }

        public async Task<MotivoDevolucaoSaldoDTO> Put(MotivoDevolucaoSaldoDTO motivoDevolucaoSaldoDTO)
        {
            if (string.IsNullOrEmpty(motivoDevolucaoSaldoDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            if (await _repositorioMotivoDevolucao.VerificaDescricaoExistente(motivoDevolucaoSaldoDTO.Descricao, motivoDevolucaoSaldoDTO.Id))
                throw new Exception($"A descrição '{motivoDevolucaoSaldoDTO.Descricao}' já existe");

            return await _repositorioMotivoDevolucao.Put(motivoDevolucaoSaldoDTO);
        }
    }
}
