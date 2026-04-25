using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioUsuario
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioUsuario(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<UsuarioDTO>>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).Where(x => x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<UsuarioDTO>>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).ToListAsync());
        }

        public async Task<UsuarioDTO> Get(Int64 Id)
        {
            return _mapper.Map<UsuarioDTO>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<UsuarioDTO> Post(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario();
            usuario.Ativo = usuarioDTO.Ativo;
            usuario.DataCadastro = usuarioDTO.DataCadastro;
            usuario.DataUltimaAlteracao = usuarioDTO.DataUltimaAlteracao;
            usuario.HabilitaLogin = usuarioDTO.HabilitaLogin;
            usuario.Id = 0;
            usuario.IdCargo = usuarioDTO.IdCargo;
            usuario.IdUsuarioAlteracao = usuarioDTO.IdUsuarioAlteracao;
            usuario.IdUsuarioCadastro = usuarioDTO.IdUsuarioCadastro;
            usuario.Login = usuarioDTO.Login;
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = usuarioDTO.Senha;
            usuario.Email=usuarioDTO.Email;

            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> Put(UsuarioDTO usuarioDTO)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == usuarioDTO.Id);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            usuario.Ativo = usuarioDTO.Ativo;
            usuario.HabilitaLogin = usuarioDTO.HabilitaLogin;
            usuario.IdCargo = usuarioDTO.IdCargo;
            usuario.Login = usuarioDTO.Login;
            usuario.Nome = usuarioDTO.Nome;
            usuario.Email=usuarioDTO.Email;
            usuario.Senha = usuarioDTO.Senha == null ? usuario.Senha : usuarioDTO.Senha;
            usuario.DataUltimaAlteracao = usuarioDTO.DataUltimaAlteracao;
            usuario.IdUsuarioAlteracao = usuarioDTO.IdUsuarioAlteracao;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<bool> VerificaLoginExistente(string login, Int64? id = null)
        {
            if (id.HasValue)
                return await _context.Usuario.AnyAsync(x => x.Login == login && x.Id != id);
            else
                return await _context.Usuario.AnyAsync(x => x.Login == login);
        }

        public async Task<List<UsuarioDTO>> Engenheiros()
        {
            return _mapper.Map<List<UsuarioDTO>>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).Where(x => x.Ativo && x.CargoUsuario.Descricao == "Engenheiro").ToListAsync());
        }

        public async Task<List<UsuarioDTO>> Diretores()
        {
            //Inicialmente pensado para ser apenas diretores, porém houve a necessidade de escolher outro usuário além de diretor.
            //return _mapper.Map<List<UsuarioDTO>>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).Where(x => x.Ativo && x.CargoUsuario.Descricao == "Diretor").ToListAsync());

            return _mapper.Map<List<UsuarioDTO>>(await _context.Usuario.AsNoTracking().Include(x => x.CargoUsuario).Where(x => x.Ativo).OrderBy(x=>x.Nome).ToListAsync());
        }
    }
}
