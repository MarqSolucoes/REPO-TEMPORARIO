using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.Cotacao;
using WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Helpers;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioSolicitacaoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private RepositorioFluxoCaixa _repositorioFluxoCaixa;
        private RepositorioObra _repositorioObra;

        public RepositorioSolicitacaoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFluxoCaixa = new RepositorioFluxoCaixa(_context, _mapper);
            _repositorioObra = new RepositorioObra(_context, _mapper);
        }

        public async Task<List<SolicitacaoCompraDTO>> Get()
        {
            return _mapper.Map<List<SolicitacaoCompraDTO>>(await _context.SolicitacaoCompra.AsNoTracking()
                .Include(x => x.StatusSolicitacaoCompra)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.Fornecedor)
                .ThenInclude(x => x.CondicaoPagamento)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.CondicaoPagamento)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Comentarios)
                .ThenInclude(x => x.UsuarioCadastro)
                .ToListAsync());
        }

        public async Task<SolicitacaoCompraDTO> Get(Int64 Id)
        {
            return _mapper.Map<SolicitacaoCompraDTO>(await _context.SolicitacaoCompra.AsNoTracking()
                .Include(x => x.StatusSolicitacaoCompra)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.UsuarioEngenheiroAprovador)
                .Include(x => x.UsuarioDiretorAprovador)

                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.Fornecedor)
                .ThenInclude(x => x.CondicaoPagamento)

                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .ThenInclude(x => x.UnidadeMaterial)

                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.CondicaoPagamento)

                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.PagamentoManual)

                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Comentarios)
                .ThenInclude(x => x.UsuarioCadastro)
                .FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<Cotacao_SolicitacaoCompraDTO> GetParaCotacao(Int64 Id)
        {
            var solicitacaoCompraDTO = await Get(Id);

            var solicitacaoCompraCotacaoDTO = new Cotacao_SolicitacaoCompraDTO();
            solicitacaoCompraCotacaoDTO.Id = solicitacaoCompraDTO.Id;
            solicitacaoCompraCotacaoDTO.Codigo = solicitacaoCompraDTO.Codigo;
            solicitacaoCompraCotacaoDTO.Obra = solicitacaoCompraDTO.CentroCustoObra?.Descricao ?? "";
            solicitacaoCompraCotacaoDTO.Cliente = solicitacaoCompraDTO.CentroCustoObra?.Cliente?.NomeFantasia ?? "";
            solicitacaoCompraCotacaoDTO.Endereco = solicitacaoCompraDTO.EnderecoEntrega;
            solicitacaoCompraCotacaoDTO.Observacao = solicitacaoCompraDTO.Observacao;
            solicitacaoCompraCotacaoDTO.ObservacaoParaFornecedor = solicitacaoCompraDTO.ObservacaoParaFornecedor;
            solicitacaoCompraCotacaoDTO.ObservacaoDeAprovacao = solicitacaoCompraDTO.ObservacaoDeAprovacao;
            solicitacaoCompraCotacaoDTO.DataEntrega = solicitacaoCompraDTO.DataEntrega;
            solicitacaoCompraCotacaoDTO.ValorEstimado = solicitacaoCompraDTO.ValorEstimado;
            solicitacaoCompraCotacaoDTO.Fornecedores = new List<Cotacao_SolicitacaoCompra_FornecedorDTO>();
            solicitacaoCompraCotacaoDTO.Materiais = new List<Cotacao_SolicitacaoCompra_MaterialDTO>();
            solicitacaoCompraCotacaoDTO.Servico = solicitacaoCompraDTO.Servico;

            solicitacaoCompraDTO.Materiais?.FirstOrDefault()?.Cotacoes.ForEach(x =>
            {
                var fornecedor = new Cotacao_SolicitacaoCompra_FornecedorDTO()
                {
                    Id = x.IdFornecedor,
                    CNPJ = x.Fornecedor.CNPJ,
                    NomeFantasia = x.Fornecedor.NomeFantasia,
                    RazaoSocial = x.Fornecedor.RazaoSocial,
                    CondicaoPagamento = _mapper.Map<CondicaoPagamentoDTO>(_context.CondicaoPagamento.FirstOrDefault(y => y.Id == x.IdCondicaoPagamento)),
                    Frete=x.Frete??0,
                    Imposto=x.Imposto??0,
                    Desconto=x.Desconto??0
                };

                var idSolicitacaoCompraMaterialCotacao = _context.SolicitacaoCompra_MateriaisCotacao.FirstOrDefault(y => y.IdFornecedor == x.IdFornecedor && y.IdSolicitacaoCompraMaterial == x.IdSolicitacaoCompraMaterial).Id;

                fornecedor.PagamentoManual = _mapper.Map<List<SolicitacaoCompraMaterialCotacaoPagamentoManualDTO>>(_context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Where(y => y.IdSolicitacaoCompraMaterialCotacao == idSolicitacaoCompraMaterialCotacao).ToList());

                solicitacaoCompraCotacaoDTO.Fornecedores.Add(fornecedor);
            });

            solicitacaoCompraCotacaoDTO.Fornecedores = solicitacaoCompraCotacaoDTO.Fornecedores.OrderBy(x => x.NomeFantasia).ToList();

            solicitacaoCompraDTO.Materiais.ForEach(material =>
            {
                var cotacaoSolicitacaoCompraMaterialDTO = new Cotacao_SolicitacaoCompra_MaterialDTO();
                cotacaoSolicitacaoCompraMaterialDTO.IdSolicitacaoCompraMaterial = material.Id;
                cotacaoSolicitacaoCompraMaterialDTO.Descricao = material.Material.Descricao;
                cotacaoSolicitacaoCompraMaterialDTO.IdMaterial = material.IdMaterial;
                cotacaoSolicitacaoCompraMaterialDTO.Quantidade = material.Quantidade;
                cotacaoSolicitacaoCompraMaterialDTO.ItemSelecionado = false;
                cotacaoSolicitacaoCompraMaterialDTO.ValorUnitarioEstimado = material.ValorUnitarioEstimado;
                cotacaoSolicitacaoCompraMaterialDTO.UnidadeMaterial = material.Material.UnidadeMaterial;

                cotacaoSolicitacaoCompraMaterialDTO.Cotacoes = new List<Cotacao_SolicitacaoCompra_Material_CotacaoDTO>();

                material.Cotacoes.ForEach(cotacao =>
                {
                    var cotacaoSolicitacaoCompraMaterialCotacaoDTO = new Cotacao_SolicitacaoCompra_Material_CotacaoDTO();
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.IdSolicitacaoCompraMaterialCotacao = cotacao.Id;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.CotacaoFinal = cotacao.CotacaoFinal;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.CotacaoMaisBarata = cotacao.CotacaoMaisBarata;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.DataEntrega = cotacao.DataEntrega;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.IdFornecedor = cotacao.IdFornecedor;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.NomeFantasia = solicitacaoCompraCotacaoDTO.Fornecedores.FirstOrDefault(x => x.Id == cotacao.IdFornecedor)?.NomeFantasia??"";
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.ValorUnitarioCotado = cotacao.ValorUnitarioCotado;
                    cotacaoSolicitacaoCompraMaterialCotacaoDTO.QuantidadeCotado = cotacao.QuantidadeCotado;

                    cotacaoSolicitacaoCompraMaterialDTO.Cotacoes.Add(cotacaoSolicitacaoCompraMaterialCotacaoDTO);

                    solicitacaoCompraCotacaoDTO.Fornecedores.FirstOrDefault(x => x.Id == cotacao.IdFornecedor).TotalCotado += (cotacao.ValorUnitarioCotado * material.Quantidade);
                });

                cotacaoSolicitacaoCompraMaterialDTO.Cotacoes = cotacaoSolicitacaoCompraMaterialDTO.Cotacoes.OrderBy(x => x.NomeFantasia).ToList();
                solicitacaoCompraCotacaoDTO.Materiais.Add(cotacaoSolicitacaoCompraMaterialDTO);
            });

            solicitacaoCompraCotacaoDTO.Materiais = solicitacaoCompraCotacaoDTO.Materiais.OrderBy(x => x.Descricao).ToList();
            return solicitacaoCompraCotacaoDTO;
        }

        public async Task<List<SolicitacaoCompraDTO>> GetByStatus(Int64 idStatus, Int64 idUsuario)
        {
            var query = _context.SolicitacaoCompra.AsNoTracking()
                .Include(x => x.StatusSolicitacaoCompra)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.Fornecedor)
                .ThenInclude(x => x.CondicaoPagamento)
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Material)
                .Include(x => x.Arquivos)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra)
                .ThenInclude(x => x.Cliente)
                .Include(x => x.Comentarios)
                .ThenInclude(x => x.UsuarioCadastro)
                .Include(x=>x.UsuarioComprador)
                .AsQueryable();

            

            if (idStatus == 3)
            {
                var usuario = _context.Usuario.Include(x => x.CargoUsuario).FirstOrDefault(x => x.Id == idUsuario);
                
                if(usuario.CargoUsuario.Descricao == "Diretor")
                {
                    //Lista apenas as aprovações do diretor
                    query = query.Where(x => x.CentroCustoObra.IdUsuarioDiretorAprovador == idUsuario && x.IdStatusSolicitacaoCompra == idStatus);
                }
                else
                {
                    if (usuario.ComprasOrdensCompraParaAprovacao)
                    {
                        //Não é um diretor e pode listar as solicitações para aprovação
                        query = query.Where(x => x.IdStatusSolicitacaoCompra == idStatus);
                        //return new List<SolicitacaoCompraDTO>();
                    }
                    else
                    {
                        //Não é um diretor não pode listar as solicitações para aprovação
                        return new List<SolicitacaoCompraDTO>();
                    }
                }
            }
            else
            {
                if (idStatus == 1)
                    query = query.Where(x => x.CentroCustoObra.UsuariosAprovadores.Any(y => y.IdUsuarioAprovacao == idUsuario) && x.IdStatusSolicitacaoCompra == idStatus);
                else
                    query = query.Where(x => x.IdStatusSolicitacaoCompra == idStatus);
            }


            query = query.OrderByDescending(x => x.DataCadastro);

            return _mapper.Map<List<SolicitacaoCompraDTO>>(await query.ToListAsync());
        }

        public async Task<SolicitacaoCompra_ResponseDTO> GetByUser(Int64 idUsuario, SolicitacaoCompra_RequestParametrosDTO filtros)
        {
            var query = _context.SolicitacaoCompra.AsNoTracking().AsQueryable();

            // Filtro obrigatório: usuário logado
            query = query.Where(x => x.IdUsuarioCadastro == idUsuario);

            // Filtros opcionais
            if (filtros != null)
            {
                if (filtros.idsObra != null && filtros.idsObra.Any())
                    query = query.Where(x => x.IdCentroCustoObra.HasValue
                                          && filtros.idsObra.Contains(x.IdCentroCustoObra.Value));

                if (filtros.idsStatus != null && filtros.idsStatus.Any())
                    query = query.Where(x => filtros.idsStatus.Contains(x.IdStatusSolicitacaoCompra));

                if (!string.IsNullOrWhiteSpace(filtros.codigo))
                    query = query.Where(x => x.Codigo.Contains(filtros.codigo));

                if (!string.IsNullOrWhiteSpace(filtros.titulo))
                    query = query.Where(x => x.Nome.Contains(filtros.titulo));

                if (filtros.dataSolicitacaoInicial.HasValue)
                    query = query.Where(x => x.DataCadastro >= filtros.dataSolicitacaoInicial.Value);

                if (filtros.dataSolicitacaoFinal.HasValue)
                {
                    var dataFim = filtros.dataSolicitacaoFinal.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.DataCadastro <= dataFim);
                }
            }

            // Conta total ANTES da paginação (para o front saber quantas páginas têm)
            var count = await query.CountAsync();

            // Proteção dos valores de paginação (take padrão de 10)
            var skip = filtros?.skip ?? 0;
            if (skip < 0) skip = 0;
            var take = filtros?.take > 0 ? filtros.take : 10;

            // Includes + OrderBy + Skip/Take DEPOIS do Count
            var lista = await query
                .Include(x => x.StatusSolicitacaoCompra)
                .Include(x => x.TipoCentroCusto)
                .Include(x => x.UsuarioFinalizacaoCotacao)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Materiais).ThenInclude(x => x.Cotacoes)
                .Include(x => x.Materiais).ThenInclude(x => x.Cotacoes).ThenInclude(x => x.Fornecedor).ThenInclude(x => x.CondicaoPagamento)
                .Include(x => x.Materiais).ThenInclude(x => x.Material)
                .Include(x => x.Materiais).ThenInclude(x => x.Cotacoes).ThenInclude(x => x.PagamentoManual)
                .Include(x => x.Arquivos).ThenInclude(x => x.UsuarioCadastro)
                .Include(x => x.CentroCustoDEF)
                .Include(x => x.CentroCustoObra).ThenInclude(x => x.Cliente)
                .Include(x => x.Comentarios).ThenInclude(x => x.UsuarioCadastro)
                .OrderByDescending(x => x.DataCadastro)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new SolicitacaoCompra_ResponseDTO
            {
                count = count,
                solicitacoes = _mapper.Map<List<SolicitacaoCompraDTO>>(lista)
            };
        }

        public async Task<List<SolicitacaoCompraMateriaisCotacaoDTO>> GetCotacoes(Int64 idSolicitacaoCompraMaterial)
        {
            return _mapper.Map<List<SolicitacaoCompraMateriaisCotacaoDTO>>(await _context.SolicitacaoCompra_MateriaisCotacao.AsNoTracking()
                .Include(x => x.Fornecedor)
                .Where(x => x.IdSolicitacaoCompraMaterial == idSolicitacaoCompraMaterial).ToListAsync());
        }

        public async Task<SolicitacaoCompraDTO> Post(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            var solicitacaoCompra = _mapper.Map<SolicitacaoCompra_Post>(solicitacaoCompraDTO);

            return await Post(solicitacaoCompra);
        }

        public async Task<SolicitacaoCompraDTO> Post(SolicitacaoCompra_Post solicitacaoCompraDTO)
        {
            var cargoUsuarioCadastro = _context.Usuario.Include(x => x.CargoUsuario).FirstOrDefault(x => x.Id == solicitacaoCompraDTO.IdUsuarioCadastro)?.CargoUsuario?.Descricao ?? "";

            var solicitacaoCompra = new SolicitacaoCompra();
            solicitacaoCompra.CodigoAno = DateTime.Now.Year;


            solicitacaoCompra.CodigoSequencia = (_context.SolicitacaoCompra.Max(x => (int?)x.CodigoSequencia) ?? 0) + 1;

            if (solicitacaoCompraDTO.IdTipoCentroCusto == 1)
                solicitacaoCompra.Codigo = $"{(await _context.Obra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoObra)).Codigo} - {(_context.SolicitacaoCompra.Count(x => x.IdCentroCustoObra == solicitacaoCompraDTO.IdCentroCustoObra)) + 1}";
            else
                solicitacaoCompra.Codigo = $"{(await _context.DEF.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoDEF)).Codigo} - {solicitacaoCompra.CodigoSequencia}";

            solicitacaoCompra.Nome = !string.IsNullOrEmpty(solicitacaoCompraDTO.Nome) ? solicitacaoCompraDTO.Nome : "";
            solicitacaoCompra.DataCadastro = solicitacaoCompraDTO.DataCadastro;
            solicitacaoCompra.DataEntrega = solicitacaoCompraDTO.DataEntrega.Value;
            solicitacaoCompra.DataFinalizacaoCotacao = null;
            solicitacaoCompra.DataUltimaAlteracao = solicitacaoCompraDTO.DataUltimaAlteracao;
            solicitacaoCompra.DataAprovacaoEngenheiro = solicitacaoCompraDTO.DataAprovacaoEngenheiro;
            solicitacaoCompra.Id = 0;
            solicitacaoCompra.IdCentroCustoDEF = solicitacaoCompraDTO.IdCentroCustoDEF;
            solicitacaoCompra.IdCentroCustoObra = solicitacaoCompraDTO.IdCentroCustoObra;
            solicitacaoCompra.IdStatusSolicitacaoCompra = solicitacaoCompraDTO.IdStatusSolicitacaoCompra != 1 ? solicitacaoCompraDTO.IdStatusSolicitacaoCompra : 1;
            solicitacaoCompra.IdTipoCentroCusto = solicitacaoCompraDTO.IdTipoCentroCusto;
            solicitacaoCompra.IdUsuarioComprador = solicitacaoCompraDTO.IdUsuarioComprador;
            solicitacaoCompra.IdUsuarioAlteracao = solicitacaoCompraDTO.IdUsuarioAlteracao;
            solicitacaoCompra.IdUsuarioCadastro = solicitacaoCompraDTO.IdUsuarioCadastro;
            solicitacaoCompra.IdEngenheiroAprovador = solicitacaoCompraDTO.IdEngenheiroAprovador;
            solicitacaoCompra.IdUsuarioFinalizacaoCotacao = null;
            solicitacaoCompra.MotivoCancelamento = null;
            solicitacaoCompra.Observacao = solicitacaoCompraDTO.Observacao;
            solicitacaoCompra.ObservacaoDeAprovacao = "";
            solicitacaoCompra.ValorEstimado = solicitacaoCompraDTO.ValorEstimado;
            solicitacaoCompra.ValorTotalCotado = null;
            solicitacaoCompra.ValorMelhorCotacao = 0;
            solicitacaoCompra.Servico = solicitacaoCompraDTO.Servico;

            if (solicitacaoCompra.ValorEstimado == 0)
                solicitacaoCompra.ValorEstimado = solicitacaoCompraDTO.Materiais.Sum(x => x.Quantidade * x.ValorUnitarioEstimado);

            if (solicitacaoCompra.IdCentroCustoObra.HasValue)
            {
                var obra = await _context.Obra.Include(x => x.Cidade).FirstOrDefaultAsync(x => x.Id == solicitacaoCompra.IdCentroCustoObra);
                solicitacaoCompra.EnderecoEntrega = $"{obra.EnderecoEntrega}";
            }

            //É uma solicitação de serviço com indicação de fornecedor e condições de pagamento
            if (solicitacaoCompra.Servico && solicitacaoCompraDTO.IdFornecedor > 0)
            {
                if (solicitacaoCompra.IdUsuarioComprador == null)
                    throw new Exception("Informe um comprador para a solicitação de compra");

                solicitacaoCompra.IdStatusSolicitacaoCompra = 3;
                solicitacaoCompra.IdUsuarioFinalizacaoCotacao = 4;
                solicitacaoCompra.DataFinalizacaoCotacao = DateTime.Now;

                solicitacaoCompra.ValorTotalCotado = solicitacaoCompraDTO.Pagamentos.Sum(x => x.valor);
                solicitacaoCompra.ValorMelhorCotacao = solicitacaoCompraDTO.Pagamentos.Sum(x => x.valor);
            }

            await _context.SolicitacaoCompra.AddAsync(solicitacaoCompra);
            await _context.SaveChangesAsync();

            if(solicitacaoCompraDTO.Servico)
            {
                solicitacaoCompraDTO.Materiais = new List<SolicitacaoCompraMateriaisDTO>();
                solicitacaoCompraDTO.Materiais.Add(new SolicitacaoCompraMateriaisDTO()
                {
                    IdMaterial = 23958,
                    Quantidade = 1,
                    ValorUnitarioEstimado = solicitacaoCompraDTO.Pagamentos?.Sum(x => x.valor) ?? 0
                });
            }
            solicitacaoCompraDTO.Materiais.ForEach(x =>
            {
                var solicitacaoCompraMaterial = new SolicitacaoCompraMateriais();
                solicitacaoCompraMaterial.Id = 0;
                solicitacaoCompraMaterial.IdCentroCustoDEF = solicitacaoCompra.IdCentroCustoDEF;
                solicitacaoCompraMaterial.IdCentroCustoObra = solicitacaoCompra.IdCentroCustoObra;
                solicitacaoCompraMaterial.IdMaterial = x.IdMaterial;
                solicitacaoCompraMaterial.IdSolicitacaoCompra = solicitacaoCompra.Id;
                solicitacaoCompraMaterial.IdTipoCentroCusto = solicitacaoCompra.IdTipoCentroCusto;
                solicitacaoCompraMaterial.Quantidade = x.Quantidade;
                solicitacaoCompraMaterial.ValorUnitarioEstimado = x.ValorUnitarioEstimado;

                _context.SolicitacaoCompra_Materiais.Add(solicitacaoCompraMaterial);
                _context.SaveChanges();

                if (solicitacaoCompra.Servico && solicitacaoCompraDTO.IdFornecedor > 0)
                {
                    var solicitacaoMaterialCotacao = new SolicitacaoCompraMateriaisCotacao();
                    solicitacaoMaterialCotacao.CotacaoFinal = true;
                    solicitacaoMaterialCotacao.CotacaoMaisBarata = true;
                    solicitacaoMaterialCotacao.DataEntrega = solicitacaoCompraDTO.DataEntrega;
                    solicitacaoMaterialCotacao.Id = 0;
                    solicitacaoMaterialCotacao.IdCondicaoPagamento = 7;
                    solicitacaoMaterialCotacao.IdFornecedor = solicitacaoCompraDTO.IdFornecedor;
                    solicitacaoMaterialCotacao.IdSolicitacaoCompraMaterial = solicitacaoCompraMaterial.Id;
                    solicitacaoMaterialCotacao.QuantidadeCotado = solicitacaoCompraMaterial.Quantidade;
                    solicitacaoMaterialCotacao.ValorUnitarioCotado = solicitacaoCompra.ValorTotalCotado ?? 0;
                    solicitacaoMaterialCotacao.ValorUnitarioComDesconto = solicitacaoCompra.ValorTotalCotado ?? 0;

                    _context.SolicitacaoCompra_MateriaisCotacao.Add(solicitacaoMaterialCotacao);
                    _context.SaveChanges();

                    solicitacaoCompraDTO.Pagamentos.ForEach(pagamento =>
                    {
                        var solicitacaoCompraPagamentoManual = new SolicitacaoCompraMaterialCotacaoPagamentoManual();
                        solicitacaoCompraPagamentoManual.Data = pagamento.data;
                        solicitacaoCompraPagamentoManual.DataCadastro = DateTime.Now;
                        solicitacaoCompraPagamentoManual.DataUltimaAlteracao = DateTime.Now;
                        solicitacaoCompraPagamentoManual.Id = 0;
                        solicitacaoCompraPagamentoManual.IdSolicitacaoCompraMaterialCotacao = solicitacaoMaterialCotacao.Id;
                        solicitacaoCompraPagamentoManual.IdUsuarioAlteracao = solicitacaoCompraDTO.IdUsuarioAlteracao;
                        solicitacaoCompraPagamentoManual.IdUsuarioCadastro = solicitacaoCompraDTO.IdUsuarioCadastro;
                        solicitacaoCompraPagamentoManual.Valor = pagamento.valor;

                        _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Add(solicitacaoCompraPagamentoManual);
                        _context.SaveChanges();
                    });

                }
            });

            return _mapper.Map<SolicitacaoCompraDTO>(await Get(solicitacaoCompra.Id));
        }

        public async Task<SolicitacaoCompraDTO> Reabrir(SolicitacaoCompraDTO solicitacaoCompraDTO, List<SolicitacaoCompra_Reabertura_MaterialDTO> materiais)
        {
            var solicitacaoCompra = new SolicitacaoCompra();
            solicitacaoCompra.CodigoAno = DateTime.Now.Year;

            if (solicitacaoCompraDTO.IdTipoCentroCusto == 1)
                solicitacaoCompra.CodigoSequencia = _context.SolicitacaoCompra.Where(x => x.IdCentroCustoObra == solicitacaoCompraDTO.IdCentroCustoObra).Count() + 1;
            else
                solicitacaoCompra.CodigoSequencia = _context.SolicitacaoCompra.Where(x => x.IdCentroCustoDEF == solicitacaoCompraDTO.IdCentroCustoDEF).Count() + 1;

            if (solicitacaoCompraDTO.IdTipoCentroCusto == 1)
                solicitacaoCompra.Codigo = $"{(await _context.Obra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoObra)).Codigo} - {solicitacaoCompra.CodigoSequencia}";
            else
                solicitacaoCompra.Codigo = $"{(await _context.DEF.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoDEF)).Codigo} - {solicitacaoCompra.CodigoSequencia}";

            solicitacaoCompra.EnderecoEntrega = (await _context.Obra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoObra)).EnderecoObra;
            solicitacaoCompra.DataCadastro = solicitacaoCompraDTO.DataCadastro;
            solicitacaoCompra.DataEntrega = solicitacaoCompraDTO.DataEntrega;
            solicitacaoCompra.DataFinalizacaoCotacao = null;
            solicitacaoCompra.DataUltimaAlteracao = solicitacaoCompraDTO.DataUltimaAlteracao;
            solicitacaoCompra.Id = 0;
            solicitacaoCompra.IdCentroCustoDEF = solicitacaoCompraDTO.IdCentroCustoDEF;
            solicitacaoCompra.IdCentroCustoObra = solicitacaoCompraDTO.IdCentroCustoObra;
            solicitacaoCompra.IdStatusSolicitacaoCompra = solicitacaoCompraDTO.IdStatusSolicitacaoCompra;
            solicitacaoCompra.IdTipoCentroCusto = solicitacaoCompraDTO.IdTipoCentroCusto;
            solicitacaoCompra.IdUsuarioAlteracao = solicitacaoCompraDTO.IdUsuarioAlteracao;
            solicitacaoCompra.IdUsuarioCadastro = solicitacaoCompraDTO.IdUsuarioCadastro;
            solicitacaoCompra.IdUsuarioFinalizacaoCotacao = null;
            solicitacaoCompra.MotivoCancelamento = null;
            solicitacaoCompra.Nome = solicitacaoCompraDTO.Nome;
            solicitacaoCompra.Observacao = solicitacaoCompraDTO.Observacao;
            solicitacaoCompra.ValorEstimado = solicitacaoCompraDTO.ValorEstimado;
            solicitacaoCompra.ValorTotalCotado = solicitacaoCompraDTO.ValorTotalCotado;
            solicitacaoCompra.Servico = solicitacaoCompraDTO.Servico;

                await _context.SolicitacaoCompra.AddAsync(solicitacaoCompra);
                await _context.SaveChangesAsync();
            
            materiais.ForEach(x =>
            {
                var solicitacaoCompraMaterial = new SolicitacaoCompraMateriais();
                solicitacaoCompraMaterial.Id = 0;
                solicitacaoCompraMaterial.IdCentroCustoDEF = solicitacaoCompra.IdCentroCustoDEF;
                solicitacaoCompraMaterial.IdCentroCustoObra = solicitacaoCompra.IdCentroCustoObra;
                solicitacaoCompraMaterial.IdMaterial = x.IdMaterial;
                solicitacaoCompraMaterial.IdSolicitacaoCompra = solicitacaoCompra.Id;
                solicitacaoCompraMaterial.IdTipoCentroCusto = solicitacaoCompra.IdTipoCentroCusto;
                solicitacaoCompraMaterial.Quantidade = x.Quantidade;
                solicitacaoCompraMaterial.ValorUnitarioEstimado = x.ValorUnitario;

                _context.SolicitacaoCompra_Materiais.Add(solicitacaoCompraMaterial);
                _context.SaveChanges();

                x.Cotacoes?.ForEach(y =>
                {
                    var solicitacaoCompraMaterialCotacao = new SolicitacaoCompraMateriaisCotacao();
                    solicitacaoCompraMaterialCotacao.Id = 0;
                    solicitacaoCompraMaterialCotacao.CotacaoFinal = y.CotacaoFinal;
                    solicitacaoCompraMaterialCotacao.CotacaoMaisBarata = y.CotacaoMaisBarata;
                    solicitacaoCompraMaterialCotacao.DataEntrega = null;
                    solicitacaoCompraMaterialCotacao.IdFornecedor = y.IdFornecedor;
                    solicitacaoCompraMaterialCotacao.IdSolicitacaoCompraMaterial = solicitacaoCompraMaterial.Id;
                    solicitacaoCompraMaterialCotacao.ValorUnitarioCotado = y.ValorUnitarioCotado;
                    solicitacaoCompraMaterialCotacao.IdCondicaoPagamento = y.IdCondicaoPagamento;

                    _context.SolicitacaoCompra_MateriaisCotacao.Add(solicitacaoCompraMaterialCotacao);
                    _context.SaveChanges();
                });
            });

            return _mapper.Map<SolicitacaoCompraDTO>(await Get(solicitacaoCompra.Id));
        }

        public async Task<SolicitacaoCompraDTO> Put(SolicitacaoCompraDTO dto)
        {
            // 1) Carga única da agregada (inclui PagamentoManual para não precisar re-consultar).
            var solicitacao = await _context.SolicitacaoCompra
                .Include(x => x.Materiais).ThenInclude(x => x.Material)
                .Include(x => x.Materiais).ThenInclude(x => x.Cotacoes).ThenInclude(x => x.Fornecedor)
                .Include(x => x.Materiais).ThenInclude(x => x.Cotacoes).ThenInclude(x => x.PagamentoManual)
                .FirstOrDefaultAsync(x => x.Id == dto.Id)
                ?? throw new Exception("Solicitação de compra não identificada");

            var nomeUsuarioAlteracao = await _context.Usuario
                .Where(u => u.Id == dto.IdUsuarioAlteracao)
                .Select(u => u.Nome)
                .FirstOrDefaultAsync() ?? "";

            // 2) Campos escalares da solicitação.
            solicitacao.Nome = dto.Nome;
            solicitacao.DataEntrega = dto.DataEntrega.Value;
            solicitacao.DataFinalizacaoCotacao = dto.DataFinalizacaoCotacao;
            solicitacao.DataUltimaAlteracao = dto.DataUltimaAlteracao;
            solicitacao.IdStatusSolicitacaoCompra = dto.IdStatusSolicitacaoCompra;
            solicitacao.IdUsuarioAlteracao = dto.IdUsuarioAlteracao;
            solicitacao.IdUsuarioFinalizacaoCotacao = dto.IdUsuarioFinalizacaoCotacao;
            solicitacao.MotivoCancelamento = dto.MotivoCancelamento;
            solicitacao.Observacao = dto.Observacao;
            solicitacao.ObservacaoParaFornecedor = dto.ObservacaoParaFornecedor;
            solicitacao.ValorEstimado = dto.ValorEstimado;
            solicitacao.Servico = dto.Servico;
            solicitacao.IdUsuarioComprador = dto.IdUsuarioComprador;
            solicitacao.IdEngenheiroAprovador = dto.IdEngenheiroAprovador;
            solicitacao.DataAprovacaoEngenheiro = dto.DataAprovacaoEngenheiro;
            solicitacao.IdDiretorAprovador = dto.IdDiretorAprovador;
            solicitacao.DataAprovacaoDiretor = dto.DataAprovacaoDiretor;

            // 3) Pré-carga de lookups em lote (evita N+1).
            var idsCondicaoPagamento = solicitacao.Materiais
                .SelectMany(m => m.Cotacoes).Select(c => c.IdCondicaoPagamento)
                .Concat(dto.Materiais.SelectMany(m => m.Cotacoes ?? new()).Select(c => c.IdCondicaoPagamento))
                .Where(id => id != 0).Distinct().ToList();

            var descricaoCondPgto = await _context.CondicaoPagamento
                .Where(c => idsCondicaoPagamento.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Descricao);

            var idsMaterialNovos = dto.Materiais.Where(m => m.Id == 0).Select(m => m.IdMaterial).Distinct().ToList();
            var descricaoMaterial = await _context.Material
                .Where(m => idsMaterialNovos.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => m.Descricao);

            // 4) Diff por Id (PK da SolicitacaoCompraMateriais). Novos vêm com Id == 0.
            var existentesPorId = solicitacao.Materiais.ToDictionary(m => m.Id);
            var idsRecebidos = dto.Materiais.Where(m => m.Id != 0).Select(m => m.Id).ToHashSet();
            var historicos = new List<Historico>();

            // 4a) Remoções: materiais que estavam carregados e não voltaram na DTO.
            foreach (var removido in solicitacao.Materiais.Where(m => !idsRecebidos.Contains(m.Id)).ToList())
            {
                historicos.Add(Helper_RepositorioSolicitacaoCompra.NovoHistorico(dto, nomeUsuarioAlteracao,
                    $"Material removido: {removido.Material.Descricao}"));

                foreach (var cot in removido.Cotacoes.ToList())
                {
                    _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.RemoveRange(cot.PagamentoManual);
                    _context.SolicitacaoCompra_MateriaisCotacao.Remove(cot);
                }
                _context.SolicitacaoCompra_Materiais.Remove(removido);
            }

            // 4b) Atualizações e adições.
            foreach (var matDto in dto.Materiais)
            {
                if (matDto.Id != 0 && existentesPorId.TryGetValue(matDto.Id, out var existente))
                {
                    Helper_RepositorioSolicitacaoCompra.AtualizaMaterial(existente, matDto, dto, nomeUsuarioAlteracao, descricaoCondPgto, historicos);
                }
                else
                {
                    _context.SolicitacaoCompra_Materiais.Add(new SolicitacaoCompraMateriais
                    {
                        Id = 0,
                        IdSolicitacaoCompra = solicitacao.Id,
                        IdMaterial = matDto.IdMaterial,
                        IdCentroCustoDEF = solicitacao.IdCentroCustoDEF,
                        IdCentroCustoObra = solicitacao.IdCentroCustoObra,
                        IdTipoCentroCusto = solicitacao.IdTipoCentroCusto,
                        Quantidade = matDto.Quantidade,
                        ValorUnitarioEstimado = matDto.ValorUnitarioEstimado,
                        Cotacoes = (matDto.Cotacoes ?? new()).Select(c => new SolicitacaoCompraMateriaisCotacao
                        {
                            Id = 0,
                            IdFornecedor = c.IdFornecedor,
                            IdCondicaoPagamento = c.IdCondicaoPagamento,
                            CotacaoFinal = c.CotacaoFinal,
                            CotacaoMaisBarata = c.CotacaoMaisBarata,
                            DataEntrega = c.DataEntrega,
                            ValorUnitarioCotado = c.ValorUnitarioCotado,
                            ValorUnitarioComDesconto = c.ValorUnitarioCotado * (1 - ((c.Desconto ?? 0) / 100)),
                        }).ToList(),
                    });

                    historicos.Add(Helper_RepositorioSolicitacaoCompra.NovoHistorico(dto, nomeUsuarioAlteracao,
                        $"Novo material - {descricaoMaterial.GetValueOrDefault(matDto.IdMaterial, "")}"));
                }
            }

            // 5) Valor estimado recalculado a partir da DTO.
            var valor = dto.Materiais.Sum(m => m.Quantidade * (m.ValorUnitarioEstimado ?? 0));
            if (valor > 0) solicitacao.ValorEstimado = valor;

            _context.Historico.AddRange(historicos);

            // 6) Uma única viagem ao banco.
            await _context.SaveChangesAsync();

            return _mapper.Map<SolicitacaoCompraDTO>(solicitacao);
        }

        public async Task<SolicitacaoPedidoCompraQuantidadesDTO> ObtemQuantidades(Int64 idUsuario)
        {
            var result = new SolicitacaoPedidoCompraQuantidadesDTO();
            result.qtdParaValidacao = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 1 && x.CentroCustoObra.UsuariosAprovadores.Any(y => y.IdUsuarioAprovacao == idUsuario));
            result.qtdEmCotacao = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 2);

                var usuario = _context.Usuario.Include(x => x.CargoUsuario).FirstOrDefault(x => x.Id == idUsuario);

            if (usuario.CargoUsuario.Descricao == "Diretor")
            {
                //Lista apenas as aprovações do diretor
                result.qtdParaAprovacao = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 3 && x.CentroCustoObra.IdUsuarioDiretorAprovador == idUsuario);
            }
            else
            {
                if (usuario.ComprasOrdensCompraParaAprovacao)
                {
                    //Não é um diretor e pode listar as solicitações para aprovação
                    result.qtdParaAprovacao = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 3);
                    //result.qtdParaAprovacao = 0;
                }
                else
                {
                    //Não é um diretor não pode listar as solicitações para aprovação
                    result.qtdParaAprovacao = 0;
                }
            }

            //result.qtdParaAprovacao = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 3 && x.CentroCustoObra.IdUsuarioDiretorAprovador == idUsuario);
            result.qtdDevolvidasDiretoria = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 7);
            result.qtdEmCompra = await _context.PedidoCompra.CountAsync(x => x.IdStatusPedidoCompra == 1);
            result.qtdPedidosFinalizados = await _context.PedidoCompra.CountAsync(x => x.IdStatusPedidoCompra == 2);
            result.qtdPedidosCancelados = await _context.PedidoCompra.CountAsync(x => x.IdStatusPedidoCompra == 3);
            result.qtdSolicitacoesFinalizadas = await _context.SolicitacaoCompra.CountAsync(x => x.IdStatusSolicitacaoCompra == 4);
            result.qtdSolicitacoesCanceladas = await _context.PedidoCompra.CountAsync(x => x.IdStatusPedidoCompra == 5);

            return result;
        }

        public async Task<SolicitacaoCompraMateriaisCotacaoDTO> Post(SolicitacaoCompraMateriaisCotacaoDTO solicitacaoCompraMateriaisCotacaoDTO, Int64 idUsuario)
        {
            var solicitacaoCompraMaterial = await _context.SolicitacaoCompra_Materiais.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial);

            if (solicitacaoCompraMaterial == null)
                throw new Exception("Material não encontrado");

            var solicitacaoCompra = await _context.SolicitacaoCompra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraMaterial.IdSolicitacaoCompra);

            if (!await _context.Fornecedor.AnyAsync(x => x.Id == solicitacaoCompraMateriaisCotacaoDTO.IdFornecedor))
                throw new Exception("Fornecedor não encontrado");

            if (await _context.SolicitacaoCompra_MateriaisCotacao.AnyAsync(x => x.IdSolicitacaoCompraMaterial==solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial && x.IdFornecedor == solicitacaoCompraMateriaisCotacaoDTO.IdFornecedor))
                throw new Exception("Cotação já existente para o fornecedor selecionado");

            //Define como cotação principal caso não haja nenhuma outra cotação
            //if (await _context.SolicitacaoCompra_MateriaisCotacao.CountAsync(x => x.IdSolicitacaoCompraMaterial == solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial) == 0)
            //    solicitacaoCompraMateriaisCotacaoDTO.CotacaoFinal = true;

            //if (solicitacaoCompraMateriaisCotacaoDTO.CotacaoFinal)
            _context.SolicitacaoCompra_MateriaisCotacao.Where(x => x.IdSolicitacaoCompraMaterial == solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial).ToList().ForEach(x =>
            {
                x.CotacaoFinal = false;

                _context.Entry(x).State = EntityState.Modified;
            });

            _context.SaveChanges();

            var solicitacaoCompraMateriaisCotacao = new SolicitacaoCompraMateriaisCotacao();
            solicitacaoCompraMateriaisCotacao.CotacaoFinal = solicitacaoCompraMateriaisCotacaoDTO.CotacaoFinal;
            solicitacaoCompraMateriaisCotacao.Id = 0;
            solicitacaoCompraMateriaisCotacao.IdFornecedor = solicitacaoCompraMateriaisCotacaoDTO.IdFornecedor;
            solicitacaoCompraMateriaisCotacao.IdSolicitacaoCompraMaterial = solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial;
            solicitacaoCompraMateriaisCotacao.ValorUnitarioCotado = solicitacaoCompraMateriaisCotacaoDTO.ValorUnitarioCotado;
            solicitacaoCompraMateriaisCotacao.DataEntrega = solicitacaoCompraMateriaisCotacaoDTO.DataEntrega.Value;
            solicitacaoCompraMateriaisCotacao.Desconto = solicitacaoCompraMateriaisCotacaoDTO.Desconto;
            solicitacaoCompraMateriaisCotacao.ValorUnitarioComDesconto = solicitacaoCompraMateriaisCotacao.ValorUnitarioCotado * (1 - ((solicitacaoCompraMateriaisCotacao.Desconto ?? 0) / 100));

            await _context.SolicitacaoCompra_MateriaisCotacao.AddAsync(solicitacaoCompraMateriaisCotacao);
            await _context.SaveChangesAsync();

            var idsSolicitacaoCompraMateriais = solicitacaoCompra.Materiais.Select(x => x.Id).ToList();

            Int64 idFornecedorPrincipal = 0;
            double? valorTotalCotadoFornecedorPrincipal = null;

            #region Cotação Principal 

            //Define a cotação principal para o fornecedor mais barato

            _context.SolicitacaoCompra_MateriaisCotacao.Where(x => idsSolicitacaoCompraMateriais.Contains(x.IdSolicitacaoCompraMaterial)).ToList().GroupBy(x => x.IdFornecedor).ToList().ForEach(x =>
            {
                var valorTotal = x.Sum(y => y.ValorUnitarioCotado);

                if (!valorTotalCotadoFornecedorPrincipal.HasValue)
                {
                    valorTotalCotadoFornecedorPrincipal = valorTotal;
                    idFornecedorPrincipal = x.Key;
                }
                else if (valorTotal < valorTotalCotadoFornecedorPrincipal)
                {
                    valorTotalCotadoFornecedorPrincipal = valorTotal;
                    idFornecedorPrincipal = x.Key;
                }
            });

            (await _context.SolicitacaoCompra_MateriaisCotacao.Where(x => idsSolicitacaoCompraMateriais.Contains(x.IdSolicitacaoCompraMaterial) && x.IdFornecedor == idFornecedorPrincipal).ToListAsync()).ForEach(x =>
             {
                 x.CotacaoFinal = true;

                 _context.Entry(x).State = EntityState.Modified;
             });

            _context.SaveChanges();
            
            #endregion

            solicitacaoCompra.IdUsuarioAlteracao = idUsuario;
            solicitacaoCompra.DataUltimaAlteracao = DateTime.Now;
            solicitacaoCompra.ValorTotalCotado = 0;

            _context.SolicitacaoCompra_Materiais.Include(x=>x.Cotacoes).Where(x => x.IdSolicitacaoCompra == solicitacaoCompra.Id).ToList().ForEach(x =>
            {
                solicitacaoCompra.ValorTotalCotado += (x.Quantidade * x.Cotacoes.FirstOrDefault(y => y.CotacaoFinal)?.ValorUnitarioCotado ?? 0);
            });

            _context.Entry(solicitacaoCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //Marca a menor cotação como principal caso a cotação a ser incluída não seja marcada como principal
            if (!solicitacaoCompraMateriaisCotacaoDTO.CotacaoFinal)
            {
                //Define todas as cotações como não principal
                _context.SolicitacaoCompra_MateriaisCotacao.Where(x => x.IdSolicitacaoCompraMaterial == solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial).ToList().ForEach(x =>
                {
                    x.CotacaoFinal = false;

                    _context.Entry(x).State = EntityState.Modified;
                    _context.SaveChanges();
                });

                //Define a menor cotação como principal
                var menorCotacao = await _context.SolicitacaoCompra_MateriaisCotacao.Where(x => x.IdSolicitacaoCompraMaterial == solicitacaoCompraMateriaisCotacaoDTO.IdSolicitacaoCompraMaterial).OrderBy(x => x.ValorUnitarioCotado).FirstOrDefaultAsync();
                menorCotacao.CotacaoFinal = true;

                _context.Entry(menorCotacao).State = EntityState.Modified;
                _context.SaveChanges();
            }
            
            return _mapper.Map<SolicitacaoCompraMateriaisCotacaoDTO>(solicitacaoCompraMateriaisCotacao);
        }

        public async Task PostFornecedor(SolicitacaoCompra_PostFornecedor fornecedores, Int64 idUsuario)
        {
            if (_context.SolicitacaoCompra_MateriaisCotacao.Any(x => fornecedores.idFornecedores.Contains( x.IdFornecedor) && _context.SolicitacaoCompra_Materiais.Where(x => x.IdSolicitacaoCompra == fornecedores.idSolicitacaoCompra).Select(x => x.Id).ToList().Contains(x.IdSolicitacaoCompraMaterial)))
                throw new Exception("Um ou mais fornecedores já existem na cotação.");

            var nomeUsuarioAlteracao = _context.Usuario.FirstOrDefault(x => x.Id == idUsuario)?.Nome ?? "";

            fornecedores.idFornecedores.ForEach(idFornecedor =>
            {
                if (!_context.Fornecedor.Any(x => x.Id == idFornecedor))
                    throw new Exception("Fornecedor não encontrado");

                if (!_context.SolicitacaoCompra.Any(x => x.Id == fornecedores.idSolicitacaoCompra))
                    throw new Exception("Solicitação não encontrada");

                

                var fornecedor = _context.Fornecedor.FirstOrDefault(x => x.Id == idFornecedor);

                (_context.SolicitacaoCompra_Materiais.Where(x => x.IdSolicitacaoCompra == fornecedores.idSolicitacaoCompra).ToList()).ForEach(solicitacaoCompraMaterial =>
                {
                    var solicitacaoCompraMaterialCotacao = new SolicitacaoCompraMateriaisCotacao();
                    solicitacaoCompraMaterialCotacao.Id = 0;
                    solicitacaoCompraMaterialCotacao.CotacaoFinal = false;
                    solicitacaoCompraMaterialCotacao.CotacaoMaisBarata = false;
                    solicitacaoCompraMaterialCotacao.IdFornecedor = idFornecedor;
                    solicitacaoCompraMaterialCotacao.IdSolicitacaoCompraMaterial = solicitacaoCompraMaterial.Id;
                    solicitacaoCompraMaterialCotacao.ValorUnitarioCotado = 0;
                    solicitacaoCompraMaterialCotacao.IdCondicaoPagamento = fornecedor.IdCondicaoPagamento;
                    solicitacaoCompraMaterialCotacao.QuantidadeCotado = 0;

                    _context.SolicitacaoCompra_MateriaisCotacao.Add(solicitacaoCompraMaterialCotacao);
                });

                var novoHistorico = new Historico();
                novoHistorico.Id = Guid.NewGuid();
                novoHistorico.Campo = $"Novo fornecedor - {fornecedor.NomeFantasia}";
                novoHistorico.Data = DateTime.Now;
                novoHistorico.IdPedidoCompra = null;
                novoHistorico.IdPedidoInterno = null;
                novoHistorico.IdSolicitacaoCompra = fornecedores.idSolicitacaoCompra;
                novoHistorico.IdUsuario = idUsuario;
                novoHistorico.ValorAntigo = null;
                novoHistorico.ValorNovo = null;
                novoHistorico.Usuario = nomeUsuarioAlteracao;

                _context.Add(novoHistorico);

            });

            await _context.SaveChangesAsync();
        }

        public async Task DefinirFornecedorPrincipal(Int64 idSolicitacaoCompra, Int64 idFornecedor)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra_MateriaisCotacao SET CotacaoFinal = 1 WHERE IdSolicitacaoCompraMaterial IN (SELECT SCM.Id FROM SolicitacaoCompra_Materiais SCM WHERE SCM.IdSolicitacaoCompra = {idSolicitacaoCompra}) AND IdFornecedor = {idFornecedor}");
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra_MateriaisCotacao SET CotacaoFinal = 0 WHERE IdSolicitacaoCompraMaterial IN (SELECT SCM.Id FROM SolicitacaoCompra_Materiais SCM WHERE SCM.IdSolicitacaoCompra = {idSolicitacaoCompra}) AND IdFornecedor != {idFornecedor}");
        }

        public async Task DefinirDataEntregaFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor, DateTime dataEntrega)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra_MateriaisCotacao SET DataEntrega = '{dataEntrega.ToString("dd-MM-yyyy")} 00:00:00' WHERE IdSolicitacaoCompraMaterial IN (SELECT SCM.Id FROM SolicitacaoCompra_Materiais SCM WHERE SCM.IdSolicitacaoCompra = {idSolicitacaoCompra}) AND IdFornecedor = {idFornecedor}");
        }

        public async Task ExcluirFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor, Int64 idUsuario)
        {
            (await _context.SolicitacaoCompra_Materiais.Where(x => x.IdSolicitacaoCompra == idSolicitacaoCompra).ToListAsync()).ForEach(x =>
            {
                _context.SolicitacaoCompra_MateriaisCotacao.Where(y => y.IdSolicitacaoCompraMaterial == x.Id).ToList().ForEach(y =>
                {
                    if (y.IdFornecedor == idFornecedor)
                    {
                        _context.SolicitacaoCompra_MateriaisCotacao.Remove(y);
                        _context.SaveChanges();
                    }
                });
            });

            var nomeFornecedor = _context.Fornecedor.FirstOrDefault(x => x.Id == idFornecedor)?.NomeFantasia ?? "";

            var nomeUsuarioAlteracao = _context.Usuario.FirstOrDefault(x => x.Id == idUsuario)?.Nome ?? "";

            var novoHistorico = new Historico();
            novoHistorico.Id = Guid.NewGuid();
            novoHistorico.Campo = $"Fornecedor removido - {nomeFornecedor}";
            novoHistorico.Data = DateTime.Now;
            novoHistorico.IdPedidoCompra = null;
            novoHistorico.IdPedidoInterno = null;
            novoHistorico.IdSolicitacaoCompra = idSolicitacaoCompra;
            novoHistorico.IdUsuario = idUsuario;
            novoHistorico.ValorAntigo = null;
            novoHistorico.ValorNovo = null;
            novoHistorico.Usuario = nomeUsuarioAlteracao;

            await _context.AddAsync(novoHistorico);
            await _context.SaveChangesAsync();
        }

        public async Task<SolicitacaoCompraDTO> FinalizarCotacao(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            var obra = await _context.Obra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.IdCentroCustoObra);

            //if (obra.AprovacaoAutomatica)
            //solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 4;
            //else
            //solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 3;
            
            solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 4;

            var solicitacaoCompra = await _context.SolicitacaoCompra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.Id);
            solicitacaoCompra.IdUsuarioAlteracao = solicitacaoCompraDTO.IdUsuarioAlteracao;
            solicitacaoCompra.IdUsuarioFinalizacaoCotacao = solicitacaoCompraDTO.IdUsuarioFinalizacaoCotacao;
            solicitacaoCompra.DataUltimaAlteracao = solicitacaoCompraDTO.DataUltimaAlteracao;
            solicitacaoCompra.DataFinalizacaoCotacao = solicitacaoCompraDTO.DataFinalizacaoCotacao;
            solicitacaoCompra.IdStatusSolicitacaoCompra = solicitacaoCompraDTO.IdStatusSolicitacaoCompra;

            _context.Entry(solicitacaoCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //if (obra.AprovacaoAutomatica)
            //{
            //    await MontaPedidoCompra(solicitacaoCompraDTO.Id, solicitacaoCompraDTO.IdUsuarioAlteracao);
            //    await _repositorioObra.CalculaCustoObra(solicitacaoCompraDTO.IdCentroCustoObra ?? 0);
            //}

            await MontaPedidoCompra(solicitacaoCompraDTO.Id, solicitacaoCompra.IdUsuarioAlteracao);

            return await Task.FromResult(_mapper.Map<SolicitacaoCompraDTO>(solicitacaoCompra));
        }

        public async Task CancelarSolicitacaoCompra(Int64 id, string motivo)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 5, MotivoCancelamento = '{motivo}' WHERE Id = {id}");
        }
        public async Task<SolicitacaoCompraDTO> AprovarCotacao(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            var solicitacaoCompra = await _context.SolicitacaoCompra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraDTO.Id);
            solicitacaoCompra.IdUsuarioAlteracao = solicitacaoCompraDTO.IdUsuarioAlteracao;
            solicitacaoCompra.DataUltimaAlteracao = solicitacaoCompraDTO.DataUltimaAlteracao;
            solicitacaoCompra.IdStatusSolicitacaoCompra = solicitacaoCompraDTO.IdStatusSolicitacaoCompra;

            _context.Entry(solicitacaoCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await Task.FromResult(_mapper.Map<SolicitacaoCompraDTO>(solicitacaoCompra));
        }

        public async Task PostCotacaoPrincipal(Int64 Id, Int64 IdUsuario)
        {
            var solicitacaoCompraMateriaisCotacao = await _context.SolicitacaoCompra_MateriaisCotacao.FirstOrDefaultAsync(x => x.Id == Id);

            if (solicitacaoCompraMateriaisCotacao == null)
                throw new Exception("Cotação não encontrada");

            var solicitacaoCompraMaterial = await _context.SolicitacaoCompra_Materiais.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraMateriaisCotacao.IdSolicitacaoCompraMaterial);
            var solicitacaoCompra = await _context.SolicitacaoCompra.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraMaterial.IdSolicitacaoCompra);

            _context.SolicitacaoCompra_MateriaisCotacao.Where(x => x.IdSolicitacaoCompraMaterial == solicitacaoCompraMateriaisCotacao.IdSolicitacaoCompraMaterial).ToList().ForEach(x =>
                {
                    if (x.Id == Id)
                        x.CotacaoFinal = true;
                    else
                        x.CotacaoFinal = false;

                    _context.Entry(x).State = EntityState.Modified;
                    _context.SaveChanges();
                });

            solicitacaoCompra.IdUsuarioAlteracao = IdUsuario;
            solicitacaoCompra.DataUltimaAlteracao = DateTime.Now;
            solicitacaoCompra.ValorTotalCotado = 0;

            _context.SolicitacaoCompra_Materiais.Include(x => x.Cotacoes).Where(x => x.IdSolicitacaoCompra == solicitacaoCompra.Id).ToList().ForEach(x =>
            {
                solicitacaoCompra.ValorTotalCotado += (x.Quantidade * x.Cotacoes.FirstOrDefault(y => y.CotacaoFinal)?.ValorUnitarioCotado ?? 0);
            });

            _context.Entry(solicitacaoCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Int64> ObtemCotacaoParaAprovacao(Int64 IdUsuario)
        {
            return (await _context.SolicitacaoCompra.Where(x => x.IdStatusSolicitacaoCompra == 3 && x.CentroCustoObra.IdUsuarioDiretorAprovador == IdUsuario).ToListAsync()).Count();
        }

        public async Task EditarCotacao(Cotacao_SolicitacaoCompraDTO cotacaoSolicitacaoCompraDTO, Int64 idUsuario)
        {
            var nomeUsuarioAlteracao = _context.Usuario.FirstOrDefault(x => x.Id == idUsuario)?.Nome ?? "";
            var historicos = new List<Historico>();

            var condicoesPagamento = _context.CondicaoPagamento.ToList();

            var solicitacaoCompra = await _context.SolicitacaoCompra
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x=>x.PagamentoManual)
                .FirstOrDefaultAsync(x => x.Id == cotacaoSolicitacaoCompraDTO.Id);

            var fornecedoresDict = cotacaoSolicitacaoCompraDTO.Fornecedores.ToDictionary(x => x.Id);

            cotacaoSolicitacaoCompraDTO.Materiais.ForEach(materialDTO =>
            {
                materialDTO.Cotacoes.ForEach(cotacaoDTO =>
                {
                    var cotacao = solicitacaoCompra.Materiais.FirstOrDefault(x => x.Id == materialDTO.IdSolicitacaoCompraMaterial).Cotacoes.FirstOrDefault(x => x.Id == cotacaoDTO.IdSolicitacaoCompraMaterialCotacao);

                    var fornecedor = fornecedoresDict[cotacaoDTO.IdFornecedor];

                    void AdicionarHistorico(string campo, string valorAntigo, string valorNovo)
                    {
                        historicos.Add(new Historico
                        {
                            Id = Guid.NewGuid(),
                            Campo = $"Material: {materialDTO.Descricao}, Fornecedor: {fornecedor.NomeFantasia} - {campo}",
                            Data = DateTime.Now,
                            IdPedidoCompra = null,
                            IdPedidoInterno = null,
                            IdSolicitacaoCompra = cotacaoSolicitacaoCompraDTO.Id,
                            IdUsuario = idUsuario,
                            ValorAntigo = valorAntigo,
                            ValorNovo = valorNovo,
                            Usuario = nomeUsuarioAlteracao
                        });
                    }

                    if (cotacaoDTO.QuantidadeCotado != cotacao.QuantidadeCotado)
                        AdicionarHistorico("Quantidade cotada", cotacao.QuantidadeCotado.ToString(), cotacaoDTO.QuantidadeCotado.ToString());

                    if (cotacaoDTO.ValorUnitarioCotado != cotacao.ValorUnitarioCotado)
                        AdicionarHistorico("Valor unitário", cotacao.ValorUnitarioCotado.ToString(), cotacaoDTO.ValorUnitarioCotado.ToString());

                    if (cotacaoDTO.DataEntrega != cotacao.DataEntrega)
                        AdicionarHistorico("Data entrega",
                            cotacao.DataEntrega?.ToString("dd/MM/yyyy") ?? "",
                            cotacaoDTO.DataEntrega?.ToString("dd/MM/yyyy") ?? "");

                    if (fornecedor.CondicaoPagamento.Id != cotacao.IdCondicaoPagamento)
                    {
                        var condicaoNova = condicoesPagamento.FirstOrDefault(x => x.Id == fornecedor.CondicaoPagamento.Id)?.Descricao ?? "";
                        var condicaoAntiga = condicoesPagamento.FirstOrDefault(x => x.Id == cotacao.IdCondicaoPagamento)?.Descricao ?? "";

                        AdicionarHistorico("Condição pagamento", condicaoAntiga, condicaoNova);
                    }

                    if (fornecedor.Frete != cotacao.Frete)
                        AdicionarHistorico("Frete", cotacao.Frete.ToString(), fornecedor.Frete.ToString());

                    if (fornecedor.Imposto != cotacao.Imposto)
                        AdicionarHistorico("Imposto", cotacao.Imposto.ToString(), fornecedor.Imposto.ToString());


                    if (fornecedor.Desconto != cotacao.Desconto)
                        AdicionarHistorico("Desconto", cotacao.Desconto.ToString(), fornecedor.Desconto.ToString());

                    cotacao.ValorUnitarioCotado = cotacaoDTO.ValorUnitarioCotado;
                    cotacao.QuantidadeCotado = cotacaoDTO.QuantidadeCotado;
                    cotacao.DataEntrega = cotacaoDTO.DataEntrega;
                    cotacao.IdCondicaoPagamento = fornecedor.CondicaoPagamento.Id;
                    cotacao.Frete = fornecedor.Frete;
                    cotacao.Imposto = fornecedor.Imposto;
                    cotacao.Desconto = fornecedor.Desconto;
                    cotacao.ValorUnitarioComDesconto = cotacao.ValorUnitarioCotado * (1 - ((cotacao.Desconto ?? 0) / 100));

                    _context.Entry(cotacao).State = EntityState.Modified;

                });


            });

            solicitacaoCompra.EnderecoEntrega = cotacaoSolicitacaoCompraDTO.Endereco;
            solicitacaoCompra.ObservacaoDeAprovacao = cotacaoSolicitacaoCompraDTO.ObservacaoDeAprovacao;
            solicitacaoCompra.ObservacaoParaFornecedor = cotacaoSolicitacaoCompraDTO.ObservacaoParaFornecedor;
            solicitacaoCompra.IdUsuarioAlteracao = idUsuario;
            solicitacaoCompra.DataUltimaAlteracao = DateTime.Now;

            #region Cotação Mais Barata

            solicitacaoCompra.Materiais.ForEach(material =>
            {
                material.Quantidade = cotacaoSolicitacaoCompraDTO.Materiais.FirstOrDefault(x => x.IdSolicitacaoCompraMaterial == material.Id)?.Quantidade ?? material.Quantidade;

                var cotacaoMaisBarata = material.Cotacoes?.Where(x => x.ValorUnitarioCotado > 0)?.OrderBy(x => x.ValorUnitarioCotado)?.FirstOrDefault();

                if (cotacaoMaisBarata != null)
                    material.Cotacoes.ForEach(cotacao =>
                    {
                        if (cotacao.Id == cotacaoMaisBarata.Id)
                        {
                            cotacao.CotacaoMaisBarata = true;
                            cotacao.CotacaoFinal = true;
                        }
                        else
                            cotacao.CotacaoMaisBarata = false;

                        _context.Entry(cotacao).State = EntityState.Modified;
                    });

                _context.Entry(material).State = EntityState.Modified;
            });

            #endregion

            solicitacaoCompra.Materiais.FirstOrDefault().Cotacoes.ToList().ForEach(cotacao =>
            {

                cotacao.PagamentoManual?.ForEach(pagamentoManual =>
                {
                    var novoHistorico = new Historico();
                    novoHistorico.Id = Guid.NewGuid();
                    novoHistorico.Campo = $"Pagamento manual '{pagamentoManual.Data.ToString("dd/MM/yyyy")}' valor {pagamentoManual.Valor.ToString().Replace(',', '.')} removido";
                    novoHistorico.Data = DateTime.Now;
                    novoHistorico.IdSolicitacaoCompra = solicitacaoCompra.Id;
                    novoHistorico.IdUsuario = idUsuario;
                    novoHistorico.Usuario = nomeUsuarioAlteracao;
                    historicos.Add(novoHistorico);

                    _context.Entry(pagamentoManual).State = EntityState.Deleted;
                });

                if (cotacao.IdCondicaoPagamento == 7)
                {
                    cotacaoSolicitacaoCompraDTO.Fornecedores.FirstOrDefault(x => x.Id == cotacao.IdFornecedor)?.PagamentoManual?.ForEach(pagamentoManualDTO =>
                    {
                        var pagamentoManual = new SolicitacaoCompraMaterialCotacaoPagamentoManual();
                        pagamentoManual.Data = pagamentoManualDTO.Data;
                        pagamentoManual.DataCadastro = DateTime.Now;
                        pagamentoManual.DataUltimaAlteracao = DateTime.Now;
                        pagamentoManual.Id = 0;
                        pagamentoManual.IdSolicitacaoCompraMaterialCotacao = cotacao.Id;
                        pagamentoManual.IdUsuarioAlteracao = idUsuario;
                        pagamentoManual.IdUsuarioCadastro = idUsuario;
                        pagamentoManual.Valor = pagamentoManualDTO.Valor;

                        _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Add(pagamentoManual);

                        var novoHistorico = new Historico();
                        novoHistorico.Id = Guid.NewGuid();
                        novoHistorico.Campo = $"Pagamento manual '{pagamentoManual.Data.ToString("dd/MM/yyyy")}' valor {pagamentoManual.Valor.ToString().Replace(',', '.')} adicionado";
                        novoHistorico.Data = DateTime.Now;
                        novoHistorico.IdSolicitacaoCompra = solicitacaoCompra.Id;
                        novoHistorico.IdUsuario = idUsuario;
                        novoHistorico.Usuario = nomeUsuarioAlteracao;
                        historicos.Add(novoHistorico);
                    });
                }
            });

            //if (solicitacaoCompra.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault().IdCondicaoPagamento == 7)
            //{
            //    var idsSolicitacaoMaterial = _context.SolicitacaoCompra_Materiais.Where(x => x.IdSolicitacaoCompra == cotacaoSolicitacaoCompraDTO.Id).Select(x => x.Id).ToList();

            //    _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Where(x => idsSolicitacaoMaterial.Contains(x.SolicitacaoCompraMateriaisCotacao.IdSolicitacaoCompraMaterial)).ToList().ForEach(x =>
            //    {
            //        _context.Entry(x).State = EntityState.Deleted;
            //        _context.SaveChanges();
            //    });

            //    cotacaoSolicitacaoCompraDTO.Fornecedores.ForEach(fornecedor =>
            //    {
            //        fornecedor.PagamentoManual.ForEach(pagamento =>
            //        {
            //            var cotacao = solicitacaoCompra.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);

            //            var pagamentoManual = new SolicitacaoCompraMaterialCotacaoPagamentoManual();
            //            pagamentoManual.Data = pagamento.Data;
            //            pagamentoManual.DataCadastro = DateTime.Now;
            //            pagamentoManual.DataUltimaAlteracao = DateTime.Now;
            //            pagamentoManual.Id = 0;
            //            pagamentoManual.IdSolicitacaoCompraMaterialCotacao = cotacao.Id;
            //            pagamentoManual.IdUsuarioAlteracao = idUsuario;
            //            pagamentoManual.IdUsuarioCadastro = idUsuario;
            //            pagamentoManual.Valor = pagamento.Valor;

            //            _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Add(pagamentoManual);
            //            _context.SaveChanges();
            //        });


            //    });
            //}

            _context.Historico.AddRange(historicos);
            _context.SaveChanges();


            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET ValorMelhorCotacao = (SELECT TOP 1 SUM(((scm.Quantidade * scmct.ValorUnitarioCotado)) * (1-(scmct.Desconto/100))) + MAX(scmct.Frete) + MAX(scmct.Imposto) AS TotalGeral FROM SolicitacaoCompra_Materiais scm JOIN SolicitacaoCompra_MateriaisCotacao scmct ON scm.Id = scmct.IdSolicitacaoCompraMaterial WHERE scm.IdSolicitacaoCompra = {solicitacaoCompra.Id} GROUP BY scmct.IdFornecedor ORDER BY TotalGeral ASC) WHERE Id = {solicitacaoCompra.Id}");



        }

        public async Task MontaPedidoCompra(Int64 IdSolicitacaoCompra, Int64 IdUsuario)
        {
            var solicitacaoCompra = await _context.SolicitacaoCompra
                .Include(x=>x.CentroCustoObra)
                .ThenInclude(x=>x.Cliente)
                .Include(x=>x.Materiais)
                .ThenInclude(x=>x.Cotacoes)
                .Include(x=>x.CentroCustoObra)
                .FirstOrDefaultAsync(x => x.Id == IdSolicitacaoCompra);

            if (solicitacaoCompra == null)
                throw new Exception("Solicitação não encontrada");

            var cotacoesPrincipais = new List<SolicitacaoCompraMateriaisCotacao>();
            solicitacaoCompra.Materiais.ForEach(x =>
            {
                cotacoesPrincipais.AddRange(x.Cotacoes.Where(x => x.QuantidadeCotado > 0).ToList());
            });

            var cotacoesAgrupadas = cotacoesPrincipais.GroupBy(x => x.IdFornecedor);
            
            foreach(var item in cotacoesAgrupadas)
            {
                //Monta pedido de compra por fornecedor
                var pedidoCompra = new PedidoCompra();
                pedidoCompra.Id = 0;
                pedidoCompra.CodigoAno = DateTime.Now.Year;
                pedidoCompra.CodigoSequencia = (_context.PedidoCompra.Where(x => x.CodigoAno == DateTime.Now.Year).Max(x => (int?)x.CodigoSequencia) ?? 97000) + 1;
                pedidoCompra.Codigo = $"P{pedidoCompra.CodigoSequencia}";
                pedidoCompra.DataCadastro = DateTime.Now;
                pedidoCompra.DataEntrega = solicitacaoCompra.DataEntrega.Value;
                pedidoCompra.DataUltimaAlteracao = DateTime.Now;
                pedidoCompra.IdCentroCustoDEF = solicitacaoCompra.IdCentroCustoDEF;
                pedidoCompra.IdCentroCustoObra = solicitacaoCompra.IdCentroCustoObra;
                pedidoCompra.IdCondicaoPagamento = cotacoesPrincipais.FirstOrDefault(x => x.IdFornecedor == item.Key).IdCondicaoPagamento;
                pedidoCompra.IdFornecedor = item.Key;
                pedidoCompra.IdSolicitacaoCompra = solicitacaoCompra.Id;
                pedidoCompra.IdStatusPedidoCompra = 1;
                pedidoCompra.IdTipoCentroCusto = solicitacaoCompra.IdTipoCentroCusto;
                pedidoCompra.IdUsuarioAlteracao = IdUsuario;
                pedidoCompra.IdUsuarioCadastro = IdUsuario;
                pedidoCompra.IdUsuarioComprador = solicitacaoCompra.IdUsuarioComprador??1;
                pedidoCompra.ImportadoParaFinanceiro = null;
                pedidoCompra.EnderecoEntrega = solicitacaoCompra.EnderecoEntrega;
                pedidoCompra.Frete = cotacoesPrincipais.FirstOrDefault(x => x.IdFornecedor == item.Key)?.Frete ?? 0;
                pedidoCompra.Imposto = cotacoesPrincipais.FirstOrDefault(x => x.IdFornecedor == item.Key)?.Imposto ?? 0;
                pedidoCompra.ObservacaoParaFornecedor = solicitacaoCompra.ObservacaoParaFornecedor != null ? solicitacaoCompra.ObservacaoParaFornecedor: "";

                var porcentagemDesconto = cotacoesPrincipais.Where(x => x.IdFornecedor == item.Key).FirstOrDefault().Desconto;

                cotacoesPrincipais.Where(x => x.IdFornecedor == item.Key).ToList().ForEach(x =>
                {
                    //pedidoCompra.ValorTotal += (x.ValorUnitarioCotado * (100 - (x.Desconto??0))/100) * x.QuantidadeCotado;
                    pedidoCompra.ValorTotal += x.ValorUnitarioCotado * x.QuantidadeCotado;
                });

                pedidoCompra.ValorDesconto = pedidoCompra.ValorTotal * ((porcentagemDesconto??0) / 100);
                pedidoCompra.ValorTotal -= pedidoCompra.ValorDesconto;
                pedidoCompra.ValorTotal += pedidoCompra.Frete;
                pedidoCompra.ValorTotal += pedidoCompra.Imposto;

                    await _context.PedidoCompra.AddAsync(pedidoCompra);
                    await _context.SaveChangesAsync();
               
                pedidoCompra = await _context.PedidoCompra
                    .Include(x => x.CondicaoPagamento)
                    .ThenInclude(x => x.Parcelas)
                    .FirstOrDefaultAsync(x => x.Id == pedidoCompra.Id);

                cotacoesPrincipais.Where(x => x.IdFornecedor == item.Key).ToList().ForEach(x =>
                {
                    var pedidoCompra_Materiais = new PedidoCompraMateriais();
                    pedidoCompra_Materiais.Id = 0;
                    pedidoCompra_Materiais.DataCadastro = DateTime.Now;
                    pedidoCompra_Materiais.DataUltimaAlteracao = DateTime.Now;
                    pedidoCompra_Materiais.IdMaterial = x.Material.IdMaterial;
                    pedidoCompra_Materiais.Quantidade = x.QuantidadeCotado;
                    pedidoCompra_Materiais.QuantidadeConciliada = 0;
                    pedidoCompra_Materiais.IdPedidoCompra = pedidoCompra.Id;
                    pedidoCompra_Materiais.IdUsuarioAlteracao = IdUsuario;
                    pedidoCompra_Materiais.IdUsuarioCadastro = IdUsuario;
                    pedidoCompra_Materiais.ValorTotal = x.ValorUnitarioComDesconto * x.QuantidadeCotado;
                    pedidoCompra_Materiais.ValorUnitario = x.ValorUnitarioComDesconto;

                    _context.PedidoCompra_Materiais.Add(pedidoCompra_Materiais);
                    _context.SaveChanges();
                });

                var condicaoPagamento = _context.CondicaoPagamento.Include(x=>x.Parcelas).FirstOrDefault(x => x.Id == pedidoCompra.IdCondicaoPagamento);

                var intChar = 65;
                var maisDeUmaParcela = condicaoPagamento.Parcelas.Count > 1 ? true : false;

                if (condicaoPagamento.Id == 7)
                {
                    //A condição de pagamento é manual, portanto precisa buscar os valores corretamente
                    var idCotacaoPrincipal = cotacoesPrincipais.FirstOrDefault(x => x.IdFornecedor == item.Key).Id;
                    var pagamentos = _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.Where(x => x.IdSolicitacaoCompraMaterialCotacao == idCotacaoPrincipal).ToList();
                    
                    pagamentos.ForEach(x =>
                    {
                        var fatura = new PedidoCompraFatura();
                        fatura.Id = 0;
                        fatura.IdPedidoCompra = pedidoCompra.Id;
                        fatura.DataFatura = x.Data;
                        fatura.PagamentoEfetuado = false;
                        fatura.Valor = x.Valor;
                        fatura.CodigoFormatado = $"{pedidoCompra.Codigo}{(maisDeUmaParcela ? $".{Convert.ToChar(intChar)}" : "")}";

                        _context.PedidoCompra_Faturas.Add(fatura);
                        _context.SaveChanges();

                        intChar++;
                    });
                }
                else
                {
                    condicaoPagamento.Parcelas.ForEach(parcelaCondicaoCompra =>
                    {
                        var fatura = new PedidoCompraFatura();
                        fatura.Id = 0;
                        fatura.IdPedidoCompra = pedidoCompra.Id;
                        fatura.DataFatura = pedidoCompra.DataCadastro.AddDays(parcelaCondicaoCompra.DiasCorridos);
                        fatura.PagamentoEfetuado = false;
                        fatura.Valor = pedidoCompra.ValorTotal / 100 * parcelaCondicaoCompra.PorcentagemValorTotal;
                        fatura.CodigoFormatado = $"{pedidoCompra.Codigo}{(maisDeUmaParcela ? $".{Convert.ToChar(intChar)}" : "")}";

                        _context.PedidoCompra_Faturas.Add(fatura);
                        _context.SaveChanges();

                        intChar++;
                    });
                }
                
            }
            
        }

        public async Task Upload(SolicitacaoCompraArquivosDTO solicitacaoCompraArquivoDTO)
        {
            if (_context.SolicitacaoCompra_Arquivos.Any(x => x.IdSolicitacaoCompra == solicitacaoCompraArquivoDTO.IdSolicitacaoCompra && x.Nome == solicitacaoCompraArquivoDTO.Nome))
                throw new Exception("Já existe um arquivo com este nome");

            var solicitacaoCompraArquivos = new SolicitacaoCompraArquivos();
            solicitacaoCompraArquivos.DataCadastro = solicitacaoCompraArquivoDTO.DataCadastro;
            solicitacaoCompraArquivos.Extensao = solicitacaoCompraArquivoDTO.Extensao;
            solicitacaoCompraArquivos.Id = solicitacaoCompraArquivoDTO.Id;
            solicitacaoCompraArquivos.IdSolicitacaoCompra = solicitacaoCompraArquivoDTO.IdSolicitacaoCompra;
            solicitacaoCompraArquivos.IdUsuarioCadastro = solicitacaoCompraArquivoDTO.IdUsuarioCadastro;
            solicitacaoCompraArquivos.Nome = solicitacaoCompraArquivoDTO.Nome;
            solicitacaoCompraArquivos.NomeLogico = solicitacaoCompraArquivoDTO.NomeLogico;
            solicitacaoCompraArquivos.TamanhoMB = solicitacaoCompraArquivoDTO.TamanhoMB;

            await _context.SolicitacaoCompra_Arquivos.AddAsync(solicitacaoCompraArquivos);
            await _context.SaveChangesAsync();
        }

        public async Task<SolicitacaoCompraArquivosDTO> GetFile(Int64 id)
        {
            return _mapper.Map<SolicitacaoCompraArquivosDTO>(await _context.SolicitacaoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<List<SolicitacaoCompraArquivosDTO>> ObtemArquivos(Int64 idSolicitacaoCompra)
        {
            return _mapper.Map<List<SolicitacaoCompraArquivosDTO>>(await _context.SolicitacaoCompra_Arquivos.Include(x => x.UsuarioCadastro).Where(x => x.IdSolicitacaoCompra == idSolicitacaoCompra).ToListAsync());
        }

        public async Task DeleteArquivo(Int64 idArquivo)
        {
            var arquivo = await _context.SolicitacaoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == idArquivo);

            if (arquivo == null)
                throw new Exception("Arquivo não encontrado");

            _context.SolicitacaoCompra_Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task PostComentario(SolicitacaoCompraComentariosDTO solicitacaoCompraComentarioDTO)
        {
            if (!_context.SolicitacaoCompra.Any(x => x.Id == solicitacaoCompraComentarioDTO.IdSolicitacaoCompra))
                throw new Exception("Solicitação de compra não encontrada");

            if (string.IsNullOrEmpty(solicitacaoCompraComentarioDTO.Observacao))
                throw new Exception("Comentário inválido");

            var solicitacaoCompraComentario = new SolicitacaoCompraComentarios();
            solicitacaoCompraComentario.Id = 0;
            solicitacaoCompraComentario.DataCadastro = solicitacaoCompraComentarioDTO.DataCadastro;
            solicitacaoCompraComentario.IdSolicitacaoCompra = solicitacaoCompraComentarioDTO.IdSolicitacaoCompra;
            solicitacaoCompraComentario.IdUsuarioCadastro = solicitacaoCompraComentarioDTO.IdUsuarioCadastro;
            solicitacaoCompraComentario.Observacao = solicitacaoCompraComentarioDTO.Observacao;

            await _context.SolicitacaoCompra_Comentarios.AddAsync(solicitacaoCompraComentario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SolicitacaoCompraComentariosDTO>> Comentarios(Int64 idSolicitacaoCompra)
        {
            return _mapper.Map<List<SolicitacaoCompraComentariosDTO>>(await _context.SolicitacaoCompra_Comentarios.AsNoTracking().Include(x => x.UsuarioCadastro).Where(x => x.IdSolicitacaoCompra == idSolicitacaoCompra).ToListAsync());
        }

        public async Task<bool> PostCotacaoReprovar(Int64 idSolicitacaoCompra)
        {
            return (await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 2 WHERE Id = {idSolicitacaoCompra}")) > 0 ? true : false;
        }

        public async Task<SolicitacaoCompraDTO> RemoverItensSelecionados(Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {
            var solicitacaoPaiDTO = await _context.SolicitacaoCompra.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cotacao_SolicitacaoCompraDTO.Id);

            var novaSolicitacaoCompraDTO = new SolicitacaoCompraDTO();
            novaSolicitacaoCompraDTO.DataCadastro = DateTime.Now;
            novaSolicitacaoCompraDTO.DataEntrega = solicitacaoPaiDTO.DataEntrega;
            novaSolicitacaoCompraDTO.DataFinalizacaoCotacao = null;
            novaSolicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;
            novaSolicitacaoCompraDTO.EnderecoEntrega = solicitacaoPaiDTO.EnderecoEntrega;
            novaSolicitacaoCompraDTO.Id = 0;
            novaSolicitacaoCompraDTO.IdCentroCustoDEF = solicitacaoPaiDTO.IdCentroCustoDEF;
            novaSolicitacaoCompraDTO.IdCentroCustoObra = solicitacaoPaiDTO.IdCentroCustoObra;
            novaSolicitacaoCompraDTO.IdTipoCentroCusto = solicitacaoPaiDTO.IdTipoCentroCusto;
            novaSolicitacaoCompraDTO.IdUsuarioAlteracao = solicitacaoPaiDTO.IdUsuarioAlteracao;
            novaSolicitacaoCompraDTO.IdUsuarioCadastro = solicitacaoPaiDTO.IdUsuarioCadastro;
            novaSolicitacaoCompraDTO.Observacao = solicitacaoPaiDTO.Observacao;
            novaSolicitacaoCompraDTO.Servico = solicitacaoPaiDTO.Servico;
            novaSolicitacaoCompraDTO.ValorEstimado = solicitacaoPaiDTO.ValorEstimado;
            novaSolicitacaoCompraDTO.ValorTotalCotado = 0;
            novaSolicitacaoCompraDTO.Nome = solicitacaoPaiDTO.Nome;
            novaSolicitacaoCompraDTO.IdUsuarioComprador = solicitacaoPaiDTO.IdUsuarioComprador;

            if (cotacao_SolicitacaoCompraDTO.AprovacaoAutomatica == true)
            {

                novaSolicitacaoCompraDTO.IdStatusSolicitacaoCompra = 2;
                novaSolicitacaoCompraDTO.DataAprovacaoEngenheiro = DateTime.Now;
                novaSolicitacaoCompraDTO.IdEngenheiroAprovador = solicitacaoPaiDTO.IdUsuarioAlteracao;
            }
            else
            {
                novaSolicitacaoCompraDTO.IdStatusSolicitacaoCompra = 1;
            }

                novaSolicitacaoCompraDTO.Materiais = new List<SolicitacaoCompraMateriaisDTO>();
            cotacao_SolicitacaoCompraDTO.Materiais.Where(x => x.ItemSelecionado).ToList().ForEach(material =>
              {
                  novaSolicitacaoCompraDTO.Materiais.Add(new SolicitacaoCompraMateriaisDTO()
                  {
                      Id = 0,
                      IdCentroCustoDEF = novaSolicitacaoCompraDTO.IdCentroCustoDEF,
                      IdCentroCustoObra = novaSolicitacaoCompraDTO.IdCentroCustoObra,
                      IdMaterial = material.IdMaterial,
                      IdTipoCentroCusto = novaSolicitacaoCompraDTO.IdTipoCentroCusto,
                      Quantidade = material.Quantidade,
                      ValorUnitarioEstimado = material.ValorUnitarioEstimado
                  });

              });


            novaSolicitacaoCompraDTO = await Post(novaSolicitacaoCompraDTO);

            cotacao_SolicitacaoCompraDTO.Materiais.Where(x => x.ItemSelecionado).ToList().ForEach(material =>
            {
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM SolicitacaoCompra_MateriaisCotacao_PagamentoManual WHERE IdSolicitacaoCompraMaterialCotacao IN (SELECT Id FROM SolicitacaoCompra_MateriaisCotacao WHERE IdSolicitacaoCompraMaterial = {material.IdSolicitacaoCompraMaterial})");
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM SolicitacaoCompra_MateriaisCotacao WHERE IdSolicitacaoCompraMaterial = {material.IdSolicitacaoCompraMaterial}");
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM SolicitacaoCompra_Materiais WHERE Id = {material.IdSolicitacaoCompraMaterial}");
            });

            _context.Database.ExecuteSqlInterpolated($"UPDATE SolicitacaoCompra SET ValorTotalCotado = (SELECT ISNULL((SELECT TOP(1) SUM(QuantidadeCotado * ValorUnitarioCotado) FROM SolicitacaoCompra_MateriaisCotacao WHERE IdSolicitacaoCompraMaterial IN (SELECT Id FROM SolicitacaoCompra_Materiais WHERE IdSolicitacaoCompra = {solicitacaoPaiDTO.Id}) GROUP BY IdFornecedor ORDER BY SUM(QuantidadeCotado * ValorUnitarioCotado) DESC), 0)) WHERE Id = {solicitacaoPaiDTO.Id}");

            _context.Database.ExecuteSqlInterpolated($"UPDATE SolicitacaoCompra SET ValorEstimado = (SELECT SUM(Quantidade * COALESCE(ValorUnitarioEstimado, 0)) FROM SolicitacaoCompra_Materiais WHERE idsolicitacaocompra = {novaSolicitacaoCompraDTO.Id})");
            _context.Database.ExecuteSqlInterpolated($"UPDATE SolicitacaoCompra SET ValorEstimado = (SELECT SUM(Quantidade * COALESCE(ValorUnitarioEstimado, 0)) FROM SolicitacaoCompra_Materiais WHERE idsolicitacaocompra = {solicitacaoPaiDTO.Id})");

            return novaSolicitacaoCompraDTO;
        }

        public async Task<SolicitacaoCompraDTO> DuplicarItensSelecionados(Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {
            var solicitacaoPaiDTO = await _context.SolicitacaoCompra.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cotacao_SolicitacaoCompraDTO.Id);

            var novaSolicitacaoCompraDTO = new SolicitacaoCompraDTO();
            novaSolicitacaoCompraDTO.DataCadastro = DateTime.Now;
            novaSolicitacaoCompraDTO.DataEntrega = solicitacaoPaiDTO.DataEntrega;
            novaSolicitacaoCompraDTO.DataFinalizacaoCotacao = null;
            novaSolicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;
            novaSolicitacaoCompraDTO.EnderecoEntrega = solicitacaoPaiDTO.EnderecoEntrega;
            novaSolicitacaoCompraDTO.Id = 0;
            novaSolicitacaoCompraDTO.IdCentroCustoDEF = solicitacaoPaiDTO.IdCentroCustoDEF;
            novaSolicitacaoCompraDTO.IdCentroCustoObra = solicitacaoPaiDTO.IdCentroCustoObra;
            novaSolicitacaoCompraDTO.IdTipoCentroCusto = solicitacaoPaiDTO.IdTipoCentroCusto;
            novaSolicitacaoCompraDTO.IdUsuarioAlteracao = solicitacaoPaiDTO.IdUsuarioAlteracao;
            novaSolicitacaoCompraDTO.IdUsuarioCadastro = solicitacaoPaiDTO.IdUsuarioCadastro;
            novaSolicitacaoCompraDTO.Observacao = solicitacaoPaiDTO.Observacao;
            novaSolicitacaoCompraDTO.Servico = solicitacaoPaiDTO.Servico;
            novaSolicitacaoCompraDTO.ValorEstimado = solicitacaoPaiDTO.ValorEstimado;
            novaSolicitacaoCompraDTO.ValorTotalCotado = 0;
            novaSolicitacaoCompraDTO.Nome = solicitacaoPaiDTO.Nome;
            novaSolicitacaoCompraDTO.IdUsuarioComprador = solicitacaoPaiDTO.IdUsuarioComprador;

            if (cotacao_SolicitacaoCompraDTO.AprovacaoAutomatica == true)
            {

                novaSolicitacaoCompraDTO.IdStatusSolicitacaoCompra = 2;
                novaSolicitacaoCompraDTO.DataAprovacaoEngenheiro = DateTime.Now;
                novaSolicitacaoCompraDTO.IdEngenheiroAprovador = solicitacaoPaiDTO.IdUsuarioAlteracao;
            }
            else
            {
                novaSolicitacaoCompraDTO.IdStatusSolicitacaoCompra = 1;
            }

            novaSolicitacaoCompraDTO.Materiais = new List<SolicitacaoCompraMateriaisDTO>();
            cotacao_SolicitacaoCompraDTO.Materiais.Where(x => x.ItemSelecionado).ToList().ForEach(material =>
            {
                novaSolicitacaoCompraDTO.Materiais.Add(new SolicitacaoCompraMateriaisDTO()
                {
                    Id = 0,
                    IdCentroCustoDEF = novaSolicitacaoCompraDTO.IdCentroCustoDEF,
                    IdCentroCustoObra = novaSolicitacaoCompraDTO.IdCentroCustoObra,
                    IdMaterial = material.IdMaterial,
                    IdTipoCentroCusto = novaSolicitacaoCompraDTO.IdTipoCentroCusto,
                    Quantidade = material.Quantidade,
                    ValorUnitarioEstimado = material.ValorUnitarioEstimado
                });

            });


            novaSolicitacaoCompraDTO = await Post(novaSolicitacaoCompraDTO);

            _context.Database.ExecuteSqlInterpolated($"UPDATE SolicitacaoCompra SET ValorEstimado = (SELECT SUM(Quantidade * COALESCE(ValorUnitarioEstimado, 0)) FROM SolicitacaoCompra_Materiais WHERE idsolicitacaocompra = {novaSolicitacaoCompraDTO.Id})");

            return novaSolicitacaoCompraDTO;
        }

        public async Task DevolverCompras(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 7, ObservacaoDeAprovacao = '{objetoAprovacaoDTO.ObservacaoDeAprovacao}', IdDiretorAprovador = {objetoAprovacaoDTO.IdDiretorAprovador}, DataAprovacaoDiretor = '{objetoAprovacaoDTO.DataAprovacaoDiretor.ToString("MM/dd/yyyy HH:mm:ss")}' WHERE Id = {objetoAprovacaoDTO.IdSolicitacaoCompra}");
        }

        public async Task EnviarDiretoria(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 3, IdUsuarioFinalizacaoCotacao = {objetoAprovacaoDTO.IdUsuarioFinalizacaoCotacao}, DataFinalizacaoCotacao = GETDATE() WHERE Id = {objetoAprovacaoDTO.IdSolicitacaoCompra}");
        }

        public async Task ReprovarSolicitacaoCompra(Int64 idSolicitacaoCompra, Int64 idUsuario)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 6, IdUsuarioAlteracao = {idUsuario}, DataUltimaAlteracao = GETDATE() WHERE Id = {idSolicitacaoCompra}");
        }

        public async Task<bool> VerificaDivergenciaValorCondicaoManual(long idSolicitacaoCompra)
        {
            const int condicao = 7;
            const double tol = 0.01d; // tolerância (centavos)

            var existeDivergencia = await
                _context.SolicitacaoCompra_MateriaisCotacao.AsNoTracking()
                    .Join(_context.SolicitacaoCompra_Materiais.AsNoTracking(),
                          mc => mc.IdSolicitacaoCompraMaterial,
                          m => m.Id,
                          (mc, m) => new { mc, m })
                    .Where(x => x.m.IdSolicitacaoCompra == idSolicitacaoCompra
                             && x.mc.IdCondicaoPagamento == condicao && x.mc.QuantidadeCotado > 0)
                    .GroupBy(x => new { x.m.IdSolicitacaoCompra, x.mc.IdFornecedor })
                    .Select(g => new
                    {
                        g.Key.IdSolicitacaoCompra,
                        g.Key.IdFornecedor,
                        ValorCotado = g.Sum(x =>
                            ((x.mc.QuantidadeCotado) * (x.mc.ValorUnitarioComDesconto)) +
                            (x.mc.Frete ?? 0d) +
                            (x.mc.Imposto ?? 0d))
                    })
                    .Join(
                        _context.SolicitacaoCompra_MateriaisCotacao.AsNoTracking()
                            .Join(_context.SolicitacaoCompra_Materiais.AsNoTracking(),
                                  mc => mc.IdSolicitacaoCompraMaterial,
                                  m => m.Id,
                                  (mc, m) => new { mc, m })
                            .Where(x => x.m.IdSolicitacaoCompra == idSolicitacaoCompra
                                     && x.mc.IdCondicaoPagamento == condicao)
                            // LEFT JOIN com pagamentos manuais
                            .GroupJoin(
                                _context.SolicitacaoCompra_MateriaisCotacao_PagamentoManual.AsNoTracking(),
                                x => x.mc.Id,
                                pm => pm.IdSolicitacaoCompraMaterialCotacao,
                                (x, pm) => new { x, pm })
                            .SelectMany(z => z.pm.DefaultIfEmpty(), (z, pm) => new { z.x, pm })
                            .GroupBy(z => new { z.x.m.IdSolicitacaoCompra, z.x.mc.IdFornecedor })
                            .Select(g => new
                            {
                                g.Key.IdSolicitacaoCompra,
                                g.Key.IdFornecedor,
                                ValorPago = g.Sum(z => z.pm != null ? (z.pm.Valor) : 0d)
                            }),
                        c => new { c.IdSolicitacaoCompra, c.IdFornecedor },
                        p => new { p.IdSolicitacaoCompra, p.IdFornecedor },
                        (c, p) => new { c.ValorCotado, p.ValorPago }
                    )
                    // existe divergência?
                    .AnyAsync(x => Math.Abs(x.ValorCotado - x.ValorPago) > tol);

            return existeDivergencia;
        }

        public async Task EnviarParaRevisao(Int64 idSolicitacaoCompra)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 8 WHERE Id = {idSolicitacaoCompra}");
        }

        public async Task EnviarParaAprovacao(Int64 idSolicitacaoCompra)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE SolicitacaoCompra SET IdStatusSolicitacaoCompra = 1 WHERE Id = {idSolicitacaoCompra}");
        }

        public async Task ExcluirMaterialCotacao(Int64 idMaterial, Int64 idSolicitacaoCompra)
        {
            var solicitacaoCompra = _context.SolicitacaoCompra
                .Include(x => x.Materiais)
                .ThenInclude(x => x.Cotacoes)
                .ThenInclude(x => x.PagamentoManual)
                .FirstOrDefault(x => x.Id == idSolicitacaoCompra);
            
            var solicitacaoCompraMaterial = solicitacaoCompra.Materiais.FirstOrDefault(x => x.IdMaterial == idMaterial);

            if(solicitacaoCompra.Materiais.Count<=1)
                throw new Exception("A solicitação de compra deve conter ao menos um material");

            if (solicitacaoCompraMaterial.Cotacoes.Any(x=>x.PagamentoManual != null && x.PagamentoManual != new List<SolicitacaoCompraMaterialCotacaoPagamentoManual>()))
            {
                //Material que guarda as condicoes de compra manual. Colocar em outro material caso haja
            }

            solicitacaoCompraMaterial.Cotacoes.ForEach(cotacao =>
            {
                cotacao.PagamentoManual.ForEach(pagamentoManual =>
                {
                    _context.Entry(pagamentoManual).State = EntityState.Deleted;
                });

                _context.Entry(cotacao).State = EntityState.Deleted;
            });

            _context.Entry(solicitacaoCompra).State = EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
