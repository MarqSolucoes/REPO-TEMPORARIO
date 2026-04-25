using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.Cotacao;
using WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra;
using WHEngenharia.Servicos.Auxiliares;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoSolicitacaoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioSolicitacaoCompra _repositorioSolicitacaoCompra;

        public ServicoSolicitacaoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioSolicitacaoCompra = new RepositorioSolicitacaoCompra(_context, _mapper);
        }

        public async Task<SolicitacaoCompraDTO> Get(Int64 Id)
        {
            return await _repositorioSolicitacaoCompra.Get(Id);
        }

        public async Task<List<SolicitacaoCompraDTO>> GetByStatus(Int64 idStatus, Int64 idUsuario)
        {
            return await _repositorioSolicitacaoCompra.GetByStatus(idStatus, idUsuario);
        }

        public async Task<SolicitacaoCompra_ResponseDTO> GetByUser(Int64 idUsuario, SolicitacaoCompra_RequestParametrosDTO parametros)
        {
            return await _repositorioSolicitacaoCompra.GetByUser(idUsuario, parametros);
        }

        public async Task<Cotacao_SolicitacaoCompraDTO> GetParaCotacao(Int64 Id)
        {
            return await _repositorioSolicitacaoCompra.GetParaCotacao(Id);
        }

        public async Task<List<SolicitacaoCompraMateriaisCotacaoDTO>> GetCotacoes(Int64 idSolicitacaoCompraMaterial)
        {
            return await _repositorioSolicitacaoCompra.GetCotacoes(idSolicitacaoCompraMaterial);
        }

        public async Task<SolicitacaoCompraDTO> Post(SolicitacaoCompra_Post solicitacaoCompraDTO)
        {
            if (!solicitacaoCompraDTO.DataEntrega.HasValue)
                throw new Exception("Data de entrega inválida");

            if (!solicitacaoCompraDTO.Servico && (solicitacaoCompraDTO.Materiais == null || solicitacaoCompraDTO.Materiais.Count == 0))
                throw new Exception("Nenhum material ou serviço informado");

            if (solicitacaoCompraDTO.IdTipoCentroCusto == 1)
            {
                solicitacaoCompraDTO.IdCentroCustoDEF = null;
                if (!solicitacaoCompraDTO.IdCentroCustoObra.HasValue || solicitacaoCompraDTO.IdCentroCustoObra.Value == 0)
                    throw new Exception("Informe uma obra para o centro de custo");
            }
            else
            {
                solicitacaoCompraDTO.IdCentroCustoObra = null;
                if (!solicitacaoCompraDTO.IdCentroCustoDEF.HasValue || solicitacaoCompraDTO.IdCentroCustoDEF.Value == 0)
                    throw new Exception("Informe centro de custo específico válido");
            }


            return await _repositorioSolicitacaoCompra.Post(solicitacaoCompraDTO);
        }

        public async Task<SolicitacaoCompraDTO> Reabrir(SolicitacaoCompraDTO solicitacaoCompraDTO, List<SolicitacaoCompra_Reabertura_MaterialDTO> materiais)
        {
            solicitacaoCompraDTO.Arquivos = new List<SolicitacaoCompraArquivosDTO>();
            solicitacaoCompraDTO.CentroCustoDEF = null;
            solicitacaoCompraDTO.CentroCustoObra = null;
            solicitacaoCompraDTO.Codigo = null;
            solicitacaoCompraDTO.CodigoAno = DateTime.Now.Year;
            solicitacaoCompraDTO.CodigoSequencia = 0;
            solicitacaoCompraDTO.Comentarios = new List<SolicitacaoCompraComentariosDTO>();
            solicitacaoCompraDTO.DataAprovacaoDiretor = null;
            solicitacaoCompraDTO.DataAprovacaoEngenheiro = null;
            solicitacaoCompraDTO.DataCadastro = DateTime.Now;
            solicitacaoCompraDTO.DataFinalizacaoCotacao = null;
            solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;
            solicitacaoCompraDTO.Id = 0;
            solicitacaoCompraDTO.IdTipoCentroCusto = 1;
            solicitacaoCompraDTO.IdCentroCustoDEF = null;
            solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 1;
            solicitacaoCompraDTO.IdUsuarioFinalizacaoCotacao = null;
            solicitacaoCompraDTO.MotivoCancelamento = null;
            solicitacaoCompraDTO.Nome = $"Clone - {solicitacaoCompraDTO.Nome}";
            solicitacaoCompraDTO.StatusSolicitacaoCompra = null;
            solicitacaoCompraDTO.UsuarioCadastro = null;
            solicitacaoCompraDTO.UsuarioFinalizacaoCotacao = null;

            if (materiais == null || materiais.Count == 0)
                throw new Exception("Nenhum material ou serviço informado");

            if (solicitacaoCompraDTO.IdTipoCentroCusto == 1)
            {
                solicitacaoCompraDTO.IdCentroCustoDEF = null;
                if (!solicitacaoCompraDTO.IdCentroCustoObra.HasValue || solicitacaoCompraDTO.IdCentroCustoObra.Value == 0)
                    throw new Exception("Informe uma obra para o centro de custo");
            }
            else
            {
                solicitacaoCompraDTO.IdCentroCustoObra = null;
                if (!solicitacaoCompraDTO.IdCentroCustoDEF.HasValue || solicitacaoCompraDTO.IdCentroCustoDEF.Value == 0)
                    throw new Exception("Informe centro de custo específico válido");
            }


            return await _repositorioSolicitacaoCompra.Reabrir(solicitacaoCompraDTO, materiais);
        }

        public async Task<bool> PostCotacaoReprovar(Int64 idSolicitacaoCompra)
        {
            return await _repositorioSolicitacaoCompra.PostCotacaoReprovar(idSolicitacaoCompra);
        }

        public async Task<SolicitacaoCompraDTO> Put(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            if (!solicitacaoCompraDTO.DataEntrega.HasValue)
                throw new Exception("Data de entrega inválida");

            return await _repositorioSolicitacaoCompra.Put(solicitacaoCompraDTO);
        }

        public async Task<SolicitacaoPedidoCompraQuantidadesDTO> ObtemQuantidades(Int64 idUsuario)
        {
            return await _repositorioSolicitacaoCompra.ObtemQuantidades(idUsuario);
        }

        public async Task<SolicitacaoCompraMateriaisCotacaoDTO> Post(SolicitacaoCompraMateriaisCotacaoDTO solicitacaoCompraMateriaisCotacao, Int64 idUsuario)
        {
            if (solicitacaoCompraMateriaisCotacao.ValorUnitarioCotado <= 0)
                throw new Exception("Valor unitário informado inválido");

            if (!solicitacaoCompraMateriaisCotacao.DataEntrega.HasValue || solicitacaoCompraMateriaisCotacao.DataEntrega.Value.Date < DateTime.Now.Date)
                throw new Exception("Informe uma data de entrega válida");

            return await _repositorioSolicitacaoCompra.Post(solicitacaoCompraMateriaisCotacao, idUsuario);
        }

        public async Task<SolicitacaoCompraDTO> FinalizarCotacao(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            return await _repositorioSolicitacaoCompra.FinalizarCotacao(solicitacaoCompraDTO);
        }

        public async Task<SolicitacaoCompraDTO> AprovarCotacao(SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            return await _repositorioSolicitacaoCompra.AprovarCotacao(solicitacaoCompraDTO);
        }

        public async Task CancelarSolicitacaoCompra(Int64 id, string motivo)
        {
            await _repositorioSolicitacaoCompra.CancelarSolicitacaoCompra(id, motivo);
        }

        public async Task PostCotacaoPrincipal(Int64 Id, Int64 IdUsuario)
        {
            await _repositorioSolicitacaoCompra.PostCotacaoPrincipal(Id, IdUsuario);
        }

        public async Task<Int64> ObtemCotacaoParaAprovacao(Int64 IdUsuario)
        {
            return await _repositorioSolicitacaoCompra.ObtemCotacaoParaAprovacao(IdUsuario);
        }

        public async Task PostFornecedor(SolicitacaoCompra_PostFornecedor fornecedores, Int64 idUsuario)
        {
            await _repositorioSolicitacaoCompra.PostFornecedor(fornecedores, idUsuario);
        }

        public async Task DefinirFornecedorPrincipal(Int64 idSolicitacaoCompra, Int64 idFornecedor)
        {
            await _repositorioSolicitacaoCompra.DefinirFornecedorPrincipal(idSolicitacaoCompra, idFornecedor);
        }

        public async Task DefinirDataEntregaFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor, DateTime? dataEntrega)
        {
            if (!dataEntrega.HasValue || dataEntrega.Value.Date < DateTime.Now.Date)
                throw new Exception("Data de entrega inválida");

            await _repositorioSolicitacaoCompra.DefinirDataEntregaFornecedor(idSolicitacaoCompra, idFornecedor, dataEntrega.Value);
        }

        public async Task ExcluirFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor, Int64 idUsuario)
        {
            await _repositorioSolicitacaoCompra.ExcluirFornecedor(idSolicitacaoCompra, idFornecedor, idUsuario);
        }

        public async Task EditarCotacao(Cotacao_SolicitacaoCompraDTO cotacaoSolicitacaoCompraDTO, Int64 idUsuario)
        {
            await _repositorioSolicitacaoCompra.EditarCotacao(cotacaoSolicitacaoCompraDTO, idUsuario);
        }

        public async Task MontaPedidoCompra(Int64 IdSolicitacaoCompra, Int64 IdUsuario)
        {
            await _repositorioSolicitacaoCompra.MontaPedidoCompra(IdSolicitacaoCompra, IdUsuario);
        }

        public async Task Upload(SolicitacaoCompraArquivosDTO solicitacaoCompraArquivoDTO)
        {
            await _repositorioSolicitacaoCompra.Upload(solicitacaoCompraArquivoDTO);
        }

        public async Task<SolicitacaoCompraArquivosDTO> GetFile(Int64 id)
        {
            return await _repositorioSolicitacaoCompra.GetFile(id);
        }

        public async Task<List<SolicitacaoCompraArquivosDTO>> ObtemArquivos(Int64 idSolicitacaoCompra)
        {
            return await _repositorioSolicitacaoCompra.ObtemArquivos(idSolicitacaoCompra);
        }

        public async Task DeleteArquivo(Int64 idArquivo)
        {
            await _repositorioSolicitacaoCompra.DeleteArquivo(idArquivo);
        }

        public async Task PostComentario(SolicitacaoCompraComentariosDTO solicitacaoCompraComentarioDTO)
        {
            await _repositorioSolicitacaoCompra.PostComentario(solicitacaoCompraComentarioDTO);
        }

        public async Task<List<SolicitacaoCompraComentariosDTO>> Comentarios(Int64 idSolicitacaoCompra)
        {
            return await _repositorioSolicitacaoCompra.Comentarios(idSolicitacaoCompra);
        }

        private void AdicionaCabecalho(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, SolicitacaoCompraDTO solicitacaoCompraDTO, XFont fonteTitulo, XFont fonteNormal, string logoPath, bool exibeCamposCotacao = true)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = 40;
            double y = margemSuperior;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 9);

            graphics.DrawString("SOLICITAÇÃO DE COMPRA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;
            graphics.DrawString("WH ENGENHARIA LTDA", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Rua Dr Bacelar, 368 - CJ 103/104 - 10º andar  CEP: 04026-001  Vila Clementino, São Paulo / SP", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Tel: (11) 5904-0522   CNPJ: 62.534.060/0001-41     E-mail: comprasobras@whengenharia.com.br", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Solicitação aberta por: '{solicitacaoCompraDTO.UsuarioCadastro?.Nome ?? ""}' ", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 2);
            graphics.DrawString($"Solicitação: {solicitacaoCompraDTO.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Endereço de entrega: {solicitacaoCompraDTO.EnderecoEntrega}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.25), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2 + 6;

            if (exibeCamposCotacao)
            {
                graphics.DrawString("SOLICITAMOS A COTAÇÃO DOS SEGUINTES MATERIAIS", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
                posicaoYLinha += espacamento * 2;


                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha + 35);
                posicaoYLinha += 6;

                graphics.DrawString($"Item", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Discriminação", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.05), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Quantidade", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.8), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento + 6;

                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
                posicaoYLinha += 10;
            }
        }

        public async Task<byte[]> GeraPdfPedidoCompra(Int64 id, string logoPath)
        {
            var numeroItensPorPagina = 25;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var solicitacaoCompraDTO = await Get(id);

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.WinAnsi));
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));

            AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath);

            var count = 0;
            var countParcial = 0;
            var numeroDeLinhasInseridas = 0;

            solicitacaoCompraDTO.Materiais.ForEach(material =>
            {
                paginaGrafico.DrawString($"{(count + 1).ToString().PadLeft(3, '0')}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawString($"{material.Quantidade} {material.Material.UnidadeMaterial.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                var descricaoList = solicitacaoCompraDTO.Servico ? solicitacaoCompraDTO.Observacao.Split('\n').ToList() : material.Material.Descricao.Split('\n').ToList();
                var numeroDescricoesInseridas = 1;

                descricaoList.ForEach(descricao =>
                {
                    while (descricao.Length > 100)
                    {
                        //Obtem a posicao do ultimo espaço " " antes do 100
                        var posicao = ServicoString.RetornaPosicaoEspacoMaisProximo(descricao, 100);

                        paginaGrafico.DrawString($"{descricao.Substring(0, posicao)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                        descricao = descricao.Remove(0, posicao);

                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                        posicaoYLinha += espacamento;
                        count++;
                        countParcial++;
                    }

                    paginaGrafico.DrawString($"{descricao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                    if (descricaoList.Count > 1 && numeroDescricoesInseridas < descricaoList.Count)
                        posicaoYLinha += espacamento;

                    numeroDescricoesInseridas++;
                    numeroDeLinhasInseridas++;
                });

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                posicaoYLinha += espacamento;
                count++;
                countParcial++;

                if (count % numeroItensPorPagina == 0)
                {
                    posicaoYLinha += espacamento * 2;

                    if (solicitacaoCompraDTO.Materiais.Count % numeroItensPorPagina == 0 && count == solicitacaoCompraDTO.Materiais.Count)
                    {
                        if (countParcial <= numeroItensPorPagina)
                        {
                            while (countParcial <= numeroItensPorPagina)
                            {
                                posicaoYLinha += espacamento;
                                countParcial++;
                            }

                            pagina.Close();
                            paginaGrafico.Dispose();
                        }
                        else
                        {
                            while (countParcial < numeroItensPorPagina)
                            {
                                posicaoYLinha += espacamento;
                                countParcial++;
                            }

                            pagina.Close();
                            paginaGrafico.Dispose();

                            pagina = documentoPdf.AddPage();
                            pagina.Size = PdfSharpCore.PageSize.A4;
                            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                            paginaGrafico = XGraphics.FromPdfPage(pagina);

                            posicaoYLinha = margemSuperior;

                        }
                    }
                    else
                    {
                        countParcial = 0;

                        pagina.Close();
                        paginaGrafico.Dispose();

                        pagina = documentoPdf.AddPage();
                        pagina.Size = PdfSharpCore.PageSize.A4;
                        pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                        paginaGrafico = XGraphics.FromPdfPage(pagina);

                        posicaoYLinha = margemSuperior;
                        AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath);
                    }
                }
                else if (solicitacaoCompraDTO.Materiais.Count % numeroItensPorPagina > 0 && count == solicitacaoCompraDTO.Materiais.Count)
                {
                    if (countParcial <= numeroItensPorPagina)
                    {
                        while (countParcial <= numeroItensPorPagina)
                        {
                            posicaoYLinha += espacamento;
                            countParcial++;
                        }


                        pagina.Close();
                        paginaGrafico.Dispose();
                    }
                    else
                    {
                        while (countParcial < numeroItensPorPagina)
                        {
                            posicaoYLinha += espacamento;
                            countParcial++;
                        }

                        pagina.Close();
                        paginaGrafico.Dispose();

                        pagina = documentoPdf.AddPage();
                        pagina.Size = PdfSharpCore.PageSize.A4;
                        pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                        paginaGrafico = XGraphics.FromPdfPage(pagina);

                        posicaoYLinha = margemSuperior;

                    }


                }

            });


            pagina.Close();
            paginaGrafico.Dispose();

            if (!solicitacaoCompraDTO.Servico)
            {
                pagina = documentoPdf.AddPage();
                pagina.Size = PdfSharpCore.PageSize.A4;
                pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                paginaGrafico = XGraphics.FromPdfPage(pagina);

                posicaoYLinha = margemSuperior;

                AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath, false);

                paginaGrafico.DrawString($"OBSERVAÇÃO DO PEDIDO:", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento * 2;

                var observacao = solicitacaoCompraDTO?.Observacao ?? "";

                while (observacao.Length > 100)
                {
                    paginaGrafico.DrawString($"{observacao.Substring(0, 100)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    observacao = observacao.Remove(0, 100);
                    posicaoYLinha += espacamento;
                }

                paginaGrafico.DrawString($"{observacao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                pagina.Close();
                paginaGrafico.Dispose();
            }

            var numeroPaginas = documentoPdf.Pages.Count;
            for (int i = 0; i < documentoPdf.Pages.Count; ++i)
            {
                PdfPage paginaAuxiliar = documentoPdf.Pages[i];

                using (XGraphics paginaGraficoAuxiliar = XGraphics.FromPdfPage(paginaAuxiliar))
                    paginaGraficoAuxiliar.DrawString($"Página {i + 1} de {numeroPaginas}", fonteNormal, XBrushes.Black, new XRect(0, paginaAuxiliar.Height.Value - 30, paginaAuxiliar.Width, paginaAuxiliar.Height), XStringFormats.TopCenter);
            }

            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                documentoPdf.Save(stream, true);
                fileContents = stream.ToArray();
            }

            return fileContents;
        }

        private void AdicionaCabecalhoCotacoes(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, SolicitacaoCompraDTO solicitacaoCompraDTO, XFont fonteTitulo, XFont fonteNormal, string logoPath, bool exibeCamposCotacao = true)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = 40;
            double y = margemSuperior;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 9);

            graphics.DrawString("COTAÇÕES DA SOLICITAÇÃO DE COMPRA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;
            graphics.DrawString("WH ENGENHARIA LTDA", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Rua Dr Bacelar, 368 - CJ 103/104 - 10º andar  CEP: 04026-001  Vila Clementino, São Paulo / SP", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Tel: (11) 5904-0522   CNPJ: 62.534.060/0001-41     E-mail: comprasobras@whengenharia.com.br", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Solicitação aberta por: '{solicitacaoCompraDTO.UsuarioCadastro?.Nome ?? ""}' ", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 2);
            graphics.DrawString($"Solicitação: {solicitacaoCompraDTO.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Endereço de entrega: {solicitacaoCompraDTO.EnderecoEntrega}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.25), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2 + 6;

            if (exibeCamposCotacao)
            {
                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha + 35);
                posicaoYLinha += 6;

                graphics.DrawString($"Item", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Discriminação", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.05), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Fornecedor", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Valor", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.85), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento + 6;

                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            }

        }

        public async Task<byte[]> GeraPdfCotacoes(Int64 id, string logoPath)
        {
            var numeroItensPorPagina = 35;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;
            var numeroCaracteresMaximosDescricao = 60;

            var solicitacaoCompraDTO = await Get(id);

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.WinAnsi));
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));

            AdicionaCabecalhoCotacoes(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath);

            var count = 0;
            var numeroItem = 0;
            solicitacaoCompraDTO.Materiais.ForEach(material =>
            {
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width - margemEsquerda, posicaoYLinha);
                paginaGrafico.DrawString($"{(numeroItem + 1).ToString().PadLeft(3, '0')}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                var descricao = solicitacaoCompraDTO.Servico ? solicitacaoCompraDTO.Observacao : material.Material.Descricao;

                while (descricao.Length > numeroCaracteresMaximosDescricao)
                {
                    paginaGrafico.DrawString($"{descricao.Substring(0, numeroCaracteresMaximosDescricao)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    descricao = descricao.Remove(0, numeroCaracteresMaximosDescricao);

                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha + 12);

                    posicaoYLinha += espacamento;
                    count++;
                }

                paginaGrafico.DrawString($"{descricao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                material.Cotacoes.ForEach(cotacao =>
                {
                    paginaGrafico.DrawString($"{cotacao.Fornecedor?.NomeFantasia ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{cotacao.ValorUnitarioCotado.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.85), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha + 12);

                    posicaoYLinha += espacamento;
                    count++;
                });

                posicaoYLinha -= espacamento;

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha + 12);

                posicaoYLinha += espacamento;
                count++;

                if (count >= numeroItensPorPagina)
                {
                    //Fecha a página atual
                    //Abre outra página

                    pagina.Close();
                    paginaGrafico.Dispose();

                    pagina = documentoPdf.AddPage();
                    pagina.Size = PdfSharpCore.PageSize.A4;
                    pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                    paginaGrafico = XGraphics.FromPdfPage(pagina);

                    posicaoYLinha = margemSuperior;
                    AdicionaCabecalhoCotacoes(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath);

                    count = 0;
                }

                numeroItem++;
            });


            pagina.Close();
            paginaGrafico.Dispose();

            if (!solicitacaoCompraDTO.Servico)
            {
                pagina = documentoPdf.AddPage();
                pagina.Size = PdfSharpCore.PageSize.A4;
                pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                paginaGrafico = XGraphics.FromPdfPage(pagina);

                posicaoYLinha = margemSuperior;

                AdicionaCabecalhoCotacoes(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, solicitacaoCompraDTO, fonteTitulo, fonteNormal, logoPath, false);

                paginaGrafico.DrawString($"OBSERVAÇÃO DO PEDIDO:", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento * 2;

                var observacao = solicitacaoCompraDTO?.Observacao ?? "";

                while (observacao.Length > 100)
                {
                    paginaGrafico.DrawString($"{observacao.Substring(0, 100)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    observacao = observacao.Remove(0, 100);
                    posicaoYLinha += espacamento;
                }

                paginaGrafico.DrawString($"{observacao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                pagina.Close();
                paginaGrafico.Dispose();
            }

            var numeroPaginas = documentoPdf.Pages.Count;
            for (int i = 0; i < documentoPdf.Pages.Count; ++i)
            {
                PdfPage paginaAuxiliar = documentoPdf.Pages[i];

                using (XGraphics paginaGraficoAuxiliar = XGraphics.FromPdfPage(paginaAuxiliar))
                    paginaGraficoAuxiliar.DrawString($"Página {i + 1} de {numeroPaginas}", fonteNormal, XBrushes.Black, new XRect(0, paginaAuxiliar.Height.Value - 30, paginaAuxiliar.Width, paginaAuxiliar.Height), XStringFormats.TopCenter);
            }

            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                documentoPdf.Save(stream, true);
                fileContents = stream.ToArray();
            }

            return fileContents;
        }

        public async Task<SolicitacaoCompraDTO> RemoverItensSelecionados(Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {

            if (!cotacao_SolicitacaoCompraDTO.Materiais.Any(x => x.ItemSelecionado))
                throw new Exception("Nenhum item selecionado");

            if (cotacao_SolicitacaoCompraDTO.Materiais.Count(x => !x.ItemSelecionado)==0)
                throw new Exception("Não é permitido remover todos os itens da solicitação");

            return await _repositorioSolicitacaoCompra.RemoverItensSelecionados(cotacao_SolicitacaoCompraDTO);
        }

        public async Task<SolicitacaoCompraDTO> DuplicarItensSelecionados(Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {

            if (!cotacao_SolicitacaoCompraDTO.Materiais.Any(x => x.ItemSelecionado))
                throw new Exception("Nenhum item selecionado");

            return await _repositorioSolicitacaoCompra.DuplicarItensSelecionados(cotacao_SolicitacaoCompraDTO);
        }

        public async Task DevolverCompras(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            await _repositorioSolicitacaoCompra.DevolverCompras(objetoAprovacaoDTO);
        }

        public async Task EnviarDiretoria(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            await _repositorioSolicitacaoCompra.EnviarDiretoria(objetoAprovacaoDTO);
        }

        public async Task ReprovarSolicitacaoCompra(Int64 idSolicitacaoCompra, Int64 idUsuario)
        {
            await _repositorioSolicitacaoCompra.ReprovarSolicitacaoCompra(idSolicitacaoCompra, idUsuario);
        }

        public async Task<bool> VerificaDivergenciaValorCondicaoManual(Int64 idSolicitacaoCompra)
        {
            return await _repositorioSolicitacaoCompra.VerificaDivergenciaValorCondicaoManual(idSolicitacaoCompra);
        }

        public async Task<byte[]> ExcelCotacoes(Int64 idSolicitacaoCompra, string logoPath)
        {
            var corDarkGray = XLColor.DarkGray;
            var corDarkSlateGray = XLColor.LightGray;

            var solicitacaoDTO = await Get(idSolicitacaoCompra);
            solicitacaoDTO.Materiais = solicitacaoDTO.Materiais.OrderBy(x => x.Material.Descricao).ToList();

            int maiorLinha = 20;
            int maiorColuna = 18;

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Cotacoes");

            var image = worksheet.AddPicture(logoPath)
                .MoveTo(1, 1);

            worksheet.Row(2).Height = 30;

            worksheet.Range(1, 1, 1, 3).Merge();
            worksheet.Range(1, 4, 1, 13).Merge();
            worksheet.Range(1, 14, 1, 16).Merge();
            worksheet.Range(1, 17, 1, 18).Merge();

            worksheet.Range(3, 1, 3, 3).Merge();
            worksheet.Range(3, 4, 3, 10).Merge();
            worksheet.Range(3, 11, 3, 14).Merge();

            worksheet.Range(4, 1, 4, 10).Merge();
            worksheet.Range(4, 11, 4, 14).Merge();

            worksheet.Range(5, 1, 7, 1).Merge();
            worksheet.Range(5, 2, 5, 10).Merge();
            worksheet.Range(5, 11, 5, 14).Merge();

            worksheet.Range(6, 2, 6, 10).Merge();
            worksheet.Range(6, 11, 7, 11).Merge();
            worksheet.Range(6, 12, 7, 12).Merge();
            worksheet.Range(6, 13, 6, 14).Merge();

            worksheet.Range(7, 2, 7, 10).Merge();

            worksheet.Cell(1, 4).Value = $"SOLICITAÇÃO DE COMPRAS";
            worksheet.Cell(1, 14).Value = $"OC Nº {solicitacaoDTO?.Codigo}";
            worksheet.Cell(1, 17).Value = $"DATA {solicitacaoDTO?.DataCadastro.ToString("dd/MM/yyyy")}";

            worksheet.Cell(1, 4).Value = $"MATERIAIS E SERVIÇOS";

            worksheet.Cell(3, 1).Value = $"Contrato Nº {solicitacaoDTO?.CentroCustoObra?.CodigoProposta}";
            worksheet.Cell(3, 4).Value = $"Cliente: {solicitacaoDTO?.CentroCustoObra?.Cliente?.NomeFantasia}";
            worksheet.Cell(3, 11).Value = $"FORNECEDOR";

            worksheet.Cell(4, 11).Value = $"TELEFONE";

            worksheet.Cell(5, 1).Value = $"ITEM";
            worksheet.Cell(5, 2).Value = $"ESPECIFICAÇÃO";
            worksheet.Cell(5, 11).Value = $"CONTATO";

            worksheet.Cell(6, 2).Value = $"MATERIAIS: MODELO, MARCA, CÓDIGO, COR E UNIDADE";
            worksheet.Cell(6, 11).Value = $"UNIDADE";
            worksheet.Cell(6, 12).Value = $"QTD";
            worksheet.Cell(6, 13).Value = $"CUSTO ORÇADO";

            worksheet.Cell(7, 2).Value = $"SERVIÇOS: DESCRIÇÃO COMPLETA A SER EXECUTADO";
            worksheet.Cell(7, 13).Value = $"UNITÁRIO";
            worksheet.Cell(7, 14).Value = $"TOTAL";


            var fornecedoresDTO = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.Select(y => y.Fornecedor).OrderBy(x => x.Nome).ToList();

            if (fornecedoresDTO.Count > 2)
                maiorColuna = 18 + ((fornecedoresDTO.Count - 2) * 2);

            maiorColuna = maiorColuna + 2;

            worksheet.Range(2, 1, 2, maiorColuna).Merge();

            int coluna = 15;
            int numeroFornecedor = 1;

            fornecedoresDTO.ForEach(x =>
            {
                worksheet.Cell(3, coluna).Value = $"{x.NomeFantasia}";
                worksheet.Cell(4, coluna).Value = $"{x.TelefoneFixo}";
                worksheet.Cell(5, coluna).Value = $"{x.NomeVendedor}";

                worksheet.Cell(6, coluna).Value = $"PREÇOS";

                worksheet.Cell(7, coluna).Value = $"UNITÁRIO";
                worksheet.Cell(7, coluna + 1).Value = $"TOTAL";

                worksheet.Range(3, coluna, 3, coluna + 1).Merge();
                worksheet.Range(4, coluna, 4, coluna + 1).Merge();
                worksheet.Range(5, coluna, 5, coluna + 1).Merge();
                worksheet.Range(6, coluna, 6, coluna + 1).Merge();

                if (numeroFornecedor % 2 != 0)
                {
                    worksheet.Cell(3, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(4, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(5, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(6, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(7, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(7, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                }
                else
                {
                    worksheet.Cell(3, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(4, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(5, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(6, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(7, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(7, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);
                }

                coluna += 2;
                numeroFornecedor++;
            });

            worksheet.Cell(6, coluna).Value = $"MELHOR PREÇO";
            worksheet.Cell(7, coluna).Value = $"UNITÁRIO";
            worksheet.Cell(7, coluna + 1).Value = $"TOTAL";

            worksheet.Range(3, coluna, 3, coluna + 1).Merge();
            worksheet.Range(4, coluna, 4, coluna + 1).Merge();
            worksheet.Range(5, coluna, 5, coluna + 1).Merge();
            worksheet.Range(6, coluna, 6, coluna + 1).Merge();


            int linha = 8;

            worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            linha++;

            var count = 1;
            solicitacaoDTO?.Materiais?.ForEach(material =>
            {
                worksheet.Range(linha, 2, linha, 10).Merge();

                worksheet.Cell(linha, 1).Value = $"{count}";
                worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(linha, 1).Style.Font.SetBold(true);

                if (solicitacaoDTO.Servico)
                    worksheet.Cell(linha, 2).Value = $"{material.Material.Descricao} - {solicitacaoDTO.Observacao}";
                else
                    worksheet.Cell(linha, 2).Value = $"{material.Material.Descricao}";

                worksheet.Cell(linha, 11).Value = $"{material.Material.UnidadeMaterial.Codigo}";
                worksheet.Cell(linha, 12).Value = $"{material.Quantidade}";

                worksheet.Cell(linha, 13).Value = $"{(material.ValorUnitarioEstimado ?? 0).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
                worksheet.Cell(linha, 14).Value = $"{((material.ValorUnitarioEstimado ?? 0) * (material.Quantidade)).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

                worksheet.Cell(linha, 13).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(linha, 14).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);

                coluna = 15;
                numeroFornecedor = 1;

                material.Cotacoes.OrderBy(x => x.Fornecedor.Nome).ToList().ForEach(cotacao =>
                {
                    worksheet.Cell(linha, coluna).Value = $"{cotacao.ValorUnitarioComDesconto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
                    worksheet.Cell(linha, coluna + 1).Value = $"{(cotacao.ValorUnitarioComDesconto * material.Quantidade).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

                    if (numeroFornecedor % 2 != 0)
                    {
                        worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                        worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    }
                    else
                    {
                        worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                        worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);
                    }

                    if (!material.Cotacoes.Any(y => y.ValorUnitarioComDesconto < cotacao.ValorUnitarioComDesconto))
                        worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGreen);

                    numeroFornecedor++;
                    coluna += 2;
                });

                var cotacoesMaiorQueZero = material.Cotacoes.Where(x => x.ValorUnitarioComDesconto > 0).ToList();
                var menorValor = 0.0;

                if (cotacoesMaiorQueZero.Count > 0)
                    menorValor = cotacoesMaiorQueZero.Min(x => x.ValorUnitarioComDesconto);

                worksheet.Cell(linha, coluna).Value = $"{menorValor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
                worksheet.Cell(linha, coluna + 1).Value = $"{(menorValor * material.Quantidade).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

                linha++;
                count++;
            });

            worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            linha++;

            worksheet.Range(linha, 1, linha, 10).Merge();
            worksheet.Range(linha, 11, linha, 12).Merge();
            worksheet.Range(linha, 13, linha, 14).Merge();
            worksheet.Cell(linha, 1).Value = $"DATA DE ENTREGA NECESSÁRIA: {solicitacaoDTO.DataEntrega?.ToString("dd/MM/yyyy")}";

            coluna = 14;
            var total = 0.0;
            for (int i = 9; i < (9 + solicitacaoDTO.Materiais.Count()); i++)
            {
                total = total + Double.Parse(worksheet.Cell(i, coluna).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));
            }

            //não possui valor dos materiais, então verificar se possui um valor estimado
            if (total == 0)
                total = solicitacaoDTO.ValorEstimado ?? 0;

            worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
            worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
            worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, coluna - 1).Value = $"{total.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            linha++;

            #region Frete e Imposto

            worksheet.Range(linha, 1, linha, 14).Merge();
            worksheet.Range(linha + 1, 1, linha + 1, 14).Merge();

            coluna = 15;
            numeroFornecedor = 1;
            fornecedoresDTO.ForEach(fornecedor =>
            {
                var cotacao = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);

                worksheet.Cell(linha, coluna).Value = "Frete";
                worksheet.Cell(linha + 1, coluna).Value = "Imposto";
                worksheet.Cell(linha, coluna + 1).Value = cotacao.Frete?.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "");
                worksheet.Cell(linha + 1, coluna + 1).Value = cotacao.Imposto?.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "");

                if (numeroFornecedor % 2 != 0)
                {
                    worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(linha + 1, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    worksheet.Cell(linha + 1, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                }
                else
                {
                    worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(linha + 1, coluna).Style.Fill.SetBackgroundColor(corDarkGray);
                    worksheet.Cell(linha + 1, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);
                }

                numeroFornecedor++;
                coluna += 2;
            });

            #endregion

            linha += 2;

            #region Valor Total por Fornecedor

            worksheet.Range(linha, 1, linha, 14).Merge();

            coluna = 16;
            numeroFornecedor = 1;
            fornecedoresDTO.ForEach(fornecedor =>
            {
                worksheet.Range(linha, coluna - 1, linha, coluna).Merge();

                var total = 0.0;
                for (int i = 9; i < (9 + solicitacaoDTO.Materiais.Count()); i++)
                {
                    total = total + Double.Parse(worksheet.Cell(i, coluna).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));
                }

                var cotacao = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);
                total += cotacao.Frete ?? 0;
                total += cotacao.Imposto ?? 0;

                worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
                worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(linha, coluna - 1).Value = $"{total.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

                if (numeroFornecedor % 2 != 0)
                    worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                else
                    worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(corDarkGray);

                numeroFornecedor++;
                coluna += 2;
            });

            #endregion

            #region Soma do melhor preço

            // Só calcula e mescla a célula de "Melhor Preço" se existir pelo menos
            // um fornecedor. Sem fornecedor, worksheet.Cell(linha, 15) está vazia
            // e o Double.Parse("") lançaria FormatException.
            if (fornecedoresDTO.Count > 0)
            {
                var menorValor = Double.Parse(worksheet.Cell(linha, 15).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));

                for (int i = 0; i < fornecedoresDTO.Count; i++)
                {
                    var valorParcial = Double.Parse(worksheet.Cell(linha, 15 + (i * 2)).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));

                    if (valorParcial < menorValor && valorParcial > 0)
                        menorValor = valorParcial;
                }

                worksheet.Range(linha, coluna - 1, linha, coluna).Merge();

                worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
                worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(linha, coluna - 1).Value = $"{menorValor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
            }

            #endregion

            linha++;

            #region Condição de pagamento

            var linhaAuxiliar = linha;
            coluna = 15;

            numeroFornecedor = 1;
            fornecedoresDTO.ForEach(fornecedor =>
            {
                var cotacao = solicitacaoDTO
                    .Materiais
                        .OrderBy(x => x.Id)
                        .FirstOrDefault()
                    .Cotacoes
                        .OrderBy(x => x.Id)
                        .FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);

                var condicaoPagamento = cotacao.CondicaoPagamento;

                worksheet.Cell(linha, coluna).Value = condicaoPagamento?.Descricao ?? "";

                if (condicaoPagamento.Descricao == "Manual")
                {
                    cotacao.PagamentoManual.ForEach(pm =>
                    {
                        worksheet.Range(linhaAuxiliar, 1, linhaAuxiliar, 14).Merge();
                        worksheet.Cell(linhaAuxiliar, coluna + 1).Value = $"{pm.Data.ToString("dd/MM/yyyy")} - {pm.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

                        if (numeroFornecedor % 2 != 0)
                            worksheet.Cell(linhaAuxiliar, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                        else
                            worksheet.Cell(linhaAuxiliar, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);

                        linhaAuxiliar++;
                    });
                }
                else
                {
                    if (numeroFornecedor % 2 != 0)
                        worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                    else
                        worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(corDarkGray);
                }

                if (numeroFornecedor % 2 != 0)
                    worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkSlateGray);
                else
                    worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(corDarkGray);

                numeroFornecedor++;

                coluna += 2;
            });

            linha = linhaAuxiliar;

            #endregion

            worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            linha++;

            worksheet.Range(linha, 1, linha, 12).Merge();
            worksheet.Range(linha, 13, linha, maiorColuna).Merge();
            worksheet.Cell(linha, 1).Value = $"APROVAÇÕES";

            worksheet.Cell(linha, 1).Style.Font.SetBold(true);
            worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            linha++;

            worksheet.Range(linha, 1, linha, 2).Merge();
            worksheet.Range(linha, 3, linha, 4).Merge();
            worksheet.Range(linha, 5, linha, 6).Merge();
            worksheet.Range(linha, 7, linha, 8).Merge();
            worksheet.Range(linha, 9, linha, 10).Merge();
            worksheet.Range(linha, 11, linha, 12).Merge();

            worksheet.Cell(linha, 1).Value = $"{solicitacaoDTO.DataCadastro.ToString("dd/MM/yyyy")}";
            worksheet.Cell(linha, 3).Value = $"{solicitacaoDTO.UsuarioCadastro.Nome}";
            worksheet.Cell(linha, 5).Value = $"{solicitacaoDTO.DataAprovacaoEngenheiro?.ToString("dd/MM/yyyy")}";
            worksheet.Cell(linha, 7).Value = $"{solicitacaoDTO.UsuarioEngenheiroAprovador?.Nome}";
            worksheet.Cell(linha, 9).Value = $"{solicitacaoDTO.DataAprovacaoDiretor?.ToString("dd/MM/yyyy")}";
            worksheet.Cell(linha, 11).Value = $"{solicitacaoDTO.UsuarioDiretorAprovador?.Nome}";

            worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 3).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 5).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 7).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 9).Style.Fill.SetBackgroundColor(XLColor.Beige);
            worksheet.Cell(linha, 11).Style.Fill.SetBackgroundColor(XLColor.Beige);

            worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Range(linha, 13, linha, maiorColuna).Merge();

            linha++;

            worksheet.Range(linha, 1, linha, 4).Merge();
            worksheet.Range(linha, 5, linha, 8).Merge();
            worksheet.Range(linha, 9, linha, 12).Merge();
            worksheet.Range(linha, 13, linha, maiorColuna).Merge();

            worksheet.Cell(linha, 1).Value = $"SOLICITANTE";
            worksheet.Cell(linha, 5).Value = $"ENGENHEIRO";
            worksheet.Cell(linha, 9).Value = $"DIRETOR";

            worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            worksheet.Cell(linha, 5).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            worksheet.Cell(linha, 9).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell(linha, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            maiorLinha = linha;

            for (int i = 1; i <= maiorLinha; i++)
            {
                for (int j = 1; j <= maiorColuna; j++)
                {
                    worksheet.Cell(i, j).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(i, j).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(i, j).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(i, j).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                }
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            var content = stream.ToArray();

            return content;
        }
        public async Task EnviarParaRevisao(Int64 idSolicitacaoCompra)
        {
            await _repositorioSolicitacaoCompra.EnviarParaRevisao(idSolicitacaoCompra);
        }

        public async Task EnviarParaAprovacao(Int64 idSolicitacaoCompra)
        {
            await _repositorioSolicitacaoCompra.EnviarParaAprovacao(idSolicitacaoCompra);
        }
        public async Task ExcluirMaterialCotacao(Int64 idMaterial, Int64 idSolicitacaoCompra)
        {
            await _repositorioSolicitacaoCompra.ExcluirMaterialCotacao(idMaterial, idSolicitacaoCompra);
        }

    }
}
