using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoAgenda
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioAgenda _repositorioAgenda;
        private RepositorioPedidoCompra _repositorioPedidoCompra;
        private RepositorioPedidoCompraNotaFiscal _repositorioPedidoCompraNotaFiscal;


        public ServicoAgenda(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioAgenda = new RepositorioAgenda(_context, _mapper);
            _repositorioPedidoCompra = new RepositorioPedidoCompra(_context, _mapper);
            _repositorioPedidoCompraNotaFiscal = new RepositorioPedidoCompraNotaFiscal(_context, _mapper);
        }

        public async Task<AgendaDTO> CadastraAgenda(AgendaDTO agendaDTO)
        {
            return await _repositorioAgenda.CadastraAgenda(agendaDTO);
        }

        public async Task AlteraDef(Int64 id, Int64 idDef, Int64 IdUsuario, string NomeUsuario)
        {
            await _repositorioAgenda.AlteraDef(id, idDef, IdUsuario, NomeUsuario);
        }

        public async Task AlteraValor(Agenda_AlteracaoValorDTO parametros)
        {
            await _repositorioAgenda.AlteraValor(parametros);
        }

        public async Task AlteraDataPagamento(Agenda_AlteracaoDataDTO parametros)
        {
            await _repositorioAgenda.AlteraDataPagamento(parametros);
        }

        public async Task AlteraDefEmLote(Agenda_AlteracaoDefLoteDTO lote)
        {
            await _repositorioAgenda.AlteraDefEmLote(lote);
        }

        public async Task AlteraDataPagamentoEmLote(Agenda_AlteracaoDataLoteDTO lote)
        {
            await _repositorioAgenda.AlteraDataPagamentoEmLote(lote);
        }

        public async Task InformarPagamentoRecebimentoLote(Agenda_PagamentoRecebimentoLoteDTO lote)
        {
            await _repositorioAgenda.InformarPagamentoRecebimentoLote(lote);
        }

        public async Task CancelarPagamentoRecebimento(Agenda_CancelamentoPagamentoRecebimentoDTO parametros)
        {
            await _repositorioAgenda.CancelarPagamentoRecebimento(parametros);
        }

        public async Task<List<AgendaDTO>> ObtemAgendaFaturamento(Agenda_RequestDTO parametros)
        {
            return await _repositorioAgenda.ObtemAgendaFaturamento(parametros);
        }

        public async Task<List<AgendaDTO>> Get(Agenda_RequestDTO parametros)
        {
            return await _repositorioAgenda.Get(parametros);
        }

        private void AdicionaCabecalhoAgenda(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 5);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"AGENDA - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 3;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.12) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.12) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.18) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.18) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.24) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.24) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.4) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.4) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"OBRA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DEF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.07), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"O.C.", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.12), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"PI", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.18), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"NF", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.24), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DT LAN", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.3), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DT PAG", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.4), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"VALOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FORNECEDOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.6), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"COLABORADOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.8), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }

        public async Task<byte[]> GeraPDFAgenda(string logoPath, List<AgendaDTO> result)
        {
            var numeroItensPorPagina = 30;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalhoAgenda(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);

            var count = 0;

            result.ForEach(item =>
            {
                paginaGrafico.DrawString($"{item.CodigoObra}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.CodigoDef}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.07), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.CodigoPedidoCompra}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.12), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.CodigoPedidoInterno}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.18), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.NumeroNF}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.24), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(item.DataLancamento.Year == 2500 ? "" : item.DataLancamento.ToString("dd/MM/yyyy"))}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.3), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(item.DataPagamento.Year == 2500 ? "" : item.DataPagamento.ToString("dd/MM/yyyy"))}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.4), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.NomeFantasiaFornecedor}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.6), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{item.NomeUsuarioBeneficiario}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);


                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.12) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.12) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.18) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.18) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.24) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.24) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 18);
                posicaoYLinha += espacamento;

                count++;

                if (count % numeroItensPorPagina == 0)
                {
                    pagina.Close();
                    paginaGrafico.Dispose();

                    pagina = documentoPdf.AddPage();
                    pagina.Size = PdfSharpCore.PageSize.A4;
                    pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                    paginaGrafico = XGraphics.FromPdfPage(pagina);

                    posicaoYLinha = margemSuperior;

                    AdicionaCabecalhoAgenda(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);

                    count = 0;
                }
            });

            pagina.Close();
            paginaGrafico.Dispose();

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

        public async Task<byte[]> ExcelFaturamento(Agenda_ExcelFaturamento_RequestDTO parametros)
        {
            var result = await _repositorioAgenda.ExcelFaturamento(parametros);

            using var workbook = new XLWorkbook();

            // ── Paleta tons de cinza ─────────────────────────────────────────────────────
            var corCinzaEscuro = XLColor.FromHtml("#2D2D2D"); // cabeçalho
            var corCinzaMedio = XLColor.FromHtml("#606060"); // totais
            var corCinzaBorda = XLColor.FromHtml("#A0A0A0"); // bordas
            var corCinzaPar = XLColor.FromHtml("#EFEFEF"); // zebra par
            var corCinzaImpar = XLColor.FromHtml("#FAFAFA"); // zebra ímpar
            var corBranco = XLColor.White;
            var corTexto = XLColor.FromHtml("#1A1A1A"); // texto nas linhas

            // ── Helpers ─────────────────────────────────────────────────────────────────

            void EstilizarCabecalho(IXLWorksheet ws, int totalColunas)
            {
                var range = ws.Range(1, 1, 1, totalColunas);
                range.Style.Fill.BackgroundColor = corCinzaEscuro;
                range.Style.Font.FontColor = corBranco;
                range.Style.Font.Bold = true;
                range.Style.Font.FontSize = 11;
                range.Style.Font.FontName = "Calibri";
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                range.Style.Border.OutsideBorderColor = corCinzaEscuro;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorderColor = XLColor.FromHtml("#555555");
                ws.Row(1).Height = 22;
            }

            void EstilizarLinhas(IXLWorksheet ws, int totalLinhas, int totalColunas)
            {
                for (int i = 2; i <= totalLinhas + 1; i++)
                {
                    var range = ws.Range(i, 1, i, totalColunas);
                    range.Style.Fill.BackgroundColor = i % 2 == 0 ? corCinzaPar : corCinzaImpar;
                    range.Style.Font.FontColor = corTexto;
                    range.Style.Font.FontSize = 10;
                    range.Style.Font.FontName = "Calibri";
                    range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorderColor = corCinzaBorda;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorderColor = corCinzaBorda;
                    ws.Row(i).Height = 18;
                }
            }

            void FormatarColunasMonetarias(IXLWorksheet ws, int totalLinhas, params int[] colunas)
            {
                foreach (var col in colunas)
                    ws.Range(2, col, totalLinhas + 1, col)
                      .Style.NumberFormat.Format = "R$ #,##0.00";
            }

            void FormatarColunasCentro(IXLWorksheet ws, int totalLinhas, params int[] colunas)
            {
                foreach (var col in colunas)
                    ws.Range(2, col, totalLinhas + 1, col)
                      .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            void AdicionarTotais(IXLWorksheet ws, int totalLinhas, int totalColunas, params int[] colunasMonetarias)
            {
                int rowTotais = totalLinhas + 2;

                var linhaTotal = ws.Range(rowTotais, 1, rowTotais, totalColunas);
                linhaTotal.Style.Fill.BackgroundColor = corCinzaMedio;
                linhaTotal.Style.Font.FontColor = corBranco;
                linhaTotal.Style.Font.Bold = true;
                linhaTotal.Style.Font.FontSize = 10;
                linhaTotal.Style.Font.FontName = "Calibri";
                linhaTotal.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                linhaTotal.Style.Border.OutsideBorderColor = corCinzaEscuro;
                ws.Row(rowTotais).Height = 20;

                var labelCell = ws.Cell(rowTotais, 1);
                labelCell.Value = "TOTAL";
                labelCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                foreach (var col in colunasMonetarias)
                {
                    var cell = ws.Cell(rowTotais, col);
                    cell.FormulaA1 = $"=SUM({ws.Column(col).ColumnLetter()}2:{ws.Column(col).ColumnLetter()}{totalLinhas + 1})";
                    cell.Style.NumberFormat.Format = "R$ #,##0.00";
                    cell.Style.Fill.BackgroundColor = corCinzaEscuro;
                    cell.Style.Font.FontColor = corBranco;
                    cell.Style.Font.Bold = true;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = corCinzaBorda;
                }
            }

            void AjustarLarguraColunas(IXLWorksheet ws)
            {
                ws.Columns().AdjustToContents();
                foreach (var col in ws.ColumnsUsed())
                {
                    if (col.Width < 12) col.Width = 12;
                    if (col.Width > 45) col.Width = 45;
                }
            }

            // ── Aba "A Faturar" ─────────────────────────────────────────────────────────

            var wsAFaturar = workbook.Worksheets.Add("A Faturar");

            wsAFaturar.Cell(1, 1).Value = "OBRA";
            wsAFaturar.Cell(1, 2).Value = "DESCRIÇÃO";
            wsAFaturar.Cell(1, 3).Value = "CNPJ";
            wsAFaturar.Cell(1, 4).Value = "CLIENTE";
            wsAFaturar.Cell(1, 5).Value = "BRUTO";
            wsAFaturar.Cell(1, 6).Value = "INSS";
            wsAFaturar.Cell(1, 7).Value = "ISS";
            wsAFaturar.Cell(1, 8).Value = "IR";
            wsAFaturar.Cell(1, 9).Value = "ART30";
            wsAFaturar.Cell(1, 10).Value = "LIQUIDO";
            wsAFaturar.Cell(1, 11).Value = "DATA FATURAMENTO";

            for (var i = 0; i < result.AFaturar.Count; i++)
            {
                var row = i + 2;
                var item = result.AFaturar[i];
                wsAFaturar.Cell(row, 1).Value = item.Codigo;
                wsAFaturar.Cell(row, 2).Value = item.Descricao;
                wsAFaturar.Cell(row, 3).Value = item.CNPJ;
                wsAFaturar.Cell(row, 4).Value = item.RazaoSocial;
                wsAFaturar.Cell(row, 5).Value = item.Valor;
                wsAFaturar.Cell(row, 6).Value = item.INSS;
                wsAFaturar.Cell(row, 7).Value = item.ISS;
                wsAFaturar.Cell(row, 8).Value = item.IR;
                wsAFaturar.Cell(row, 9).Value = item.ART30;
                wsAFaturar.Cell(row, 10).Value = item.ValorLiquido;
                wsAFaturar.Cell(row, 11).Value = item.DataPagamento.ToString("dd/MM/yyyy");
            }

            EstilizarCabecalho(wsAFaturar, 11);
            EstilizarLinhas(wsAFaturar, result.AFaturar.Count, 11);
            FormatarColunasMonetarias(wsAFaturar, result.AFaturar.Count, 5, 6, 7, 8, 9, 10);
            FormatarColunasCentro(wsAFaturar, result.AFaturar.Count, 1, 3, 11);
            AdicionarTotais(wsAFaturar, result.AFaturar.Count, 11, 5, 6, 7, 8, 9, 10);
            AjustarLarguraColunas(wsAFaturar);

            // ── Aba "Faturado a Receber" ─────────────────────────────────────────────────

            var wsFatAReceber = workbook.Worksheets.Add("Faturado a receber");

            wsFatAReceber.Cell(1, 1).Value = "OBRA";
            wsFatAReceber.Cell(1, 2).Value = "CLIENTE";
            wsFatAReceber.Cell(1, 3).Value = "NF";
            wsFatAReceber.Cell(1, 4).Value = "BRUTO";
            wsFatAReceber.Cell(1, 5).Value = "INSS";
            wsFatAReceber.Cell(1, 6).Value = "ISS";
            wsFatAReceber.Cell(1, 7).Value = "IR";
            wsFatAReceber.Cell(1, 8).Value = "ART30";
            wsFatAReceber.Cell(1, 9).Value = "DESCONTO";
            wsFatAReceber.Cell(1, 10).Value = "SINAL";
            wsFatAReceber.Cell(1, 11).Value = "NÃO COMISSIONADO";
            wsFatAReceber.Cell(1, 12).Value = "LIQUIDO";
            wsFatAReceber.Cell(1, 13).Value = "DATA FATURAMENTO";
            wsFatAReceber.Cell(1, 14).Value = "DATA RECEBIMENTO PREVISTO";

            for (var i = 0; i < result.FaturadoAReceber.Count; i++)
            {
                var row = i + 2;
                var item = result.FaturadoAReceber[i];
                wsFatAReceber.Cell(row, 1).Value = item.Obra.Codigo;
                wsFatAReceber.Cell(row, 2).Value = item.Obra.Cliente.RazaoSocial;
                wsFatAReceber.Cell(row, 3).Value = item.NumeroNF;
                wsFatAReceber.Cell(row, 4).Value = item.ValorBruto;
                wsFatAReceber.Cell(row, 5).Value = item.ValorINSS;
                wsFatAReceber.Cell(row, 6).Value = item.ValorISS;
                wsFatAReceber.Cell(row, 7).Value = item.ValorIR;
                wsFatAReceber.Cell(row, 8).Value = item.ValorArt30;
                wsFatAReceber.Cell(row, 9).Value = item.ValorDesconto;
                wsFatAReceber.Cell(row, 10).Value = item.ValorSinal;
                wsFatAReceber.Cell(row, 11).Value = item.ValorNaoComissionado;
                wsFatAReceber.Cell(row, 12).Value = item.ValorLiquido;
                wsFatAReceber.Cell(row, 13).Value = item.DataFaturamento.ToString("dd/MM/yyyy");
                wsFatAReceber.Cell(row, 14).Value = item.DataRecebimentoPrevisto.ToString("dd/MM/yyyy");
            }

            EstilizarCabecalho(wsFatAReceber, 14);
            EstilizarLinhas(wsFatAReceber, result.FaturadoAReceber.Count, 14);
            FormatarColunasMonetarias(wsFatAReceber, result.FaturadoAReceber.Count, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            FormatarColunasCentro(wsFatAReceber, result.FaturadoAReceber.Count, 1, 3, 13, 14);
            AdicionarTotais(wsFatAReceber, result.FaturadoAReceber.Count, 14, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            AjustarLarguraColunas(wsFatAReceber);

            // ── Aba "Faturado Recebido" ──────────────────────────────────────────────────

            var wsFatRecebido = workbook.Worksheets.Add("Faturado recebido");

            wsFatRecebido.Cell(1, 1).Value = "OBRA";
            wsFatRecebido.Cell(1, 2).Value = "CLIENTE";
            wsFatRecebido.Cell(1, 3).Value = "NF";
            wsFatRecebido.Cell(1, 4).Value = "BRUTO";
            wsFatRecebido.Cell(1, 5).Value = "INSS";
            wsFatRecebido.Cell(1, 6).Value = "ISS";
            wsFatRecebido.Cell(1, 7).Value = "IR";
            wsFatRecebido.Cell(1, 8).Value = "ART30";
            wsFatRecebido.Cell(1, 9).Value = "DESCONTO";
            wsFatRecebido.Cell(1, 10).Value = "SINAL";
            wsFatRecebido.Cell(1, 11).Value = "NÃO COMISSIONADO";
            wsFatRecebido.Cell(1, 12).Value = "LIQUIDO";
            wsFatRecebido.Cell(1, 13).Value = "DATA FATURAMENTO";
            wsFatRecebido.Cell(1, 14).Value = "DATA RECEBIMENTO";

            for (var i = 0; i < result.FaturadoRecebido.Count; i++)
            {
                var row = i + 2;
                var item = result.FaturadoRecebido[i];
                wsFatRecebido.Cell(row, 1).Value = item.Obra.Codigo;
                wsFatRecebido.Cell(row, 2).Value = item.Obra.Cliente.RazaoSocial;
                wsFatRecebido.Cell(row, 3).Value = item.NumeroNF;
                wsFatRecebido.Cell(row, 4).Value = item.ValorBruto;
                wsFatRecebido.Cell(row, 5).Value = item.ValorINSS;
                wsFatRecebido.Cell(row, 6).Value = item.ValorISS;
                wsFatRecebido.Cell(row, 7).Value = item.ValorIR;
                wsFatRecebido.Cell(row, 8).Value = item.ValorArt30;
                wsFatRecebido.Cell(row, 9).Value = item.ValorDesconto;
                wsFatRecebido.Cell(row, 10).Value = item.ValorSinal;
                wsFatRecebido.Cell(row, 11).Value = item.ValorNaoComissionado;
                wsFatRecebido.Cell(row, 12).Value = item.ValorLiquido;
                wsFatRecebido.Cell(row, 13).Value = item.DataFaturamento.ToString("dd/MM/yyyy");
                wsFatRecebido.Cell(row, 14).Value = item.DataRecebimentoRealizado?.ToString("dd/MM/yyyy");
            }

            EstilizarCabecalho(wsFatRecebido, 14);
            EstilizarLinhas(wsFatRecebido, result.FaturadoRecebido.Count, 14);
            FormatarColunasMonetarias(wsFatRecebido, result.FaturadoRecebido.Count, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            FormatarColunasCentro(wsFatRecebido, result.FaturadoRecebido.Count, 1, 3, 13, 14);
            AdicionarTotais(wsFatRecebido, result.FaturadoRecebido.Count, 14, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            AjustarLarguraColunas(wsFatRecebido);

            // ── Serialização ─────────────────────────────────────────────────────────────

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> PdfFaturamento(Agenda_ExcelFaturamento_RequestDTO parametros, string logoPath)
        {
            var result = await _repositorioAgenda.ExcelFaturamento(parametros);
            var cultura = new CultureInfo("pt-BR");

            // ── Paleta formal (controle de faturamento WH) ─────────────────────────────
            var corPreto = XColor.FromArgb(0, 0, 0);
            var corCinzaCab = XColor.FromArgb(75, 75, 75);       // faixa de cabeçalho de seção (estilo Modelo 17)
            var corCinzaTit = XColor.FromArgb(210, 210, 210);    // faixa de títulos de colunas
            var corCinzaPar = XColor.FromArgb(245, 245, 245);
            var corCinzaLabel = XColor.FromArgb(235, 235, 235);  // células de label do bloco identificação
            var corBranco = XColors.White;
            var corTexto = XColor.FromArgb(20, 20, 20);
            var corCinzaMedio = XColor.FromArgb(100, 100, 100);

            var fTitulo = new XFont("Arial", 13, XFontStyle.Bold);
            var fSecao = new XFont("Arial", 10, XFontStyle.Bold);
            var fLabel = new XFont("Arial", 8, XFontStyle.Bold);
            var fValor = new XFont("Arial", 8.5, XFontStyle.Regular);
            var fValorBold = new XFont("Arial", 8.5, XFontStyle.Bold);
            var fCabecalhoCol = new XFont("Arial", 7.5, XFontStyle.Bold);
            var fCorpo = new XFont("Arial", 8, XFontStyle.Regular);
            var fCorpoBold = new XFont("Arial", 8, XFontStyle.Bold);
            var fRodape = new XFont("Arial", 7.5, XFontStyle.Regular);

            var brBranco = new XSolidBrush(corBranco);
            var brTexto = new XSolidBrush(corTexto);
            var brPreto = new XSolidBrush(corPreto);
            var brCinzaCab = new XSolidBrush(corCinzaCab);
            var brCinzaTit = new XSolidBrush(corCinzaTit);
            var brCinzaPar = new XSolidBrush(corCinzaPar);
            var brCinzaLabel = new XSolidBrush(corCinzaLabel);
            var brCinzaMedio = new XSolidBrush(corCinzaMedio);

            var penPretoGrosso = new XPen(corPreto, 1.2);
            var penPretoFino = new XPen(corPreto, 0.6);
            var penPretoMuitoFino = new XPen(corPreto, 0.4);

            var doc = new PdfDocument();
            doc.Info.Title = "Controle de Faturamento - WH Engenharia";
            doc.Info.Author = "WH Engenharia Ltda";

            const double margem = 30;
            const double alturaLinha = 16;
            const double alturaCabTab = 20;
            const double alturaTotal = 20;
            const double topoTabela = 130; // após bloco de identificação
            const double reservaRodape = 35;

            XImage logo = null;
            try { if (File.Exists(logoPath)) logo = XImage.FromFile(logoPath); } catch { }

            double sAFatValor = result.AFaturar.Sum(x => x.Valor);
            double sAFatINSS = result.AFaturar.Sum(x => x.INSS);
            double sAFatISS = result.AFaturar.Sum(x => x.ISS);
            double sAFatIR = result.AFaturar.Sum(x => x.IR);
            double sAFatART30 = result.AFaturar.Sum(x => x.ART30);
            double sAFatLiq = result.AFaturar.Sum(x => x.ValorLiquido);
            
            double sRecBruto = result.FaturadoAReceber.Sum(x => x.ValorBruto);
            double sRecINSS = result.FaturadoAReceber.Sum(x => x.ValorINSS);
            double sRecISS = result.FaturadoAReceber.Sum(x => x.ValorISS);
            double sRecIR = result.FaturadoAReceber.Sum(x => x.ValorIR);
            double sRecART30 = result.FaturadoAReceber.Sum(x => x.ValorArt30);
            double sRecDesc = result.FaturadoAReceber.Sum(x => x.ValorDesconto);
            double sRecSinal = result.FaturadoAReceber.Sum(x => x.ValorSinal);
            double sRecNaoCom = result.FaturadoAReceber.Sum(x => x.ValorNaoComissionado);
            double sRecLiq = result.FaturadoAReceber.Sum(x => x.ValorLiquido);
            
            double sRcbBruto = result.FaturadoRecebido.Sum(x => x.ValorBruto);
            double sRcbINSS = result.FaturadoRecebido.Sum(x => x.ValorINSS);
            double sRcbISS = result.FaturadoRecebido.Sum(x => x.ValorISS);
            double sRcbIR = result.FaturadoRecebido.Sum(x => x.ValorIR);
            double sRcbART30 = result.FaturadoRecebido.Sum(x => x.ValorArt30);
            double sRcbDesc = result.FaturadoRecebido.Sum(x => x.ValorDesconto);
            double sRcbSinal = result.FaturadoRecebido.Sum(x => x.ValorSinal);
            double sRcbNaoCom = result.FaturadoRecebido.Sum(x => x.ValorNaoComissionado);
            double sRcbLiq = result.FaturadoRecebido.Sum(x => x.ValorLiquido);
            
            double totalGeralBruto = sAFatValor + sRecBruto + sRcbBruto;
            double totalGeralLiquido = sAFatLiq + sRecLiq + sRcbLiq;

            // ═══════════════════════════════════════════════════════════════════════════
            // Configuração das seções (cálculo dinâmico de largura monetária)
            // ═══════════════════════════════════════════════════════════════════════════
            var pagCapa = doc.AddPage();
            pagCapa.Size = PdfSharpCore.PageSize.A4;
            pagCapa.Orientation = PdfSharpCore.PageOrientation.Landscape;
            double pw = pagCapa.Width, ph = pagCapa.Height, cw = pw - 2 * margem;
            var gfxMedidor = XGraphics.FromPdfPage(pagCapa);

            Func<string, IEnumerable<double>, double, double> larguraMinColuna = (titulo, valores, totalCol) =>
            {
                double max = gfxMedidor.MeasureString(titulo, fCabecalhoCol).Width;
                foreach (var v in valores) { var w = gfxMedidor.MeasureString(v.ToString("C2", cultura), fCorpoBold).Width; if (w > max) max = w; }
                var wt = gfxMedidor.MeasureString(totalCol.ToString("C2", cultura), fCorpoBold).Width;
                if (wt > max) max = wt;
                return max + 10;
            };

            // ─── Seção 1: A Faturar ─────────────────────────────────────────────────────
            double l1_bruto = larguraMinColuna("BRUTO", result.AFaturar.Select(x => x.Valor), sAFatValor);
            double l1_inss = larguraMinColuna("INSS", result.AFaturar.Select(x => x.INSS), sAFatINSS);
            double l1_iss = larguraMinColuna("ISS", result.AFaturar.Select(x => x.ISS), sAFatISS);
            double l1_ir = larguraMinColuna("IR", result.AFaturar.Select(x => x.IR), sAFatIR);
            double l1_art = larguraMinColuna("ART30", result.AFaturar.Select(x => x.ART30), sAFatART30);
            double l1_liq = larguraMinColuna("LÍQUIDO", result.AFaturar.Select(x => x.ValorLiquido), sAFatLiq);
            var larguras1 = new double[] { 50, 0, 95, 0, l1_bruto, l1_inss, l1_iss, l1_ir, l1_art, l1_liq, 70 };
            double sobra1 = cw - larguras1.Sum(); if (sobra1 < 120) sobra1 = 120;
            larguras1[1] = sobra1 * 0.55; larguras1[3] = sobra1 * 0.45;
            var titulos1 = new[] { "OBRA", "DESCRIÇÃO", "CNPJ", "CLIENTE", "BRUTO", "INSS", "ISS", "IR", "ART30", "LÍQUIDO", "DATA FAT." };
            var alinh1 = new[] { "center", "left", "center", "left", "right", "right", "right", "right", "right", "right", "center" };
            var linhas1 = result.AFaturar.Select(i => new[] { i.Codigo ?? "", i.Descricao ?? "", i.CNPJ ?? "", i.RazaoSocial ?? "", (i.Valor).ToString("C2", cultura), (i.INSS).ToString("C2", cultura), (i.ISS).ToString("C2", cultura), (i.IR).ToString("C2", cultura), (i.ART30).ToString("C2", cultura), (i.ValorLiquido).ToString("C2", cultura), i.DataPagamento.ToString("dd/MM/yyyy") }).ToList();
            var totais1 = new[] { "TOTAL", "", "", "", sAFatValor.ToString("C2", cultura), sAFatINSS.ToString("C2", cultura), sAFatISS.ToString("C2", cultura), sAFatIR.ToString("C2", cultura), sAFatART30.ToString("C2", cultura), sAFatLiq.ToString("C2", cultura), "" };
            var monet1 = new[] { false, false, false, false, true, true, true, true, true, true, false };

            // ─── Seção 2: Faturado a Receber ───────────────────────────────────────────
            double l2_bruto = larguraMinColuna("BRUTO", result.FaturadoAReceber.Select(x => x.ValorBruto), sRecBruto);
            double l2_inss = larguraMinColuna("INSS", result.FaturadoAReceber.Select(x => x.ValorINSS), sRecINSS);
            double l2_iss = larguraMinColuna("ISS", result.FaturadoAReceber.Select(x => x.ValorISS), sRecISS);
            double l2_ir = larguraMinColuna("IR", result.FaturadoAReceber.Select(x => x.ValorIR), sRecIR);
            double l2_art = larguraMinColuna("ART30", result.FaturadoAReceber.Select(x => x.ValorArt30), sRecART30);
            double l2_desc = larguraMinColuna("DESC.", result.FaturadoAReceber.Select(x => x.ValorDesconto), sRecDesc);
            double l2_sinal = larguraMinColuna("SINAL", result.FaturadoAReceber.Select(x => x.ValorSinal), sRecSinal);
            double l2_ncom = larguraMinColuna("N. COMIS.", result.FaturadoAReceber.Select(x => x.ValorNaoComissionado), sRecNaoCom);
            double l2_liq = larguraMinColuna("LÍQUIDO", result.FaturadoAReceber.Select(x => x.ValorLiquido), sRecLiq);
            var larguras2 = new double[] { 45, 0, 55, l2_bruto, l2_inss, l2_iss, l2_ir, l2_art, l2_desc, l2_sinal, l2_ncom, l2_liq, 60, 60 };
            double sobra2 = cw - larguras2.Sum(); if (sobra2 < 80) sobra2 = 80;
            larguras2[1] = sobra2;
            var titulos2 = new[] { "OBRA", "CLIENTE", "NF", "BRUTO", "INSS", "ISS", "IR", "ART30", "DESC.", "SINAL", "N. COMIS.", "LÍQUIDO", "DT. FAT.", "DT. REC. PREV." };
            var alinh2 = new[] { "center", "left", "center", "right", "right", "right", "right", "right", "right", "right", "right", "right", "center", "center" };
            var linhas2 = result.FaturadoAReceber.Select(i => new[] { i.Obra.Codigo ?? "", i.Obra.Cliente.RazaoSocial ?? "", i.NumeroNF ?? "", (i.ValorBruto).ToString("C2", cultura), (i.ValorINSS).ToString("C2", cultura), (i.ValorISS).ToString("C2", cultura), (i.ValorIR).ToString("C2", cultura), (i.ValorArt30).ToString("C2", cultura), (i.ValorDesconto).ToString("C2", cultura), (i.ValorSinal).ToString("C2", cultura), (i.ValorNaoComissionado).ToString("C2", cultura), (i.ValorLiquido).ToString("C2", cultura), i.DataFaturamento.ToString("dd/MM/yyyy"), i.DataRecebimentoPrevisto.ToString("dd/MM/yyyy") }).ToList();
            var totais2 = new[] { "TOTAL", "", "", sRecBruto.ToString("C2", cultura), sRecINSS.ToString("C2", cultura), sRecISS.ToString("C2", cultura), sRecIR.ToString("C2", cultura), sRecART30.ToString("C2", cultura), sRecDesc.ToString("C2", cultura), sRecSinal.ToString("C2", cultura), sRecNaoCom.ToString("C2", cultura), sRecLiq.ToString("C2", cultura), "", "" };
            var monet2 = new[] { false, false, false, true, true, true, true, true, true, true, true, true, false, false };

            // ─── Seção 3: Faturado Recebido ────────────────────────────────────────────
            double l3_bruto = larguraMinColuna("BRUTO", result.FaturadoRecebido.Select(x => x.ValorBruto), sRcbBruto);
            double l3_inss = larguraMinColuna("INSS", result.FaturadoRecebido.Select(x => x.ValorINSS), sRcbINSS);
            double l3_iss = larguraMinColuna("ISS", result.FaturadoRecebido.Select(x => x.ValorISS), sRcbISS);
            double l3_ir = larguraMinColuna("IR", result.FaturadoRecebido.Select(x => x.ValorIR), sRcbIR);
            double l3_art = larguraMinColuna("ART30", result.FaturadoRecebido.Select(x => x.ValorArt30), sRcbART30);
            double l3_desc = larguraMinColuna("DESC.", result.FaturadoRecebido.Select(x => x.ValorDesconto), sRcbDesc);
            double l3_sinal = larguraMinColuna("SINAL", result.FaturadoRecebido.Select(x => x.ValorSinal), sRcbSinal);
            double l3_ncom = larguraMinColuna("N. COMIS.", result.FaturadoRecebido.Select(x => x.ValorNaoComissionado), sRcbNaoCom);
            double l3_liq = larguraMinColuna("LÍQUIDO", result.FaturadoRecebido.Select(x => x.ValorLiquido), sRcbLiq);
            var larguras3 = new double[] { 45, 0, 55, l3_bruto, l3_inss, l3_iss, l3_ir, l3_art, l3_desc, l3_sinal, l3_ncom, l3_liq, 60, 60 };
            double sobra3 = cw - larguras3.Sum(); if (sobra3 < 80) sobra3 = 80;
            larguras3[1] = sobra3;
            var titulos3 = new[] { "OBRA", "CLIENTE", "NF", "BRUTO", "INSS", "ISS", "IR", "ART30", "DESC.", "SINAL", "N. COMIS.", "LÍQUIDO", "DT. FAT.", "DT. REC." };
            var linhas3 = result.FaturadoRecebido.Select(i => new[] { i.Obra.Codigo ?? "", i.Obra.Cliente.RazaoSocial ?? "", i.NumeroNF ?? "", (i.ValorBruto).ToString("C2", cultura), (i.ValorINSS).ToString("C2", cultura), (i.ValorISS).ToString("C2", cultura), (i.ValorIR).ToString("C2", cultura), (i.ValorArt30).ToString("C2", cultura), (i.ValorDesconto).ToString("C2", cultura), (i.ValorSinal).ToString("C2", cultura), (i.ValorNaoComissionado).ToString("C2", cultura), (i.ValorLiquido).ToString("C2", cultura), i.DataFaturamento.ToString("dd/MM/yyyy"), i.DataRecebimentoRealizado?.ToString("dd/MM/yyyy") ?? "-" }).ToList();
            var totais3 = new[] { "TOTAL", "", "", sRcbBruto.ToString("C2", cultura), sRcbINSS.ToString("C2", cultura), sRcbISS.ToString("C2", cultura), sRcbIR.ToString("C2", cultura), sRcbART30.ToString("C2", cultura), sRcbDesc.ToString("C2", cultura), sRcbSinal.ToString("C2", cultura), sRcbNaoCom.ToString("C2", cultura), sRcbLiq.ToString("C2", cultura), "", "" };
            var monet3 = monet2;

            gfxMedidor.Dispose();
            doc.Pages.Remove(pagCapa);

            var secoes = new[]
            {
        new { Titulo = "RELATÓRIO DE FATURAMENTO (A FATURAR)", Larguras = larguras1, Titulos = titulos1, Alinh = alinh1, Linhas = linhas1, Totais = totais1, Monet = monet1, TotalBruto = sAFatValor, TotalLiq = sAFatLiq, Qtd = result.AFaturar.Count },
        new { Titulo = "RELATÓRIO DE FATURAMENTO (FATURADO A RECEBER)", Larguras = larguras2, Titulos = titulos2, Alinh = alinh2, Linhas = linhas2, Totais = totais2, Monet = monet2, TotalBruto = sRecBruto, TotalLiq = sRecLiq, Qtd = result.FaturadoAReceber.Count },
        new { Titulo = "RELATÓRIO DE FATURAMENTO (FATURADO RECEBIDO)", Larguras = larguras3, Titulos = titulos3, Alinh = alinh2, Linhas = linhas3, Totais = totais3, Monet = monet3, TotalBruto = sRcbBruto, TotalLiq = sRcbLiq, Qtd = result.FaturadoRecebido.Count }
    };

            bool primeiraSecao = true;
            foreach (var sec in secoes)
            {
                var pag = doc.AddPage();
                pag.Size = PdfSharpCore.PageSize.A4;
                pag.Orientation = PdfSharpCore.PageOrientation.Landscape;
                pw = pag.Width; ph = pag.Height;
                var g = XGraphics.FromPdfPage(pag);

                // ── BLOCO DE IDENTIFICAÇÃO (estilo Modelo 17) ──────────────────────────
                // Retângulo externo que envolve logo + título
                double blocoTopoH = 45;
                g.DrawRectangle(penPretoGrosso, margem, margem, cw, blocoTopoH);

                // Logo à esquerda dentro de uma subcélula
                double celLogoW = 90;
                g.DrawLine(penPretoGrosso, margem + celLogoW, margem, margem + celLogoW, margem + blocoTopoH);
                if (logo != null)
                {
                    double lH = blocoTopoH - 10, lW = logo.PointWidth * (lH / logo.PointHeight);
                    if (lW > celLogoW - 10) { lW = celLogoW - 10; lH = logo.PointHeight * (lW / logo.PointWidth); }
                    g.DrawImage(logo, margem + (celLogoW - lW) / 2, margem + (blocoTopoH - lH) / 2, lW, lH);
                }
                else
                {
                    g.DrawString("WH", new XFont("Arial", 18, XFontStyle.Bold), brPreto, new XRect(margem, margem, celLogoW, blocoTopoH), XStringFormats.Center);
                }

                // Título à direita do logo
                g.DrawString(sec.Titulo, fTitulo, brPreto, new XRect(margem + celLogoW, margem, cw - celLogoW, blocoTopoH), XStringFormats.Center);

                // ── LINHA DE INFORMAÇÕES (estilo "campos" do Modelo 17) ────────────────
                // Três células: EMPRESA | DATA DE EMISSÃO | REGISTROS
                double y = margem + blocoTopoH;
                double hCampo = 22;
                double c1 = cw * 0.50, c2 = cw * 0.28, c3 = cw - c1 - c2;

                // Célula 1: EMPRESA
                g.DrawRectangle(penPretoGrosso, margem, y, c1, hCampo);
                g.DrawRectangle(brCinzaLabel, margem + 0.6, y + 0.6, 75, hCampo - 1.2);
                g.DrawString("EMPRESA :", fLabel, brPreto, new XRect(margem + 5, y, 75, hCampo), XStringFormats.CenterLeft);
                g.DrawString("WH ENGENHARIA LTDA", fValorBold, brTexto, new XRect(margem + 85, y, c1 - 90, hCampo), XStringFormats.CenterLeft);

                // Célula 2: DATA EMISSÃO
                g.DrawRectangle(penPretoGrosso, margem + c1, y, c2, hCampo);
                g.DrawRectangle(brCinzaLabel, margem + c1 + 0.6, y + 0.6, 90, hCampo - 1.2);
                g.DrawString("DATA EMISSÃO :", fLabel, brPreto, new XRect(margem + c1 + 5, y, 90, hCampo), XStringFormats.CenterLeft);
                g.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm", cultura), fValor, brTexto, new XRect(margem + c1 + 100, y, c2 - 105, hCampo), XStringFormats.CenterLeft);

                // Célula 3: REGISTROS
                g.DrawRectangle(penPretoGrosso, margem + c1 + c2, y, c3, hCampo);
                g.DrawRectangle(brCinzaLabel, margem + c1 + c2 + 0.6, y + 0.6, 70, hCampo - 1.2);
                g.DrawString("REGISTROS :", fLabel, brPreto, new XRect(margem + c1 + c2 + 5, y, 70, hCampo), XStringFormats.CenterLeft);
                g.DrawString(sec.Qtd.ToString(), fValorBold, brTexto, new XRect(margem + c1 + c2 + 80, y, c3 - 85, hCampo), XStringFormats.CenterLeft);

                // Segunda linha de informações: TOTAL BRUTO | TOTAL LÍQUIDO
                y += hCampo;
                double cb1 = cw * 0.50, cb2 = cw - cb1;
                g.DrawRectangle(penPretoGrosso, margem, y, cb1, hCampo);
                g.DrawRectangle(brCinzaLabel, margem + 0.6, y + 0.6, 110, hCampo - 1.2);
                g.DrawString("TOTAL BRUTO :", fLabel, brPreto, new XRect(margem + 5, y, 110, hCampo), XStringFormats.CenterLeft);
                g.DrawString(sec.TotalBruto.ToString("C2", cultura), fValorBold, brTexto, new XRect(margem + 120, y, cb1 - 125, hCampo), XStringFormats.CenterLeft);

                g.DrawRectangle(penPretoGrosso, margem + cb1, y, cb2, hCampo);
                g.DrawRectangle(brCinzaLabel, margem + cb1 + 0.6, y + 0.6, 110, hCampo - 1.2);
                g.DrawString("TOTAL LÍQUIDO :", fLabel, brPreto, new XRect(margem + cb1 + 5, y, 110, hCampo), XStringFormats.CenterLeft);
                g.DrawString(sec.TotalLiq.ToString("C2", cultura), fValorBold, brTexto, new XRect(margem + cb1 + 120, y, cb2 - 125, hCampo), XStringFormats.CenterLeft);

                // ── TABELA ─────────────────────────────────────────────────────────────
                y = margem + topoTabela;

                // Faixa de título da tabela — estilo "CONDIÇÕES DE PAGAMENTO"
                double hFaixaSecao = 18;
                g.DrawRectangle(brCinzaCab, margem, y, cw, hFaixaSecao);
                g.DrawRectangle(penPretoGrosso, margem, y, cw, hFaixaSecao);
                g.DrawString("DETALHAMENTO", fSecao, brBranco, new XRect(margem, y, cw, hFaixaSecao), XStringFormats.Center);
                y += hFaixaSecao;

                // Cabeçalho de colunas (fundo cinza claro, texto preto)
                double xc = margem;
                for (int k = 0; k < sec.Larguras.Length; k++)
                {
                    g.DrawRectangle(brCinzaTit, xc, y, sec.Larguras[k], alturaCabTab);
                    g.DrawRectangle(penPretoFino, xc, y, sec.Larguras[k], alturaCabTab);
                    g.DrawString(sec.Titulos[k], fCabecalhoCol, brPreto, new XRect(xc, y, sec.Larguras[k], alturaCabTab), XStringFormats.Center);
                    xc += sec.Larguras[k];
                }
                y += alturaCabTab;

                for (int i = 0; i < sec.Linhas.Count; i++)
                {
                    if (y + alturaLinha > ph - reservaRodape)
                    {
                        g.Dispose();
                        pag = doc.AddPage();
                        pag.Size = PdfSharpCore.PageSize.A4;
                        pag.Orientation = PdfSharpCore.PageOrientation.Landscape;
                        pw = pag.Width; ph = pag.Height;
                        g = XGraphics.FromPdfPage(pag);

                        // Mini cabeçalho nas continuações
                        g.DrawRectangle(penPretoGrosso, margem, margem, cw, 30);
                        if (logo != null) { double lH = 22, lW = logo.PointWidth * (lH / logo.PointHeight); g.DrawImage(logo, margem + 5, margem + 4, lW, lH); }
                        g.DrawString(sec.Titulo + " (continuação)", fTitulo, brPreto, new XRect(margem + 90, margem, cw - 90, 30), XStringFormats.CenterLeft);
                        g.DrawString("Pág. contínua", fRodape, brCinzaMedio, new XRect(margem, margem, cw - 10, 30), XStringFormats.CenterRight);

                        y = margem + 40;
                        // Refaz cabeçalho de colunas
                        xc = margem;
                        for (int k = 0; k < sec.Larguras.Length; k++)
                        {
                            g.DrawRectangle(brCinzaTit, xc, y, sec.Larguras[k], alturaCabTab);
                            g.DrawRectangle(penPretoFino, xc, y, sec.Larguras[k], alturaCabTab);
                            g.DrawString(sec.Titulos[k], fCabecalhoCol, brPreto, new XRect(xc, y, sec.Larguras[k], alturaCabTab), XStringFormats.Center);
                            xc += sec.Larguras[k];
                        }
                        y += alturaCabTab;
                    }

                    var brFundo = i % 2 == 0 ? brBranco : brCinzaPar;
                    xc = margem;
                    for (int k = 0; k < sec.Larguras.Length; k++)
                    {
                        g.DrawRectangle(brFundo, xc, y, sec.Larguras[k], alturaLinha);
                        g.DrawRectangle(penPretoMuitoFino, xc, y, sec.Larguras[k], alturaLinha);
                        string txt = k < sec.Linhas[i].Length ? (sec.Linhas[i][k] ?? "") : "";
                        double dw = sec.Larguras[k] - 8;
                        bool ehMonet = k < sec.Monet.Length && sec.Monet[k];
                        if (!ehMonet && g.MeasureString(txt, fCorpo).Width > dw && txt.Length > 3) { while (txt.Length > 3 && g.MeasureString(txt + "…", fCorpo).Width > dw) txt = txt.Substring(0, txt.Length - 1); txt += "…"; }
                        string al = k < sec.Alinh.Length ? sec.Alinh[k] : "left";
                        XStringFormat fmt = al == "center" ? XStringFormats.Center : al == "right" ? XStringFormats.CenterRight : XStringFormats.CenterLeft;
                        double ox = al == "center" ? 0 : al == "right" ? -4 : 4;
                        g.DrawString(txt, fCorpo, brTexto, new XRect(xc + ox, y, sec.Larguras[k], alturaLinha), fmt);
                        xc += sec.Larguras[k];
                    }
                    y += alturaLinha;
                }

                // Linha de totais
                if (y + alturaTotal > ph - reservaRodape)
                {
                    g.Dispose();
                    pag = doc.AddPage();
                    pag.Size = PdfSharpCore.PageSize.A4;
                    pag.Orientation = PdfSharpCore.PageOrientation.Landscape;
                    pw = pag.Width; ph = pag.Height;
                    g = XGraphics.FromPdfPage(pag);
                    g.DrawRectangle(penPretoGrosso, margem, margem, cw, 30);
                    if (logo != null) { double lH = 22, lW = logo.PointWidth * (lH / logo.PointHeight); g.DrawImage(logo, margem + 5, margem + 4, lW, lH); }
                    g.DrawString(sec.Titulo + " (continuação)", fTitulo, brPreto, new XRect(margem + 90, margem, cw - 90, 30), XStringFormats.CenterLeft);
                    y = margem + 40;
                }

                xc = margem;
                for (int k = 0; k < sec.Larguras.Length; k++)
                {
                    g.DrawRectangle(brCinzaCab, xc, y, sec.Larguras[k], alturaTotal);
                    g.DrawRectangle(penPretoFino, xc, y, sec.Larguras[k], alturaTotal);
                    string txt = k < sec.Totais.Length ? (sec.Totais[k] ?? "") : "";
                    double dw = sec.Larguras[k] - 8;
                    bool ehMonet = k < sec.Monet.Length && sec.Monet[k];
                    if (!ehMonet && g.MeasureString(txt, fCorpoBold).Width > dw && txt.Length > 3) { while (txt.Length > 3 && g.MeasureString(txt + "…", fCorpoBold).Width > dw) txt = txt.Substring(0, txt.Length - 1); txt += "…"; }
                    string al = k < sec.Alinh.Length ? sec.Alinh[k] : "center";
                    XStringFormat fmt = al == "center" ? XStringFormats.Center : al == "right" ? XStringFormats.CenterRight : XStringFormats.CenterLeft;
                    double ox = al == "center" ? 0 : al == "right" ? -4 : 4;
                    g.DrawString(txt, fCorpoBold, brBranco, new XRect(xc + ox, y, sec.Larguras[k], alturaTotal), fmt);
                    xc += sec.Larguras[k];
                }

                g.Dispose();
                primeiraSecao = false;
            }

            // Rodapé formal em cada página
            int totPag = doc.Pages.Count;
            for (int i = 0; i < totPag; i++)
            {
                var p = doc.Pages[i];
                using var gr = XGraphics.FromPdfPage(p, XGraphicsPdfPageOptions.Append);
                double yr = p.Height - margem - 3;
                gr.DrawLine(penPretoFino, margem, yr - 12, p.Width - margem, yr - 12);
                gr.DrawString("WH ENGENHARIA LTDA — Controle de Faturamento", fRodape, brPreto, new XPoint(margem, yr));
                gr.DrawString("Documento confidencial — Uso interno", fRodape, brCinzaMedio, new XRect(margem, yr - 10, p.Width - 2 * margem, 10), XStringFormats.Center);
                string pgStr = $"Página {i + 1} / {totPag}";
                var szPg = gr.MeasureString(pgStr, fRodape);
                gr.DrawString(pgStr, fRodape, brPreto, new XPoint(p.Width - margem - szPg.Width, yr));
            }

            using var ms = new MemoryStream();
            doc.Save(ms, false);
            return ms.ToArray();
        }
        public List<Agenda_CarimboResultDTO> ObtemNotasComCarimbo(Agenda_ExcelCarimboRequestDTO parametros)
        {
            return _repositorioAgenda.ObtemNotasComCarimbo(parametros);
        }

        public async Task<Tuple<MemoryStream, string>> ObtemNotaFiscalComCarimbo(Int64 idPedidoCompraNotaFiscal, string diretorio)
        {

            var pedidoCompraNotaFiscal = await _repositorioPedidoCompraNotaFiscal.Get(idPedidoCompraNotaFiscal);

            var arquivo = await _repositorioPedidoCompra.GetFile(pedidoCompraNotaFiscal.IdPedidoCompraArquivo);

            var filePath = Path.Combine(diretorio, arquivo.NomeLogico);

            var document = PdfReader.Open(File.OpenRead(filePath), PdfDocumentOpenMode.Modify);

            // Detecta orientação da última página
            var ultimaPagina = document.Pages[document.Pages.Count - 1];

            // Cria nova página com mesma orientação
            var page = document.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;
            page.Orientation = ultimaPagina.Orientation;

            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 14, XFontStyle.Bold);
            var brush = XBrushes.Red;

            var carimbo = await _repositorioPedidoCompra.ObtemNotaFiscalPagamento(pedidoCompraNotaFiscal.Id);

            var valorLiquido = carimbo.ValorBruto - carimbo.ValorIR - carimbo.ValorISS - carimbo.ValorINSS - carimbo.ValorArt30;

            var linhas = new List<string>();
            linhas.Add($"Valor bruto: R$ {carimbo.ValorBruto?.ToString("N2", CultureInfo.CurrentCulture) ?? ""}");
            linhas.Add($"IR - Valor: R$ {carimbo.ValorIR?.ToString("N2", CultureInfo.CurrentCulture) ?? ""} - Pagamento: {carimbo.DataPagamentoIR?.ToString("dd/MM/yyyy") ?? ""}");
            linhas.Add($"ISS - Valor: R$ {carimbo.ValorISS?.ToString("N2", CultureInfo.CurrentCulture) ?? ""} - Pagamento: {carimbo.DataPagamentoISS?.ToString("dd/MM/yyyy") ?? ""}");
            linhas.Add($"INSS - Valor: R$ {carimbo.ValorINSS?.ToString("N2", CultureInfo.CurrentCulture) ?? ""} - Pagamento: {carimbo.DataPagamentoINSS?.ToString("dd/MM/yyyy") ?? ""}");
            linhas.Add($"ART30 - Valor: R$ {carimbo.ValorArt30?.ToString("N2", CultureInfo.CurrentCulture) ?? ""} - Pagamento: {carimbo.DataPagamentoArt30?.ToString("dd/MM/yyyy") ?? ""}");
            linhas.Add($"Valor liquido: R$ {valorLiquido?.ToString("N2", CultureInfo.CurrentCulture) ?? ""}");
            linhas.Insert(0, "CARIMBO FISCAL");

            double boxWidth = 400;
            double boxHeight = 200;

            double startX = (page.Width - boxWidth) / 2;
            double startY = (page.Height - boxHeight) / 2;

            // Desenha retângulo com traço pequeno
            var pen = new XPen(XColors.Red, 2)
            {
                DashStyle = XDashStyle.Solid,
            };
            gfx.DrawRectangle(pen, startX, startY, boxWidth, boxHeight);

            double lineHeight = font.GetHeight() + 10;
            for (int i = 0; i < linhas.Count; i++)
            {
                var text = linhas[i];
                double y = startY + 10 + i * lineHeight;

                gfx.DrawString(text, font, brush, new XRect(startX, y, boxWidth, lineHeight), XStringFormats.Center);
            }

            using var outStream = new MemoryStream();
            document.Save(outStream, false);
            outStream.Position = 0;

            return new Tuple<MemoryStream, string>(outStream, arquivo.Nome);
        }


        public async Task<List<Int64>> ObtemIdsPedidosComNotasComCarimbos(Agenda_ExcelCarimboRequestDTO parametros)
        {
            return await _repositorioAgenda.ObtemIdsPedidosComNotasComCarimbos(parametros);
        }

        public async Task AjusteManual(Int64 idFluxoCaixa, List<Agenda_AjusteManualRequestDTO> parametros)
        {
            await _repositorioAgenda.AjusteManual(idFluxoCaixa, parametros);
        }

    }
}