using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoUsuario
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioUsuario _repositorioUsuario;

        public ServicoUsuario(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioUsuario = new RepositorioUsuario(_context, _mapper);
        }

        public async Task<List<UsuarioDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioUsuario.Get(apenasAtivos);
        }

        public async Task<UsuarioDTO> Get(Int64 Id)
        {
            return await _repositorioUsuario.Get(Id);
        }

        public async Task<UsuarioDTO> Post(UsuarioDTO usuarioDTO)
        {
            if (string.IsNullOrEmpty(usuarioDTO.Nome.Trim()))
                throw new Exception("O campo 'Nome' deve ser informado");

            if (string.IsNullOrEmpty(usuarioDTO.Login.Trim()))
                throw new Exception("O campo 'Login' deve ser informado");

            if (string.IsNullOrEmpty(usuarioDTO.Senha.Trim()))
                throw new Exception("O campo 'Senha' deve ser informado");

            if (string.IsNullOrEmpty(usuarioDTO.Email.Trim()))
                throw new Exception("O campo 'Email' deve ser informado");

            if (await _repositorioUsuario.VerificaLoginExistente(usuarioDTO.Login))
                throw new Exception($"O login '{usuarioDTO.Login}' já existe");

            return await _repositorioUsuario.Post(usuarioDTO);
        }

        public async Task<UsuarioDTO> Put(UsuarioDTO usuarioDTO)
        {
            if (string.IsNullOrEmpty(usuarioDTO.Nome.Trim()))
                throw new Exception("O campo 'Nome' deve ser informado");

            if (string.IsNullOrEmpty(usuarioDTO.Login.Trim()))
                throw new Exception("O campo 'Login' deve ser informado");

            if (await _repositorioUsuario.VerificaLoginExistente(usuarioDTO.Login, usuarioDTO.Id))
                throw new Exception($"O login '{usuarioDTO.Login}' já existe");

            return await _repositorioUsuario.Put(usuarioDTO);
        }

        public async Task<List<UsuarioDTO>> Engenheiros()
        {
            return await _repositorioUsuario.Engenheiros();
        }

        public async Task<List<UsuarioDTO>> Diretores()
        {
            return await _repositorioUsuario.Diretores();
        }
    }
}
