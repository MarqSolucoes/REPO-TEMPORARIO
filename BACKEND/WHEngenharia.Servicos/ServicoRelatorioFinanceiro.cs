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
using WHEngenharia.SQL;

namespace WHEngenharia.Servicos
{
    public class ServicoRelatorioFinanceiro
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public ServicoRelatorioFinanceiro(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private void AdicionaRodape(ref XGraphics paginaGrafico, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage pagina, ref PdfDocument doc, PedidoCompraDTO pedidoCompraDTO, XFont fonteTitulo, XFont fonteNormal, XFont fonteCaixaAlta)
        {
            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 3);
            paginaGrafico.DrawString($"Valor total da mercadoria", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total do desconto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total do imposto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Valor total pedido", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            paginaGrafico.DrawString($"{pedidoCompraDTO.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"R$ 0,00", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"R$ 0,00", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 3);
            paginaGrafico.DrawString($"SR FORNECEDOR: MERCADORIA DESTINADA A USO OU CONSUMO PRÓPRIO PARA UTILIZAÇÃO NA PRESTAÇÃO DE SERVIÇOS, POR", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"EMPRESA NÃO CONTRIBUINTE. PORTANTO O ICMS DEVERÁ SER COBRADO MEDIANTE APLICAÇÃO DA ALÍQUOTA INTERNA DO", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 10;
            paginaGrafico.DrawString($"ESTADO DE LOCALIZAÇÃO DO ESTABELECIMENTO FORNECEDOR", fonteCaixaAlta, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento + 4;

            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 4);
            paginaGrafico.DrawString($"Condições de pagamento", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Prazo de Entrega", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Cliente", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            paginaGrafico.DrawString($"VERIFICAR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.DataEntrega.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.25), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.CentroCustoObra?.Cliente?.RazaoSocial}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.45), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;
            paginaGrafico.DrawString($"Endereço de entrega: {pedidoCompraDTO.CentroCustoObra?.EnderecoObra}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            paginaGrafico.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, pagina.Width - (2 * margemEsquerda) + 10, espacamento * 8 + 6);
            paginaGrafico.DrawString($"Condições de fornecimento", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento;
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
            posicaoYLinha += 30;

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width * 0.4, posicaoYLinha);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width * 0.9, posicaoYLinha);
            posicaoYLinha += 6;
            paginaGrafico.DrawString($"Gerência de compras", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Autorização", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += 20;

            paginaGrafico.DrawString($"RECEBI A ORDEM DE COMPRA ACIMA DISCRIMINADA E ACEITO AS CONDIÇÕES MENCIONADAS", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopCenter);
            posicaoYLinha += 30;

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width * 0.4, posicaoYLinha);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width * 0.9, posicaoYLinha);
            posicaoYLinha += 6;
            paginaGrafico.DrawString($"Data", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Assinatura", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + pagina.Width * 0.5, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

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
            paginaGrafico.DrawString($"R$ 0,00", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{pedidoCompraDTO.ValorTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.75), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;
        }
        private void AdicionaCabecalho(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento;
            graphics.DrawString($"Relatório Financeiro Mensal - 05/2023", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);


        }

        public async Task<byte[]> GeraPdfPedidoCompra(Int64 id, string logoPath)
        {
            var numeroItensPorPagina = 40;

            var margemEsquerda = 30;
            var margemSuperior = 20;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var documentoPdf = new PdfDocument();
            var page = documentoPdf.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;
            page.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var graphics = XGraphics.FromPdfPage(page);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalho(ref graphics, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref page, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);

            posicaoYLinha += espacamento * 2;
            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.06) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.06) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.16) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.16) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.26) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.26) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.54) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.54) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"OBRA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DEF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.06), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"O.C.", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.10), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"NF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.16), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"REGISTRO PAGTO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.26), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"VALOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.40), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FORNECEDOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.54), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;

            _context.Obra.Select(x => x.Codigo).ToList().ForEach(x =>
            {
                graphics.DrawString($"{x}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"03.01", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.06), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"P85194A", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.10), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"NF 12345678", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.16), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"01/01/2022 01/01/2023", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.26), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"R$ 1.234.456,00", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.40), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
                graphics.DrawString($"CODARIN SHOPPING DA CONSTRUÇÃO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.54), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);

                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.06) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.06) - 2, posicaoYLinha + 12);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.10) - 2, posicaoYLinha + 12);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.16) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.16) - 2, posicaoYLinha + 12);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.26) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.26) - 2, posicaoYLinha + 12);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.40) - 2, posicaoYLinha + 12);
                graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.54) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.54) - 2, posicaoYLinha + 12);

                posicaoYLinha += espacamento;
            });

            page.Close();
            graphics.Dispose();

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
    }
}
