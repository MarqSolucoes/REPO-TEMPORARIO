using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoInterno;
using WHEngenharia.Servicos.Auxiliares;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoPedidoInterno
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioPedidoInterno _repositorioPedidoInterno;

        public ServicoPedidoInterno(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioPedidoInterno = new RepositorioPedidoInterno(_context, _mapper);
        }

        public async Task<List<PedidoInternoDTO>> Get()
        {
            return await _repositorioPedidoInterno.Get();
        }

        public async Task<List<PedidoInternoDTO>> ObtemPedidosInternosParaAprovacao(Int64 idUsuario)
        {
            return await _repositorioPedidoInterno.ObtemPedidosInternosParaAprovacao(idUsuario);
        }

        public async Task AprovarReprovarPedidoInterno( Int64 idPedidoInterno,  bool valor)
        {
            await _repositorioPedidoInterno.AprovarReprovarPedidoInterno(idPedidoInterno, valor);
        }

        public async Task<PedidoInternoDTO> Get(Int64 Id)
        {
            return await _repositorioPedidoInterno.Get(Id);
        }

        public async Task<List<PedidoInternoDTO>> GetByUser(Int64 idUsuario)
        {
            return await _repositorioPedidoInterno.GetByUser(idUsuario);
        }

        public async Task<PedidoInternoDTO> Post(PedidoInternoDTO pedidoInternoDTO)
        {
            if (string.IsNullOrEmpty(pedidoInternoDTO.Descricao.Trim()))
                throw new Exception("O campo 'Descrição' deve ser informado");

            //if (pedidoInternoDTO.Parcelas.Any(x => x.ParcelaDEFs.Count == 0))
            //    throw new Exception("Informe corretamente os DEFs para as parcelas");

            return await _repositorioPedidoInterno.Post(pedidoInternoDTO);
        }

        public async Task<PedidoInternoDTO> Put(PedidoInternoDTO pedidoInternoDTO)
        {
            if (string.IsNullOrEmpty(pedidoInternoDTO.Descricao.Trim()))
                throw new Exception("O campo'Descrição' deve ser informado");

            return await _repositorioPedidoInterno.Put(pedidoInternoDTO);
        }

        public async Task DefinirParcelaPaga(Int64 idParcela)
        {
            await _repositorioPedidoInterno.DefinirParcelaPaga(idParcela);
        }

        public async Task AlterarParcela(PedidoInterno_AlteracaoParcelaDTO pedidoInternoAlteracaoDataPagamentoDTO)
        {
            await _repositorioPedidoInterno.AlterarParcela(pedidoInternoAlteracaoDataPagamentoDTO);
        }

        public async Task<string> ProximoCodigo()
        {
            return await _repositorioPedidoInterno.ProximoCodigo();
        }

        public async Task<PedidoInternoArquivosDTO> Upload(PedidoInternoArquivosDTO pedidoInternoArquivosDTO)
        {
            return await _repositorioPedidoInterno.Upload(pedidoInternoArquivosDTO);
        }

        public async Task<PedidoInternoArquivosDTO> GetFile(Int64 id)
        {
            return await _repositorioPedidoInterno.GetFile(id);
        }

        public async Task DeleteArquivo(int idArquivo)
        {
            await _repositorioPedidoInterno.DeleteArquivo(idArquivo);
        }

        public async Task<List<PedidoInternoArquivosDTO>> ObtemArquivos(Int64 idPedidoInterno)
        {
            return await _repositorioPedidoInterno.ObtemArquivos(idPedidoInterno);
        }

        private void AdicionaCabecalho(ref XGraphics graphics, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage page, ref PdfDocument doc, PedidoInternoDTO pedidoInternoDTO, XFont fonteTitulo, XFont fonteNormal, string logoPath)
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

            graphics.DrawString($"Tel: (11) 5904-0522   CNPJ: 62.534.060/0001-41", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento;

            graphics.DrawString($"IE: 110.191.500.117", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 2);
            graphics.DrawString($"Data Solicitação: {pedidoInternoDTO.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Solicitado por: {pedidoInternoDTO.UsuarioCadastro?.Nome ?? ""}", fonteNormal, XBrushes.Black, new XRect((margemEsquerda + (page.Width.Value * 0.40)), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Obra/DEF: {pedidoInternoDTO.Obra?.Codigo ?? pedidoInternoDTO.DEF?.Codigo??""}", fonteNormal, XBrushes.Black, new XRect((margemEsquerda + (page.Width.Value * 0.70)), posicaoYLinha, page.Width, page.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            graphics.DrawRectangle(XPens.Black, margemEsquerda - 5, posicaoYLinha - 5, page.Width - (2 * margemEsquerda) + 10, espacamento * 2);
            graphics.DrawString($"PEDIDO INTERNO: {pedidoInternoDTO.CodigoFormatado}", fonteTitulo, XBrushes.Black, new XRect(0, posicaoYLinha, page.Width, page.Height), XStringFormats.TopCenter);
            posicaoYLinha += espacamento * 3;       
        }

        private void AdicionaRodape(ref XGraphics paginaGrafico, ref int posicaoYLinha, ref int espacamento, ref int margemEsquerda, ref int margemSuperior, ref PdfPage pagina, ref PdfDocument doc, PedidoInternoDTO pedidoInternoDTO, XFont fonteTitulo, XFont fonteNormal, XFont fonteCaixaAlta)
        {
            paginaGrafico.DrawString($"{pedidoInternoDTO.UsuarioCadastro?.Nome ?? ""}", new XFont("Segoe Script", 18, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"{(pedidoInternoDTO.DataAprovacao.HasValue ? (pedidoInternoDTO.UsuarioAprovacao?.Nome ?? "") : "")}", new XFont("Segoe Script", 18, XFontStyle.Regular), XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

            posicaoYLinha += espacamento + 10;

            paginaGrafico.DrawString($"Solicitado em {pedidoInternoDTO.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss") ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            paginaGrafico.DrawString($"Aprovado em {pedidoInternoDTO.DataAprovacao?.ToString("dd/MM/yyyy HH:mm:ss") ?? ""}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.5), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
        }

        public async Task<byte[]> GeraPdfPedidoCompra(Int64 id, string logoPath)
        {
            var numeroMaximoLinhasPorPagina = 50;

            var margemEsquerda = 30;
            var margemSuperior = 30;

            var espacamento = 12;
            var posicaoYLinha = margemSuperior;
            var quantidadeDeCaracteresNaDiscriminacao = 110;

            var pedidoInternoDTO = await Get(id);

            var documentoPdf = new PdfDocument();
            var pagina = documentoPdf.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;

            var paginaGrafico = XGraphics.FromPdfPage(pagina);

            XFont fonteTitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont fonteNormal = new XFont("Arial", 10, XFontStyle.Regular);
            XFont fonteCaixaAlta = new XFont("Arial", 8, XFontStyle.Regular);

            AdicionaCabecalho(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoInternoDTO, fonteTitulo, fonteNormal, logoPath);

            var numeroDeLinhasInseridas = 0;
            var numeroDeMateriaisInseridos = 0;
            var item = 0;
            double valorParcial = 0;

            paginaGrafico.DrawString($"DESCRIÇÃO DO PEDIDO", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento*2;

            var descricaoList = pedidoInternoDTO.Descricao.Split('\n').ToList();
            var numeroDescricoesInseridas = 1;

            descricaoList.ForEach(descricao =>
            {
                while (descricao.Length > quantidadeDeCaracteresNaDiscriminacao)
                {
                    //Obtem a posicao do ultimo espaço " " antes do 100
                    var posicao = ServicoString.RetornaPosicaoEspacoMaisProximo(descricao, quantidadeDeCaracteresNaDiscriminacao);

                    paginaGrafico.DrawString($"{descricao.TrimStart().Substring(0, posicao)}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                    descricao = descricao.Remove(0, posicao);

                    posicaoYLinha += espacamento;
                    numeroDeLinhasInseridas++;
                }

                paginaGrafico.DrawString($"{descricao.TrimStart()}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                if (descricaoList.Count > 1 && numeroDescricoesInseridas < descricaoList.Count)
                    posicaoYLinha += espacamento;

                numeroDescricoesInseridas++;
                numeroDeLinhasInseridas++;
            });

            posicaoYLinha += espacamento * 2;
            paginaGrafico.DrawString($"PAGAMENTO", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
            posicaoYLinha += espacamento * 2;

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha);

            pedidoInternoDTO.Parcelas.ForEach(parcela =>
            {
                paginaGrafico.DrawString($"{parcela.CodigoFormatado}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda, posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{parcela.DataPagamento.ToString("dd/MM/yyyy")}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.2), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);
                paginaGrafico.DrawString($"{parcela.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}", fonteNormal, XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.4), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

                paginaGrafico.DrawLine(XPens.Black, margemEsquerda - 2, posicaoYLinha, margemEsquerda - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.2) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 12);
                paginaGrafico.DrawLine(XPens.Black, margemEsquerda - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha);

                posicaoYLinha += espacamento;
            });


            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha + 12);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha + 12);
            paginaGrafico.DrawLine(XPens.Black, margemEsquerda - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha);
            paginaGrafico.DrawString($"{pedidoInternoDTO.Parcelas.Sum(x=>x.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))}", new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Black, new XRect(margemEsquerda + (pagina.Width.Value * 0.4), posicaoYLinha, pagina.Width, pagina.Height), XStringFormats.TopLeft);

            posicaoYLinha += espacamento;

            paginaGrafico.DrawLine(XPens.Black, margemEsquerda + (pagina.Width.Value * 0.4) - 2, posicaoYLinha, margemEsquerda + (pagina.Width.Value * 0.6) - 2, posicaoYLinha);

            posicaoYLinha = (int)pagina.Height.Value - 100;
            AdicionaRodape(ref paginaGrafico, ref posicaoYLinha, ref espacamento, ref margemEsquerda, ref margemSuperior, ref pagina, ref documentoPdf, pedidoInternoDTO, fonteTitulo, fonteNormal, fonteCaixaAlta);

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
    }
}
