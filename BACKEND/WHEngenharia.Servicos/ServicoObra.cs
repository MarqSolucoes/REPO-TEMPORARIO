using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Obra;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoObra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioObra _repositorioObra;

        public ServicoObra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioObra = new RepositorioObra(_context, _mapper);
        }

        public async Task<List<ObraDTO>> Get(Int64 idUsuario, bool apenasAtivos = false)
        {
            return await _repositorioObra.Get(idUsuario, apenasAtivos);
        }

        public async Task<ObraDTO> Get(Int64 Id)
        {
            var obra = await _repositorioObra.Get(Id);
            obra.Faturamentos = obra.Faturamentos.OrderBy(x => x.Data).ToList();
            obra.ETOs = obra.ETOs.OrderBy(x => x.Data).ToList();
            //obra.SaldoETO = obra.ControleCusto.Sum(x => x.ValorPrevisto.Value);
            //obra.SaldoETOAjustado = obra.ValorCusto - obra.ControleCusto.Sum(x => x.ValorPrevistoAjustado.Value);
            //obra.SaldoMedicao = obra.ValorTotal - obra.Medicoes.Where(x => x.ValorPrevisto.HasValue).Sum(x => x.ValorPrevisto.Value);
            //obra.SaldoMedicaoAjustado = obra.ValorTotal - obra.Medicoes.Where(x => x.ValorPrevisto.HasValue).Sum(x => x.ValorPrevistoAjustado.Value);
            return obra;
        }

        public async Task<ObraDTO> Post(ObraDTO obraDTO)
        {
            if (string.IsNullOrEmpty(obraDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descricao' deve ser informado");

            return await _repositorioObra.Post(obraDTO);
        }

        public async Task<ObraDTO> Put(ObraDTO obraDTO)
        {
            if (string.IsNullOrEmpty(obraDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descricao' deve ser informado");

            return await _repositorioObra.Put(obraDTO);
        }

        public async Task BloquearDesbloquear(Int64 idObra, Int64 idUsuario, bool status)
        {
            await _repositorioObra.BloquearDesbloquear(idObra, idUsuario, status);
        }

        public async Task CalculaCustoObra(Int64 idObra)
        {
            await _repositorioObra.CalculaCustoObra(idObra);
        }

        public async Task PostMedicoes(List<ObraFaturamentoDTO> medicoesDTO)
        {
            await _repositorioObra.PostMedicoes(medicoesDTO);
        }

        public async Task PostETOs(List<ObraETODTO> etosDTO)
        {
            await _repositorioObra.PostETOs(etosDTO);
        }

        public async Task<List<Obra_ItemContaCorrenteDTO>> ObtemDadosContaCorrente(Int64 IdObra)
        {
            return await _repositorioObra.ObtemDadosContaCorrente(IdObra);
        }

        public async Task<List<Obra_ItemContaCorrenteDTO>> ObtemDadosETO(Int64 IdObra)
        {
            return await _repositorioObra.ObtemDadosETO(IdObra);
        }

        public async Task<List<Obra_ItemResumoETODTO>> ObtemDadosResumoETO()
        {
            return await _repositorioObra.ObtemDadosResumoETO();
        }

        private void AdicionaCabecalhoResumoETO(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 5);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"RESUMO CONTROLE DE ETO - OBRAS - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 3;


            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.07) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.32) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.32) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.67) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.67) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.77) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.77) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.87) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.87) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"OBRA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"NOME", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.07), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"ESCOPO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.32), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"ETO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.67), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"GASTO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.77), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"SALDO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.87), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }

        private void AdicionaCabecalhoContaCorrente(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath, ObraDTO obraDTO)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 5);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"CONTA CORRENTE - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 3;

            graphics.DrawString($"OBRA: {obraDTO.Codigo} {obraDTO.Cliente?.NomeFantasia ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"PAGAMENTOS", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha + 35);
            graphics.DrawString($"RECEBIMENTOS", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.09) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.09) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.17) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.17) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.24) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.24) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.34) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.34) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.5) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.6) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.67) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.67) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.74) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.74) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"O.C.", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"N. FISCAL", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.09), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"DATA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.17), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"VALOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.24), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FORNECEDOR/CLIENTE", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.34), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"VALOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.5), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"FATURA.", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.6), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"RECEB. P", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.67), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"SALDO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.74), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }

        private void AdicionaCabecalhoETOObra(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, XFont fonteTitulo, XFont fonteNormal, string logoPath, ObraDTO obraDTO)
        {
            XImage image = XImage.FromFile(logoPath);
            double x = page.Width - image.PointWidth - 40;
            double y = margemSuperior + 5;
            graphics.DrawImage(image, x, y);

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, margemSuperior - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 9);

            graphics.DrawString("WH ENGENHARIA LTDA", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"ETO - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fonteNormal, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 2;

            graphics.DrawString($"Cliente: {obraDTO.Cliente?.NomeFantasia ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Prazo (dias): {obraDTO.PrazoDias}", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.7), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Obra Nº:", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.82), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"{obraDTO.Codigo}", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Red, new XRect((page.Width.Value * 0.87), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Escopo dos serviços: {obraDTO.Descricao}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Início: {obraDTO.DataInicio.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.7), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Fim: {obraDTO.DataFim.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.82), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Custo orçado: {string.Format("{0:C}", obraDTO.ETO)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Proposta Nº: {obraDTO.CodigoProposta}", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.7), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"Responsável: {string.Join(',', obraDTO.UsuariosAprovadores.Select(x => x.Usuario.Nome).ToList())}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Data proposta: {obraDTO.DataProposta.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect((page.Width.Value * 0.7), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.1) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.1) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.2) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.2) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.3) - 2, posicaoYLinha + 35);
            graphics.DrawLine(XPens.Black, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (page.Width.Value * 0.8) - 2, posicaoYLinha + 35);
            posicaoYLinha += 6;

            graphics.DrawString($"DATA", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"OC/PI", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.1), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"VALOR", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.2), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"BENEFICIÁRIO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.3), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"SALDO", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (page.Width.Value * 0.80), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento + 6;

            graphics.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, page.Width - margemEsquerda, posicaoYLinha);
            posicaoYLinha += 10;
        }

        public async Task<byte[]> GeraPDFContaCorrente(string logoPath, Int64 idObra)
        {
            var numeroItensPorPagina = 30;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var obraDTO = Get(idObra).Result;

            var valores = ObtemDadosContaCorrente(idObra).Result;

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalhoContaCorrente(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, obraDTO);

            var saldo = 0.0;
            var count = 0;

            valores.ForEach(item =>
            {
                if (!item.Credito)
                {
                    paginaGrafico.DrawString($"{(item.OrdemCompra.Contains('.') ? item.OrdemCompra : item.OrdemCompra)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.NotaFiscal ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.09), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.Data?.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.17), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.24), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{(item.Fornecedor != null && item.Fornecedor.Length > 25 ? (item.Fornecedor.Substring(0, 25) + "...") : item.Fornecedor)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.34), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                    saldo -= item.Valor;
                }
                else
                {
                    //paginaGrafico.DrawString($"{item.NotaFiscal ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.09), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.Fornecedor ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.34), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.Data?.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.6), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    paginaGrafico.DrawString($"{item.DataRecebimentoPrevisto.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.67), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                    saldo += item.Valor;
                }

                paginaGrafico.DrawString($"{saldo.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.74), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.09) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.09) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.17) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.17) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.24) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.24) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.34) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.34) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.5) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.67) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.67) - 2, posicaoYLinha + 18);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.74) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.74) - 2, posicaoYLinha + 18);
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

                    AdicionaCabecalhoContaCorrente(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, obraDTO);

                    count = 0;
                }
            });

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda, posicaoYLinha, pagina.Width - margemEsquerda, posicaoYLinha);
            paginaGrafico.DrawString($"{valores.Where(x => !x.Credito).Sum(x => x.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.24), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{valores.Where(x => x.Credito).Sum(x => x.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

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

        public async Task<byte[]> GeraPDFETOObra(string logoPath, Int64 idObra)
        {
            var numeroItensPorPagina = 30;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var obraDTO = Get(idObra).Result;

            var valores = ObtemDadosETO(idObra).Result;

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalhoETOObra(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, obraDTO);

            var saldo = obraDTO.ETO;
            var count = 0;

            paginaGrafico.DrawString($"{obraDTO.ETO.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha + 35);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha + 35);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha + 35);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 35);
            posicaoYLinha += espacamento;

            count++;

            valores.ToList().ForEach(item =>
              {
                  if (item.Credito)
                      saldo += item.Valor;
                  else
                      saldo -= item.Valor;

                  paginaGrafico.DrawString($"{item.Data?.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                  paginaGrafico.DrawString($"{(string.IsNullOrEmpty(item.NotaFiscal) ? item.OrdemCompra : item.NotaFiscal)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.1), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                  paginaGrafico.DrawString($"{item.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.2), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                  paginaGrafico.DrawString($"{item.Fornecedor}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.3), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                  paginaGrafico.DrawString($"{saldo.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.8), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                  paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.1) - 2, posicaoYLinha + 35);
                  paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha + 35);
                  paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.3) - 2, posicaoYLinha + 35);
                  paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.8) - 2, posicaoYLinha + 35);
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

                      AdicionaCabecalhoETOObra(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath, obraDTO);

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

        public async Task<byte[]> GeraPDFResumoETO(string logoPath)
        {
            var numeroItensPorPagina = 40;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;

            var obras = await ObtemDadosResumoETO();

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);
            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalhoResumoETO(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, fonteTitulo, fonteNormal, logoPath);

            obras.ForEach(obra =>
            {
                paginaGrafico.DrawString($"{obra.CodigoObra}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(obra.Cliente.Length > 33 ? obra.Cliente.Substring(0, 33) + "..." : obra.Cliente)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.07), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{(obra.Escopo.Length > 45 ? obra.Escopo.Substring(0, 45) + "..." : obra.Escopo)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.32), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{obra.ETO.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.67), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{obra.Gasto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.77), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{obra.Saldo.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.87), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);


                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.07) - 2, posicaoYLinha + 35);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.32) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.32) - 2, posicaoYLinha + 35);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.67) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.67) - 2, posicaoYLinha + 35);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.77) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.77) - 2, posicaoYLinha + 35);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.87) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.87) - 2, posicaoYLinha + 35);

                posicaoYLinha += espacamento;


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

        public MemoryStream MontaExcelAjusteETO(bool incluirDatasComValores = true)
        {
            var obras = ObtemETOsAjuste();

            var datas = obras
                .SelectMany(o => o.ETO)
                .Select(t => t.Item1.Date)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("AjusteETO");

            // Cabeçalho fixo
            string[] headers = { "Obra", "Local", "ETO", "Gasto", "Saldo", "Status" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            int coluna = headers.Length + 1;

            if (incluirDatasComValores)
            {
                // Datas como cabeçalhos
                foreach (var data in datas)
                {
                    var cell = worksheet.Cell(1, coluna++);
                    cell.Value = data;
                    cell.Style.DateFormat.Format = "dd-MM-yyyy";
                }
            }
            else
            {
                datas = new List<DateTime>();
                var hoje = DateTime.Today;

                for (int i = 0; i < 6; i++)
                {
                    // Avança i meses a partir do mês atual
                    DateTime dataBase = hoje.AddMonths(i);
                    int ano = dataBase.Year;
                    int mes = dataBase.Month;

                    // Dias fixos
                    datas.Add(new DateTime(ano, mes, 10));
                    datas.Add(new DateTime(ano, mes, 20));

                    // Dia 30, ou o último dia do mês caso não haja dia 30
                    int ultimoDiaDoMes = DateTime.DaysInMonth(ano, mes);
                    int dia30 = Math.Min(30, ultimoDiaDoMes);
                    datas.Add(new DateTime(ano, mes, dia30));
                }

                foreach (var data in datas)
                {
                    var cell = worksheet.Cell(1, coluna++);
                    cell.Value = data;
                    cell.Style.DateFormat.Format = "dd-MM-yyyy";
                }
            }

                // Colunas finais
            worksheet.Cell(1, coluna++).Value = "Total";
            worksheet.Cell(1, coluna++).Value = "Saldo";
            worksheet.Cell(1, coluna++).Value = "Redução";

            // Estilo do cabeçalho
            for (int i = 1; i < coluna; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                cell.Style.Font.Bold = true;

                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.Black;
            }

            int linha = 0;
            // Dados
            for (int row = 0; row < obras.Count; row++)
            {
                linha = row + 2;
                var obra = obras[row];

                worksheet.Cell(linha, 1).Value = obra.CodigoObra;
                worksheet.Cell(linha, 2).Value = obra.Cliente;

                worksheet.Cell(linha, 3).Value = obra.Custo;
                worksheet.Cell(linha, 3).Style.NumberFormat.Format = "R$ #,##0.00";

                worksheet.Cell(linha, 4).Value = obra.Gasto;
                worksheet.Cell(linha, 4).Style.NumberFormat.Format = "R$ #,##0.00";

                worksheet.Cell(linha, 5).Value = obra.Saldo;
                worksheet.Cell(linha, 5).Style.NumberFormat.Format = "R$ #,##0.00";

                worksheet.Cell(linha, 6).Value = obra.Status;

                int col = 7;

                if (incluirDatasComValores)
                {
                    foreach (var data in datas)
                    {
                        var valor = obra.ETO.FirstOrDefault(y => y.Item1.Date == data.Date)?.Item2;

                        if (valor.HasValue)
                        {
                            worksheet.Cell(linha, col).Value = valor.Value;
                            worksheet.Cell(linha, col).Style.NumberFormat.Format = "R$ #,##0.00";
                        }

                        col++;
                    }
                }
                else
                {
                    for (int i = 0; i < datas.Count; i++)
                    {
                        worksheet.Cell(linha, col).Style.NumberFormat.Format = "R$ #,##0.00";

                        col ++;
                    }
                }
                // Colunas Total, Saldo, Redução
                
                worksheet.Cell(linha, col).FormulaA1 = $"=SUM(G{linha}:X{linha})";
                worksheet.Cell(linha, col).Style.NumberFormat.Format = "R$ #,##0.00";
                //worksheet.Cell(linha, col).Value = "0";

                col++;
                worksheet.Cell(linha, col).FormulaA1 = $"=E{linha}-Y{linha}";
                worksheet.Cell(linha, col).Style.NumberFormat.Format = "R$ #,##0.00";

                col++;
                worksheet.Cell(linha, col).Style.NumberFormat.Format = "R$ #,##0.00";

                col++;

                // Estilo da linha
                for (int i = 1; i < col; i++)
                {
                    var cell = worksheet.Cell(linha, i);
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.Black;
                }
            }

            linha = obras.Count + 2;

            //Total ETO
            worksheet.Cell(linha, 3).FormulaA1 = $"=SUM(C2:C{linha-1})";
            worksheet.Cell(linha, 3).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 3).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 3).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 3).Style.Font.Bold = true;

            //Total GASTO
            worksheet.Cell(linha, 4).FormulaA1 = $"=SUM(D2:D{linha - 1})";
            worksheet.Cell(linha, 4).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 4).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 4).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 4).Style.Font.Bold = true;

            //Total SALDO
            worksheet.Cell(linha, 5).FormulaA1 = $"=SUM(E2:E{linha - 1})";
            worksheet.Cell(linha, 5).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 5).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 5).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 5).Style.Font.Bold = true;

            coluna = 7;
            foreach (var data in datas)
            {
                worksheet.Cell(linha, coluna).FormulaA1 = $"=SUM({(char)('A' + coluna-1)}2:{(char)('A' + coluna-1)}{linha - 1})";
                worksheet.Cell(linha, coluna).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(linha, coluna).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                worksheet.Cell(linha, coluna).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(linha, coluna).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(linha, coluna).Style.Font.Bold = true;

                coluna++;
            }

            //TOTAL
            worksheet.Cell(linha, coluna).FormulaA1 = $"=SUM({(char)('A' + coluna - 1)}2:{(char)('A' + coluna - 1)}{linha - 1})";
            worksheet.Cell(linha, coluna).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, coluna).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, coluna).Style.Font.Bold = true;
            coluna++;

            //Total SALDO
            worksheet.Cell(linha, coluna).FormulaA1 = $"=SUM({(char)('A' + coluna - 1)}2:{(char)('A' + coluna - 1)}{linha - 1})";
            worksheet.Cell(linha, coluna).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, coluna).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, coluna).Style.Font.Bold = true;
            coluna++;

            //Total REDUCÃO
            worksheet.Cell(linha, coluna).FormulaA1 = $"=SUM(AA2:AA{linha - 1})";
            worksheet.Cell(linha, coluna).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, coluna).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, coluna).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, coluna).Style.Font.Bold = true;

            linha++;

            worksheet.Cell(linha, 8).Value = datas[2].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 8).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 8).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 8).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 8).Style.Font.Bold = true;
            worksheet.Cell(linha, 9).FormulaA1 = $"=SUM(G{linha - 1} + H{linha - 1} + I{linha - 1})";
            worksheet.Cell(linha, 9).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 9).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 9).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 9).Style.Font.Bold = true;

            worksheet.Cell(linha, 11).Value = datas[5].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 11).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 11).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 11).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 11).Style.Font.Bold = true;
            worksheet.Cell(linha, 12).FormulaA1 = $"=SUM(J{linha - 1} + K{linha - 1} + L{linha - 1})";
            worksheet.Cell(linha, 12).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 12).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 12).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 12).Style.Font.Bold = true;

            worksheet.Cell(linha, 14).Value = datas[8].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 14).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 14).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 14).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 14).Style.Font.Bold = true;
            worksheet.Cell(linha, 15).FormulaA1 = $"=SUM(M{linha - 1} + N{linha - 1} + O{linha - 1})";
            worksheet.Cell(linha, 15).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 15).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 15).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 15).Style.Font.Bold = true;

            worksheet.Cell(linha, 17).Value = datas[11].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 17).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 17).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 17).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 17).Style.Font.Bold = true;
            worksheet.Cell(linha, 18).FormulaA1 = $"=SUM(P{linha - 1} + Q{linha - 1} + R{linha - 1})";
            worksheet.Cell(linha, 18).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 18).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 18).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 18).Style.Font.Bold = true;

            worksheet.Cell(linha, 20).Value = datas[14].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 20).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 20).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 20).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 20).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 20).Style.Font.Bold = true;
            worksheet.Cell(linha, 21).FormulaA1 = $"=SUM(S{linha - 1} + T{linha - 1} + U{linha - 1})";
            worksheet.Cell(linha, 21).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 21).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 21).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 21).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 21).Style.Font.Bold = true;

            worksheet.Cell(linha, 23).Value = datas[17].ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")).ToUpper();
            worksheet.Cell(linha, 23).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 23).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 23).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 23).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 23).Style.Font.Bold = true;
            worksheet.Cell(linha, 24).FormulaA1 = $"=SUM(V{linha - 1} + W{linha - 1} + X{linha - 1})";
            worksheet.Cell(linha, 24).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha, 24).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            worksheet.Cell(linha, 24).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(linha, 24).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(linha, 24).Style.Font.Bold = true;


            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        public List<Obra_AjusteETOResultDTO> ObtemETOsAjuste()
        {
            return _repositorioObra.ObtemETOsAjuste();
        }

        public void AtualizaPrevisoesETO(Obra_AjusteETODTO previsao, Int64 IdUsuario)
        {
            _repositorioObra.AtualizaPrevisoesETO(previsao, IdUsuario);
        }

        public void SalvaAjuste(ObraAjusteDTO ajusteDTO)
        {
            _repositorioObra.SalvaAjuste(ajusteDTO);
        }

        public async Task<List<ObraAjusteDTO>> ObtemAjustes()
        {
            return await _repositorioObra.ObtemAjustes();
        }

        public async Task<string> GetFile(Int64 id, int tipo)
        {
            return await _repositorioObra.GetFile(id, tipo);
        }

        public async Task Cancelar(Int64 idObra)
        {
            await _repositorioObra.Cancelar(idObra);
        }
    }
}
