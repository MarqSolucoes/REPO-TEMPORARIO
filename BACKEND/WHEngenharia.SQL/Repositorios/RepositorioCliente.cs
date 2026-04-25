using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioCliente
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioCliente(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClienteDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<ClienteDTO>>(await _context.Cliente.Include(x => x.Cidade).AsNoTracking().Where(x => x.Ativo).OrderBy(x => x.RazaoSocial).ToListAsync());
            else
                return _mapper.Map<List<ClienteDTO>>(await _context.Cliente.Include(x => x.Cidade).AsNoTracking().OrderBy(x => x.RazaoSocial).ToListAsync());
        }

        public async Task<ClienteDTO> Get(Int64 Id)
        {
            return _mapper.Map<ClienteDTO>(await _context.Cliente.Include(x => x.Cidade).AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<ClienteDTO> Post(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente();
            cliente.Ativo = clienteDTO.Ativo;
            cliente.Bairro = clienteDTO.Bairro;
            cliente.CEP = clienteDTO.CEP;
            cliente.CNPJ = clienteDTO.CNPJ;
            cliente.DataCadastro = clienteDTO.DataCadastro;
            cliente.DataUltimaAlteracao = clienteDTO.DataUltimaAlteracao;
            cliente.DiasDePagamento = clienteDTO.DiasDePagamento;
            cliente.EmailComercial = clienteDTO.EmailComercial;
            cliente.EmailFinanceiro = clienteDTO.EmailFinanceiro;
            cliente.Endereco = clienteDTO.Endereco;
            cliente.Id = 0;
            cliente.IdCidade = clienteDTO.IdCidade;
            cliente.IdUsuarioAlteracao = clienteDTO.IdUsuarioAlteracao;
            cliente.IdUsuarioCadastro = clienteDTO.IdUsuarioCadastro;
            cliente.InscricaoEstadual = clienteDTO.InscricaoEstadual;
            cliente.NomeFantasia = clienteDTO.NomeFantasia;
            cliente.Observacao = clienteDTO.Observacao;
            cliente.RazaoSocial = clienteDTO.RazaoSocial;
            cliente.ResponsavelComercial = clienteDTO.ResponsavelComercial;
            cliente.TelefoneCelular = clienteDTO.TelefoneCelular;
            cliente.TelefoneFixo = clienteDTO.TelefoneFixo;

            await _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> Put(ClienteDTO clienteDTO)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.Id == clienteDTO.Id);

            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            cliente.Ativo = clienteDTO.Ativo;
            cliente.Bairro = clienteDTO.Bairro;
            cliente.CEP = clienteDTO.CEP;
            cliente.DataUltimaAlteracao = clienteDTO.DataUltimaAlteracao;
            cliente.DiasDePagamento = clienteDTO.DiasDePagamento;
            cliente.EmailComercial = clienteDTO.EmailComercial;
            cliente.EmailFinanceiro = clienteDTO.EmailFinanceiro;
            cliente.Endereco = clienteDTO.Endereco;
            cliente.IdCidade = clienteDTO.IdCidade;
            cliente.IdUsuarioAlteracao = clienteDTO.IdUsuarioAlteracao;
            cliente.InscricaoEstadual = clienteDTO.InscricaoEstadual;
            cliente.NomeFantasia = clienteDTO.NomeFantasia;
            cliente.Observacao = clienteDTO.Observacao;
            cliente.RazaoSocial = clienteDTO.RazaoSocial;
            cliente.ResponsavelComercial = clienteDTO.ResponsavelComercial;
            cliente.TelefoneCelular = clienteDTO.TelefoneCelular;
            cliente.TelefoneFixo = clienteDTO.TelefoneFixo;

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<bool> VerificaCNPJExistente(string cnpj)
        {
            return await _context.Cliente.AnyAsync(x => x.CNPJ == cnpj);
        }
    }
}
