using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Fornecedor;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioFornecedor
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioFornecedor(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FornecedorDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<FornecedorDTO>>(await _context.Fornecedor.Include(x => x.TipoFornecedor).Include(x => x.Cidade).Include(x => x.CondicaoPagamento).AsNoTracking().Where(x => x.Ativo).OrderBy(x => x.NomeFantasia).ToListAsync());
            else
                return _mapper.Map<List<FornecedorDTO>>(await _context.Fornecedor.Include(x => x.TipoFornecedor).Include(x => x.Cidade).Include(x => x.CondicaoPagamento).AsNoTracking().OrderBy(x => x.NomeFantasia).ToListAsync());
        }

        public async Task<FornecedorDTO> Get(Int64 Id)
        {
            return _mapper.Map<FornecedorDTO>(await _context.Fornecedor.Include(x => x.TipoFornecedor).Include(x => x.Cidade).Include(x => x.CondicaoPagamento).AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<List<FornecedorDTO>> GetFiltrado(Fornecedor_FiltroDTO fornecedor_FiltroDTO)
        {
            var query = _context.Fornecedor.AsQueryable().AsNoTracking();

            query = query.Where(x => x.CNPJ.Contains(fornecedor_FiltroDTO.CNPJ));
            query = query.Where(x => x.Endereco.Contains(fornecedor_FiltroDTO.Endereco));
            query = query.Where(x => x.RazaoSocial.Contains(fornecedor_FiltroDTO.RazaoSocial));
            query = query.Where(x => x.NomeFantasia.Contains(fornecedor_FiltroDTO.NomeFantasia));

            query = query.Include(x => x.TipoFornecedor);
            query = query.Include(x => x.Cidade);
            query = query.Include(x => x.CondicaoPagamento);

            var fornecedores = await query.ToListAsync();
            return _mapper.Map<List<FornecedorDTO>>(fornecedores);
        }

        public async Task<FornecedorDTO> Post(FornecedorDTO fornecedorDTO)
        {
            try
            {
                if (!_context.CondicaoPagamento.Any(x => x.Id == fornecedorDTO.IdCondicaoPagamento))
                    throw new Exception("Condição de pagamento não encontrada");

                var fornecedor = new Fornecedor();
                fornecedor.Ativo = fornecedorDTO.Ativo;
                fornecedor.Bairro = fornecedorDTO.Bairro;
                fornecedor.CEP = fornecedorDTO.CEP;
                fornecedor.CNPJ = fornecedorDTO.CNPJ;
                fornecedor.CPF = fornecedorDTO.CPF;
                fornecedor.DataCadastro = fornecedorDTO.DataCadastro;
                fornecedor.DataUltimaAlteracao = fornecedorDTO.DataUltimaAlteracao;
                fornecedor.Email = fornecedorDTO.Email;
                fornecedor.Endereco = fornecedorDTO.Endereco;
                fornecedor.Id = 0;
                fornecedor.IdCidade = fornecedorDTO.IdCidade;
                fornecedor.IdCondicaoPagamento = fornecedorDTO.IdCondicaoPagamento;
                fornecedor.IdTipoFornecedor = fornecedorDTO.IdTipoFornecedor;
                fornecedor.IdUsuarioAlteracao = fornecedorDTO.IdUsuarioAlteracao;
                fornecedor.IdUsuarioCadastro = fornecedorDTO.IdUsuarioCadastro;
                fornecedor.InscricaoEstadual = fornecedorDTO.InscricaoEstadual;
                fornecedor.Nome = fornecedorDTO.Nome;
                fornecedor.NomeFantasia = fornecedorDTO.NomeFantasia;
                fornecedor.NomeVendedor = fornecedorDTO.NomeVendedor;
                fornecedor.NumeroCadastral = fornecedorDTO.NumeroCadastral;
                fornecedor.Observacao = fornecedorDTO.Observacao;
                fornecedor.RazaoSocial = fornecedorDTO.RazaoSocial;
                fornecedor.TelefoneCelular = fornecedorDTO.TelefoneCelular;
                fornecedor.TelefoneFixo = fornecedorDTO.TelefoneFixo;
                fornecedor.Banco = fornecedorDTO.Banco;
                fornecedor.Agencia = fornecedorDTO.Agencia;
                fornecedor.Conta = fornecedorDTO.Conta;
                fornecedor.TipoConta = fornecedorDTO.TipoConta;

                await _context.Fornecedor.AddAsync(fornecedor);
                await _context.SaveChangesAsync();

                return _mapper.Map<FornecedorDTO>(fornecedor);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<FornecedorDTO> Put(FornecedorDTO fornecedorDTO)
        {
            var fornecedor = await _context.Fornecedor.FirstOrDefaultAsync(x => x.Id == fornecedorDTO.Id);

            if (fornecedor == null)
                throw new Exception("Fornecedor não encontrado");

            if (!_context.CondicaoPagamento.Any(x => x.Id == fornecedorDTO.IdCondicaoPagamento))
                throw new Exception("Condição de pagamento não encontrada");

            fornecedor.Ativo = fornecedorDTO.Ativo;
            fornecedor.Bairro = fornecedorDTO.Bairro;
            fornecedor.CEP = fornecedorDTO.CEP;
            fornecedor.CNPJ = fornecedorDTO.CNPJ;
            fornecedor.CPF = fornecedorDTO.CPF;
            fornecedor.DataUltimaAlteracao = fornecedorDTO.DataUltimaAlteracao;
            fornecedor.Email = fornecedorDTO.Email;
            fornecedor.Endereco = fornecedorDTO.Endereco;
            fornecedor.IdCidade = fornecedorDTO.IdCidade;
            fornecedor.IdCondicaoPagamento = fornecedorDTO.IdCondicaoPagamento;
            fornecedor.IdUsuarioAlteracao = fornecedorDTO.IdUsuarioAlteracao;
            fornecedor.InscricaoEstadual = fornecedorDTO.InscricaoEstadual;
            fornecedor.Nome = fornecedorDTO.Nome;
            fornecedor.NomeFantasia = fornecedorDTO.NomeFantasia;
            fornecedor.NomeVendedor = fornecedorDTO.NomeVendedor;
            fornecedor.NumeroCadastral = fornecedorDTO.NumeroCadastral;
            fornecedor.Observacao = fornecedorDTO.Observacao;
            fornecedor.RazaoSocial = fornecedorDTO.RazaoSocial;
            fornecedor.TelefoneCelular = fornecedorDTO.TelefoneCelular;
            fornecedor.TelefoneFixo = fornecedorDTO.TelefoneFixo;
            fornecedor.Banco = fornecedorDTO.Banco;
            fornecedor.Agencia = fornecedorDTO.Agencia;
            fornecedor.Conta = fornecedorDTO.Conta;
            fornecedor.TipoConta = fornecedorDTO.TipoConta;

            _context.Entry(fornecedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<FornecedorDTO>(fornecedor);
        }

        public async Task<bool> VerificaCPFExistente(string cpf)
        {
            return await _context.Fornecedor.AnyAsync(x => x.CPF == cpf);
        }

        public async Task<bool> VerificaCNPJExistente(string cnpj)
        {
            return await _context.Fornecedor.AnyAsync(x => x.CNPJ == cnpj);
        }

        public async Task<List<FornecedorDTO>> ObtemFiliais(Int64 Id)
        {
            var cnpj = (await _context.Fornecedor.FirstOrDefaultAsync(x => x.Id == Id)).CNPJ;

            var raizCnpj = cnpj.Split('/')[0] ?? "";

            return _mapper.Map<List<FornecedorDTO>>(await _context.Fornecedor.Where(x => x.CNPJ.StartsWith(raizCnpj)).ToListAsync());
        }
    }
}
