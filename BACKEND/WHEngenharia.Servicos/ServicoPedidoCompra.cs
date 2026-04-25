using AutoMapper;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Conciliacao;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoCompra;
using WHEngenharia.Servicos.Auxiliares;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoPedidoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioPedidoCompra _repositorioPedidoCompra;

        public ServicoPedidoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioPedidoCompra = new RepositorioPedidoCompra(_context, _mapper);
        }

        public async Task<PedidoCompraDTO> Get(Int64 id)
        {
            return await _repositorioPedidoCompra.Get(id);
        }

        public async Task<List<PedidoCompraDTO>> GetByStatus(Int64 idStatus)
        {
            return await _repositorioPedidoCompra.GetByStatus(idStatus);
        }

        public async Task<List<PedidoCompraDTO>> ObtemFinalizadosFiltrados(PedidoCompra_FiltroPedidosFinalizadosDTO parametrosDTO)
        {
            return await _repositorioPedidoCompra.ObtemFinalizadosFiltrados(parametrosDTO);
        }

        public async Task<List<PedidoCompraDTO>> GetParaConciliacaoPorUsuario(Int64 idUsuario)
        {
            return await _repositorioPedidoCompra.GetParaConciliacaoPorUsuario(idUsuario);
        }

        public async Task<List<Conciliacao_ReprovadaDirecaoDTO>> ConciliacaoReprovadasDirecao()
        {
            return await _repositorioPedidoCompra.ConciliacaoReprovadasDirecao();
        }

        public async Task MarcarVisualizacaoNotaFiscalReprovada(Int64 idPedidoCompraNotaFiscal)
        {
            await _repositorioPedidoCompra.MarcarVisualizacaoNotaFiscalReprovada(idPedidoCompraNotaFiscal);
        }

        public async Task ReenviarNotaFiscalParaDiretoria(Int64 idPedidoCompraNotaFiscal)
        {
            await _repositorioPedidoCompra.ReenviarNotaFiscalParaDiretoria(idPedidoCompraNotaFiscal);
        }

        public async Task<List<Conciliacao_ReprovadaDirecaoDTO>> ConciliacaoReprovadasFinanceiro()
        {
            return await _repositorioPedidoCompra.ConciliacaoReprovadasFinanceiro();
        }

        public async Task ReenviarNotaFiscalParaFinanceiro(Int64 idPedidoCompraNotaFiscal)
        {
            await _repositorioPedidoCompra.ReenviarNotaFiscalParaFinanceiro(idPedidoCompraNotaFiscal);
        }

        public async Task<Conciliacao_PedidoCompraDTO> GetParaConciliacao(Int64 id)
        {
            return await _repositorioPedidoCompra.GetParaConciliacao(id);
        }

        public async Task DevolverParaCotacao(Int64 id, Int64 idUsuario)
        {
            await _repositorioPedidoCompra.DevolverParaCotacao(id, idUsuario);
        }

        public async Task<List<PedidoCompraNotaFiscalDTO>> ObtemNotasConciliadas(Conciliacao_NotasConciliadasParametrosDTO parametrosDTO)
        {
            return await _repositorioPedidoCompra.ObtemNotasConciliadas(parametrosDTO);
        }

        public async Task<PedidoCompraDTO> Put(PedidoCompraDTO pedidoCompraDTO)
        {
            return await _repositorioPedidoCompra.Put(pedidoCompraDTO);
        }

        public async Task<PedidoCompraNotaFiscalDTO> Post(PedidoCompraNotaFiscalDTO pedidoCompraNotaFiscalDTO)
        {
            return await _repositorioPedidoCompra.Post(pedidoCompraNotaFiscalDTO);
        }

        public async Task ExcluiNotaFiscal(Int64 idPedidoCompraNotaFiscal)
        {
            await _repositorioPedidoCompra.ExcluiNotaFiscal(idPedidoCompraNotaFiscal);
        }

        public async Task ConciliarNotaFiscal(Conciliacao_PedidoCompraDTO conciliacao_PedidoCompraDTO, Int64 idPedidoCompraNotaFiscal, Int64 idUsuario)
        {
            //if (string.IsNullOrEmpty(conciliacao_PedidoCompraDTO.Descricao) || string.IsNullOrWhiteSpace(conciliacao_PedidoCompraDTO.Descricao))
            //    throw new Exception("Informe uma descrição válida");

            if (conciliacao_PedidoCompraDTO.Materiais.Any(x => x.QuantidadeParaConciliar>0 && x.ValorConciliado==0))
                throw new Exception("Não é possível conciliar um material com valor de conciliação zerado.");

            if (!conciliacao_PedidoCompraDTO.Materiais.Any(x => x.ValorConciliado > 0))
                throw new Exception("Não é possível conciliar valor de material igual a zero");

            await _repositorioPedidoCompra.ConciliarNotaFiscal(conciliacao_PedidoCompraDTO, idPedidoCompraNotaFiscal, idUsuario);
        }

        public async Task<PedidoCompraDTO> CancelarSaldoAbrirPedido(Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            return await _repositorioPedidoCompra.CancelarSaldoAbrirPedido(conciliacao_CancelarSaldoAbrirPedidoDTO);
        }

        public async Task<FornecedorDTO> TrocarFornecedor(Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            return await _repositorioPedidoCompra.TrocarFornecedor(conciliacao_CancelarSaldoAbrirPedidoDTO);
        }

        public async Task<PedidoCompraArquivosDTO> Upload(PedidoCompraArquivosDTO pedidoCompraArquivoDTO)
        {
            return await _repositorioPedidoCompra.Upload(pedidoCompraArquivoDTO);
        }

        public async Task<PedidoCompraArquivosDTO> GetFile(Int64 id)
        {
            return await _repositorioPedidoCompra.GetFile(id);
        }

        public async Task<PedidoCompraArquivosDTO> ObtemArquivo(Int64 idPedidoCompraArquivo)
        {
            return await _repositorioPedidoCompra.ObtemArquivo(idPedidoCompraArquivo);
        }

        public async Task<List<PedidoCompraArquivosDTO>> ObtemArquivos(Int64 idPedidoCompra)
        {
            return await _repositorioPedidoCompra.ObtemArquivos(idPedidoCompra);
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            await _repositorioPedidoCompra.DeleteArquivo(idArquivo);
        }

        private void AdicionaRodape(ref XGraphics paginaGrafico, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage pagina, ref PdfDocument doc, PedidoCompraDTO pedidoCompraDTO, XFont fonteTitulo, XFont fonteNormal, XFont fonteCaixaAlta, bool exibirDataPagamento)
        {
            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 3);
            paginaGrafico.DrawString($"Valor total do frete", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //paginaGrafico.DrawString($"Valor total do desconto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total do imposto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total pedido", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            paginaGrafico.DrawString($"{pedidoCompraDTO.Frete.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //paginaGrafico.DrawString($"{(pedidoCompraDTO.ValorDesconto).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.Imposto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 3);
            paginaGrafico.DrawString($"SR FORNECEDOR: MERCADORIA DESTINADA A USO OU CONSUMO PRÓPRIO PARA UTILIZAÇÃO NA PRESTAÇÃO DE SERVIÇOS, POR", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"EMPRESA NÃO CONTRIBUINTE. PORTANTO O ICMS DEVERÁ SER COBRADO MEDIANTE APLICAÇÃO DA ALÍQUOTA INTERNA DO", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"ESTADO DE LOCALIZAÇÃO DO ESTABELECIMENTO FORNECEDOR", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento + 4;

            paginaGrafico.DrawString($"Endereço de entrega: {pedidoCompraDTO.EnderecoEntrega}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 8 + 6);
            paginaGrafico.DrawString($"Condições de fornecimento", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"1 - Reservamo-nos o direito de cancelar esta ordem e devolver os materiais sem nenhum ônus à WH, caso estes venham em desacordo com as", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"      especificações de preços, prazos ou demais condições.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"2 - Quaisquer alterações eventuais nesta ordem, só serão válidas com a concodância por escrito da WH.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"3 - Toda a mercadoria sujeita a quebras, terá seu recebimento como provisório, sujeito a confirmação posterior.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"4 - Em qualquer cobrança referente a prestação de serviços, a WH reserva-se o direito de só liberar a emissão de respectivas faturas após aprovação", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"      dos mesmos pela empresa.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"5 - A WH reserva-se o direito de não aceitar que seus fornecedores negociem com terceiros os títulos da empresa.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"6 - Os pagamentos em carteira serão realizados às terças e quintas após as 14hs.", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 20;

            
            paginaGrafico.DrawString($"{pedidoCompraDTO.SolicitacaoCompra?.UsuarioFinalizacaoCotacao?.Nome ?? ""}", new XFont("Segoe Script", 14, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.SolicitacaoCompra?.UsuarioDiretorAprovador?.Nome ?? ""}", new XFont("Segoe Script", 14, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            
            posicaoYLinha += espacamento + 10;
            
            paginaGrafico.DrawString($"Comprador {pedidoCompraDTO.SolicitacaoCompra?.DataFinalizacaoCotacao?.ToString("dd/MM/yyyy HH:mm:ss") ?? ""}", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Aprovação diretoria {pedidoCompraDTO.SolicitacaoCompra?.DataAprovacaoDiretor?.ToString("dd/MM/yyyy HH:mm:ss") ?? ""}", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            
            posicaoYLinha += espacamento;

            //paginaGrafico.DrawString($"{pedidoCompraDTO.SolicitacaoCompra?.UsuarioEngenheiroAprovador?.Nome ?? ""}", new XFont("Segoe Script", 14, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.SolicitacaoCompra?.UsuarioCadastro?.Nome ?? ""}", new XFont("Segoe Script", 14, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

            posicaoYLinha += espacamento + 10;

            //paginaGrafico.DrawString($"Engenheiro(a) responsável", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Solicitante", fonteCaixaAlta, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width * 0.4, posicaoYLinha);
            //paginaGrafico.DrawLine(XPens.Black, margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width * 0.9, posicaoYLinha);
            //posicaoYLinha += 6;
            //paginaGrafico.DrawString($"Gerência de compras", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //paginaGrafico.DrawString($"Autorização", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //posicaoYLinha += 20;

            //paginaGrafico.DrawString($"RECEBI A ORDEM DE COMPRA ACIMA DISCRIMINADA E ACEITO AS CONDIÇÕES MENCIONADAS", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            //posicaoYLinha += 30;

            //paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width * 0.4, posicaoYLinha);
            //paginaGrafico.DrawLine(XPens.Black, margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width * 0.9, posicaoYLinha);
            //posicaoYLinha += 6;
            //paginaGrafico.DrawString($"Data", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            //paginaGrafico.DrawString($"Assinatura", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

        }
        private void AdicionaRodapeParcial(ref XGraphics paginaGrafico, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage pagina, ref PdfDocument doc, PedidoCompraDTO pedidoCompraDTO, XFont fonteTitulo, XFont fonteNormal, XFont fonteCaixaAlta, double valorParcial)
        {
            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 3);
            paginaGrafico.DrawString($"Valor parcial da mercadoria", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total do desconto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total do imposto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total pedido", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            paginaGrafico.DrawString($"{valorParcial.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"R$ 0,00", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.Imposto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento*2;
        }
        private void AdicionaCabecalho(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, PedidoCompraDTO pedidoCompraDTO, XFont fonteTitulo, XFont fonteNormal, string logoPath, string emailUsuario, bool exibeCamposCotacao = true)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 6);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Rua Dr Bacelar, 368 - CJ 103/104 - 10º andar  CEP: 04026-001  Vila Clementino, São Paulo / SP", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Tel: (11) 5904-0522   CNPJ: 62.534.060/0001-41     E-mail: {emailUsuario}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"IE: 110.191.500.117", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento*2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 2);
            graphics.DrawString($"Solicitação: {pedidoCompraDTO.SolicitacaoCompra.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Ordem de Compra: {pedidoCompraDTO.CodigoSequencia}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.35), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Obra: {pedidoCompraDTO.CentroCustoObra?.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.6), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                        posicaoYLinha += espacamento * 2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 5);
            graphics.DrawString($"Fornecedor: {pedidoCompraDTO.Fornecedor.RazaoSocial}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Data Emissão: {pedidoCompraDTO.DataCadastro.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.60), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            graphics.DrawString($"Endereço: {pedidoCompraDTO.Fornecedor.Endereco}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"CNPJ: {pedidoCompraDTO.Fornecedor.CNPJ}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.60), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            graphics.DrawString($"CEP: {pedidoCompraDTO.Fornecedor.CEP}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Bairro: {pedidoCompraDTO?.Fornecedor?.Bairro} - Cidade: {pedidoCompraDTO.Fornecedor.Cidade?.Nome?.Replace($"{pedidoCompraDTO.Fornecedor.Cidade?.UF} - ", "")} - {pedidoCompraDTO.Fornecedor.Cidade?.UF}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.15), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            //graphics.DrawString($"UF: {pedidoCompraDTO.Fornecedor.Cidade?.UF}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Inscrição Estadual: {pedidoCompraDTO.Fornecedor.InscricaoEstadual}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.60), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            graphics.DrawString($"Contato: {pedidoCompraDTO.Fornecedor.NomeVendedor}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Telefone Fixo: {pedidoCompraDTO.Fornecedor.TelefoneFixo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.25), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Telefone Celular: {pedidoCompraDTO.Fornecedor.TelefoneFixo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2 + 6;

            if (exibeCamposCotacao)
            {
                graphics.DrawString("SOLICITAMOS FORNECER OS SEGUINTES MATERIAIS NAS CONDIÇÕES ABAIXO ESTABELECIDAS", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
                posicaoYLinha += espacamento * 2;


                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.05) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.7) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.7) - 2, posicaoYLinha + 35);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha + 35);
                posicaoYLinha += 6;

                graphics.DrawString($"Item", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Discriminação", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.05), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Quantidade", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.6), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Preço unit.", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.7), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"Preço Total", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.8), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento + 6;

                graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
                posicaoYLinha += 10;
            }
        }

        public void AdicionaWatermark(ref PdfPage pagina, ref XGraphics paginaGrafico)
        {
            // Variation 1: Draw a watermark as a text string.

            var watermark = "Não Válido";
            XFont fonteWatermark = new XFont("Arial", 80, XFontStyle.Bold);

            // Get the size (in points) of the text.
            var size = paginaGrafico.MeasureString(watermark, fonteWatermark);

            // Define a rotation transformation at the center of the page.
            paginaGrafico.TranslateTransform(pagina.Width / 2, pagina.Height / 2);
            paginaGrafico.RotateTransform(-Math.Atan(pagina.Height / pagina.Width) * 180 / Math.PI);
            paginaGrafico.TranslateTransform(-pagina.Width / 2, -pagina.Height / 2);

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Create a dimmed red brush.
            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));

            // Draw the string.
            paginaGrafico.DrawString(watermark, fonteWatermark, brush,
                new XPoint((pagina.Width - size.Width) / 2, (pagina.Height - size.Height) / 2),
                format);

            paginaGrafico.Dispose();

            paginaGrafico = XGraphics.FromPdfPage(pagina);
        }

        public async Task<byte[]> GeraPdfPedidoCompra(Int64 id, bool exibirDataPagamento, string logoPath, string emailUsuario)
        {
            var numeroMaximoLinhasPorPagina = 40;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;
            var quantidadeDeCaracteresNaDiscriminacao = 55;

            var pedidoCompraDTO = await Get(id);

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);

            if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                AdicionaWatermark(ref pagina, ref paginaGrafico);

            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, logoPath, emailUsuario);

            var numeroDeLinhasInseridas = 0;
            var numeroDeMateriaisInseridos = 0;
            var item = 0;
            double valorParcial = 0;

            pedidoCompraDTO.Materiais.ForEach(material =>
            {
                paginaGrafico.DrawString($"{(item + 1).ToString().PadLeft(3, '0')}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{material.Quantidade} {material.Material.UnidadeMaterial.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.6), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{material.ValorUnitario.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.7), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{material.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                
                var descricaoList = pedidoCompraDTO.SolicitacaoCompra.Servico ? $"{material.Material.Descricao}\n{pedidoCompraDTO.SolicitacaoCompra.Observacao}\n\nFAVOR MENCIONAR EM SUA NOTA FISCAL: OC {pedidoCompraDTO.Codigo} {pedidoCompraDTO.SolicitacaoCompra?.CentroCustoObra?.Codigo??""}\n\nENDEREÇO DA OBRA: {pedidoCompraDTO.SolicitacaoCompra.EnderecoEntrega}".Split('\n').ToList() : material.Material.Descricao.Split('\n').ToList();
                var numeroDescricoesInseridas = 1;
                 
                descricaoList.ForEach(descricao =>
                {
                    while (descricao.Length > quantidadeDeCaracteresNaDiscriminacao)
                    {
                        //Obtem a posicao do ultimo espaço " " antes do 100
                        var posicao = ServicoString.RetornaPosicaoEspacoMaisProximo(descricao, quantidadeDeCaracteresNaDiscriminacao);

                        paginaGrafico.DrawString($"{descricao.Substring(0, posicao)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                        descricao = descricao.Remove(0, posicao);

                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 12);
                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha + 12);
                        paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                        posicaoYLinha += espacamento;
                        numeroDeLinhasInseridas++;
                    }

                    paginaGrafico.DrawString($"{descricao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.05), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha + 12);
                    paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                    if (descricaoList.Count > 1 && numeroDescricoesInseridas<descricaoList.Count)
                        posicaoYLinha += espacamento;

                    numeroDescricoesInseridas++;
                    numeroDeLinhasInseridas++;
                });

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.05) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.7) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 12);

                posicaoYLinha += espacamento;
                valorParcial += material.ValorTotal;

                item++;
                numeroDeMateriaisInseridos++;

                if (numeroDeLinhasInseridas >= numeroMaximoLinhasPorPagina)
                {
                    //Inseriu todas as linhas possíveis na página

                    //Verifica se existe mais material a ser informado
                    //Caso exista então coloca-se um rodapé parcial e adiciona uma nova página
                    //Caso não exista, então verifica se existe espaço para o rodapé final

                    if (numeroDeMateriaisInseridos < pedidoCompraDTO.Materiais.Count)
                    {
                        posicaoYLinha += espacamento;
                        posicaoYLinha += espacamento;

                        AdicionaRodapeParcial(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, valorParcial);

                        pagina.Close();
                        paginaGrafico.Dispose();

                        pagina = documentoPdf.AddPage();
                        pagina.Size = PdfSharpCore.PageSize.A4;

                        paginaGrafico = XGraphics.FromPdfPage(pagina);

                        if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                            AdicionaWatermark(ref pagina, ref paginaGrafico);

                        posicaoYLinha = margemSuperior;
                        AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, logoPath, emailUsuario);
                    }
                    else
                    {
                        if (numeroDeLinhasInseridas <= 25)
                        {
                            //Adiciona rodapé final

                            AdicionaRodape(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, exibirDataPagamento);

                            pagina.Close();
                            paginaGrafico.Dispose();
                        }
                        else
                        {
                            //Adiciona rodapé parcial
                            //Adiciona nova página
                            //Adiciona rodapé final

                            posicaoYLinha += espacamento;
                            posicaoYLinha += espacamento;

                            AdicionaRodapeParcial(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, valorParcial);

                            pagina.Close();
                            paginaGrafico.Dispose();

                            pagina = documentoPdf.AddPage();
                            pagina.Size = PdfSharpCore.PageSize.A4;

                            paginaGrafico = XGraphics.FromPdfPage(pagina);

                            if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                                AdicionaWatermark(ref pagina, ref paginaGrafico);

                            posicaoYLinha = margemSuperior;

                            AdicionaRodape(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, exibirDataPagamento);
                        }
                    }

                    numeroDeLinhasInseridas = 0;
                }

                if (numeroDeMateriaisInseridos == pedidoCompraDTO.Materiais.Count)
                {
                    //Inseriu todos os materiais

                    if (numeroDeLinhasInseridas <= 25)
                    {
                        //Adiciona rodapé final

                        while(numeroDeLinhasInseridas<25)
                        {
                            posicaoYLinha += espacamento;
                            numeroDeLinhasInseridas++;
                        }

                        AdicionaRodape(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, exibirDataPagamento);

                        pagina.Close();
                        paginaGrafico.Dispose();
                    }
                    else
                    {
                        //Adiciona rodapé parcial
                        //Adiciona nova página
                        //Adiciona rodapé final

                        posicaoYLinha += espacamento;
                        posicaoYLinha += espacamento;

                        AdicionaRodapeParcial(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, valorParcial);

                        pagina.Close();
                        paginaGrafico.Dispose();

                        pagina = documentoPdf.AddPage();
                        pagina.Size = PdfSharpCore.PageSize.A4;

                        paginaGrafico = XGraphics.FromPdfPage(pagina);

                        if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                            AdicionaWatermark(ref pagina, ref paginaGrafico);

                        posicaoYLinha = margemSuperior;

                        AdicionaRodape(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, fonteCaixaAlta, exibirDataPagamento);
                    }
                }
            });

            pagina.Close();
            paginaGrafico.Dispose();

            if (!pedidoCompraDTO.SolicitacaoCompra.Servico)
            {
                pagina = documentoPdf.AddPage();
                pagina.Size = PdfSharpCore.PageSize.A4;

                paginaGrafico = XGraphics.FromPdfPage(pagina);

                if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                    AdicionaWatermark(ref pagina, ref paginaGrafico);

                posicaoYLinha = margemSuperior;

                AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, logoPath, emailUsuario, false);

                paginaGrafico.DrawString($"Condições de pagamento", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"Prazo de Entrega", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"Cliente", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento;

                if (exibirDataPagamento)
                    paginaGrafico.DrawString($"{pedidoCompraDTO.CondicaoPagamento?.Descricao ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                else
                    paginaGrafico.DrawString($"Pagamento conforme medição", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawString($"{pedidoCompraDTO.DataEntrega.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{pedidoCompraDTO.CentroCustoObra?.Cliente?.RazaoSocial}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                posicaoYLinha += 2*espacamento;

                paginaGrafico.DrawString($"Parcelas: ", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                posicaoYLinha += espacamento;

                if (exibirDataPagamento)
                {
                    var cultura = CultureInfo.GetCultureInfo("pt-BR");

                    // Margens e layout básicos
                    double margemDireita = margemEsquerda;   // ajuste se tiver uma margem direita diferente
                    double margemInferior = 40;               // espaço pro rodapé (opcional)
                    double larguraMaxLinha = pagina.Width - margemEsquerda - margemDireita;

                    double x = margemEsquerda;       // cursor horizontal
                    double y = posicaoYLinha;        // cursor vertical (começa onde você já está)
                    double gapHorizontal = 16;       // espaço entre itens na mesma linha

                    foreach (var fatura in pedidoCompraDTO.Faturas.OrderBy(x=>x.DataFatura).ToList())
                    {
                        string data = fatura.DataFatura.ToString("dd/MM/yyyy");
                        string valor = $"{Math.Truncate(fatura.Valor * 100) / 100:N2}";
                        string texto = $"{data} - R$ {valor}";

                        // Mede o tamanho que o texto vai ocupar
                        XSize tamanho = paginaGrafico.MeasureString(texto, fonteNormal);

                        // Se não couber na linha atual, quebra pra próxima
                        if (x > margemEsquerda && x + tamanho.Width > margemEsquerda + larguraMaxLinha)
                        {
                            x = margemEsquerda;
                            y += espacamento;
                        }

                        // Desenha o texto na posição atual
                        paginaGrafico.DrawString(
                            texto,
                            fonteNormal,
                            XBrushes.Black,
                            new XRect(x, y, tamanho.Width, espacamento),
                            XStringFormats.TopLeft
                        );

                        // Avança o cursor horizontal
                        x += tamanho.Width + gapHorizontal;
                    }

                    posicaoYLinha = (int)(y);
                }
                else
                    paginaGrafico.DrawString($"Faturas: Pagamento conforme medição", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                posicaoYLinha += 2*espacamento;

                paginaGrafico.DrawString($"OBSERVAÇÃO DO PEDIDO:", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento * 2;

                var observacao = pedidoCompraDTO.ObservacaoParaFornecedor ?? "";

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
            else
            {
                pagina = documentoPdf.AddPage();
                pagina.Size = PdfSharpCore.PageSize.A4;

                paginaGrafico = XGraphics.FromPdfPage(pagina);

                if (pedidoCompraDTO.IdStatusPedidoCompra == 1)
                    AdicionaWatermark(ref pagina, ref paginaGrafico);

                posicaoYLinha = margemSuperior;

                AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoCompraDTO, fonteTitulo, fonteNormal, logoPath, emailUsuario, false);

                paginaGrafico.DrawString($"Condições de pagamento", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"Prazo de Entrega", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"Cliente", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                posicaoYLinha += espacamento;

                if (exibirDataPagamento)
                    paginaGrafico.DrawString($"{pedidoCompraDTO.CondicaoPagamento?.Descricao ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                else
                    paginaGrafico.DrawString($"Pagamento conforme medição", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawString($"{pedidoCompraDTO.DataEntrega.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{pedidoCompraDTO.CentroCustoObra?.Cliente?.RazaoSocial}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                posicaoYLinha += 2 * espacamento;

                paginaGrafico.DrawString($"Parcelas: ", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                posicaoYLinha += espacamento;

                if (exibirDataPagamento)
                {
                    var cultura = CultureInfo.GetCultureInfo("pt-BR");

                    // Margens e layout básicos
                    double margemDireita = margemEsquerda;   // ajuste se tiver uma margem direita diferente
                    double margemInferior = 40;               // espaço pro rodapé (opcional)
                    double larguraMaxLinha = pagina.Width - margemEsquerda - margemDireita;

                    double x = margemEsquerda;       // cursor horizontal
                    double y = posicaoYLinha;        // cursor vertical (começa onde você já está)
                    double gapHorizontal = 16;       // espaço entre itens na mesma linha

                    foreach (var fatura in pedidoCompraDTO.Faturas.OrderBy(x=>x.DataFatura).ToList())
                    {
                        string data = fatura.DataFatura.ToString("dd/MM/yyyy");
                        string valor = fatura.Valor.ToString("N2", cultura); // sem "R$" aqui
                        string texto = $"{data} - R$ {valor}";

                        // Mede o tamanho que o texto vai ocupar
                        XSize tamanho = paginaGrafico.MeasureString(texto, fonteNormal);

                        // Se não couber na linha atual, quebra pra próxima
                        if (x > margemEsquerda && x + tamanho.Width > margemEsquerda + larguraMaxLinha)
                        {
                            x = margemEsquerda;
                            y += espacamento;
                        }

                        // Desenha o texto na posição atual
                        paginaGrafico.DrawString(
                            texto,
                            fonteNormal,
                            XBrushes.Black,
                            new XRect(x, y, tamanho.Width, espacamento),
                            XStringFormats.TopLeft
                        );

                        // Avança o cursor horizontal
                        x += tamanho.Width + gapHorizontal;

                        // (Opcional) quebra de página simples para não ultrapassar o rodapé
                        if (y + espacamento > pagina.Height - margemInferior)
                        {
                            // Se precisar, aqui você adiciona nova página e reinicia x/y.
                            // Exemplo:
                            // var novaPagina = document.AddPage();
                            // paginaGrafico = XGraphics.FromPdfPage(novaPagina);
                            // x = margemEsquerda;
                            // y = /* topo desejado */;
                            // continue;
                            break; // por simplicidade, interrompe na página atual
                        }
                    }

                    // Deixa o cursor logo abaixo do último bloco escrito
                    posicaoYLinha = (int)(y);
                }
                else
                    paginaGrafico.DrawString($"Faturas: Pagamento conforme medição", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

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

        public async Task<PedidoCompraDevolucaoSaldoDTO> CancelarSaldo(PedidoCompraDevolucaoSaldoDTO pedidoCompraDevolucaoSaldoDTO)
        {
            if (string.IsNullOrEmpty(pedidoCompraDevolucaoSaldoDTO.Observacao) || string.IsNullOrWhiteSpace(pedidoCompraDevolucaoSaldoDTO.Observacao))
                throw new Exception("Observação inválida");

            if (pedidoCompraDevolucaoSaldoDTO.Valor <= 0)
                throw new Exception("Valor inválido");

            return await _repositorioPedidoCompra.CancelarSaldo(pedidoCompraDevolucaoSaldoDTO);
        }

        public async Task<List<PedidoCompra_NotaFiscalParaAprovacaoDTO>> ObtemNotasFiscaisParaAprovacao(Int64 IdUsuario)
        {
            return await _repositorioPedidoCompra.ObtemNotasFiscaisParaAprovacao(IdUsuario);
        }

        public async Task AprovarReprovarNotaFiscal(Int64 idPedidoCompraNotaFiscal, bool aprovada, Int64 idUsuario)
        {
            await _repositorioPedidoCompra.AprovarReprovarNotaFiscal(idPedidoCompraNotaFiscal, aprovada, idUsuario);
        }

        public async Task CancelarNotaFiscal(Int64 idPedidoCompraNotaFiscal, Int64 idUsuario)
        {
            await _repositorioPedidoCompra.CancelarNotaFiscal(idPedidoCompraNotaFiscal, idUsuario);
        }
        public async Task AlterarObraNotaFiscal(Int64 idPedidoCompraNotaFiscal, Int64 idNovaObra, Int64 idUsuario)
        {
            await _repositorioPedidoCompra.AlterarObraNotaFiscal(idPedidoCompraNotaFiscal, idNovaObra, idUsuario);
        }

        public async Task<PedidoCompraNotaFiscalPagamentoDTO> PostNotaFiscalPagamento(PedidoCompraNotaFiscalPagamentoDTO pagamentoDTO)
        {
            return await _repositorioPedidoCompra.PostNotaFiscalPagamento(pagamentoDTO);
        }

        public async Task<PedidoCompraNotaFiscalPagamentoDTO> ObtemNotaFiscalPagamento(Int64 idPedidoCompraNotaFiscal)
        {
            return await _repositorioPedidoCompra.ObtemNotaFiscalPagamento(idPedidoCompraNotaFiscal);
        }

        public async Task<List<PedidoCompraNotaFiscalDTO>> ObtemNotasFiscaisAlteracaoData(Int64 idPedidoCompraNotaFiscal)
        {
            return await _repositorioPedidoCompra.ObtemNotasFiscaisAlteracaoData(idPedidoCompraNotaFiscal);
        }

        public async Task CancelarPedido(Int64 id, string motivoCancelamento)
        {
            await _repositorioPedidoCompra.CancelarPedido(id, motivoCancelamento);
        }
    }
}
