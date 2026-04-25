using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.Dominio.Modelos.Genericos.Faturamento;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoFaturamento
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFaturamento _repositorioFaturamento;

        public ServicoFaturamento(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFaturamento = new RepositorioFaturamento(_context, _mapper);
        }

        public async Task<List<FaturamentoDTO>> Get()
        {
            return await _repositorioFaturamento.Get();
        }

        public async Task<FaturamentoDTO> Get(Int64 Id)
        {
            return await _repositorioFaturamento.Get(Id);
        }

        public async Task<List<FaturamentoDTO>> GetObra(Int64 idObra)
        {
            return await _repositorioFaturamento.GetObra(idObra);
        }

        public async Task<Faturamento_DetalheClienteDTO> GetClienteDetalhe(Int64 idCliente)
        {
            return await _repositorioFaturamento.GetClienteDetalhe(idCliente);
        }

        public async Task<Faturamento_FiltradoReturnDTO> GetFiltrado(FaturamentoRequestDTO parametros)
        {
            return await _repositorioFaturamento.GetFiltrado(parametros);
        }

        public async Task CancelarFaturamento(Faturamento_CancelarFaturamentoDTO parametros)
        {
            await _repositorioFaturamento.CancelarFaturamento(parametros);
        }

        public async Task<List<Faturamento_ValoresAFaturarDTO>> ObtemValoresAFaturar(FaturamentoRequestDTO parametros)
        {
            return await _repositorioFaturamento.ObtemValoresAFaturar(parametros);
        }

        public async Task<List<Faturamento_ValoresAFaturarDTO>> ObtemValoresAFaturar(Int64 idObra)
        {
            return await _repositorioFaturamento.ObtemValoresAFaturar(idObra);
        }

        public async Task AjustaFaturamento(Faturamento_AjusteFaturamentoDTO faturamentos)
        {
               await _repositorioFaturamento.AjustaFaturamento(faturamentos);
        }

        public async Task<FaturamentoDTO> Post(FaturamentoDTO faturamentoDTO)
        {
            return await _repositorioFaturamento.Post(faturamentoDTO);
        }

        public async Task<FaturamentoDTO> Put(FaturamentoDTO faturamentoDTO)
        {
            return await _repositorioFaturamento.Put(faturamentoDTO);
        }

        public async Task<FaturamentoArquivosDTO> Upload(FaturamentoArquivosDTO faturamentoArquivosDTO)
        {
            return await _repositorioFaturamento.Upload(faturamentoArquivosDTO);
        }

        public async Task<FaturamentoArquivosDTO> GetFile(Int64 id)
        {
            return await _repositorioFaturamento.GetFile(id);
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            await _repositorioFaturamento.DeleteArquivo(idArquivo);
        }

        public async Task<List<FaturamentoArquivosDTO>> ObtemArquivos(Int64 idFaturamento)
        {
            return await _repositorioFaturamento.ObtemArquivos(idFaturamento);
        }

        public async Task AjustaDataFaturamento()
        {
            await _repositorioFaturamento.AjustaDataFaturamento();
        }

        public async Task AlteraData(Faturamento_AlteracaoDataDTO parametros)
        {
            await _repositorioFaturamento.AlteraData(parametros);
        }

        public async Task AlteraValor(Faturamento_AlteracaoValorDTO parametros)
        {
            if (parametros.Valor < 0)
                throw new Exception("Valor deve ser igual ou maior que zero");

            await _repositorioFaturamento.AlteraValor(parametros);
        }

        public async Task InformarFaturamento(Faturamento_InformarFaturamentoDTO faturamento, Int64 idUsuario)
        {
            await _repositorioFaturamento.InformarFaturamento(faturamento, idUsuario);
        }
    }
}
