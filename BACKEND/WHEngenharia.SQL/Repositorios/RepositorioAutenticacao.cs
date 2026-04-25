using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioAutenticacao
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioAutenticacao(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UsuarioDTO VerificaLogin(string usuario, string senha)
        {
               return _mapper.Map<UsuarioDTO>(_context.Usuario.FirstOrDefault(x => x.Login == usuario && x.Senha == senha && x.HabilitaLogin));
        }
    }
}
