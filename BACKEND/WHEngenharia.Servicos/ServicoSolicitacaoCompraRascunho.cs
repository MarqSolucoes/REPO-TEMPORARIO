using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;
using WHEngenharia.SQL;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.Servicos
{
    public class ServicoSolicitacaoCompraRascunho
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioSolicitacaoCompraRascunho _repositorioSolicitacaoCompraRascunho;

        public ServicoSolicitacaoCompraRascunho(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioSolicitacaoCompraRascunho = new RepositorioSolicitacaoCompraRascunho(_context, _mapper);
        }

        public async Task<List<SolicitacaoCompraRascunhoDTO>> Get(Int64 IdUsuario)
        {
            return await _repositorioSolicitacaoCompraRascunho.Get(IdUsuario);
        }

        public async Task Delete(Int64 Id)
        {
            await _repositorioSolicitacaoCompraRascunho.Delete(Id);
        }

        public async Task Post(SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            await _repositorioSolicitacaoCompraRascunho.Post(solicitacaoCompraRascunhoDTO);
        }

        public async Task Put(SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            await _repositorioSolicitacaoCompraRascunho.Put(solicitacaoCompraRascunhoDTO);
        }
    }
}
