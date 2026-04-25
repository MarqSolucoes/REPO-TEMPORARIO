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
using WHEngenharia.Dominio.Modelos.Genericos.Relatorio;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoRelatorioETO
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioRelatorioETO _repositorioRelatorioETO;

        public ServicoRelatorioETO(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioRelatorioETO = new RepositorioRelatorioETO(_context, _mapper);
        }

        public async Task<List<Relatorio_ControleEtoDTO>> Get(bool obraBloqueada)
        {
            return await _repositorioRelatorioETO.Get(obraBloqueada);
        }

        public async Task<List<Relatorio_ControleEtoProximasDatasDTO>> GetProximasDatasAjuste(int idObra)
        {
            var result = new List<Relatorio_ControleEtoProximasDatasDTO>();

            for (int i = 1; i <= 6; i++)
            {
                var diaDezProximoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 10).AddMonths(i);
                var diaVinteProximoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 20).AddMonths(i);
                var ultimoDiaProximoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(i + 1).AddDays(-1);

                while (ultimoDiaProximoMes.Day > 30)
                    ultimoDiaProximoMes = ultimoDiaProximoMes.AddDays(-1);

                result.Add(new Relatorio_ControleEtoProximasDatasDTO() { Data = diaDezProximoMes, Descricao = $"{diaDezProximoMes.ToString("dd/MM/yyyy")}", Id = result.Count, Valor = 0, IdObraMedicao = 0 });
                result.Add(new Relatorio_ControleEtoProximasDatasDTO() { Data = diaVinteProximoMes, Descricao = $"{diaVinteProximoMes.ToString("dd/MM/yyyy")}", Id = result.Count, Valor = 0, IdObraMedicao = 0 });
                result.Add(new Relatorio_ControleEtoProximasDatasDTO() { Data = ultimoDiaProximoMes, Descricao = $"{ultimoDiaProximoMes.ToString("dd/MM/yyyy")}", Id = result.Count, Valor = 0, IdObraMedicao = 0 });
            }

           (await _repositorioRelatorioETO.ObtemAjustes(idObra)).ForEach(x =>
           {
               if (result.Any(y => y.Data == x.Data))
                   result.FirstOrDefault(y => y.Data == x.Data).Valor = x.Valor;
               else
                   result.Add(new Relatorio_ControleEtoProximasDatasDTO() { Data = x.Data, Descricao = $"{x.Data.ToString("dd/MM/yyyy")}", Id = result.Count, Valor = x.Valor, IdObraMedicao = x.Id });
           });

            result = result.OrderBy(x => x.Data).ToList();

            return result;
        }

        //public async Task<bool> Ajuste(Relatorio_ControleEtoAjusteDTO relatorioControleEtoAjusteDTO)
        //{
        //    return await _repositorioRelatorioETO.Ajuste(relatorioControleEtoAjusteDTO);
        //}

        public async Task<bool> VisualizarComentario(Int64 idAjuste)
        {
            return await _repositorioRelatorioETO.VisualizarComentario(idAjuste);
        }

        public async Task<List<ObraFaturamentoDTO>> ObtemAjustes(List<Int64> idsObras)
        {
            return await _repositorioRelatorioETO.ObtemAjustes(idsObras);
        }
        public async Task<byte[]> GeraPdf(string logoPath)
        {
            var numeroItensPorPagina = 33;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var dadosETO = await Get(true);

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);

            var count = 0;
            dadosETO.ForEach(x =>
            {
                paginaGrafico.DrawString($"{x.Codigo}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(x.Cliente.Length > 30 ? $"{x.Cliente.Substring(0, 30)}..." : x.Cliente)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.07), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(x.Descricao.Length > 30 ? $"{x.Descricao.Substring(0, 30)}..." : x.Descricao)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.3), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{x.Custo.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.55), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{x.Gasto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.70), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{x.Saldo.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "").Replace("$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.85), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha + 25);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha + 25);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.55) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.55) - 2, posicaoYLinha + 25);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.70) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.70) - 2, posicaoYLinha + 25);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.85) - 2, posicaoYLinha + 25);

                posicaoYLinha += espacamento;

                count++;

                if (count % numeroItensPorPagina == 0)
                {
                    count = 0;
                    pagina.Close();
                    paginaGrafico.Dispose();

                    pagina = documentoPdf.AddPage();
                    pagina.Size = PdfSharpCore.PageSize.A4;
                    pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

                    paginaGrafico = XGraphics.FromPdfPage(pagina);

                    posicaoYLinha = margemSuperior;

                    AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);
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

        private void AdicionaCabecalho(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior;
            graphics.DrawImage(image, x, y);


            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString("Resumo Controle de ETO - Obras", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;
            posicaoYLinha += 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.55) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.55) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.70) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.70) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.85) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"Obra", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Nome", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.07), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Escopo", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.3), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"ETO Obra", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.55), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Gasto", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.70), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Saldo ETO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.85), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }
    }
}
