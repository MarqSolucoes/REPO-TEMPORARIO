using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Fornecedor;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoFornecedor
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFornecedor _repositorioFornecedor;

        public ServicoFornecedor(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFornecedor = new RepositorioFornecedor(_context, _mapper);
        }

        public async Task<List<FornecedorDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioFornecedor.Get(apenasAtivos);
        }

        public async Task<FornecedorDTO> Get(Int64 Id)
        {
            return await _repositorioFornecedor.Get(Id);
        }

        public async Task<List<FornecedorDTO>> GetFiltrado(Fornecedor_FiltroDTO fornecedor_FiltroDTO)
        {
            return await _repositorioFornecedor.GetFiltrado(fornecedor_FiltroDTO);
        }

        public async Task<FornecedorDTO> Post(FornecedorDTO fornecedorDTO)
        {
            if (fornecedorDTO.IdTipoFornecedor == 1) //Pessoa Física
            {
                if (string.IsNullOrEmpty(fornecedorDTO.Nome.Trim()))
                    throw new Exception("O campo 'Nome' deve ser informado");

                if (string.IsNullOrEmpty(fornecedorDTO.CPF.Trim()))
                    throw new Exception("O campo 'CPF' deve ser informado");

                if (await _repositorioFornecedor.VerificaCPFExistente(fornecedorDTO.CPF))
                    throw new Exception($"O CPF '{fornecedorDTO.CPF}' já existe");
            }
            else if (fornecedorDTO.IdTipoFornecedor == 2) //Pessoa Jurídica
            {
                if (string.IsNullOrEmpty(fornecedorDTO.RazaoSocial.Trim()))
                    throw new Exception("O campo 'Razão Social' deve ser informado");

                if (string.IsNullOrEmpty(fornecedorDTO.NomeFantasia.Trim()))
                    throw new Exception("O campo 'Nome Fantasia' deve ser informado");

                if (string.IsNullOrEmpty(fornecedorDTO.CNPJ.Trim()))
                    throw new Exception("O campo 'CNPJ' deve ser informado");

                if (await _repositorioFornecedor.VerificaCNPJExistente(fornecedorDTO.CNPJ))
                    throw new Exception($"O CNPJ '{fornecedorDTO.CNPJ}' já existe");
            }
            else
                throw new Exception("Tipo de fornecedor inválido");

            return await _repositorioFornecedor.Post(fornecedorDTO);
        }

        public async Task<FornecedorDTO> Put(FornecedorDTO fornecedorDTO)
        {
            if (fornecedorDTO.IdTipoFornecedor == 1) //Pessoa Física
            {
                fornecedorDTO.RazaoSocial = null;
                fornecedorDTO.NomeFantasia = null;
                fornecedorDTO.CNPJ = null;
                fornecedorDTO.InscricaoEstadual = null;

                if (string.IsNullOrEmpty(fornecedorDTO.Nome.Trim()))
                    throw new Exception("O campo 'Nome' deve ser informado");
            }
            else if (fornecedorDTO.IdTipoFornecedor == 2) //Pessoa Jurídica
            {
                fornecedorDTO.Nome = null;
                fornecedorDTO.CPF = null;

                if (string.IsNullOrEmpty(fornecedorDTO.RazaoSocial.Trim()))
                    throw new Exception("O campo 'Razão Social' deve ser informado");

                if (string.IsNullOrEmpty(fornecedorDTO.NomeFantasia.Trim()))
                    throw new Exception("O campo 'Nome Fantasia' deve ser informado");
            }
            else
                throw new Exception("Tipo de fornecedor inválido");

            return await _repositorioFornecedor.Put(fornecedorDTO);
        }

        public async Task<List<FornecedorDTO>> ObtemFiliais(Int64 Id)
        {
            return await _repositorioFornecedor.ObtemFiliais(Id);
        }
    }
}
