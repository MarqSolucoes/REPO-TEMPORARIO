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
    public class ServicoDef
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioDef _repositorioDef;

        public ServicoDef(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioDef = new RepositorioDef(_context, _mapper);
        }

        public async Task<List<DEFDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioDef.Get(apenasAtivos);
        }

        public async Task<DEFDTO> Get(Int64 Id)
        {
            return await _repositorioDef.Get(Id);
        }

        public async Task<DEFDTO> Post(DEFDTO defDTO)
        {
            if (string.IsNullOrEmpty(defDTO.Codigo.Trim()))
                throw new Exception("O campo 'Código' deve ser informado");

            if (string.IsNullOrEmpty(defDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            return await _repositorioDef.Post(defDTO);
        }

        public async Task<DEFDTO> Put(DEFDTO defDTO)
        {
            if (string.IsNullOrEmpty(defDTO.Codigo.Trim()))
                throw new Exception("O campo 'Código' deve ser informado");

            if (string.IsNullOrEmpty(defDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            return await _repositorioDef.Put(defDTO);
        }
    }
}
