using AutoMapper;
using System;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoAutenticacao
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioAutenticacao _repositorioAutenticacao;

        public ServicoAutenticacao(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAutenticacao = new RepositorioAutenticacao(_context, _mapper);
        }

        public UsuarioDTO VerificaLogin(string usuario, string senha)
        {
            try
            {
                return _repositorioAutenticacao.VerificaLogin(usuario, senha);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
